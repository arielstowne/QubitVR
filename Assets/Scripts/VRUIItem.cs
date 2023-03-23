using UnityEngine;

/** Attaches to buttons to allow Laser Pointer clicks.
  *
  * Attach this to any buttons to make them clickable in VR.
  */
[RequireComponent(typeof(RectTransform))]
public class VRUIItem : MonoBehaviour
{
    private BoxCollider boxCollider;
    private RectTransform rectTransform;

    // This function is called when the object becomes enabled and active.
    private void OnEnable()
    {
        ValidateCollider();
    }

    // This function is called when the script is loaded or a value is changed in the Inspector.
    private void OnValidate()
    {
        ValidateCollider();
    }

    // This automatically adds a boxcollider of the correct size to the gameObject.
    // Usually used for buttons/UI
    private void ValidateCollider()
    {
        rectTransform = GetComponent<RectTransform>();

        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }
        // The gameObject needs a Width and Height for the box collider to be added.
        boxCollider.size = rectTransform.sizeDelta;
    }
}
