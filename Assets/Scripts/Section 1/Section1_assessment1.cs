using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QubitMath;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Events;

/** Manages content and state of Mod 1 Sec 1 Asmnt 1.
  *
  * For module 1 section 1's first assessment, manages menus, prepares and checks questions, and handles file I/O for Reports.
  * Attached to the Section1_Assessment_Part1 gameobject.
  */

public class Section1_assessment1 : ModuleManager
{
    ///@{
    /** Content panels that need to be assigned in the Editor */
    public GameObject introduction, correctScreen, incorrectScreen,
        assessment_I_2, assessment_I_3, assessment_I_4, assessment_I_5, assessment_I_conclusion_1;
    ///@}

    /** References flashlight game object from Toolbelt to hide/enable tool as necessary. */
    public GameObject flashlight;

    ///@{
    //* Counters for assessment */
    static int correctAnswers, questionNumber;
    ///@}

    ///@{
    /** Used for reporting */
    string questionGenerated = "";
    string questionedAnswered = "";
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
        flashlight.SetActive(false); // Flashlight is disabled because it is not needed within this assessment
    }

	  /** Called by onclick of assessment_I_intro_2 next_btn.	*/
    public void startAssessment()
    {
        generateAssessmentQubit(0);
        // Start stopwatch for assessment.
        EventManager.StartedAssessment.Invoke();
    }

    /** Called by onclick of correct_screen and incorrect_screen next_btn.
  	*
    * Sets the main qubit to a random state and captures the timestamp for reporting.
    * See QubitManager documentation for more details regarding random qubit states.
    * @param n the QubitManager reference to the qubit being manipulated
    */
    public void generateAssessmentQubit(int n)
    {
        QubitManager.enableQubit(n);
        QubitManager.SetRandomState(n);
        questionGenerated = GetTimeStamp();
    }

    ///@{
    /** OnClicks for Assessment answer panel buttons */
    public void clickedCertainly1()
    {
        Debug.Log("Clicked certainly 1!");
        displayFeedback(checkAnswer("down"));
    }

    public void clickedLikely1()
    {
        Debug.Log("Clicked likely 1!");
        displayFeedback(checkAnswer("likely_down"));
    }

    public void clickedEquallyLikely()
    {
        Debug.Log("Clicked equally likely!");
        displayFeedback(checkAnswer("equator"));
    }

    public void clickedLikely0()
    {
        Debug.Log("Clicked likely 0!");
        displayFeedback(checkAnswer("likely_up"));
    }

    public void clickedCertainly0()
    {
        Debug.Log("Clicked certainly 0!");
        displayFeedback(checkAnswer("up"));
    }
    ///@}

    ///@{
    /** OnClicks for Assessment answer panel buttons
    */
    private void displayFeedback(bool userAnsweredCorrectly)
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

    /** Checks assessment answers for correctness
    *
    * This fetches the correct answer for the randomly set qubit from the ApplyGate script.
    * Answers are generated along with the random qubit state in SetRandomState.
    * See QubitManager documentation for more details.
    * @param userAnswer the string value of the answer that the user selected
    **/
    private bool checkAnswer(string userAnswer)
    {
        // Get the current qubit and qubit's script to check its state.
        GameObject currentQubit = QubitManager.getQubit(0);
        ApplyGate currentScript = currentQubit.transform.GetChild(0)?.gameObject.GetComponent<ApplyGate>();

        questionedAnswered = GetTimeStamp();
        SaveManager.AppendToReport(GetReportLine(currentScript, userAnswer));

        // If the user's answer matches the correct answer, move to the correct screen and increment
        // number of correct answers so far. Otherwise, move to the incorrect screen.
        if (currentScript.assessmentAnswer.Equals(userAnswer))
            return true;
        else
            return false;
    }

    /** OnClick for moving to the next assessment question.
    *
    * Uses questionNumber to track the current question being presented,
    * changes the current shown content panel and increments questionNumber
    */
    public void nextAssessmentQuestion()
    {
        questionNumber++;
        Debug.Log("Question number: " + questionNumber);
        Debug.Log("Correct number: " + correctAnswers);

        switch (questionNumber)
        {
            case 2:
                assessment_I_2.SetActive(true);
                break;
            case 3:
                assessment_I_3.SetActive(true);
                break;
            case 4:
                assessment_I_4.SetActive(true);
                break;
            case 5:
                assessment_I_5.SetActive(true);
                break;
            case 6:
                QubitManager.disableQubit(0);
                // This stops the stopwatch.
                EventManager.FinishedAssessment.Invoke();
                assessment_I_conclusion_1.SetActive(true);
                break;
            default:
                break;
        }
    }


    /** Collects and sends data for reporting.
    *
    * See SaveManager documentation for more information.
    * @param currentScript the ApplyGate script of the current qubit
    * @param  uesrAnswer the string value of the answer that the user selected
    * @return contains data for reporting
    */
    string[] GetReportLine(ApplyGate currentScript, string userAnswer)
    {
        /** Holds the information for data reporting, and is eventually sent to SaveManager
        *
        * Currently holding: 0 = Section Title, 1 = qubit state generated, 2 = assessment answer,
        * 3 = question presented timestamp 4 = ??, 5 = user answer, 6 = question answered timestamp
        */
        string[] returnable = new string[7];

        returnable[0] = "Section 1 Assessment 1";
        returnable[1] = currentScript.assessmentAnswer; // qubit state generated start
        returnable[2] = currentScript.assessmentAnswer; // qubit state generated target
        returnable[4] = "0";     // what component of the answer was applied
        returnable[5] = userAnswer; // user answer
        returnable[3] = questionGenerated; // time when question was presented
        returnable[6] = questionedAnswered; // time when question was answered

        return returnable;
    }

    static string GetTimeStamp()
    {
        return System.DateTime.UtcNow.ToString();
    }
}
