//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates how to create a simple interactable object
//
//=============================================================================

using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem.Sample
{
	//-------------------------------------------------------------------------
	/** Controls VR interactions with gates (e.g. grabbing)
	*
	* Example file provided by Valve. Heavily modified for custom functionality.
	* Modifications include sound, gate alerts to ModuleManager, and grab validation.
	*/
	[RequireComponent(typeof(Interactable))]
	public class InteractiveObject : MonoBehaviour
	{
		public TextMeshProUGUI toolText;
		public GameObject ModuleManager;

		// SteamVR probably has an indicator like this, but this is simple and easily accessable...
		public bool AttachedToHand { get; set; }

		private Interactable interactable;
		private Vector3 oldPosition;
		private Quaternion oldRotation;
		private GameObject qubitTriggered = null;

		private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);

		public AudioSource audioSource;
		public AudioClip gatePickup, gateApplied;

		//-------------------------------------------------
		void Awake()
		{
			toolText.enabled = false;

			interactable = this.GetComponent<Interactable>();
			AttachedToHand = false;
		}


		//-------------------------------------------------
		// Called when a Hand starts hovering over this object
		//-------------------------------------------------
		private void OnHandHoverBegin(Hand hand)
		{
			// Turn on the label of the tools name.
			toolText.enabled = true;
		}


		//-------------------------------------------------
		// Called when a Hand stops hovering over this object
		//-------------------------------------------------
		private void OnHandHoverEnd(Hand hand)
		{
			// Turn off the label of the tools name.
			toolText.enabled = false;
		}


		//-------------------------------------------------
		// Called every Update() while a Hand is hovering over this object
		//-------------------------------------------------
		private void HandHoverUpdate(Hand hand)
		{
			GrabTypes startingGrabType = hand.GetGrabStarting();
			bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

			// User can only grab the object with the grip button.
			// This was chosen because the trigger button triggers the laserpointer and causes unpleasant flashes when grabbing objects.
			if (interactable.attachedToHand == null && startingGrabType == GrabTypes.Grip) // startingGrabType != GrabTypes.None
			{
				// Save our position/rotation so that we can restore it when we detach
				oldPosition = transform.localPosition;
				oldRotation = transform.localRotation;

				// Rotate the object so it aligns with the hand before it attaches.
				transform.forward = hand.transform.forward;

				// Call this to continue receiving HandHoverUpdate messages,
				// and prevent the hand from hovering over anything else
				hand.HoverLock(interactable);

				// Attach this object to the hand
				hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
			}
			else if (isGrabEnding)
			{
				// If the gate gameObject is in contact with a qubit when the user lets go of it, apply the gate.
				if (qubitTriggered != null)
                {
					// Applying gates still works the way it is currently, so this isn't a bug but more of a design choice:
					// If you don't like that a gate can still be applied while the preview/animation for another gate 'plays',
					// (applying the other gate just cuts the animation off) then use this in an if statement:
					// GameObject.ReferenceEquals(gameObject, qubitTriggered.GetComponent<QubitTriggers>().TriggerObject)

					// Move the vector immediately and set the state.
					qubitTriggered.GetComponent<ApplyGate>().ToolsInBounds(gameObject);
					// We don't want the vector to return to the rotation it had before the gate entered the qubit.
					// This would happen because QubitTriggers resets the rotation in onTriggerExit().
					qubitTriggered.GetComponent<QubitTriggers>().ResetRotation = false;

					// If is a gate, play gate applied sound.
          if (!checkGateType().Equals("flashlightStatus"))
          {
						audioSource.clip = gateApplied;
						audioSource.Play();
					}
				}

				// Detach this object from the hand
				hand.DetachObject(gameObject);
				// Call this to undo HoverLock
				hand.HoverUnlock(interactable);

				// Restore position/rotation
				transform.localPosition = oldPosition;
				transform.localRotation = oldRotation;
			}
		}


		//-------------------------------------------------
		// Called when this GameObject becomes attached to the hand
		//-------------------------------------------------
		private void OnAttachedToHand(Hand hand)
		{
			// Turn off the label of the tools name.
			toolText.enabled = false;
			// Tell ModuleManager which tool is being held for the tutorials.
			ModuleManager.BroadcastMessage(checkGateType(), true);
			AttachedToHand = true;

			if (!checkGateType().Equals("flashlightStatus"))
			{
				audioSource.clip = gatePickup;
				audioSource.Play(0);
			}

			// This turns off the laserpointer while the object is held by the right hand
			// (We only have the laser pointer on the right hand)
			if (hand.handType == SteamVR_Input_Sources.RightHand)
				EventManager.PickedUpItem.Invoke();
		}



		//-------------------------------------------------
		// Called when this GameObject is detached from the hand
		//-------------------------------------------------
		private void OnDetachedFromHand(Hand hand)
		{
			// Tell ModuleManager that the tool is not being held anymore.
			ModuleManager.BroadcastMessage(checkGateType(), false);
			AttachedToHand = false;
			// This turns on the laserpointer because the object is no longer held
			EventManager.DroppedItem.Invoke();
		}

		/** Checks and returns gate type to be sent to Module Managers in scene */
		private string checkGateType()
		{
			if (gameObject.CompareTag("Flashlight"))
				return "flashlightStatus";
			else if (gameObject.CompareTag("H"))
				return "hStatus";
			else if (gameObject.CompareTag("NOT"))
				return "notStatus";
			else if (gameObject.CompareTag("T"))
				return "tStatus";
			else if (gameObject.CompareTag("S"))
				return "sStatus";
			else
				return "unknownStatus";
		}

		private bool lastHovering = false;
		private void Update()
		{
			if (interactable.isHovering != lastHovering) //save on the .tostrings a bit
				lastHovering = interactable.isHovering;
		}

		public void OnTriggerEnter(Collider other)
		{
			// Keep track of when the tool is in a qubit to know if we need to apply it to the qubit.
			if (other.tag == "Qubit")
				qubitTriggered = other.gameObject;
		}

		public void OnTriggerExit(Collider other)
		{
			qubitTriggered = null;
		}

	}
}

/*
		//-------------------------------------------------
		// Called every Update() while this GameObject is attached to the hand
		//-------------------------------------------------
		private void HandAttachedUpdate(Hand hand)
		{
		}

		//-------------------------------------------------
		// Called when this attached GameObject becomes the primary attached object
		//-------------------------------------------------
		private void OnHandFocusAcquired(Hand hand)
		{
		}


		//-------------------------------------------------
		// Called when another attached GameObject becomes the primary attached object
		//-------------------------------------------------
		private void OnHandFocusLost(Hand hand)
		{
		}
*/
