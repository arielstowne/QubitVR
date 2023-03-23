using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Concepts;

public class concepts_screen : MenuFiller
{
    public TextMeshProUGUI concept1;
    public string Concept1
    {
        set { concept1.text = value; }
    }

    public TextMeshProUGUI concept2;
    public string Concept2
    {
        set { concept2.text = value; }
    }

    public TextMeshProUGUI concept3;
    public string Concept3
    {
        set { concept3.text = value; }
    }

    public TextMeshProUGUI concept4;
    public string Concept4
    {
        set { concept4.text = value; }
    }

    public TextMeshProUGUI descriptions;
    public string Descriptions
    {
        set {  descriptions.text = value; }
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
        switch(MenuFiller.CurrentModule)
        {
            case ModuleState.SingleQubit:
                Concept1 = ConceptState.Superposition.ToString();
                Concept2 = ConceptState.Measurement.ToString();
                Concept3 = ConceptState.Operators.ToString();
                Concept4 = ConceptState.Noise.ToString();
                Descriptions = "Manipulate the states of qubits\n\n" +
                               "Manipulate and measure qubits\n\n" +
                               "Apply gates to qubits\n\n" +
                               "Apply noise to qubits and gates and measure qubits";
                break;
            case ModuleState.TwoQubit:
                break;
            case ModuleState.Algorithms:
                break;
            default:
                break;
        }
        
    }
}
