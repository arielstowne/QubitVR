using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Controls the positioning of tools */
public class Endpoints : MonoBehaviour
{
    public bool RightEndpoint;
    public float HowFarToTheSide;
    public float height = .59f;

    void Update()
    {
        float temp = HowFarToTheSide;

        // A negative distance will place the endpoint to the users left
        if (RightEndpoint)
        {
            temp *= -1;
        }

        // Get position on the right (or left) of camera
        Vector3 resultingPosition = Camera.main.transform.position + Camera.main.transform.right * temp;
        // Set the position to a specific height (at a fraction of users height)
        transform.position = new Vector3(resultingPosition.x, Camera.main.transform.position.y * height, resultingPosition.z);
    }
}
