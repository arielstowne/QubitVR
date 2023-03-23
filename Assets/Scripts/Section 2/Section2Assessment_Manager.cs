using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QubitMath;
using Section2_Assessment_Questions;
using Random = UnityEngine.Random;
using TMPro;

/** Manages content and state of Mod 1 Sec 2 Asmnt.
  *
  * For section 2's assessment, manages menus, prepares and checks questions, and handles file I/O for Reports.
  * Attached to the Section2_Assessment gameobject.
  */
public class Section2Assessment_Manager : ModuleManager
{
    ///@{
    /** Content panels/images that need to be assigned in the Editor */
    public GameObject introduction, conclusion, success, fail, targetVector;
    ///@}

    ///@{
    /** Used for enabling/disabling gates */
    public GameObject H_Gate, X_Gate, S_Gate, T_Gate;
    ///@}

    /** 10 assessment menus */
    public GameObject[] assessmentMenus;

    /** Used to generate assessment questions */
    private Assessment assessment;

    /** Used to interact with questions within the assessment object */
    Question question;

    int gatesApplied = 0;

    /** Controls the difficulty of questions */
    int[] questionDifficulties = {1, 1, 1, 1, 2, 2, 2, 2, 3, 3};
    /** Ensures a question is not selected multiple times */
    bool [,] questionHash;

    ///@{
    /** Used for reporting */
    string questionGenerated = "";
    string questionedAnswered = "";
    ///@}

    public AudioClip correct, incorrect;

    /** Set first slide active and generate assessment */
    protected override void init()
    {
        introduction.SetActive(true);
        assessment = new Assessment();
    }

    /** Sets the target vector for an assessment question */
    public void pointTargetVector(Matrix state)
    {
        ApplyGate script = QubitManager.getScript(0);

        float x = script.GetCoordinate(state, 'x');
        float y = script.GetCoordinate(state, 'y');
        float z = script.GetCoordinate(state, 'z');

        targetVector.SetActive(true);
        Vector3 qubitPosition = QubitManager.qubits[0].transform.position;
        Vector3 target = new Vector3(x, z, y); // z and y are switched in unity and traditional mathematics.

        targetVector.transform.LookAt(qubitPosition + target);
    }

    public void startAssessment()
    {
        // Set step to 1 and generate the first question
        setStep(1);
        QubitManager.enableQubit(0, States.UP);
        questionHash = new bool[3, 6];
        generateQuestion(questionDifficulties[step-1]);

        allowGates(true, true, true, true);

        // Start stopwatch for assessment.
        EventManager.StartedAssessment.Invoke();
    }

    private void endAssessment()
    {
        EventManager.FinishedAssessment.Invoke();
        // Load current menu, qubit, and conclusion menu.
        assessmentMenus[step - 1].SetActive(false);
        QubitManager.disableQubit(0);
        conclusion.SetActive(true);
    }

    public void nextQuestion()
    {
        // End assessment after 10 questions
        if (step == 10)
        {
            endAssessment();
            return;
        }

        // Generate the next question
        incrementStep();
        generateQuestion(questionDifficulties[step-1]);
    }

    /** Helper function to setup parameters when loading an alternate menu (fail, success, etc.) */
    public void alternateMenuSetup()
    {
        assessmentMenus[step - 1].SetActive(false);
        QubitManager.setQubitLock(0, true);
        history.Clear();
        gatesApplied = 0;
    }

    /** After generating a question, apply to a question using this function  */
    public void setupQubit()
    {
        // Set up the qubit using the returned variables.
        assessmentMenus[step - 1].SetActive(true);
        QubitManager.setQubitLock(0, false);
        QubitManager.setQubitState(0, question.start);
        pointTargetVector(question.target);

        // CSV report (moment when question is presented to the user)
        questionGenerated = GetTimeStamp();
        SaveManager.AppendToReport(GetReportLine());
        Debug.Log("<color=green>Report updated successfully!</color>");
    }

    /** Uses RNG to select a random question from the assessment
    *
    * Generate a random number to pull a question from assessment.
    * Questions are hashed so no repeat. Keep generating a random number until a unique question is found.
    */
    private void generateQuestion(int gates)
    {
        int questionNum = Random.Range(0, 6);
        while (questionHash[gates-1, questionNum])
            questionNum = Random.Range(0, 6);

        questionHash[gates-1, questionNum] = true;

        if (gates == 1)
            question = assessment.singleGateQuestions[questionNum];
        else if (gates == 2)
            question = assessment.doubleGateQuestions[questionNum];
        else if (gates == 3)
            question = assessment.tripleGateQuestions[questionNum];
        else
            return;

        setupQubit();
    }

    /** Function to only allow certain gates in the toolbelt to be used at any given time. */
    public void allowGates(bool H, bool X, bool S, bool T)
    {

        H_Gate.SetActive(H);
        X_Gate.SetActive(X);
        S_Gate.SetActive(S);
        T_Gate.SetActive(T);
    }

    void Update()
    {
        // Detect if the user applied a gate in order generate a line in the report.
        if (history != null)
        {
            if (gatesApplied != history.length)
            {
                // CSV report (moment when user applies a gate)
                questionedAnswered = GetTimeStamp();
             // SaveManager.AppendToReport(GetReportLine());
                Debug.Log("<color=magenta>Report updated successfully!</color>");
            }

            gatesApplied = history.length;
        }

        // Once the user has applied 'question.numGates' number of gates, check that solution with question.solutions
        if (question != null && history != null && (gatesApplied == question.numGates))
        {
            Debug.Log("<color=red>Questions answered!</color>");

            // Iterate through each solution in question.solutions and compare the contents of each with the contents of
            // the user's solution (history.gates)
            for (int solutionIndex = 0; solutionIndex < question.numSolutions; solutionIndex++)
            {
                // Flag keeping track of whether the current solution in memory matches the users current solution
                bool correctSolution = true;
                for (int gateIndex = 0; gateIndex < question.numGates; gateIndex++)
                {
                    // If any component of the answer does not match, break early.
                    if (history.gates[gateIndex] != question.solutions[solutionIndex, gateIndex])
                    {
                        correctSolution = false;
                        break;
                    }
                }

                // If the particular solution was correct, display the Success menu, clear the qubit's history,
                // and return from Update() entirely.
                if (correctSolution)
                {
                    success.SetActive(true);
                    audioSource.clip = correct;
                    audioSource.PlayDelayed(0.3f);
                    alternateMenuSetup();
                    return;
                }
            }

            // If the loop completes (no correct solutions were found), then enable the Fail menu.
            fail.SetActive(true);
            audioSource.clip = incorrect;
            audioSource.PlayDelayed(0.3f);
            alternateMenuSetup();
            return;
        }
    }

    /** Used for reporting */
    string [] GetReportLine()
    {
        string [] returnable = new string[7];

        if (history == null || history.length == 0)
        {
            returnable[4] = "0";     // what component of the answer was applied
            returnable[5] = "empty"; // user answer
            returnable[6] = "user has not answered"; // time when question was answered
        }
        else
        {
            Debug.Log("history.length = " + history.length);
            returnable[4] = history.length.ToString(); // what component of the answer was applied
            returnable[5] = history.gates[history.length - 1].ToString(); // user answer
        }

        returnable[0] = "Section 2 Assesment 1";
        returnable[1] = question.start.ReportToString(); // qubit state generated start
        returnable[2] = question.target.ReportToString(); // qubit state generated target
        returnable[3] = questionGenerated; // time when question was presented
        returnable[6] = questionedAnswered; // time when question was answered

        return returnable;
    }

    static string GetTimeStamp()
    {
        return System.DateTime.UtcNow.ToString();
    }
}
