using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/** Depreciated */
public class GameOver : MonoBehaviour
{
  private GameObject gameOverUI = null;

  // Module Manager Should be in charge of this
  // and not the Game Manager.
  public void RetryAssesment()
  {
    FindObjectOfType<GameManager>().Restart();
  }

  public void QuitModule()
  {
    Debug.Log("Existing the Module");

  }
}
