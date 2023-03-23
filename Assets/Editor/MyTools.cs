using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NewBehaviourScript 
{
  [MenuItem("My Tools/1. Add Defaults To Report %F1")]
  static void DEV_AppendDefaultsToReport() {
    SaveManager.AppendToReport(
      new string[3] {
        Random.Range(0,100).ToString(),
        Random.Range(0,100).ToString(),
        Random.Range(0,100).ToString()
      }
    );
    EditorApplication.Beep();
    Debug.Log("<color=green>Report updated successfully!</color>");
  }

  [MenuItem("My Tools/2. Reset Report %F12")]
  static void DEV_RestReport() {
    SaveManager.CreateReport();
    EditorApplication.Beep();
    Debug.Log("<color=orange>Report was rest</color>");
  }

  public static void DEV_AppendSpecificsToReport(string[] strings) {
    SaveManager.AppendToReport(strings);
    EditorApplication.Beep();
    Debug.Log("<color=green>Report updated successfully!</color>");
  }
}
