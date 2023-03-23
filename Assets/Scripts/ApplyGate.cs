using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QubitMath; // Contains definitions for Matrix and Complex.

/** Primary controller for qubit engine.
  *
  * Controls math and vector rotations and creates and stores History.
  * Stores current qubit state and converts into xyx coord for vector.
  * Stores matrices for gates.
  * Note that this class alerts ModuleManager when a qubit is measured.
  * Attached to qubit shell game object.
  */
public class ApplyGate : MonoBehaviour
{
    /** When true, user cannot apply gates or measure */
    public bool gateLock;

    ///@{
    /** Colors when locked/unlocked */
    public Material locked;
    public Material unlocked;
    ///@}

    /** Holds current state of unit vector.
    *
    * See QubitMath for more information
    */
    Matrix state;

    /** Holds history of gate applications.
    *
    * See QubitMath for more information
    */
    History history;

    ///@{
    /** Gate definitions to be used throughout entire script. */
    Matrix PAULIX = new Matrix(new Complex[2, 2] { { new Complex(0, 0), new Complex(1, 0) }, { new Complex(1, 0), new Complex(0, 0) } });
    Matrix PAULIY = new Matrix(new Complex[2, 2] { { new Complex(0, 0), new Complex(0, -1) }, { new Complex(0, 1), new Complex(0, 0) } });
    Matrix PAULIZ = new Matrix(new Complex[2, 2] { { new Complex(1, 0), new Complex(0, 0) }, { new Complex(0, 0), new Complex(-1, 0) } });
    Matrix S = new Matrix(new Complex[2, 2] { { new Complex(1, 0), new Complex(0, 0) }, { new Complex(0, 0), new Complex(0, 1) } });
    Matrix H = new Matrix(new Complex[2, 2] {{new Complex(1 / (float)Math.Sqrt(2), 0), new Complex(1 / (float)Math.Sqrt(2), 0)},
                                        {new Complex(1 / (float)Math.Sqrt(2), 0), new Complex(-1 / (float)Math.Sqrt(2), 0)}});
    Matrix T = new Matrix(new Complex[2, 2] {{new Complex(1, 0), new Complex(0, 0)},
                                        {new Complex(0, 0), new Complex(1 / (float)Math.Sqrt(2), 1 / (float)Math.Sqrt(2))}});
    ///@}

    /** String that holds the answer to the assessment "what state is the qubit most likely in?" in section 1 assessment
    *
    * Options are up, down, equator, likely_up, and likely_down.
    */
    public string assessmentAnswer = "";

    public void setGateLock(bool val)
    {
        gateLock = val;

        if (gameObject.GetComponent<Renderer>().material != null)
        {
            if (gateLock)
                gameObject.GetComponent<Renderer>().material = locked;
            else
                gameObject.GetComponent<Renderer>().material = unlocked;
        }
    }

    /** Takes in a state and returns the desired component of the related vector
    *
    * @param state the matrix of complex numbers
    * @param the desired component of the vector, x, y, or z
    * @return the resulting coordinate value
    */
    public float GetCoordinate(Matrix state, char coordType)
    {
        Matrix result = null;

        if (coordType == 'x')
            result = state.ConjugateTranspose().Multiply(PAULIX.Multiply(state));
        else if (coordType == 'y')
            result = state.ConjugateTranspose().Multiply(PAULIY.Multiply(state));
        else if (coordType == 'z')
            result = state.ConjugateTranspose().Multiply(PAULIZ.Multiply(state));

        return result == null ? -1 : result.GetSingleRealIntVal();
    }

    /** Uses probability to collapse qubit state.
    *
    * Called when the flashlight is detected and the lock if false.
    */
    private void PerformMeasurement()
    {
        // Calculate probability of collapsing to state |0> with P(|0>) = magnitude(alpha)^2
        Complex alpha = state.matrix[0, 0];
        Complex beta = state.matrix[1, 0];
        float probability = (alpha.a * alpha.a) + (alpha.b * alpha.b);
        float randomNumber = UnityEngine.Random.Range(0f, 1f);

        // If event occurs, qubit collapses to state |0>, else state |1>.
        if (randomNumber < probability)
        {
            alpha.a = 1;
            alpha.b = 0;
            beta.a = 0;
            beta.b = 0;
            PointVector(0, 0, 1); // xyz in math language (not unity)
            //setState(new Matrix(2, 1) {new Complex(1, 0), new Complex(0, 0)});
        }
        else
        {
            alpha.a = 0;
            alpha.b = 0;
            beta.a = 1;
            beta.b = 0;
            PointVector(0, 0, -1); // xyz in math language (not unity)
            //setState(new Matrix(2, 1) {new Complex(0, 0), new Complex(1, 0)});
        }
        Debug.Log("Prob: " + probability + "\nRand: " + randomNumber);
    }

    /** Point the unit vector in the desired direction
    *
    * @param moveVectorSlowly if true, vector will animate. if false, vector snaps to localPosition
    * @param halfCirclePath used for certain gate rotation animations to rotate along correct axis
    * return the new vector
    */
    public Vector3 PointVector(float x, float y, float z, bool moveVectorSlowly = false, bool halfCirclePath = false)
    {
        // Acquire the unitVector from current transforms child, the current position of the qubit,
        // and then the new target given by (x, y, z).
        GameObject unitVector = this.gameObject.transform.GetChild(0).gameObject;
        Vector3 qubitPosition = this.gameObject.transform.parent.gameObject.transform.position;
        Vector3 target = new Vector3(x, z, y); // z and y are switched in unity and traditional mathematics.

        // Stop moving the vector if user released the gate in the qubit while the vector was still animating.
        gameObject.GetComponent<AnimateVector>().IsRotating = false;

        // moveVectorSlowly indicates if the caller wanted the animated vector rotation, or an immediate turn.
        if (!moveVectorSlowly)
            // Immediately turn the unit vector to the new position.
            unitVector.transform.LookAt(qubitPosition + target);
        else
            // Turn the vector to the new position at a slow speed was specified.
            StartCoroutine(gameObject.GetComponent<AnimateVector>().rotateOverTime(qubitPosition + target, halfCirclePath));

        return (qubitPosition + target);
    }

    /** Set the state and initiate the vector movement
    * @param moveVectorSlowly if true, vector will animate. if false, vector snaps to localPosition
    * @param halfCirclePath used for certain gate rotation animations to rotate along correct axis
    * @param changeState if false, the state of the qubit is not saved
    * return the new vector
    */
    public Vector3 setState(Matrix state, bool moveVectorSlowly = false, bool halfCirclePath = false, bool changeState = true)
    {
        // Set class variable 'state'
        if (changeState)
            this.state = state;

        // Get the x-y-z coordinates from alpha beta values.
        float x = GetCoordinate(state, 'x');
        float y = GetCoordinate(state, 'y');
        float z = GetCoordinate(state, 'z');

        // Function to physically move the unit vector to point at (x, y, z).
        return PointVector(x, y, z, halfCirclePath, moveVectorSlowly);
    }

    /** Detects a gate collision and applies the relavant state changes.
    *
    * The optional parameters are used for animating the vector, either for the preview of gate rotations or
    * for slow rotations to make a change of state more obvious in tutorials (see Section 1 tutorial scene).
    */
    public Vector3 ToolsInBounds(GameObject other, bool moveVectorSlowly = false, bool changeState = true)
    {
        Matrix gate;
        GameObject moduleManager = transform.root.gameObject;
        Vector3 currentVectorPosition = transform.GetChild(0).gameObject.transform.position;

        if (gateLock)
        {
            Debug.Log("Qubit is locked, cannot apply gates.");
            return currentVectorPosition;
        }
        else if (other == null)
        {
            Debug.Log("No tool was provided to ToolsInBounds.");
            return currentVectorPosition;
        }

        // The NOT and H gates have half circle rotations, just change it for S and T.
        bool halfCirclePath = true;
        // Apply appropriate gate based on the tag of the incoming collision.
        if (other.CompareTag("NOT")) // NOT Gate
            gate = PAULIX;
        else if (other.CompareTag("H")) // Hadamard Gate
            gate = H;
        else if (other.CompareTag("S")) // Half-Turn Phase Shift Gate
        {
            gate = S;
            halfCirclePath = false;
        }
        else if (other.CompareTag("T")) // Quarter-Turn Phase Shift Gate
        {
            gate = T;
            halfCirclePath = false;
        }
        else if (other.CompareTag("Measure"))
        {
            PerformMeasurement();
            gameObject.GetComponent<QubitTriggers>().ResetRotation = false;
            //history.Add(other.tag, state, transform);
            moduleManager.BroadcastMessage("performMeasurement");
            return currentVectorPosition;
        }
        else
        {
            Debug.Log("Invalid Gate");
            return currentVectorPosition;
        }
        // Apply gate to the qubit state.
        Matrix tempState = gate.Multiply(state);
        Debug.Log(tempState.ToString());

        // Slightly corrects the value of alpha and beta if necessary.
        tempState.matrix[0, 0].SmartRound();
        tempState.matrix[1, 0].SmartRound();

        if (changeState)
        {
            // Add the state and gate applied to the qubit
            history.Add(other.tag, tempState, transform);
            history.Print();
        }

        //moduleManager.transform.GetComponent<Module1Manager>().setGate(history);
        // Set the state and point the unitVector to represent 'state'.
        return setState(tempState, halfCirclePath, moveVectorSlowly, changeState);
    }

    void Start()
    {
        history = new History();
    }
}
