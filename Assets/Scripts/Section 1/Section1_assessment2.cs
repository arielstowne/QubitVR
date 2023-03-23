using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QubitMath;
using System;
using Random = UnityEngine.Random;

/** Manages content and state of Mod 1 Sec 1 Asmnt 2.
  *
  * For module 1 section 2's second assessment, manages menus, prepares and checks questions, and handles file I/O for Reports
  * Attached to the Section1_Assessment_Part2 gameobject.
  */
public class Section1_assessment2 : ModuleManager
{
    ///@{
    /** Content panels that need to be assigned in the Editor */
    public GameObject introduction, correctScreen, incorrectScreen,
        assessment_II_2, assessment_II_3, assessment_II_4, assessment_II_5, assessment_II_conclusion_1;
    ///@}

    ///@{
    //* Counters for assessment */
    static int correctAnswers, questionNumber;
    ///@}

    ///@{
    //* Plays user feedback sounds */
    public AudioClip correct, incorrect;
    ///@}

    protected override void init()
    {
        introduction.SetActive(true);
        questionNumber = 1;
        correctAnswers = 0;
    }

    /** Called by onclick of assessment_II_intro_4 next_btn to begin assessment questions after introduction */
    public void startAssessment()
    {
        randomizeSecondaryQubits();
        // Change the permissions for qubit selection by the user with the laser pointer.
        QubitManager.allowSelecting();
        // Start stopwatch for assessment.
        EventManager.StartedAssessment.Invoke();
    }

    ///@{
    /** OnClicks for Assessment answer panel buttons
    */
    public void displayFeedback(bool userAnsweredCorrectly)
    {
        if (userAnsweredCorrectly)
            answeredCorrectly();
        else
            answeredIncorrectly();
    }

    private void answeredCorrectly()
    {
        Debug.Log("Correct!");
        correctAnswers++;
        correctScreen.SetActive(true);
        audioSource.clip = correct;
        audioSource.PlayDelayed(0.3f);
    }

    private void answeredIncorrectly()
    {
        Debug.Log("Incorrect!");
        incorrectScreen.SetActive(true);
        audioSource.clip = incorrect;
        audioSource.PlayDelayed(0.3f);
    }
    ///@}

    private void randomizeSecondaryQubits()
    {
        for (int i = 0; i < QubitManager.qubits.Length; i++)
        {
            randomizeState(i);
            QubitManager.setQubitLock(i, true);
            QubitManager.enableQubit(i);
        }
    }

    /** Randomizes a qubit's state with weights towards |0> or |1>.
    *
    *
    * Randomizes the qubit state using random number choice.
    * If choice falls below certain thresholds, set the qubit state manually to up or down.
    * Otherwise, set the state randomly using the SetRandomState method in the Qubit Manager.
    * The distribution is designed to increase the chances of UP or DOWN states in assessment qubits;
    * otherwise, the assessment questions very often have all qubits as the answer.
    */
    void randomizeState(int n)
    {
        ///@{
        //* Thresholds for randomizing qubits. Must sum to 100. */
        int upChance = 5, downChance = 5, anyChance = 90;
        ///@}

        float choice = Random.Range(0, 100);

        // If choice is below the up thresholds, set state to up and update assessment answer to "up".
        if (choice < upChance)
        {
            QubitManager.getScript(n).setState(States.UP);
            QubitManager.getScript(n).assessmentAnswer = "up";
        }

        // If choice is above up threshold but below down threshold, set state to down and update assessment answer to "down".
        else if (choice < upChance + downChance)
        {
            QubitManager.getScript(n).setState(States.DOWN);
            QubitManager.getScript(n).assessmentAnswer = "down";
        }

        // If choice passes the up/down thresholds, then we use the orginal algorithm.
        else if (choice < upChance + downChance + anyChance)
        {
            QubitManager.SetRandomState(n);
            return;
        }

        Debug.Log("Set random state on qubit " + n + ". State is now " + QubitManager.getScript(n).assessmentAnswer + ".");
    }


    /** Returns the only state that would make a quibit incorrect
    *
    * Only qubits in the opposite state will not measure to the target state.
    * So return this opposite state that would make a qubit "incorrect".
	  * @return the only state that is incorrect for the current question
    **/
    private string getUserAnswerOpposite()
    {
        switch (questionNumber)
        {
            case 1:
                return "down";
            case 2:
                return "up";
            case 3:
                return "down";
            case 4:
                return "up";
            case 5:
                return "down";
            default:
                return "";
        }
    }

    /**  Called by onclick of each assessment_II_# next_btn to check qubit selection for correctness
    *
    * Cycles through the assessment's qubits and compares them to the expected answer.
    * It checks whether a qubit is currently selected using the qubitSelected variable within Qubit_Handler.
    * A selected qubit is correct if it does not match the opposite state of the prompt qubit.
    * A deselected qubit is correct if it does match the opposite state of the prompt qubit.
    * If correct, do nothing; otherwise, set the assessment question to a fails state and continue the assessment.
    **/
    public void checkQubits()
    {
        bool userCorrect = true;
        string userAnswerOpposite = getUserAnswerOpposite();
        for (int i = 0; i < QubitManager.qubits.Length; i++)
        {
            // If the qubit was selected, check if the qubit was correct
            if (QubitManager.getQubit(i).transform.GetChild(0).gameObject.GetComponent<Qubit_Handler>().qubitSelected)
            {
                // Chose an incorrect answer
                if (checkAnswer(userAnswerOpposite, i))
                {
                    userCorrect = false;
                    break;
                }
            }
            // If the qubit was not selected, check if the qubit was correct
            else
            {
                // Missed a correct answer
                if (!checkAnswer(userAnswerOpposite, i))
                {
                    userCorrect = false;
                    break;
                }
            }
        }

        if (userCorrect)
            answeredCorrectly();
        else
            answeredIncorrectly();
    }

    /** Fetches qubit's correct answer and compares it to input
    *
    * This fetches the correct answer for a randomly set qubit from the ApplyGate script.
    * Answers are generated along with the random qubit state in SetRandomState.
    * See QubitManager documentation for more details.
    * @param userAnswer the string value of the answer that the user selected
    * @param qubitNumber the QubitManager reference to the current qubit (typically 0)
    **/
    private bool checkAnswer(string userAnswer, int qubitNumber)
    {
        // Get the current qubit and qubit's script to check its state.
        ApplyGate currentScript = QubitManager.getScript(qubitNumber);

		// Compare the state of the qubit to the state that would make it "incorrect", then
        // return true if the qubit would be an incorrect answer or false if the qubit would be a correct answer.
        // (regardless whether the user selected the qubit as an answer or not)
        if (currentScript.assessmentAnswer.Equals(userAnswer))
            return true;
        else
            return false;
    }

    /** OnClick for moving to the next assessment question.
    *
    * Uses questionNumber to track the current question being presented,
    * changes the current shown content panel and increments questionNumber
    * It then sets the assessment qubits to unselected, enables selection, and randomizes them.
    */
    public void nextAssessmentQuestion()
    {
        questionNumber++;
        Debug.Log("Question number: " + questionNumber);
        Debug.Log("Correct number: " + correctAnswers);

        switch (questionNumber)
        {
            case 2:
                assessment_II_2.SetActive(true);
                break;
            case 3:
                assessment_II_3.SetActive(true);
                break;
            case 4:
                assessment_II_4.SetActive(true);
                break;
            case 5:
                assessment_II_5.SetActive(true);
                break;
            case 6:
                EventManager.FinishedAssessment.Invoke();
                assessment_II_conclusion_1.SetActive(true);
                QubitManager.resetQubits();
                return;
            default:
                break;
        }

        // Set all qubits to an unselected state.
        for (int i = 0; i < QubitManager.qubits.Length; i++)
        {
            Qubit_Handler Handler = QubitManager.getQubit(i).transform.GetChild(0).gameObject.GetComponent<Qubit_Handler>();

            Handler.isAltered = false;
            Handler.qubitSelected = false;
        }

        // TODO: Make sure that a color change is implemented for lockAllQubits()
        //QubitManager.lockAllQubits();
        randomizeSecondaryQubits();
        QubitManager.allowSelecting();
    }
}
