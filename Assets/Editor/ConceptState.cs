using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Concepts
{
    public enum ModuleState
    {
        SingleQubit,
        TwoQubit,
        Algorithms
    }

    public enum ConceptState
    {
        Superposition,
        Measurement,
        Operators,
        Noise
    }

    public static class ModuleStateExtensions
    {
        // Was going to use Description Attribute (ex: [Description("Quantum Noise")] ),
        // but the reflection might be more expensivethan a switch statement 
        public static string ToFriendlyString(this ModuleState module)
        {
            switch (module)
            {
                case ModuleState.SingleQubit:
                    return "Single Qubit";
                case ModuleState.TwoQubit:
                    return "Two Qubit";
                case ModuleState.Algorithms:
                    return "Grover's Algorithm";
                default:
                    return "No Module";
            }
        }
    }

        public static class ConceptStateExtensions
    {
        // Was going to use [Description("Quantum Noise")]
        public static string ToFriendlyString(this ConceptState concept)
        {
            switch (concept)
            {
                case ConceptState.Superposition:
                    return "Superposition";
                case ConceptState.Measurement:
                    return "Measurement";
                case ConceptState.Operators:
                    return "Unitary Operators";
                case ConceptState.Noise:
                    return "Quantum Noise";
                default:
                    return "No Concept";
            }
        }
        /*
        // Was originally going to use this to avoid switch statements for each menu, every time the concept changes
        // but reflection might be more expensive
        public static MethodInfo MenuFunction(ConceptState concept)
        {
            switch (concept)
            {
                case ConceptState.Superposition:
                    return typeof(MenuFiller).GetMethod("Superposition");
                case ConceptState.Measurement:
                    return typeof(MenuFiller).GetMethod("Measurement");
                case ConceptState.Operators:
                    return typeof(MenuFiller).GetMethod("Operators");
                case ConceptState.Noise:
                    return typeof(MenuFiller).GetMethod("Noise");
                default:
                    return null;
            }
        }
        */
    }

    public static class ConceptStateMethods
    {
        public static void MenuFunction(this MenuFiller menu, ConceptState concept)
        {
            switch (concept)
            {
                case ConceptState.Superposition:
                    menu.Superposition();
                    break;
                case ConceptState.Measurement:
                    menu.Measurement();
                    break;
                case ConceptState.Operators:
                    menu.Operators();
                    break;
                case ConceptState.Noise:
                    menu.Noise();
                    break;
                default:
                    Debug.LogWarning("Menu function for " + concept.ToString() + " not called.");
                    break;
            }
        }
    }
}