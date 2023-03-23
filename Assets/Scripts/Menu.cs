using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/** Controls the main menu. */
public class Menu : MonoBehaviour
{
  public int currentModule = 0;
  public Button singleQubitButton = null;
  public Button twoQubitButton = null;
  public Button algorithmButton = null;

  // Currently this method is attached to the Single Qubit Module
  // There could be a pretutorial in which the qubit is described in more
  // detail before being with module 1.
  public void StartGame()
  {
    if (currentModule == 0)
    {
      currentModule = ++FindObjectOfType<GameManager>().highestCompletedModule;
      Debug.Log("<color=blue>StartGame(): currentModule is: </color>" + currentModule);
    }
    // SceneManager.LoadScene(currentModule);
  }

  void ModuleSelector()
  {
    currentModule = FindObjectOfType<GameManager>().GetCurrentModule();
    Debug.Log("<color=orange>currentModule: </color>" + currentModule);
    GameObject playButton = GameObject.Find("next_btn");
    if (playButton != null && currentModule >= 1)
      playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Resume";
    if (singleQubitButton != null && currentModule >= 1)
      singleQubitButton.interactable = true;
    if (twoQubitButton != null && currentModule >= 2)
      twoQubitButton.interactable = true;
    if (algorithmButton != null && currentModule >= 1)
      algorithmButton.interactable = true;
  }

  public void BuildSceneSelector(int sceneBuild)
  {
    // if (sceneBuild <= FindObjectOfType<GameManager>().GetCurrentModule())
      SceneManager.LoadScene(sceneBuild);
  }

  public void LoadNextScene()
  {
    Debug.Log("<color=blue>Section completed and loading next scene</color>");
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // GameObject.Find("LevelComplete").SetActive(false);
  }

  public void LoadPreviousScene()
  {
    Debug.Log("<color=blue>Section completed and loading next scene</color>");
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        // GameObject.Find("LevelComplete").SetActive(false);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
