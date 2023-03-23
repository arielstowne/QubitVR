using System.Collections;
using UnityEngine;
using QubitMath;

public class SandboxManager : ModuleManager
{
  public GameObject introduction;
  
  protected override void init()
  {
    introduction.SetActive(true);
  }

  public void enableQubit()
  {
    QubitManager.enableQubit(0, States.UP);
    QubitManager.setQubitLock(0, false);
  }
}
