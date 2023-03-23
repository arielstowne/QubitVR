using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Concepts;

public class assessment_difficulty : MenuFiller
{
    public TextMeshProUGUI title;
    public string Title
    {
        set { title.text = value; }
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
        Title = "You completed the " + ConceptStateExtensions.ToFriendlyString(CurrentConcept) + " tutorial";
    }
}
