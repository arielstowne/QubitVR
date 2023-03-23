using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Controls the wedge feature in gate animations
  *
  * Currently in use in section 2.
  * Attaches to section2_manager->section2_tutorial->Animated Qubit->Shell->unitVector->axisOfRotation->Origin->Line->Wedge
  */
public class WedgeParticleSystem : MonoBehaviour
{
    public GameObject Shell;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule em;
    private AnimateVector vector;
    private QubitTriggers triggers;

    void Start()
    {
        // For the wedge, we want the initial rotation of each particle to be local,
        // but then we want it to stay in that rotation- so it needs to stay in world alignment.
        ps = gameObject.GetComponent<ParticleSystem>();
        vector = Shell.GetComponent<AnimateVector>();
        triggers = Shell.GetComponent<QubitTriggers>();

        var main = ps.main;
        main.startRotation3D = true;
        // Unity doesn't offer what we need, so we need to change
        // the start rotation constantly while keeping world alignment.
        gameObject.GetComponent<ParticleSystemRenderer>().alignment = ParticleSystemRenderSpace.World;

        // Don't emit immediately, or the wedge doesn't look as good.
        em = ps.emission;
        em.enabled = false;
    }

    private void FixedUpdate()
    {
        // Makes the wedge cleaner and look better.
        if (vector.IsRotating)
            em.enabled = true;
        else
            em.enabled = false;

        // The wedge only disappears if the animated gate is not touching the qubit. This allows the
        // wedge to stay for a few moments after the vector completes its rotation before disappearing.
        if (triggers.TriggerObject == null)
        {
            ps.Clear();
            ps.Play();
        }

        // Always make the start rotation local to the Wedge object.
        var main = ps.main;
        main.startRotationX = transform.eulerAngles.x * Mathf.Deg2Rad;
        main.startRotationY = transform.eulerAngles.y * Mathf.Deg2Rad;
        main.startRotationZ = transform.eulerAngles.z * Mathf.Deg2Rad;
    }
}
