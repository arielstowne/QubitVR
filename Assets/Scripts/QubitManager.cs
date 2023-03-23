using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QubitMath;
using System;
using Random = UnityEngine.Random;

/** Manages qubits alongside a content manager (ModuleManager) */
public class QubitManager : MonoBehaviour
{

    public GameObject[] qubits;
    protected ApplyGate[] qubitScript;

    protected bool selectionEnabled { get; set; }

    /** Creates a qubit whose unit vector is pointing in a random direction.
    *
    * Will generate a qubit with vector pointing up in 10% of cases, down in 10% of cases, on the equator 20% of cases
    * and upper hemisphere in 30% of cases and lower hemisphere in 30% of cases.
    * !! NOTE !! The upper/lower hemispheres generate values that are relatively far away from the poles and the equator
    * so the learner doesn't get confused about what option they are supposed to pick.
    */
    public void SetRandomState(int n)
    {
      if (checkQubitExists(n))
      {
        // Here we specify the probabilities for each possible set of states, as stated above function.
        // Lower hemisphere chance is 100 - (upChance + downChance + equatorChance + upperHemisphereChance).
        int upChance = 10, downChance = 10, equatorChance = 20, upperHemisphereChance = 30;
        float choice = Random.Range(0, 100);

        // If random state is up, set state to up and update assessment answer to "up".
        if(choice < upChance)
        {
            qubitScript[n].setState(States.UP);
            qubitScript[n].assessmentAnswer = "up";
        }

        // If random state is down, set state to down and update assessment answer to "down".
        else if (choice < upChance + downChance)
        {
            qubitScript[n].setState(States.DOWN);
            qubitScript[n].assessmentAnswer = "down";
        }

        // If random state is along the equator, set state to that and update update assessment answer to "equator".
        else if (choice < upChance + downChance + equatorChance)
        {
            SetRandomEquatorQubit(qubits[n], qubitScript[n]);
            qubitScript[n].assessmentAnswer = "equator";
        }

        // If random state is likely up, set state to upper hemisphere and update update assessment answer to "likely_up".
        else if (choice < upChance + downChance + equatorChance + upperHemisphereChance)
        {
            SetRandomHemisphereQubit(qubits[n], qubitScript[n], true);
            qubitScript[n].assessmentAnswer = "likely_up";
        }

        // If random state is likely down, set state to lower hemisphere and update update assessment answer to "likely_down".
        else
        {
            SetRandomHemisphereQubit(qubits[n], qubitScript[n], false);
            qubitScript[n].assessmentAnswer = "likely_down";
        }

        Debug.Log("Set random state on qubit " + n + ". State is now " + qubitScript[n].assessmentAnswer + ".");
      }
    }

    /** Sets a qubit's state along the equator. */
    protected void SetRandomEquatorQubit(GameObject qubit, ApplyGate script)
    {
        float x = Random.Range(-1f, 1f);
        int sign = Random.Range(0, 2) == 1 ? 1 : -1;
        float z = (float)(sign * Math.Sqrt(1 - Math.Pow(x, 2)));

        script.PointVector(x, z, 0);
    }

    /** Sets a qubit's state in the upper or lower hemisphere (not including poles or equator). */
    protected void SetRandomHemisphereQubit(GameObject qubit, ApplyGate script, Boolean upper)
    {
        // Generate random x-coordinate.
        float x = Random.Range(-1f, 1f);

        // Generate z-coordinate based on x-coordinate such that
        // -(sqrt(1-x^2)) <= z <= +(sqrt(1-x^2))
        float zLimit = (float)Math.Sqrt(1 - Math.Pow(x, 2));
        float z = Random.Range(-1 * zLimit, zLimit);

        // Generate y-coordinate based on x and z-coordinates. If it's being created in the upper hemisphere,
        // its sign is +1, otherwise -1.
        int sign = upper == true ? 1 : -1;
        float y = (float)(Math.Sqrt(1 - Math.Pow(x, 2) - Math.Pow(z, 2))) * sign;

        // epsilon is making sure that y's value isn't too close to either of the up/down poles.
        // epsilon2 is making sure that y's value isn't too close to the equator.
        float epsilon = 0.1f, epsilon2 = 0.2f;

        // If they are, need to generate a new random value for x and re-calculate z and y coordinates
        // until they're not too close.
        while (1 - Math.Abs(y) < epsilon || Math.Abs(y) < epsilon2)
        {
            x = Random.Range(-1f, 1f);
            zLimit = (float)Math.Sqrt(1 - Math.Pow(x, 2));
            z = Random.Range(-1 * zLimit, zLimit);
            y = sign * (float)(Math.Sqrt(1 - Math.Pow(x, 2) - Math.Pow(z, 2)));
            /// Debug.Log(x + ", " + y + ", " + z);
        }

        // Point the qubit with the chosen x, y, z-coordinates.
        script.PointVector(x, z, y);
    }

    public virtual bool checkQubitExists(int n)
    {
      return (qubits.Length > n && qubits[n] != null);
    }

    /** Can use this to animate the vector manually (eg: make rotations/changes in states more obvious in tutorials) */
    public void setQubitState(int n, Matrix state, bool moveVectorSlowly = false, bool halfCirclePath = false, bool changeState = true)
    {
      if (checkQubitExists(n))
      {
        qubitScript[n].setState(state, halfCirclePath, moveVectorSlowly, changeState);
        Debug.Log("Altering state of qubit " + n);
      }
      else
        Debug.Log("ERROR: Qubit " + n + " does not exist. Cannot change state.");
    }

    public void setQubitLock(int n, bool val)
    {
        // Changed this to reset colors. Particularly after a qubit has been selected in an assessment.
        // if (checkQubitExists(n) && qubitScript[n].gateLock != val)
        if (checkQubitExists(n))
        {
            qubitScript[n].setGateLock(val);
            Debug.Log("Qubit " + n + " lock set to " + val + ".");
        }
    }

    public void toggleQubitLock(int n)
    {

      if (checkQubitExists(n))
      {
        qubitScript[n].setGateLock(!qubitScript[n].gateLock);
        Debug.Log("Qubit " + n + " lock set to " + qubitScript[n].gateLock + ".");
      }
    }

    public void enableQubit(int n)
    {
      if (checkQubitExists(n) && !qubits[n].activeSelf)
        qubits[n].SetActive(true);
    }

    public void enableQubit(int n, Matrix state)
    {
        // Some reason theres a bug when trying to do this manually
        enableQubit(n);
        setQubitState(n, state);
    }

    public void disableQubit(int n)
    {
      if (checkQubitExists(n))
        qubits[n].SetActive(false);
    }

    public virtual void lockAllQubits()
    {
        for (int i = 0; i < qubits.Length; i++)
            setQubitLock(i, true);
    }

    public void resetQubits()
    {
      lockAllQubits();
      for (int i = 0; i < qubits.Length; i++)
        qubits[i].SetActive(false);
    }

    public GameObject getQubit(int n)
    {
      return qubits[n];
    }

    public ApplyGate getScript(int n)
    {
      return qubitScript[n];
    }

    /** Start with all of the qubits as not being triggered by any objects (probably not necessary, but just in case) */
    public void unTriggerAllQubits()
    {
        for (int i = 0; i < qubits.Length; i++)
            qubits[i].transform.GetChild(0).gameObject.GetComponent<QubitTriggers>().TriggerObject = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        qubitScript = new ApplyGate[qubits.Length];
        denySelecting();
        for (int i = 0; i < qubits.Length; i++)
            qubitScript[i] = qubits[i].transform.GetChild(0).gameObject.GetComponent<ApplyGate>();
        resetQubits();
        unTriggerAllQubits();
    }

    /** Allow the user to select the qubits with the laser pointer. */
    public void allowSelecting()
    {
        selectionEnabled = true;
    }

    /** Do not allow the user to select the qubits with the laser pointer. */
    public void denySelecting()
    {
        selectionEnabled = false;
    }

    /** Check if the user is allowed to select the qubits with the laser pointer. */
    public bool checkSelectingPermissions()
    {
        return selectionEnabled;
    }
}
