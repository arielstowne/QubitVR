using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QubitMath;
using Section2_Assessment_Questions;
using Random = UnityEngine.Random;
using TMPro;

/** Depreciated Manager for Mod 1 Sec 2 with Scoreboard feature */
public class Section2Assessment_ManagerWithScoreboard : ModuleManager
{
    public GameObject introduction, conclusion, success, fail, correct, incorrect, redundant, targetVector; // alternate menus
    public GameObject[] assessmentMenus; // 10 assessment menus
    private Assessment assessment;
    Question question;
    Scoreboard_Manager scoreboard;
    bool failAfterCorrect = false;
    bool[] solutionHash;
    int attemptsRemaining, solutionsFound, gatesApplied = 0;
    int[] questionDifficulties = {1, 1, 1, 1, 2, 2, 2, 2, 3, 3};

    string questionGenerated = "";
    string questionedAnswered = "";

    protected override void init()
    {
        // Set first slidea active, generate assessment and scoreboard instances
        introduction.SetActive(true);
        assessment = new Assessment();
        scoreboard = GetComponent<Scoreboard_Manager>();
    }

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
        generateQuestion(questionDifficulties[step-1]);

        // Initialize the scoreboard
        scoreboard.scoreboard.SetActive(true);
    }

    private void endAssessment()
    {
        // Unload scoreboard, current menu, and qubit. Load conclusion menu.
        scoreboard.scoreboard.SetActive(false);
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

        // Reset scoreboard and generate the next question
        scoreboard.resetScoreboard();
        incrementStep();
        generateQuestion(questionDifficulties[step-1]);
    }

    public void alternateMenuSetup()
    {
        // Helper fucntion to setup parameters when loading an alternate menu (fail, success, etc.)
        assessmentMenus[step - 1].SetActive(false);
        QubitManager.setQubitLock(0, true);
    }

    public void setupQubit()
    {
        // Sets the menu for the current question active (used when the user guesses incorrectly and reload is required from 'assessment_incorrect')
        // Special condition where the user gives a correct solution, but is out of atttempts
        if (failAfterCorrect)
        {
            fail.SetActive(true);
            failAfterCorrect = false;
        }
        else
        {
            // Set up the qubit using the returned variables.
            assessmentMenus[step - 1].SetActive(true);
            QubitManager.setQubitLock(0, false);
            QubitManager.setQubitState(0, question.start);
            pointTargetVector(question.target);
            // CSV report
            questionGenerated = GetTimeStamp();
            SaveManager.AppendToReport(GetReportLine());
            Debug.Log("<color=green>Report updated successfully!</color>");
        }
    }

    private void generateQuestion(int gates)
    {
        // Read in a question from the question bank in 'assessment'.
        int questionNum = Random.Range(0, 4);

        if (gates == 1)
            question = assessment.singleGateQuestions[questionNum];
        else if (gates == 2)
            question = assessment.doubleGateQuestions[questionNum];
        else if (gates == 3)
            question = assessment.tripleGateQuestions[0];
        else
            return;

        setupQubit();

        // Setup hash array to prevent finding the same solution twice.
        solutionHash = new bool[question.numSolutions];
        attemptsRemaining = question.attempts;
        solutionsFound = 0;

        // Initialize scoreboard properties
        scoreboard.Initialize(question);
        scoreboard.updateTitle(step, question);
        scoreboard.updateAttempts(attemptsRemaining);
    }

    // Update is called once per frame
    void Update()
    {
        // Detect gate applied
        if (history != null)
        {
            if (gatesApplied != history.length)
            {
                scoreboard.enableGate(history.gates[history.length - 1], history.length);
                // CSV report
                questionedAnswered = GetTimeStamp();
                SaveManager.AppendToReport(GetReportLine());
                Debug.Log("<color=magenta>Report updated successfully!</color>");
            }

            gatesApplied = history.length;
        }

        // Once the user has applied 'question.numGates' number of gates, check that solution with question.solutions
        if (question != null && history != null && (gatesApplied == question.numGates))
        {
            Debug.Log("<color=red>Questions answered!</color>");
            // Decrease the attempts remaining by 1, then update the scoreboard text.
            attemptsRemaining--;
            scoreboard.updateAttempts(attemptsRemaining);

            // Iterate through each solution in question.solutions and compare the contents of each with the contents of
            // the user's solution (history.gates)
            for (int m=0; m < question.numSolutions; m++)
            {
                // Flag keeping track of whether the current solution in memory matches the users current solution
                bool correctSolution = true;
                for (int n=0; n < question.numGates; n++)
                {
                    // If any component of the answer does not match, break early.
                    if (history.gates[n] != question.solutions[m, n])
                    {
                        correctSolution = false;

                        // only load the 'incorrect' menu if they still have attempts remaining AND have searched through all
                        // solutions (m equaling question.numSolutions-1)
                        if (m == question.numSolutions - 1)
                        {
                            if (attemptsRemaining > 0)
                            {
                                incorrect.SetActive(true);
                                alternateMenuSetup();
                            }
                            scoreboard.setRowBorder("red");
                        }
                        break;
                    }
                }

                // If the particular solution was correct, mark that solution as found and stop checking for solutions.
                if (correctSolution)
                {
                    // If the particular solution was already found, warn the user that they have already found this solution. Skip straight
                    // to the fail menu if the last attempt is redundant.
                    if (solutionHash[m] == true)
                    {
                        if (attemptsRemaining == 0)
                            break;

                        redundant.SetActive(true);
                        alternateMenuSetup();
                        break;
                    }

                    scoreboard.setRowBorder("green");
                    solutionHash[m] = true;
                    solutionsFound++;

                    // If the user has not found all of the solutions
                    if (solutionsFound != question.numSolutions)
                    {
                        correct.SetActive(true);
                        alternateMenuSetup();

                        // If this correct solution ends up being your last attempt, then show the fail menu AFTER the correct menu
                        if (attemptsRemaining == 0)
                        {
                            failAfterCorrect = true;
                            scoreboard.pauseBlinking();
                        }
                    }
                    break;
                }
            }

            // Once the user finds all solutions or is out of attempts for this question
            if (solutionsFound == question.numSolutions)
            {
                success.SetActive(true);
                alternateMenuSetup();
                scoreboard.pauseBlinking();
            }
            else if (attemptsRemaining == 0 && !failAfterCorrect) // if failAfterCorrect is true, reroute fail.SetActive to setupQubit()
            {
                //scoreboard.setRowBorder("red")
                fail.SetActive(true);
                alternateMenuSetup();
                scoreboard.pauseBlinking();
            }

            // Clear the qubit history and reset the qubit (to its current question) after each set of gates
            history.Clear();
            gatesApplied = 0;
        }
    }

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
