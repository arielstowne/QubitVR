using UnityEngine.Events;

/** Static class holding UnityEvents for the player object. */
public static class EventManager
{
    // These all affect the player object, so they might be particularly useful if
    // the "Don't Destroy On Load" option for the player is enabled.
    public static readonly UnityEvent PickedUpItem = new UnityEvent();
    public static readonly UnityEvent DroppedItem = new UnityEvent();
    public static readonly UnityEvent StartedAssessment = new UnityEvent();
    public static readonly UnityEvent PausedAssessment = new UnityEvent();
    public static readonly UnityEvent ResumedAssessment = new UnityEvent();
    public static readonly UnityEvent FinishedAssessment = new UnityEvent();
}
