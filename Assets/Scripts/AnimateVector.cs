using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Controls vector animations
*
* Attached to qubit shell gameobject
*/
public class AnimateVector : MonoBehaviour
{
    ///@{
    /** Set these to their corresponding qubit children in the Editor */
    public GameObject qubit;
    public GameObject unitVector;
    public GameObject Terminus;
    ///@}

    private Quaternion TargetRotation;

    public float speed;
    public float degreesPerFrame;

    public bool MoveVector { get; set; }
    public bool IsRotating { get; set; }

    /** Rotates the vector to the correct position
    *
    * @param targetPosition the final location of the unit vector
    * @param halfCirclePath true if the gate applied causes a semi-circle rotation (e.g. hadamard)
    * @return nothing
    */
    public IEnumerator rotateOverTime(Vector3 targetPosition, bool halfCirclePath)
    {
        //Make sure there is only one instance of this function running so nothing else tries to move the vector at the same time
        if (IsRotating)
        {
            Debug.LogWarning("rotateOverTime() has already been called for qubit " + gameObject.GetInstanceID());
            yield break; ///exit if this is still running
        }
        IsRotating = true;

        GameObject vector = unitVector;

        // Uses the position that the vector needs to look at
        // to find the rotation required to do so.
        Vector3 direction = targetPosition - vector.transform.position;
        TargetRotation = Quaternion.LookRotation(direction);


        // Speed should be a value between 0 and 1.
        if (speed > 1f)
            speed = 1f;
        else if (speed <= 0f)
            yield break;

        Vector3 targetTerminusPosition = Vector3.zero;
        Vector3 axis = qubit.transform.position;

        if (halfCirclePath)
        {
            // Find the point directly between the initial and final position of the terminus
            // and use that as the point to rotate around for the half circle path.
            Vector3 initTerminusPosition = Terminus.transform.position;
            targetTerminusPosition = qubit.transform.position + ((Terminus.transform.localPosition.y * qubit.transform.localScale.z) * ((targetPosition - qubit.transform.position).normalized));
            axis -= Vector3.Lerp(initTerminusPosition, targetTerminusPosition, 0.5f);
        }

        MoveVector = true;
        while (IsRotating && MoveVector)
        {
            // Move the vector a fraction of the total rotation per frame, based on the path type.
            if(!halfCirclePath)
                vector.transform.rotation = Quaternion.Slerp(vector.transform.rotation, TargetRotation, speed);
            else
            {
                vector.transform.RotateAround(vector.transform.position, axis, degreesPerFrame);
            }

            Quaternion temp;
            // Don't care about the sign of X because AnnoyingRotationCheck(X) checks for both pos and neg 90.
            float X = WrapAngle(vector.transform.eulerAngles.x);

            // Changing the Z value of the vectors rotation will not change the rotation relavant to the qubits state,
            // but it is considered a different rotation to Unity.
            if (AnnoyingRotationCheck(X))
            {
                temp = Quaternion.Euler(vector.transform.eulerAngles.x, 0f, 0f);
            }
            else
                temp = Quaternion.Euler(vector.transform.eulerAngles.x, vector.transform.eulerAngles.y, 0f);

            // If the vector is at the correct rotation, then we need to stop the rotation.
            if (Mathf.Abs(Quaternion.Angle(temp, TargetRotation)) < 1f)
            {
                MoveVector = false;
            }
            yield return null;
        }

        // The function is no longer running.
        IsRotating = false;
    }

    /** Returns the equivalent rotation of <= 180.
    *
    * Keep in mind that the sign of the return value might not be accurate. */
    private static float WrapAngle(float angle)
    {
        angle %= 180;
        if (angle > 90)
            return angle - 180;

        return angle;
    }

    /** Just like the Z rotation doesn't really matter in our vector rotations, the same SOMETIMES applies to the Y rotation.
    * The Y rotation doesn't matter ONLY if the X rotation is 90 or -90 (if vector is pointing straight up or straight down).
    */
    private static bool AnnoyingRotationCheck(float X)
    {
        if (((X < 91f) && (X > 89f)) || ((X > -91f) && (X < -89f)))
            return true;
        else
            return false;
    }
}
