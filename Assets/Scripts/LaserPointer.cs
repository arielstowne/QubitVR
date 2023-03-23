using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

/** Controls the laser pointer feature */
[RequireComponent(typeof(SteamVR_LaserPointer))]
public class LaserPointer : MonoBehaviour
{
    SteamVR_LaserPointer pointer;

    void Awake()
    {
        pointer = gameObject.GetComponent<SteamVR_LaserPointer>();

        // When an object is picked up, we want the laserpointer to "turn off".
        EventManager.PickedUpItem.AddListener(TurnOff);
        // When an object is released, we want the laserpointer to "turn on".
        EventManager.DroppedItem.AddListener(TurnOn);
    }

    private void OnDestroy()
    {
        // This is necessary because the event needs to be aware of only valid event listeners with active game objects
        EventManager.PickedUpItem.RemoveListener(TurnOff);
        EventManager.DroppedItem.RemoveListener(TurnOn);
    }

    // Setting laserpointer inactive doesn't make it disappear, so just make it "invisible".
    void TurnOff()
    {
        pointer.thickness = 0f;
    }

    // This is the original thickness used in all scenes.
    void TurnOn()
    {
        pointer.thickness = 0.007f;
    }
}
