using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModuleSelector : MonoBehaviour
{
  public void StartModule(int module)
  {
    if (module <= FindObjectOfType<GameManager>().GetCurrentModule())
      SceneManager.LoadScene(module);
  }
}
