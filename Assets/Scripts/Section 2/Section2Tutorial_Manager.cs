using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QubitMath;

/** Manages content and state of Mod 1 Sec 2 Tutorial.
*
* For section 2's tutorial, manages menus, prepares and checks interactive content
* Attached to the Section2_Tutorial gameobject.
*/
public class Section2Tutorial_Manager : ModuleManager
{
    ///@{
    /** Content panels that need to be assigned in the Editor
    *
    * All menus that do NOT require a next button to progress are listed in code below and must have their references added in the Unity editor.
    * All other menus handle their transitions in the editor with the help of some functions below.
    */
    public GameObject introduction, gate_practice_I_1, gate_practice_I_2, gate_practice_I_3, gate_practice_I_4, gate_practice_I_5, gate_practice_I_6,
                        gate_practice_I_7, gate_practice_I_8, gate_practice_I_9, gate_practice_I_10, gate_practice_I_12, gate_practice_I_13,
                        gate_practice_I_14, gate_practice_II_2, gate_practice_II_3, gate_practice_II_4, gate_practice_II_5, gate_practice_II_6,
                        gate_practice_II_7, gate_practice_II_10, gate_practice_II_11, gate_practice_II_12, gate_practice_II_13,
                        gate_practice_II_14, gate_practice_II_15, gate_practice_II_16, gate_practice_II_17, gate_practice_II_18;
    ///@}

    ///@{
    /** Used for enabling/disabling gates */
    public GameObject H_Gate, X_Gate, S_Gate, T_Gate, GateAxis;
    ///@}

    ///@{
    /** Used for animations and imagery during tutorial */
    public GameObject Wedge, Gate_Parent, Animate_H, Animate_X, Animate_S, Animate_T,
        Placeholder_Parent, Placeholder_S, Placeholder_T;
    ///@}

    /** Used as a temp variable for storing history.length */
    int length;

    protected override void init()
    {
        // Initialize toolbelt settings, set first menus active.
        allowGates(false, false, false, false);
        introduction.SetActive(true);
        QubitManager.disableQubit(0);
    }

    /** // Procedure performed every time you want to change to the next menu / slide. */
    private void next()
    {
        clearGate();
        incrementStep();
        allowGates(false, false, false, false);
    }

    public void startGatePractice1()
    {
        // Enable the first qubit and set step to 1
        QubitManager.enableQubit(0, States.UP);
        Wedge.SetActive(false);
        unlockQubit();
        setStep(1);
    }

    public void startGatePractice2()
    {
        // Enable the qubit again
        QubitManager.enableQubit(0, States.UP);
        Wedge.SetActive(false);
        unlockQubit();
    }

    public void unlockQubit()
    {
        QubitManager.setQubitLock(0, false);
        setStateTrue();
    }

    public void lockQubit()
    {
        QubitManager.setQubitLock(0, true);
        setStateFalse();
    }

    /** Set the rotation of the cylindrical axis for the display of each gate rotation. */
    public void setGateAxis(string gate)
    {
        if (gate == "Off")
        {
            GateAxis.SetActive(false);
            return;
        }

        Quaternion quat = new Quaternion(0, 0, 0, 1);

        if (gate == "H")
            quat.eulerAngles = new Vector3(0f, 0f, 135f);
        else if (gate == "X")
            quat.eulerAngles = new Vector3(0f, 0f, 90f);
        else // S or T
            quat.eulerAngles = new Vector3(0f, 0f, 0f);

        // Set the rotation of the GateAxis after setting it active.
        GateAxis.SetActive(true);
        GateAxis.transform.rotation = quat;
    }

    /** Set the rotation of the wedge object for the display of each gate rotation. */
    private void SetWedgeRotation(bool vertical)
    {
        if (vertical)
            Wedge.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        else
            Wedge.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }

    /** Animates the Hadamard tutorial section */
    public void startAnimationH(bool playForward)
    {
        turnOffAnimatedGates();
        Wedge.SetActive(true);

        // The first display of the H gate shows the rotation from UP to BACKWARD
        if (playForward)
            QubitManager.enableQubit(0, States.UP);
        // The second display of the H gate shows the rotation from BACKWARD to UP
        else
            QubitManager.enableQubit(0, States.BACKWARD);

        SetWedgeRotation(playForward);

        unlockQubit();
        setStateFalse();
        Gate_Parent.SetActive(true);
        Animate_H.SetActive(true);
    }

    /** Animates the X tutorial section */
    public void startAnimationX(bool playForward)
    {
        turnOffAnimatedGates();
        Wedge.SetActive(true);

        // The first display of the X gate shows the rotation from UP to DOWN
        if (playForward)
            QubitManager.enableQubit(0, States.UP);
        // The second display of the X gate shows the rotation from DOWN to UP
        else
            QubitManager.enableQubit(0, States.DOWN);

        SetWedgeRotation(true);

        unlockQubit();
        setStateFalse();
        Gate_Parent.SetActive(true);
        Animate_X.SetActive(true);
    }

    /** Start the animation that shows 1 iteration of the S gate rotation. */
    public void startAnimationS1()
    {
        turnOffAnimatedGates();
        Wedge.SetActive(true);

        // The display of the S1 gate shows the rotation starting from BACKWARD
        QubitManager.enableQubit(0, States.BACKWARD);

        SetWedgeRotation(false);

        unlockQubit();
        setStateFalse();
        Gate_Parent.SetActive(true);
        Animate_S.SetActive(true);
    }

    /** Start the animation that continuously applies the S gate. */
    public void startAnimationS2()
    {
        turnOffAnimatedGates();
        Wedge.SetActive(true);

        // The display of the S2 gate shows the rotation starting from BACKWARD
        QubitManager.enableQubit(0, States.BACKWARD);

        SetWedgeRotation(false);

        unlockQubit();
        setStateFalse();
        Placeholder_Parent.SetActive(true);
        Placeholder_S.SetActive(true);

        setGateAxis("S");
    }

    /** Start the animation that shows 1 iteration of the T gate rotation. */
    public void startAnimationT1()
    {
        turnOffPlaceholderGates();
        Wedge.SetActive(true);

        // The display of the T1 gate shows the rotation starting from BACKWARD
        QubitManager.enableQubit(0, States.BACKWARD);

        SetWedgeRotation(false);

        unlockQubit();
        setStateFalse();
        Gate_Parent.SetActive(true);
        Animate_T.SetActive(true);
    }

    /** Start the animation that continuously applies the T gate. */
    public void startAnimationT2()
    {
        turnOffAnimatedGates();
        Wedge.SetActive(true);

        // The display of the T2 gate shows the rotation starting from BACKWARD
        QubitManager.enableQubit(0, States.BACKWARD);

        SetWedgeRotation(false);

        unlockQubit();
        setStateFalse();
        Placeholder_Parent.SetActive(true);
        Placeholder_T.SetActive(true);
    }

    /** Stop the H, X, S1, and T1 animations */
    public void stopAnimation()
    {
        // Stops all four gate animations.
        turnOffAnimatedGates();
        lockQubit();
        QubitManager.disableQubit(0);

        setGateAxis("Off");
    }

    /** Stop the S2, and T2 animations */
    public void stopPlaceholderAnimation()
    {
        // Stops all four gate animations.
        turnOffPlaceholderGates();
        lockQubit();
        QubitManager.disableQubit(0);

        setGateAxis("Off");
    }

    /** Turn off the H, X, S1, and T1 gates */
    private void turnOffAnimatedGates()
    {
        setGateAxis("Off");
        Animate_H.SetActive(false);
        Animate_X.SetActive(false);
        Animate_S.SetActive(false);
        Animate_T.SetActive(false);
        Gate_Parent.SetActive(false);
    }

    /** Turn off the S2, and T2 gates */
    private void turnOffPlaceholderGates()
    {
        setGateAxis("Off");
        Placeholder_S.SetActive(false);
        Placeholder_T.SetActive(false);
        Placeholder_Parent.SetActive(false);
    }

    /** Function to only allow certain gates in the toolbelt to be used at any given time. */
    public void allowGates(bool H, bool X, bool S, bool T)
    {
        H_Gate.SetActive(H);
        X_Gate.SetActive(X);
        S_Gate.SetActive(S);
        T_Gate.SetActive(T);
    }

    /** End of the tutorial, enable all gates for the assessment */
    public void prepareAssessment()
    {
        allowGates(true, true, true, true);
    }

    void Update()
    {
        // Method of locking all activities in update() when section is complete.
        if (!state)
            return;

        // Contains all menu transitions that do not use the 'Next Button'. All other transitions are done in the editor.
        // ----------------------------------------------------------------------------------------------------------------
        // currentGate is used every frame to check whether the user has applied a gate.
        // clearGate() is used to clear the previous gate and start listening for a new one.
        // incrementStep() is used to move to the next switch case.
        // lockQubit() is used to prevent the user from applying any gates. unlockQubit() is only used in the editor.
        // setStateFalse() is used to lock the Update() function
        // history.gates is used to check a qubits history for progression-conditions requiring multiple steps.
        switch(step)
        {
            case 1:
                allowGates(false, true, false, false); // H, X, S, T

                if (currentGate == "NOT")
                {
                    gate_practice_I_1.SetActive(false);
                    gate_practice_I_2.SetActive(true);
                    next();
                }
                break;
            case 2:
                allowGates(false, true, false, false); // H, X, S, T

                // Check for back to back NOT gates in history (short-circuits if history is too small)
                length = history.length;
                if (length > 1 && history.gates[length - 1] == "NOT" && history.gates[length - 2] == "NOT")
                {
                    gate_practice_I_2.SetActive(false);
                    gate_practice_I_3.SetActive(true);
                    lockQubit();
                    next();
                }
                break;
            case 3:
                allowGates(true, false, false, false); // H, X, S, T

                if (currentGate == "H")
                {
                    gate_practice_I_4.SetActive(false);
                    gate_practice_I_5.SetActive(true);
                    next();
                }
                break;
            case 4:
                allowGates(true, false, false, false); // H, X, S, T

                length = history.length;
                if (length > 1 && history.gates[length - 1] == "H" && history.gates[length - 2] == "H")
                {
                    gate_practice_I_5.SetActive(false);
                    gate_practice_I_6.SetActive(true);
                    lockQubit();
                    next();
                }
                break;
            case 5:
                allowGates(false, true, false, false); // H, X, S, T

                if (currentGate == "NOT")
                {
                    gate_practice_I_7.SetActive(false);
                    gate_practice_I_8.SetActive(true);
                    next();
                }
                break;
            case 6:
                allowGates(true, false, false, false); // H, X, S, T

                if (currentGate == "H")
                {
                    gate_practice_I_8.SetActive(false);
                    gate_practice_I_9.SetActive(true);
                    next();
                }
                break;
            case 7:
                allowGates(false, true, false, false); // H, X, S, T

                if (currentGate == "NOT")
                {
                    gate_practice_I_9.SetActive(false);
                    gate_practice_I_10.SetActive(true);
                    lockQubit();
                    next();
                }
                break;
            case 8:
                allowGates(true, false, false, false); // H, X, S, T

                if (currentGate == "H")
                {
                    gate_practice_I_12.SetActive(false);
                    gate_practice_I_13.SetActive(true);
                    next();
                }
                break;
            case 9:
                allowGates(false, true, false, false); // H, X, S, T

                if (currentGate == "NOT")
                {
                    gate_practice_I_13.SetActive(false);
                    gate_practice_I_14.SetActive(true);
                    lockQubit();
                    next();
                }
                break;// the h gate is accessable during the S & T gate intro bc of the next step
            case 10:
                allowGates(true, false, false, false); // H, X, S, T

                if (currentGate == "H")
                {
                    gate_practice_II_2.SetActive(false);
                    gate_practice_II_3.SetActive(true);
                    next();
                }
                break;
            case 11:
                allowGates(false, false, true, false); // H, X, S, T


                if (currentGate == "S")
                {
                    gate_practice_II_3.SetActive(false);
                    gate_practice_II_4.SetActive(true);
                    next();
                }
                break;
            case 12:
                allowGates(false, false, true, false); // H, X, S, T

                // Check for 4 'S' gates in a row. (short-circuits if history is too small)
                length = history.length;
                if (length > 3 && history.gates[length-1] == "S" && history.gates[length-2] == "S" && history.gates[length-3] == "S"
                                && history.gates[length-4] == "S")
                {
                    gate_practice_II_4.SetActive(false);
                    gate_practice_II_5.SetActive(true);
                    lockQubit();
                    next();
                }
                break;
            case 13:
                allowGates(false, false, false, true); // H, X, S, T

                // Check for 2 'T' gates in a row. (short-circuits if history is too small)
                length = history.length;
                if (length > 1 && history.gates[length-1] == "T" && history.gates[length-2] == "T")
                {
                    gate_practice_II_6.SetActive(false);
                    gate_practice_II_7.SetActive(true);
                    lockQubit();
                    next();
                }
                break;
            case 14:
                allowGates(false, true, false, false); // H, X, S, T

                if (currentGate == "NOT")
                {
                    gate_practice_II_10.SetActive(false);
                    gate_practice_II_11.SetActive(true);
                    next();
                }
                break;
            case 15:
                allowGates(false, false, false, true); // H, X, S, T

                // Check for 2 'T' gates in a row. (short-circuits if history is too small)
                length = history.length;
                if (length > 1 && history.gates[length-1] == "T" && history.gates[length-2] == "T")
                {
                    gate_practice_II_11.SetActive(false);
                    gate_practice_II_12.SetActive(true);
                    lockQubit();
                    next();
                }
                break;
            case 16:
                allowGates(false, false, false, true); // H, X, S, T

                if (currentGate == "T")
                {
                    gate_practice_II_13.SetActive(false);
                    gate_practice_II_14.SetActive(true);
                    next();
                }
                break;
            case 17:
                allowGates(true, false, false, false); // H, X, S, T

                if (currentGate == "H")
                {
                    gate_practice_II_14.SetActive(false);
                    gate_practice_II_15.SetActive(true);
                    next();
                }
                break;
            case 18:
                allowGates(false, true, false, false); // H, X, S, T

                if (currentGate == "NOT")
                {
                    gate_practice_II_15.SetActive(false);
                    gate_practice_II_16.SetActive(true);
                    next();
                }
                break;
            case 19:
                allowGates(true, false, false, false); // H, X, S, T

                if (currentGate == "H")
                {
                    gate_practice_II_16.SetActive(false);
                    gate_practice_II_17.SetActive(true);
                    next();
                }
                break;
            case 20:
                allowGates(false, false, true, true); // H, X, S, T

                if (currentGate == "T")
                {
                    next();
                    allowGates(false, false, true, false);
                }
                else if (currentGate == "S")
                {
                    next();
                    allowGates(false, false, false, true);
                }
                break;
            case 21:
                // Test to see if the two most recent gates were a T and an S
                length = history.length;
                if ((history.gates[length-1] == "S" && history.gates[length-2] == "T") ||
                    (history.gates[length-1] == "T" && history.gates[length-2] == "S"))
                {
                    gate_practice_II_17.SetActive(false);
                    gate_practice_II_18.SetActive(true);
                    lockQubit();
                }
                break;
        }
    }
}
