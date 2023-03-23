using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Stores colors for qubits.
  *
  * Contains a list of colors and stores the various shades of each color.
  * Used to retrieve a shade of a specific color using another shade of that color.
  * In use in Module 1 Section 1.
  * Attached to Section 1 Gameobject
  */
public class Color_Coordinator : MonoBehaviour
{
    /** Custom class for defining a color theme
    *
    * Currently has the original shade of a color, and a lighter hightlight shade of that color.
    */
    [System.Serializable]
    public class Color
    {
        public Material original;
        public Material highlight;
    }

    /** Public array of structs where each index stores different shades of a color.
    *
    * Adding colors can be done by referencing them using this variable in the Editor.
    */
    public Color[] qubitColors;

    /** Called when you want the original shade that corresponds to a given material.
    *
    * @param shellRenderer is the renderer component of the gameobject that you want to find the original shade for.
    * @return will either be the current material of the gameobject or the original shade of the color that corresponds to the current material.
    */
    public Material findQubitOriginal(Renderer shellRenderer)
    {
        Color color = searchAll(shellRenderer.sharedMaterial);
        if (color != null)
            return color.original;
        else
            return shellRenderer.sharedMaterial;
    }

    /** Called when you want the highlight shade that corresponds to a given material.
    *
    * @param shellRenderer is the renderer component of the gameobject that you want to find the highlight shade for.
    * @return will either be the current material of the gameobject or the highlight shade of the color that corresponds to the current material.
    */
    public Material findQubitHighlight(Renderer shellRenderer)
    {
        Color color = searchAll(shellRenderer.sharedMaterial);
        if (color != null)
            return color.highlight;
        else
            return shellRenderer.sharedMaterial;

    }

    /** Check the array of qubitColors for the given color.
    *
    * searchAll is a helper function called by findQubitOriginal() and findQubitHighlight().
    * @param shade is the current material of the qubit, and will be compared to all qubitColors[] to find a match.
    * @return will be null if no match was found, otherwise it will be a reference to the color.
    */
    private Color searchAll(Material shade)
    {
        foreach(Color color in qubitColors)
        {
            if (checkMatch(shade, color))
                return color;
        }
        return null;
    }

    /** Check if the color has the given shade.
    *
    * Simply compares the current material of the qubit to each shade of a specific color in qubitColors[].
    * @param shade
    * @param color
    * @return boolean if shade matches color.original or color.highlight
    */
    private bool checkMatch(Material shade, Color color)
    {
        if(shade == color.original || shade == color.highlight)
            return true;
        return false;
    }
}
