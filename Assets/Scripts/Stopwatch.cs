using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/** Controls the stopwatch on the right hand.
  *
  * Attaches to Player->SteamVRObjects->RightHand
  */
public class Stopwatch : MonoBehaviour
{
    /** Attach the stopwatch object at Player->SteamVRObjects->RightHand->Stopwatch */
    public GameObject stopwatch;
    /** Set true to turn the watch on, false to turn the watch off */
    public bool useStopwatch;

    void Awake()
    {
        // Start the app with the stopwatch off because it is for assessments.
        TurnOff();
        if (useStopwatch)
        {
            // When any assessment begins, we want the stopwatch to turn on.
            EventManager.StartedAssessment.AddListener(TurnOn);
            // When any assessment ends, we want the stopwatch to turn off.
            EventManager.FinishedAssessment.AddListener(TurnOff);
        }
        
    }

    private void OnDestroy()
    {
        if (useStopwatch)
        {
            // This is necessary because the event needs to be aware of only valid event listeners with active game objects
            EventManager.StartedAssessment.RemoveListener(TurnOn);
            EventManager.FinishedAssessment.RemoveListener(TurnOff);
        }
    }

    // The stopwatch runs itself. Simply turn it on to start running the time.
    void TurnOn()
    {
        stopwatch.SetActive(true);
    }

    void TurnOff()
    {
        stopwatch.SetActive(false);
    }
}
