// /* SceneHandler.cs*/
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using QubitMath;
//
// public class DynamicModule0Manager : MonoBehaviour
// {
//   public string[] messages; // This can be used to hold variables for cleanliness. Not currently in use.
//   private bool state; // true = pass, fail = fail. Used for determining path in state tree.
//   private int step; // Current step user is on.
//   private string currentGate; // Holds last applied gate for step checks.
//   public GameObject qubitShell; // Can be used to call qubit script functions.
//   public GameObject messageBox; // Used for changing menu text.
//   public TextMeshProUGUI messageTitle;
//   public TextMeshProUGUI message;
//   public GameObject nextButton;
//   private Button nextButtonComponent;
//
//   QubitManager qubitManager;
//
//   void Start()
//   {
//     /* -- Depreciated --
//     Initialize objects/variables
//     qubit = this.gameObject.transform.Find("Qubit/Shell").gameObject;
//     messageBox = this.gameObject.transform.Find("Menus/generic_message").gameObject;
//     messageTitle = messageBox.transform.Find("menu/title").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
//     message = messageBox.transform.Find("menu/description").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
//     nextButton = messageBox.transform.Find("menu/next_btn").gameObject;
//     */
//     nextButtonComponent = nextButton.GetComponent<Button>();
//     state = true;
//     currentGate = "empty";
//
//     qubitManager = transform.parent.GetComponent<QubitManager>();
//     Matrix qubitState = new Matrix(2, 1);
//     qubitState.matrix[0, 0] = new Complex(1, 0);
//     qubitState.matrix[1, 0] = new Complex(0, 0);
//     GameObject qubit = qubitManager.createQubit(new Vector3(0, 1, 0), qubitState);
//   }
//
//   // -- Depreciated --
//   // Called from qubit script on gate application.
//   /*
//   public void setGate(Collider other)
//   {
//     currentGate = other.tag;
//   }
//   */
//
//   public void setGate(History history)
//   {
//     currentGate = history.gates[history.length - 1];
//   }
//
//   public void setStateFalse()
//   {
//     currentGate = "empty";
//     state = false;
//     Debug.Log("State set to "+state);
//     nextButtonComponent.onClick.RemoveListener(setStateFalse);
//     nextButton.SetActive(false);
//   }
//
//   public void setStateTrue()
//   {
//     currentGate = "empty";
//     state = true;
//     Debug.Log("State set to "+state);
//     nextButtonComponent.onClick.RemoveListener(setStateTrue);
//     nextButton.SetActive(false);
//   }
//
//
//   // Increment or decrement step depending on dir.
//   public void incrementStep()
//   {
//     nextButtonComponent.onClick.RemoveListener(incrementStep);
//     currentGate = "empty";
//     state = true;
//     Debug.Log("State set to "+state);
//     nextButton.SetActive(false);
//     step++;
//     Debug.Log(step);
//   }
//
//   void Update()
//   {
//     // State tree
//     switch (step)
//     {
//       case 0:
//         //Debug.Log("Step 0");
//         if (state) // State is true by default. If user has not made an error yet, continue.
//         {
//           //Debug.Log("State True");
//           // In general, step 0 will always greet user and initialize qubit to correct position.
//           nextButton.SetActive(false);
//           messageTitle.text = "Applying Gates";
//           message.text = "Grab the Hadamard gate and pass it through the qubit to apply it.";
//           qubitShell.transform.GetComponent<ApplyGate>().setGateLock(false);
//
//           // If the current gate is not empty, the user has made an action.
//           // Check it.
//           if (currentGate.CompareTo("empty") != 0)
//           {
//             // If gate matches, user has succeeded. Clean gate out and increment state.
//             if (currentGate.CompareTo("H") == 0)
//             {
//               Debug.Log("H gate detected!");
//               currentGate = "empty";
//               qubitShell.transform.GetComponent<ApplyGate>().setGateLock(true);
//               step++;
//               break;
//             }
//             else // User has applied an incorrect gate. Set state to fail and clean gate out.
//             {
//               Debug.Log("Incorrect gate applied. Need to roll back.");
//               nextButton.SetActive(true);
//               message.text = "Oops!\nYou applied the wrong gate.\nClick the next button to try again.";
//               currentGate = "empty";
//               // set state to fail (false) and set button listener
//               state = false;
//               nextButtonComponent.onClick.RemoveAllListeners();
//               nextButtonComponent.onClick.AddListener(setStateTrue);
//               qubitShell.transform.GetComponent<ApplyGate>().setGateLock(true);
//               break;
//             }
//           }
//           else   // User has not entered gate yet. Just break to allow update to re-run.
//             break;
//         }
//         else // If the state is false, simply wait for the user to hit next and reset state.
//           break;
//       case 1:
//         // State is working here as a guard against adding multiple listeners.
//         // By changing the state after adding the listener, we prevent duplicate listeners from being added
//         if (state)
//         {
//           nextButtonComponent.onClick.RemoveAllListeners();
//           nextButtonComponent.onClick.AddListener(setStateFalse);
//           nextButton.SetActive(true);
//           messageTitle.text = "Applying Gates";
//           message.text = "Great! The qubit is now in superposition.\nNow lets try measurement...";
//         }
//         else
//         {
//           nextButton.SetActive(true);
//           messageTitle.text = "Measurement";
//           message.text = "Measuring a qubit in superposition will cause it to collapse into an on or off state.";
//           nextButtonComponent.onClick.RemoveAllListeners();
//           nextButtonComponent.onClick.AddListener(incrementStep);
//         }
//         break;
//       case 2:
//         // wait for measurement to be used
//         if (state)
//         {
//           nextButton.SetActive(false);
//           messageTitle.text = "Measurement";
//           message.text = "Grab the flashlight and pass it through the qubit to measure the qubit's state.";
//           qubitShell.transform.GetComponent<ApplyGate>().setGateLock(false);
//
//           // If the current gate is not empty, the user has made an action.
//           // Check it.
//           if (currentGate.CompareTo("empty") != 0)
//           {
//             // If gate matches, user has succeeded. Clean gate out and increment state.
//             if (currentGate.CompareTo("Measure") == 0)
//             {
//               Debug.Log(currentGate+" detected!");
//               currentGate = "empty";
//               qubitShell.transform.GetComponent<ApplyGate>().setGateLock(true);
//               step++;
//               break;
//             }
//             else // User has applied an incorrect gate. Set state to fail and clean gate out.
//             {
//               Debug.Log("Incorrect action applied. Need to roll back.");
//               nextButton.SetActive(true);
//               message.text = "Oops!\nYou didn't use the right tool.\nClick the next button and try again.";
//               currentGate = "empty";
//               // set state to fail (false) and set button listener
//               state = false;
//               nextButtonComponent.onClick.AddListener(setStateTrue);
//               qubitShell.transform.GetComponent<ApplyGate>().setGateLock(true);
//               break;
//             }
//           }
//           else   // User has not entered gate yet. Just break to allow update to re-run.
//             break;
//         }
//         else // If the state is false, simply wait for the user to hit next and reset state.
//           break;
//         break;
//       case 3:
//         // nothing as of yet
//         message.text = "Great job!";
//         break;
//       default:
//         // some kind of error
//         break;
//     }
//   }
// }
