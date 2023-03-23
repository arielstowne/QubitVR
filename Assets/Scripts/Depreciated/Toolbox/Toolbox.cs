using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Depreciated */
public class Toolbox : MonoBehaviour
{
    // These hold the colors for this toolbox
    // They are referenced by the toolboxs buttons
    public Material TrayHighlight;
    public Material TrayColor;

    // The position and rotation of the toolbox is determined relative to the anchor and user.
    public GameObject Anchor;

    // Using these instead of referencing the children of "Gates" because
    // their locations change in the hierarchy as the user interacts with them.
    public GameObject NOTGate;
    public GameObject HGate;
    public GameObject SGate;
    public GameObject TGate;

    void Update()
    {
        // This positions the toolbox between the user and the qubit,
        // but closer to the user than the qubit
        Vector3 resultingPosition = Positioning.BetweenObjects(Camera.main.gameObject, Anchor, .3f, .2f);
        gameObject.transform.position = resultingPosition;

        // This rotates the toolbox so that it is always facing the user.
        // This works because the toolbox is always between the user and qubit,
        // so when the toolbox "looks at" the qubit, the rotation is consistent from the users POV.
        gameObject.transform.LookAt(Anchor.transform);

        // This rotates the toolbox along the x-axis so that it is always at a -35 degree angle.
        var RotateAngleX = -35f - gameObject.transform.eulerAngles.x;
        gameObject.transform.Rotate(Vector3.right, RotateAngleX);
    }

    public void ChangeMaterial(GameObject Object, Material Color)
    {
        // Changes the color of the object to a specified color.
        Object.GetComponent<MeshRenderer>().material = Color;
    }

    public void DisableGates(GameObject VisibleChild, Vector3 Position)
    {
        // Might change it to disable the Mesh Renderer instead.
        // GetComponentsInChildren<MeshRenderer>();
        NOTGate.SetActive(false);
        HGate.SetActive(false);
        SGate.SetActive(false);
        TGate.SetActive(false);

        if(VisibleChild != null)
        {
            VisibleChild.transform.position = Position;
            VisibleChild.SetActive(true);
        }
    }
}
