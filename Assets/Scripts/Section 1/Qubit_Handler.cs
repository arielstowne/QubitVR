using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

/** This triggers events when the laser pointer is used on qubits in the scene.
*
* This handles the selection of qubit using the laser pointer. Currently deals with keeping track of which qubits are selected,
* and changing the qubit's color when it is selected.
* In use in Module 1 Section 1.
* Attached to:
*/
public class Qubit_Handler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public QubitManager manager;
    public Color_Coordinator colors;
    public Material selected;
    public Material highlightSelected;

    private Material original;

    /** Keeps track of whether the qubit is currently selected (true) or unselected (false) */
    public bool qubitSelected { get; set; }
    /** false until the first time the laser pointer enters the qubit (until the value is reset in an assessment manager) */
    public bool isAltered { get; set; }

    void Start()
    {
        isAltered = false;
        qubitSelected = false;
    }




    /** Allows selection of qubit.
    *
    * Allows qubit to be selected by user and changes the color accordingly.
    * Called from SceneHandler.cs. Do not call manually.
    */
    public void OnPointerClick(PointerEventData e)
    {
        // Only let the qubit be clicked if the managers have given permission for the qubits to be selected.
        if (manager.checkSelectingPermissions())
        {
            // Toggle the selection of the qubit.
            qubitSelected = !qubitSelected;

            // Change the color of the qubit to indicate whether it is selected or unselected.
            if (!qubitSelected)
                gameObject.GetComponent<Renderer>().material = original;
            else
                gameObject.GetComponent<Renderer>().material = selected;
        }
    }

    /** Highlights the qubit on hover.
    *
    * Highlights a qubit when the laser pointer hovers over by changing the color accordingly.
    * Called from SceneHandler.cs. Do not call manually.
    */
    public void OnPointerEnter(PointerEventData e)
    {
        /* Record the original color of the qubit because it may vary in different situations/assessments
        * (qubit may start with the locked color, or unlocked color, or a seperaate color
        * specifically designated to indicate unselected, etc)
        */
        if (!isAltered)
            original = gameObject.GetComponent<Renderer>()?.sharedMaterial;

        isAltered = true;

        // Only let the qubit be highlighted if the managers have given permission for the qubits to be selected.
        if (manager.checkSelectingPermissions())
        {
            // Change the color of the qubit to indicate whether it can be selected or unselected.
            if (!qubitSelected)
                // Find the highlight shade that corresponds to the original shade.
                gameObject.GetComponent<Renderer>().material = colors.findQubitHighlight(gameObject.GetComponent<Renderer>());
            else
                // This may not be necessary if the selected and highlightSelected shades are stored as a color in Color_Coordinator.cs
                gameObject.GetComponent<Renderer>().material = highlightSelected;
        }

    }

    /** Unhighlights the qubit on hover exit.
    *
    * Unhighlights a qubit when the laser pointer leaves hover by changing the color accordingly.
    * Called from SceneHandler.cs. Do not call manually.
    */
    public void OnPointerExit(PointerEventData e)
    {
        if (manager.checkSelectingPermissions())
        {
            // Return the color of the qubit to a non-higlight based on its selection state.
            if(!qubitSelected)
                gameObject.GetComponent<Renderer>().material = original;
            else
                gameObject.GetComponent<Renderer>().material = selected;
        }
    }
}
