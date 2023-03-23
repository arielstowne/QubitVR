using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/** Stopwatch controller */
public class DisplayTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float startTime;
    private float pauseTime;
    private bool display = true;
    private Canvas canvas;

    void Awake()
    {
        EventManager.PausedAssessment.AddListener(OnPause);
        EventManager.ResumedAssessment.AddListener(OnResume);
        canvas = gameObject.GetComponent<Canvas>();
    }

    private void OnDestroy()
    {
        // This is necessary because the event needs to be aware of only valid event listeners with active game objects
        EventManager.PausedAssessment.RemoveListener(OnPause);
        EventManager.ResumedAssessment.RemoveListener(OnResume);
    }

    // The stopwatch is set active at the start of each assessment in their corresponding scripts.
    void OnEnable()
    {
        startTime = Time.time;
    }

    void OnPause()
    {
        display = false;
        pauseTime = Time.time;
    }

    void OnResume()
    {
        display = true;
        startTime += (Time.time - pauseTime);
    }

    void Update()
    {
        // If the user looks at the back of their "hand" (which is where the stopwatch is located),
        // then the stopwatch will appear until they look away or turn their hand away.
        float dotProd = Vector3.Dot(gameObject.transform.forward, Camera.main.transform.forward);

        if ((dotProd > -1.2f) && (dotProd < -.8f) && display)
        {
            // startTime is the time stored when the user started the assessment
            float timeToDisplay = Time.time - startTime;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            timeText.text = string.Format("Stopwatch\n\n{0:00}:{1:00}", minutes, seconds);
            canvas.enabled = true;
        }
        else
        {
            canvas.enabled = false;
        }
    }
}
