using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QubitMath;
using System;
using Random = UnityEngine.Random;

/** Depreciated class used by DynamicModuleManagers for dynamically running a tutorial. */
public class DynamicQubitManager : QubitManager
{
    public static int MAX_QUBITS = 5;
    public int qubitCount;
    public GameObject qubitPREFAB; // Must assign in Unity: the qubit prefab

    public void createQubit(Vector3 location, Matrix state)
    {
      if (qubitCount < MAX_QUBITS)
      {
        // Create a qubit with the given (x,y,z) location and (x,y,z) state.
        qubits[qubitCount] = Instantiate<GameObject>(qubitPREFAB, location, qubitPREFAB.transform.rotation, transform);

        // Find the script attached to the qubit and use it to set the qubits state.
        qubitScript[qubitCount] = qubits[qubitCount].transform.GetChild(0).gameObject.GetComponent<ApplyGate>();
        qubitScript[qubitCount].setState(state);
        qubitCount++;
        Debug.Log("Generating qubit. " + qubitCount + " qubits now exist.");
      }
      else
        Debug.Log("ERROR: Cannot generate additional qubits. Maximum reached.");
    }

    // Creates a qubit whose unit vector is pointing in a random direction.
    // Will generate a qubit facing up in 10% of cases, down in 10% of cases, on the equator 20% of cases
    // and upper hemisphere in 30% of cases and lower hemisphere in 30% of cases.
    // !! NOTE !! The upper/lower hemispheres generate values that are relatively far away from the poles and the equator
    // so the learner doesn't get confused about what option they are supposed to pick.
    public void CreateRandomQubit(Vector3 location)
    {
      if (qubitCount < MAX_QUBITS)
      {
        // Instantiate qubit.
        qubits[qubitCount] = Instantiate<GameObject>(qubitPREFAB, location, qubitPREFAB.transform.rotation, transform);

        // Get the qubit's script so its state can be changed later on.
        qubitScript[qubitCount] = qubits[qubitCount].transform.GetChild(0).gameObject.GetComponent<ApplyGate>();

        // Here we specify the probabilities for each possible set of states, as stated above function.
        // Lower hemisphere chance is 100 - (upChance + downChance + equatorChance + upperHemisphereChance).
        int upChance = 10, downChance = 10, equatorChance = 20, upperHemisphereChance = 30;
        float choice = Random.Range(0, 100);

        // If random state is up, set state to up and update assessment answer to "up".
        if(choice < upChance)
        {
            qubitScript[qubitCount].setState(States.UP);
            qubitScript[qubitCount].assessmentAnswer = "up";
        }

        // If random state is down, set state to down and update assessment answer to "down".
        else if (choice < upChance + downChance)
        {
            qubitScript[qubitCount].setState(States.DOWN);
            qubitScript[qubitCount].assessmentAnswer = "down";
        }

        // If random state is along the equator, set state to that and update update assessment answer to "equator".
        else if (choice < upChance + downChance + equatorChance)
        {
            SetRandomEquatorQubit(qubits[qubitCount], qubitScript[qubitCount]);
            qubitScript[qubitCount].assessmentAnswer = "equator";
        }

        // If random state is likely up, set state to upper hemisphere and update update assessment answer to "likely_up".
        else if (choice < upChance + downChance + equatorChance + upperHemisphereChance)
        {
            SetRandomHemisphereQubit(qubits[qubitCount], qubitScript[qubitCount], true);
            qubitScript[qubitCount].assessmentAnswer = "likely_up";
        }

        // If random state is likely down, set state to lower hemisphere and update update assessment answer to "likely_down".
        else
        {
            SetRandomHemisphereQubit(qubits[qubitCount], qubitScript[qubitCount], false);
            qubitScript[qubitCount].assessmentAnswer = "likely_down";
        }

        qubitCount++;
        Debug.Log("Generating random qubit. " + qubitCount + " qubits now exist.");
      }
    }


    public void destroyQubit(int n)
    {
      if (checkQubitExists(n))
      {
        Destroy(qubits[n]);
        qubits[n] = null;
        qubitScript[n] = null;
        qubitCount--;
        Debug.Log("Destroying qubit. " + qubitCount + " qubits now exist.");
      }
      else
        Debug.Log("Qubit "+n+" does not exist so cannot destroy.");
    }

    public override bool checkQubitExists(int n)
    {
      return (qubitCount >= n && qubits[n] != null);
    }


    public override void lockAllQubits()
    {
      for (int i = 0; i < qubitCount; i++)
      {
        setQubitLock(i, true);
      }
    }

    // Start is called before the first frame update
    void Start()
    {
      qubits = new GameObject[MAX_QUBITS];        // Instatiated qubits
      qubitScript = new ApplyGate[MAX_QUBITS];    // Used to manipulate individual qubits
      qubitCount = 0;                             // Counter for number of qubits
    }

    // Update is called once per frame
    void Update()
    {

    }
}
