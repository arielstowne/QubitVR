using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioning : MonoBehaviour
{
    // This returns a position at a choosen fraction of the way between two objects.
    public static Vector3 BetweenObjects(GameObject A, GameObject B, float distance, float height)
    {
        Vector3 pointA = A.transform.position;
        Vector3 pointB = B.transform.position;
        Vector3 resultingPosition = Vector3.Lerp(pointA, pointB, distance);
        resultingPosition.y *= height;
        return resultingPosition;
    }

    // Returns a position directly in the center of the cameras view, at a specified distance.
    public static Vector3 CenterOfCamera(float distance)
    {
        Vector3 resultingPosition = Camera.main.transform.position + Camera.main.transform.forward * distance;
        return resultingPosition;
    }
}
