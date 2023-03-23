using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Concepts;

public class congratulations : MenuFiller
{
    public TextMeshProUGUI description;
    public string Description
    {
        set { description.text = value; }
    }

    public override void Superposition()
    {
        AllConcepts();
    }

    public override void Measurement()
    {
        AllConcepts();
    }

    public override void Operators()
    {
        AllConcepts();
    }

    public override void Noise()
    {
        AllConcepts();
    }

    public void AllConcepts()
    {
        Description = "You have completed the " + ConceptStateExtensions.ToFriendlyString(CurrentConcept) + " section.";
    }
}
