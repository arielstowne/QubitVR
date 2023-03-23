using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/** Depreciated */
public class ToolboxButtons : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Toolbox ToolBox;
    public GameObject Gate;

    void Start()
    {
        ToolBox = gameObject.transform.parent.gameObject.GetComponent<Toolbox>();
    }

    public void OnPointerClick(PointerEventData e)
    {
        // This will have the gate appear between the user and the qubit,
        // but closer to the user than the qubit
        Vector3 resultingPosition = Positioning.BetweenObjects(Camera.main.gameObject, ToolBox.Anchor, .3f, 1f);

        // This is an option to have the gate appear directly in front of the camera every time.
        // Vector3 resultingPosition = Positioning.CenterOfCamera(1f);

        ToolBox.DisableGates(Gate, resultingPosition);
        Gate?.transform.LookAt(Camera.main.gameObject.transform.position);
    }

    public void OnPointerEnter(PointerEventData e)
    {
        // This hightlights the Menu button on the tray
        ToolBox.ChangeMaterial(gameObject, ToolBox.TrayHighlight);
    }

    public void OnPointerExit(PointerEventData e)
    {
        // This returns the Menu button on the tray to its normal color
        ToolBox.ChangeMaterial(gameObject, ToolBox.TrayColor);
    }
}
