using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using Valve.VR.InteractionSystem.Sample;

/** Controls flashlight beam and controller mappings
*/
public class Flashlight : MonoBehaviour
{
	public Light spotLight;
	public Collider cone;

	public SteamVR_Action_Boolean flashlightButton;
	public SteamVR_Input_Sources handType;

	public AudioSource audioSource;
	public AudioClip flashlightNoise;

	void Start()
	{
		if (spotLight != null && cone != null)
		{
			spotLight.enabled = false;
			cone.enabled = false;
		}
	}

	///@{
	/** Enable these when using a VR headset to allow button mappings to work on the flashlight */
	/* Begin VR-only functions */
	void Awake()
	{
		flashlightButton.AddOnStateUpListener(AButtonToggle, handType);
		flashlightButton.AddOnStateDownListener(AButtonToggle, handType);
	}

	private void OnDestroy()
	{
		// This is necessary because the event needs to be aware of only valid event listeners with active game objects
		flashlightButton.RemoveOnStateDownListener(AButtonToggle, handType);
		flashlightButton.RemoveOnStateUpListener(AButtonToggle, handType);
	}

	// The A button on the right controller toggles the flashlight on and off.
	public void AButtonToggle(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
	{
		// Only let the user turn the flashlight on and off if they are holding it.
		if (!gameObject.GetComponent<InteractiveObject>().AttachedToHand)
			return;

		if (spotLight == null || cone == null)
			return;

		audioSource.clip = flashlightNoise;
		audioSource.Play();

		// Toggle the light and its triggering capabilities.
		spotLight.enabled = !spotLight.enabled;
		cone.enabled = !cone.enabled;
	}

	// Keep the flashlight off when the user isn't holding it.
	private void OnDetachedFromHand(Hand hand)
	{
		if (spotLight == null || cone == null)
			return;

		spotLight.enabled = false;
			cone.enabled = false;
	}
	/* End VR-only */
	///@}

	///@{
	/** Enable these to use SteamVR's simulation mode */
	/* Begin non-VR only
	 
	private void OnAttachedToHand(Hand hand)
	{
		if (spotLight != null)
		{
			spotLight.GetComponent<Light>().enabled = true;
			cone = spotLight.transform.GetChild(0).gameObject.GetComponent<Collider>();
		}

		audioSource.clip = flashlightNoise;
		audioSource.Play();

		if (cone != null)
			cone.enabled = true;
	}

	private void OnDetachedFromHand(Hand hand)
	{
		if (spotLight != null)
		{
			spotLight.GetComponent<Light>().enabled = false;
			cone = spotLight.transform.GetChild(0).gameObject.GetComponent<Collider>();
		}

		if (cone != null)
			cone.enabled = false;
	}
	/* End non-VR only	*/
	///@}
}
