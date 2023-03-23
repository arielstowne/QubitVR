/* SceneHandler.cs*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QubitMath;

/** Parent of all other module, section, and assessment Managers
*
* Built in variables and functions for tracking qubits, gates, History.
* Can be used to create music.
* Controls the state tree.
*/
public class ModuleManager : MonoBehaviour
{
    /** Used as a guard/sentinel in the state tree */
    protected bool state;

    ///@{
    /** Tracks which tools a user is interacting with */
    protected bool isHoldingFlashlight, isHoldingH, isHoldingNOT, isHoldingS, isHoldingT; // State used for dynamic content to track progress in module step
    ///@}

    /** Current step user is on in dynamic content */
    protected int step;
    /** Holds last applied gate for step checks */
    protected string currentGate;
    /** Parent script for spawning and controlling all qubits */
    protected QubitManager QubitManager;

    protected History history;

    public AudioSource audioSource;
    public AudioClip measure;

    void Start()
    {
        // Initializing QubitManager first is necessary for a lot of the init() functions in derived classes to work.
        QubitManager = GetComponent<QubitManager>();  // Can spawn qubits (add more control?)
        init();
        clearGate();                                  // Initialize currentGate to "empty"
        setStateTrue();                               // Initialize state to true
        setStep(0);
        initGates();
    }

    /** Sets all gate states to false */
    private void initGates()
    {
        isHoldingFlashlight = false;
        isHoldingH = false;
        isHoldingNOT = false;
        isHoldingS = false;
        isHoldingT = false;
    }

    /** Overide and add module specific instantiations here in child managers.
    *
    * References to QubitManager in init() may not work, as the QubitManager has to initialize.
    */
    protected virtual void init()
    {
        Debug.Log("Parent ModuleManager init().");
    }

    /** Called by ApplyGate to announce measurement */
    public void performMeasurement()
    {
        currentGate = "Measure";
        audioSource.clip = measure;
        audioSource.Play();
        Debug.Log("Detected " + currentGate);
    }

    /** Called  by ApplyGate to announce a gate application and send history */
    public void setGate(History history)
    {
        currentGate = history.gates[history.length - 1];
        this.history = history;
        Debug.Log("Detected " + currentGate);
    }

    // Clears the currently set gate.
    public void clearGate()
    {
        currentGate = "empty";
        Debug.Log("Gate is " + currentGate);
    }

    ///@{
    /** State tree controls */
    public virtual void setStateFalse()
    {
        state = false;
        Debug.Log("State is " + state);
    }

    public virtual void setStateTrue()
    {
        state = true;
        Debug.Log("State is " + state);
    }

    public void setStep(int n)
    {
        step = n;
        Debug.Log("Step set to " + step);
    }

    public virtual void incrementStep()
    {
        step++;
        Debug.Log("Step incremented to " + step);
    }
    ///@}

    ///@{
    /** All the following are called by InteractiveObject to update the held status of tools. */
    public void flashlightStatus(bool val)
    {
        isHoldingFlashlight = val;
        Debug.Log("flashlight is " + val);
    }

    public void notStatus(bool val)
    {
        isHoldingNOT = val;
        Debug.Log("not gate is " + val);
    }

    public void hStatus(bool val)
    {
        isHoldingH = val;
        Debug.Log("h gate is " + val);
    }

    public void sStatus(bool val)
    {
        isHoldingS = val;
        Debug.Log("s gate is " + val);
    }

    public void tStatus(bool val)
    {
        isHoldingT = val;
        Debug.Log("t gate is " + val);
    }
    ///@}

    /** This is called if an object undefined in InteractiveObject is held. */
    public void unknownStatus(bool val)
    {
        Debug.Log("unkown object is " + val);
        Debug.Log("<color=red>This shouldn't happen. Are gate tags and properties set correctly?</color>");
    }

}
