/* SceneHandler.cs*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

/** This is used to trigger events associated with the laser pointer.
  *
  * The laser pointer can be used on objects by either putting if statements in this script that checks e.target.name or tag
  * or by writing a script for the object that implements OnPointerClick, OnPointerEnter, OnPointerExit (see Qubit_Handler for example).
  * Attaches to Player object.
  */
public class SceneHandler : MonoBehaviour
{
    /** Attach Player->SteamVRObjects->RightHand */
    public SteamVR_LaserPointer laserPointer;
    public AudioSource clickSound;

    void Awake()
    {
        // Interprets where steam_laserpointer is pointing.
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        // Triggers OnClicks functions
        IPointerClickHandler clickHandler = e.target.GetComponent<IPointerClickHandler>();
        if (clickHandler == null)
        {
            return;
        }
        clickSound.Play();
        Debug.Log("playing audiosource " + clickSound);
        clickHandler.OnPointerClick(new PointerEventData(EventSystem.current));

    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        IPointerEnterHandler pointerEnterHandler = e.target.GetComponent<IPointerEnterHandler>();
        if (pointerEnterHandler == null)
        {
            return;
        }
        pointerEnterHandler.OnPointerEnter(new PointerEventData(EventSystem.current));
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        IPointerExitHandler pointerExitHandler = e.target.GetComponent<IPointerExitHandler>();
        if (pointerExitHandler == null)
        {
            return;
        }
        pointerExitHandler.OnPointerExit(new PointerEventData(EventSystem.current));
    }
}
