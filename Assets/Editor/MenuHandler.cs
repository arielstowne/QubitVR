using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Concepts;

// This was a dynamic system for changing the text of the main menus based on the module and concept that the user is in.

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private List<MenuFiller> menus = new List<MenuFiller>();
    public void UpdateMenus(ModuleState CurrentModule, ConceptState CurrentConcept)
    {
        MenuFiller.CurrentModule = CurrentModule;
        MenuFiller.CurrentConcept = CurrentConcept;
        if (menus != null) {
            //MethodInfo MenuFunction = ConceptStateExtensions.MenuFunction(CurrentConcept);
            // Call the same function on all menus, because they all implement the MenuFiller abstract class
            foreach (MenuFiller menu in menus)
            {
                //MenuFunction.Invoke(menu, null);
                ConceptStateMethods.MenuFunction(menu, CurrentConcept);
            }
        }
    }

    public GameObject ActiveMenu;

    public void ChangeActiveMenu(GameObject menu)
    {
        ActiveMenu = menu;
    }

    // Update is called once per frame
    void Update()
    {
        // The menu will always face the user as they walk around.
        // There is a tilt, depending on height,
        // which can be changed if necessary (reference Toolbox script)
        ActiveMenu.transform.LookAt(Camera.main.transform.position);
    }
}

// Base class
public abstract class MenuFiller : MonoBehaviour
{
    public static ModuleState CurrentModule { get; set; }
    public static ConceptState CurrentConcept { get; set; }

    static MenuFiller()
    {
        CurrentModule = ModuleState.SingleQubit;
        CurrentConcept = ConceptState.Superposition;
    }

    public abstract void Superposition();
    public abstract void Measurement();
    public abstract void Operators();
    public abstract void Noise();
}
