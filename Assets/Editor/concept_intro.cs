using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Concepts;

public class concept_intro : MenuFiller
{
    public TextMeshProUGUI module_name;
    public string Module_name
    {
        set { module_name.text = value; }
    }

    public TextMeshProUGUI concept_name;
    public string Concept_name
    {
        set { concept_name.text = value; }
    }

    public TextMeshProUGUI intro;
    public string Intro
    {
        set { intro.text = value; }
    }

    public override void Superposition()
    {
        AllConcepts();
        Intro = "Something about Superposition";
    }

    public override void Measurement()
    {
        AllConcepts();
        Intro = "Something about Measurement";
    }

    public override void Operators()
    {
        AllConcepts();
        Intro = "Something about Operators";
    }

    public override void Noise()
    {
        AllConcepts();
        Intro = "Something about Noise";
    }

    public void AllConcepts()
    {
        Module_name = ModuleStateExtensions.ToFriendlyString(CurrentModule);
        Concept_name = ConceptStateExtensions.ToFriendlyString(CurrentConcept);
    }
}
