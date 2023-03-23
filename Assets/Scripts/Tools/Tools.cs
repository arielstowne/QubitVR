using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Controls toolbelt positioning */
public class Tools : MonoBehaviour
{
    public GameObject LeftEndpoint;
    public GameObject RightEndpoint;

    // The bigger the number, the further left the tool will be.
    // But the tools will still be between the endpoints.
    // BTW, when HowFarLeft=0, HowFarForward seems to stay at 0
    public float HowFarLeft;
    public float HowFarForward = .13f;

    // Update is called once per frame
    void Update()
    {
        // Determine position between the endpoints.
        Vector3 resultingPosition = Vector3.Lerp(LeftEndpoint.transform.position, RightEndpoint.transform.position, HowFarLeft);
        // Get vector from that position to an endpoint.
        Vector3 vector1 = resultingPosition - LeftEndpoint.transform.position;
        // Use that vector with the UP vector to get a perpendicular vector that points straight away from the user.
        Vector3 normVector = Vector3.Cross(vector1, Vector3.down).normalized;
        // Determine the distance from the user.
        transform.position = resultingPosition + (normVector * HowFarForward);
        // Have the tool look at the user so that its orientation always looks the same to the user.
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
    }
}
