using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** Controls scene transitions */
public class GameManager : MonoBehaviour
{
  // Game Manager Instance
  public static GameManager instance = null;

  public GameObject completedModuleUI = null;

  public GameObject gameOverUI = null;

  public int highestCompletedModule = 0;

  public bool gameHasEnded = false;

  private void Awake()
  {
    MakeSingleton();
    //user = new UserData();
  }

  public void MakeSingleton()
  {
    if (instance != null)
    {
      Destroy(gameObject);
    }
    else
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }

  public void CompletedModule()
  {
    if (completedModuleUI.activeSelf)
      completedModuleUI.SetActive(false);
    if (SceneManager.GetActiveScene().buildIndex + 1 > highestCompletedModule)
      highestCompletedModule++;

    Debug.Log("Module Completed!");

    completedModuleUI.SetActive(true);
  }

  public void EndGame()
  {
    gameOverUI.SetActive(true);
  }

  public void Restart()
  {
    gameHasEnded = false;
    gameOverUI.SetActive(false);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  public int GetCurrentModule()
  {
    return highestCompletedModule;
  }
}
