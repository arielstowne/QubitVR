using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Controls animated gate movements for the section 2 tutorial.
*
* Controls Section 2's tutorial sections in which gates are animated to collide with qubits.
* Attached to section2_manager->section2_tutorial->Animated Gates->Placeholders->S2, T2.
* Ensure the Animated Gates object is in the same location as the qubit.
*/
public class Section2_Animations : MonoBehaviour
{
    /** Attach the shell of the qubit being used for the animations */
    public GameObject QubitShell;

    /** Attach the Animated Gates->Functional gate that corresponds to this Placeholders gate */
    public GameObject functionalGate;

    /** Applies the gate being moved into the qubit
    *
    * Gates are not normally applied unless they are in a users hand. This is used to bypass that.
    */
    private void OnTriggerEnter(Collider other)
    {
        ApplyGate applyGate = null;
        QubitTriggers triggers = null;

        // If the gate triggers the qubit specified for animations, then get the necessary scripts.
        if (GameObject.ReferenceEquals(other.gameObject, QubitShell))
        {
            applyGate = other.gameObject?.GetComponent<ApplyGate>();
            triggers = other.gameObject?.GetComponent<QubitTriggers>();
        }

        // This will trigger a slow animated vector rotation that
        // will also set the state of the qubit to that of the new rotation.
        if(applyGate != null && triggers != null && functionalGate != null)
            applyGate.ToolsInBounds(functionalGate, true, true);
    }
}
