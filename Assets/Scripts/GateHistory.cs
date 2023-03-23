using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* In Progress/Depreciated: for showing gate history on in-game object */
public class GateHistory : MonoBehaviour
{
    public GameObject Qubit;
    public TextMeshProUGUI History;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
   /*     Vector3 newPosition = Qubit.transform.position - Camera.main.transform.position;
        newPosition = Vector3.Cross(Vector3.up, newPosition);
        newPosition = Vector3.Normalize(newPosition) * .7f;
        newPosition.y = gameObject.transform.position.y;
        gameObject.transform.position = newPosition;*/

        gameObject.transform.position = Camera.main.transform.position;
        gameObject.transform.RotateAround(Qubit.transform.position, Vector3.up, 90);

        gameObject.transform.LookAt(Camera.main.transform);
    }

    void UpdateGateHistoryDisplay(string nextGate)
    {
        History.text = History.text + "\n" + nextGate;
    }
}
