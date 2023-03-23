using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class PauseMenu : MonoBehaviour
{
  //  public PauseMenu instance = null;

    public bool GameIsPaused = false;
    public bool InAssessment { get; set; }

    public GameObject pauseTutorialUI;
    public GameObject pauseAssessmentUI;
    public GameObject completedSectionUI;

    public GameObject SectionManager;
    public GameObject TutorialManager;
    public GameObject[] AssessmentManagers;

  //  public GameObject moduleConcepts;
 //   public bool isActive = false;

    private int buildIndex = 0;
    public SteamVR_Action_Boolean pauseButton;
    public SteamVR_Input_Sources handType;


    void Awake()
    {
        if (GameIsPaused)
            Resume();
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        pauseButton.AddOnStateUpListener(BButtonAction, handType);
        pauseButton.AddOnStateDownListener(BButtonAction, handType);

        Debug.Log("Awake() and activeInHierarchy: " + pauseTutorialUI.activeInHierarchy);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else if (SceneManager.GetActiveScene().buildIndex != 0)
                Pause();
            else
                Resume();
        }
    }

    private void OnDestroy()
    {
        pauseButton.RemoveOnStateUpListener(BButtonAction, handType);
        pauseButton.RemoveOnStateDownListener(BButtonAction, handType);
    }

    public void BButtonAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("B is Pressed");
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;
        else if (GameIsPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        if (InAssessment)
        {
            pauseAssessmentUI.SetActive(false);
            EventManager.ResumedAssessment.Invoke();
        }
        else
            pauseTutorialUI.SetActive(false);

        SectionManager.SetActive(true);

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        if (InAssessment)
        {
            pauseAssessmentUI.SetActive(true);
            EventManager.PausedAssessment.Invoke();
        }
        else
            pauseTutorialUI.SetActive(true);

        SectionManager.SetActive(false);

        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenuScene()
    {
        Resume();
        SceneManager.LoadScene(0);
    }

    public void SkipTutorial()
    {
        Resume();
        InAssessment = true;
        TutorialManager.SetActive(false);
        AssessmentManagers[0].SetActive(true);
    }

    public void SkipAssessment()
    {
        Resume();
        TutorialManager.SetActive(false);
        foreach(GameObject assessment in AssessmentManagers)
            assessment.SetActive(false);
        completedSectionUI.SetActive(true);
        EventManager.FinishedAssessment.Invoke();
    }
}
