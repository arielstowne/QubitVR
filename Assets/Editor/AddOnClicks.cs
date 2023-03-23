using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Events;

public class AddOnClicks : MonoBehaviour
{
    private void OnValidate()
    {
        PopulateOnClicks("welcome_screen", "next_btn", "modules_screen");
        PopulateOnClicks("modules_screen", "mod1_btn", "concepts_screen");
        PopulateOnClicks("modules_screen", "mod2_btn", "concepts_screen");
        PopulateOnClicks("modules_screen", "mod3_btn", "concepts_screen");
        PopulateOnClicks("concepts_screen", "concept1_btn", "concept_intro");
        PopulateOnClicks("concepts_screen", "concept2_btn", "concept_intro");
        PopulateOnClicks("concepts_screen", "concept3_btn", "concept_intro");
        PopulateOnClicks("concepts_screen", "concept4_btn", "concept_intro");
        PlaceOnClick("concept_intro", "next_btn", "concept_intro", false);
        PopulateOnClicks("assessment_difficulty", "next_btn", "assessment_objective");
        PlaceOnClick("assessment_objective", "next_btn", "assessment_objective", false);
        PopulateOnClicks("congratulations", "next_btn", "concept_completed");
        PopulateOnClicks("pause_tutorial", "skip_tutorial_btn", "assessment_difficulty");
        PopulateOnClicks("pause_tutorial", "skip_concept_btn", "concept_intro");
        PopulateOnClicks("pause_tutorial", "new_concept_btn", "modules_screen");
        PlaceOnClick("pause_tutorial", "resume_btn", "pause_tutorial", false);
        PlaceOnClick("pause_assessment", "hint_btn", "pause_assessment", false);
        PopulateOnClicks("pause_assessment", "return_tutorial_btn", "concept_intro");
        PopulateOnClicks("pause_assessment", "new_concept_btn", "modules_screen");
        PlaceOnClick("pause_assessment", "resume_btn", "pause_assessment", false);
        PlaceOnClick("concept_completed", "review_btn", "concept_completed", false);
        PopulateOnClicks("concept_completed", "return_tutorial_btn", "concept_intro");
        PopulateOnClicks("concept_completed", "new_concept_btn", "modules_screen");
        PopulateOnClicks("concept_completed", "continue_btn", "concept_intro");

        PopulateOnClicks("assessment_I_intro_0", "next_btn", "assessment_I_intro_1");
        PopulateOnClicks("assessment_I_intro_1", "next_btn", "assessment_I_intro_2");
        PopulateOnClicks("assessment_I_intro_2", "next_btn", "assessment1_q1_prompt");
        PopulateOnClicks("assessment1_q1_prompt", "next_btn", "assessment1_q2_prompt");
        PopulateOnClicks("assessment1_q2_prompt", "next_btn", "assessment1_q3_prompt");
        PopulateOnClicks("assessment1_q3_prompt", "next_btn", "assessment1_q4_prompt");
        PopulateOnClicks("assessment1_q4_prompt", "next_btn", "assessment1_q5_prompt");
        PopulateOnClicks("assessment1_q5_prompt", "next_btn", "assessment_I_conclusion_1");
        PopulateOnClicks("assessment_I_conclusion_1", "next_btn", "assessment_II_intro_1");
        PopulateOnClicks("assessment_II_intro_1", "next_btn", "assessment_II_intro_2");
        PopulateOnClicks("assessment_II_intro_2", "next_btn", "assessment_II_intro_3");
        PopulateOnClicks("assessment_II_intro_3", "next_btn", "assessment_II_intro_4");
        PopulateOnClicks("assessment_II_intro_4", "next_btn", "assessment_II_1");
        PopulateOnClicks("assessment_II_1", "next_btn", "assessment_II_2");
        PopulateOnClicks("assessment_II_2", "next_btn", "assessment_II_3");
        PopulateOnClicks("assessment_II_3", "next_btn", "assessment_II_4");
        PopulateOnClicks("assessment_II_4", "next_btn", "assessment_II_5");
        PopulateOnClicks("assessment_II_5", "next_btn", "assessment_II_conclusion_1");
        PopulateOnClicks("assessment_II_conclusion_1", "next_btn", "completed_1");
    }

    public void PopulateOnClicks(string ButtonScreen, string ButtonName, string NextScreen)
    {
        // Turns the current menu off and the next menu on.
        PlaceOnClick(ButtonScreen, ButtonName, ButtonScreen, false);
        PlaceOnClick(ButtonScreen, ButtonName, NextScreen, true);
    }

    public void PlaceOnClick(string ButtonScreen, string ButtonName, string NextScreen, bool OnorOff)
    {
        // Gets a reference to the button component.
        GameObject go = GameObject.Find("section1_content");

        if (go == null)
            return;

        Button btn = go.transform.Find(ButtonScreen + "/menu/" + ButtonName)?.gameObject.GetComponent<Button>();

        if (btn == null)
            return;

        // Locates the menu that the OnClick will make appear or disappear.
        GameObject AffectedMenu = go.transform.Find(NextScreen)?.gameObject;

        if (AffectedMenu == null)
            return;

        // Checks if there is already an OnClick listener that triggers the AffectedMenu.
        int num = btn.onClick.GetPersistentEventCount();
        if (num > 0)
        {
            bool DuplicateFound = ComparePersistentListeners(btn, AffectedMenu);
            if (DuplicateFound == true)
            {
                Debug.Log("Persistent listener for " + NextScreen + " already on " + ButtonScreen + ": " + ButtonName);
                return;
            }
                
        }

        // Identifies the gameobject and function that the OnClick will affect.
        UnityAction<bool> action = new UnityAction<bool>(AffectedMenu.SetActive);

        // btn.onClick identifies where the OnClick will go (on which button).
        // action specifies the values for the GameObject and Function sections of the OnClick.
        // OnorOff will be either true or false to turn the GameObject on or off, respectively.
        UnityEventTools.AddBoolPersistentListener(btn.onClick, action, OnorOff);
    }

    public bool ComparePersistentListeners(Button btn, GameObject AffectedMenu)
    {
        int num = btn.onClick.GetPersistentEventCount();

        // Compares the target of each listener to the AffectedMenu.
        for (int i = 0; i < num; i++)
        {
            Object target = btn.onClick.GetPersistentTarget(i);
            if (target == AffectedMenu)
            {
                return true;
            }
        }

        return false;
    }
}
