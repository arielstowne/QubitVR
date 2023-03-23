/* SceneHandler.cs*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QubitMath;
using System;
using Random = UnityEngine.Random;

/** Manages content and state of Modd 1 Sec 1 Tutorial
  *
  * For module 1 section 1's tutorial, manages menus, prepares and checks interactive content
  * Attached to the Section1_Tutorial gameobject.
  */
public class Section1_tutorial : ModuleManager
{
    ///@{
    /** Content panels that need to be assigned in the Editor */
    public GameObject introduction, measurePrompt, measureHeld, measureFail, measureSuccess,
                        example1Prompt, example1Answer, example2Prompt, example2Answer,
                        example3Prompt, example3Answer;
    ///@}

    /** References flashlight game object to hide/enable tool as necessary. */
    public GameObject flashlight;

    protected override void init()
    {
        introduction.SetActive(true);
        allowFlashlight(false);
    }

    public void mainQubitState(string state)
    {
        if (state.CompareTo("UP") == 0)
            QubitManager.setQubitState(0, States.UP);
        else if (state.CompareTo("DOWN") == 0)
            // This uses the slow rotation to make it more obvious that the vector changed its state.
            QubitManager.setQubitState(0, States.DOWN, true, true);
        else if (state.CompareTo("LEFT") == 0)
            // This uses the slow rotation to make it more obvious that the vector changed its state.
            QubitManager.setQubitState(0, States.LEFT, true, true);
    }

    ///@{
    /** OnClicks for tutorial content panel buttons */
    public void enableMainQubit()
    {
        QubitManager.enableQubit(0, States.UP);
    }

    public void allowFlashlight(bool allow)
    {
        flashlight.SetActive(allow);
    }

    public void enableSecondaryQubits()
    {
        QubitManager.enableQubit(1, States.RIGHT);
        QubitManager.enableQubit(2, States.UP_RIGHT);
        QubitManager.enableQubit(3, States.DOWN_LEFT);
        QubitManager.enableQubit(4, States.LEFT_BACKWARD);
    }

    public void disableSecondaryQubits()
    {
        QubitManager.disableQubit(4);
        QubitManager.disableQubit(3);
        QubitManager.disableQubit(2);
        QubitManager.disableQubit(1);
    }

    public void prepareQubits()
    {
        QubitManager.setQubitState(0, States.UP);
        QubitManager.setQubitState(1, States.RIGHT);
        QubitManager.setQubitState(2, States.UP_RIGHT);
        QubitManager.setQubitState(3, States.DOWN_LEFT);
        QubitManager.setQubitState(4, States.LEFT_BACKWARD);
        QubitManager.setQubitState(5, States.UP_LEFT);
        QubitManager.setQubitState(6, States.DOWN_RIGHT);
        QubitManager.setQubitState(7, States.DOWN_RIGHT);
    }

    public void prepareExample1()
    {
        QubitManager.disableQubit(0);
        QubitManager.enableQubit(5, States.UP_LEFT);
    }

    public void prepareExample2()
    {
        QubitManager.disableQubit(5);
        QubitManager.enableQubit(6, States.DOWN_RIGHT);
    }

    public void prepareExample3()
    {
        QubitManager.disableQubit(6);
        QubitManager.enableQubit(7);
    }
    ///@}

    /** Called in Update() to await user grabbing the flashlight
      *
      * isHoldingFlashlight is set true/false by a message sent by InteractiveObject.cs
      * based on whether or not the flashlight is being held.
      */
    public void awaitFlashlight()
    {
        if (isHoldingFlashlight)
            incrementStep();
    }

    /** Called in Update() to await user applying the flashlight
      *
      * currentGate is set by a message sent by ApplyGate.cs based on the last applied gate.
      */
    public void awaitMeasurement()
    {
        if (currentGate.CompareTo("Measure") == 0)
        {
            QubitManager.setQubitLock(0, true);
            incrementStep();
        }
    }

    /** Called in Update() to await user applying the flashlight
      *
      * currentGate is set by a message sent by ApplyGate.cs based on the last applied gate.
      * In this case, it will also set the qubit's state to a pre-defined state whether than
      * allowing the engine to measure the qubit.
      * @param qubitState the desired qubit state after a measurement is applied
      */
    public void awaitMeasurement(Matrix qubitState)
    {
        if (currentGate.CompareTo("Measure") == 0)
        {
            QubitManager.setQubitLock(0, true);
            QubitManager.setQubitState(0, qubitState);
            //clearGate();
            setStateTrue();
            incrementStep();
        }
    }

    /** Contains the state tree for the section's tutorial content.
    *
    * Uses step as a counter for the state tree switch.
    * Uses boolean state as a guard for flipping between conditions within a single step.
    */
    void Update()
    {
        switch (step)
        {
            case 1: // Measurement Tutorial
                awaitFlashlight(); // When user grabs flashlight, progress
                break;

            case 2:
                // Disable previous menu
                measurePrompt.SetActive(false);
                if (state)
                {
                    QubitManager.setQubitLock(0, false);
                    measureFail.SetActive(false);
                    measureHeld.SetActive(true);
                    awaitMeasurement(States.UP);
                    if (!isHoldingFlashlight)
                        state = false;
                }
                else
                {
                    QubitManager.setQubitLock(0, true);
                    measureFail.SetActive(true);
                    measureHeld.SetActive(false);
                    if (isHoldingFlashlight)
                        state = true;
                }
                break;

            case 3: // End Measurement Tutorial
                if (state)
                {
                    clearGate();
                    QubitManager.setQubitLock(0, true);
                    measureFail.SetActive(false);
                    measureHeld.SetActive(false);
                    measureSuccess.SetActive(true);
                    setStateFalse();
                }
                break;

            case 4: // Example 1
                QubitManager.setQubitLock(5, false);
                awaitMeasurement(States.UP);
                break;

            case 5:
                if (state)
                {
                    clearGate();
                    QubitManager.setQubitLock(5, true);
                    example1Prompt.SetActive(false);
                    example1Answer.SetActive(true);
                    setStateFalse();
                }
                break;

            case 6: // Example 2
                QubitManager.setQubitLock(6, false);
                awaitMeasurement(States.DOWN);
                break;

            case 7:
                if (state)
                {
                    clearGate();
                    QubitManager.setQubitLock(6, true);
                    example2Prompt.SetActive(false);
                    example2Answer.SetActive(true);
                    setStateFalse();
                }
                break;

            case 8: // Example 3
                QubitManager.setQubitLock(7, false);
                awaitMeasurement(States.UP);
                break;

            case 9:
                if (state)
                {
                    QubitManager.setQubitState(7, States.UP);
                    clearGate();
                    QubitManager.setQubitLock(7, true);
                    example3Prompt.SetActive(false);
                    example3Answer.SetActive(true);
                    setStateFalse();
                }
                break;

            default:
                break;
        }

    }

}
