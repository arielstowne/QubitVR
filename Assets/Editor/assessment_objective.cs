using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Concepts;

public class assessment_objective : MenuFiller
{
    public TextMeshProUGUI objective;
    public string Objective
    {
        set { objective.text = value; }
    }

    public override void Superposition()
    {
    }

    public override void Measurement()
    {
    }

    public override void Operators()
    {
    }

    public override void Noise()
    {
    }
}
