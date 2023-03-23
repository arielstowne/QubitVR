using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

[RequireComponent(typeof(ApplyGate))]
[RequireComponent(typeof(AnimateVector))]
public class QubitTriggers : MonoBehaviour
{
    // This is available for developers to choose to not show any preview
    // of a gate's effect on the vector. (useful for assessments)
    public bool TurnOffPreview;

    public GameObject unitVector;
    public GameObject Line;
    public GameObject Terminus;
    public Material Metal_Simple_Mat;
    public Material Metal_Simple_Mat_Fade;

    private Quaternion InitRotation;

    public bool ResetRotation { get; set; }
    public GameObject TriggerObject { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        // We don't want the toolbelt itself to react with the qubit.
        InteractiveObject tool = other.gameObject.GetComponent<InteractiveObject>();
        if (tool != null && !tool.AttachedToHand)
            return;

        // This checks if the object is a tool on the toolbelt and it IS attached to the hand,
        // BUT the developer doesn't want to show the preview/animation of the vector for a particular qubit. (for assessments)
        if (tool != null && tool.AttachedToHand && TurnOffPreview)
            return;

        // Only let one object trigger OnTriggerEnter at a time and ensure that only this object can trigger OnTriggerExit
        if (TriggerObject != null)
        {
            Debug.Log("Qubit " + gameObject.GetInstanceID() + " cannot be triggered by multiple objects at a time.");
            return;
        }

        TriggerObject = other.gameObject;

        // ReliableOnTriggerExit.cs triggers OnTriggerExit if the object enters the qubit then is disabled.
        // This is helpful for the section 2 animated gates, but probably would help in future cases as well.
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        // Keep track of the vector's initial rotation and assume this rotation will be reset,
        // unless told otherwise (refer to InteractiveObject.cs for how a gate is actually applied to the qubit).
        ResetRotation = true;
        InitRotation = unitVector.transform.rotation;
        setVectorMaterials(Metal_Simple_Mat_Fade);

        // Get the target position that the animated vector should LookAt and slowly rotate it.
        Vector3 targetPosition = gameObject.GetComponent<ApplyGate>().ToolsInBounds(other.gameObject, true, false);

        // This prevents the unit vector from being semi-transparent to anything other than registered gates (refer to ApplyGate.ToolsInBounds).
        // Or use this to prevent the unit vector from being semi-transparent if the vector doesn't move at all, even if a gate is touching the qubit:
        // Vector3.Cross(targetPosition - unitVectorOrigin.transform.position, animatedVector.transform.position - unitVectorOrigin.transform.position) == Vector3.zero)
        if (targetPosition == unitVector.transform.position)
        {
            setVectorMaterials(Metal_Simple_Mat);
            // If the turn was immediate (due to a gate release in the qubit- refer to InteractiveObject.cs),
            // then this will prevent the vector from going back to it's initial position.
            ResetRotation = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // This makes sure that only objects that are currently in the qubit and successfully triggered OnTriggerEnter
        // also trigger OnTriggerExit. Mostly used to prevent additional toolbelt and other object interference.
        if (!GameObject.ReferenceEquals(other.gameObject, TriggerObject))
            return;

        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        // Stop the vector's slow rotation if the user pulls a gate out without letting it go and the vector hadn't stopped rotating.
        gameObject.GetComponent<AnimateVector>().IsRotating = false;

        setVectorMaterials(Metal_Simple_Mat);

        // This will not return the vector to it's initial position if the gate is released in the qubit
        // (which happens in InteractiveObject.cs).
        if (ResetRotation)
            unitVector.transform.rotation = InitRotation;

        TriggerObject = null;
    }

    private void setVectorMaterials(Material material)
    {
        Line.GetComponent<MeshRenderer>().material = material;
        Terminus.GetComponent<MeshRenderer>().material = material;
    }
}
