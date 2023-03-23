/* SceneHandler.cs*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QubitMath;

/** Depreciated class for dynamically running a tutorial. */
public class DynamicModule1Manager : ModuleManager
{
  public GameObject genericMessageObject;
  public TextMeshProUGUI messageTitle;
  public TextMeshProUGUI message;
  public GameObject nextButton;
  private Button nextButtonComponent;
  private DynamicQubitManager DQubitManager;



  // DEPRECIATED: Override example for dynamic content
  // void Start()
  // {
  //   Debug.Log("In child start method.");
  //   qubits = new GameObject[MAX_QUBITS];
  //   qubitScript = new ApplyGate[MAX_QUBITS];
  //   nextButtonComponent = nextButton.GetComponent<Button>();
  //   qubitManager = GetComponent<QubitManager>();
  //   isHoldingFlashlight = false;
  //   qubitCount = 0;
  //   clean();
  // }

  protected override void init()
  {
    DQubitManager = GetComponent<DynamicQubitManager>();
    genericMessageObject.SetActive(true);
    Debug.Log("Child DynamicModule1Manager init().");
    nextButtonComponent = nextButton.GetComponent<Button>();
    clean();
  }

  public override void setStateFalse()
  {
    clean();
    state = false;
  }

  public override void setStateTrue()
  {
    clean();
  }

  // Increment the step and set state to true.
  public override void incrementStep()
  {
    clean();
    step++;
  }

  private void clean()
  {
    currentGate = "empty";
    state = true;
    nextButton.SetActive(false);
    nextButtonComponent.onClick.RemoveAllListeners();
  }

  private void setupNextButton()
  {
    nextButtonComponent.onClick.RemoveAllListeners();
    if (state)
      nextButtonComponent.onClick.AddListener(setStateFalse);
    else
      nextButtonComponent.onClick.AddListener(incrementStep);
    nextButton.SetActive(true);
  }

  private void lecture(string[,] lectureParams)
  {
    if (state)
    {
      setupNextButton();
      messageTitle.text = lectureParams[0,0];
      message.text = lectureParams[1,0];
    }
    else
    {
      setupNextButton();
      messageTitle.text = lectureParams[0,1];
      message.text = lectureParams[1,1];
    }
  }

  void Update()
  {
    string[,] lectureParams; /* 2D array for passing title and message to a step with lectures and no user input.
                                 example:   lectureParams = new string[,] {
                                              { "title1", "title2" },
                                              { "message1", "messaage2" }
                                            };
                              */

    // State tree
    switch (step)
    {
      case 0:
        lectureParams = new string[,]
        {
          {
            "Introduction to Module 1", "Introduction to Module 1"
          },
          {
            "Welcome to Module 1!\nOur focus this module will be on qubits and how we represent them in QubitVR.",
            "We will discuss Bloch-spheres, qubit states, the Superposition Principle, and the Measurement Principle."
          }
        };
        lecture(lectureParams);
        break;
      case 1:
        lectureParams = new string[,]
        {
          {
            "Qubits and Bloch-Spheres", "Qubits and Bloch-Spheres"
          },
          {
            "First, let's define what a qubit is...",
            "Qubit stands for 'quantum bit'.\nIt acts much like a classical computing bit but with a few key differences that we will demonstrate in this module."
          }
        };
        lecture(lectureParams);
        break;
      case 2:
        if (!DQubitManager.checkQubitExists(0))
         DQubitManager.createQubit(new Vector3(-2, 1, 0), States.UP);
       DQubitManager.setQubitLock(0, true);
        lectureParams = new string[,]
        {
          {
            "Qubits and Bloch-Spheres", "Qubits and Bloch-Spheres"
          },
          {
            "The sphere you see here is a Bloch-Sphere. It is a common way to represent a qubit and its quantum state in 3D.",
            "A quantum state is a mathematical representation of a physical system, such as an atom, and provides the basis for processing quantum information."
          }
        };
        lecture(lectureParams);
        break;
      case 3:
        lectureParams = new string[,]
        {
          {
            "The Unit Vector and States", "The Unit Vector and States"
          },
          {
            "See the line inside the Bloch-sphere? That is the Unit Vector. It represents the current state of the qubit.",
            "When the unit vector is pointing straight up like in this qubit, the qubit is in state |0>. It's common to say that a qubit in state |0> is off."
          }
        };
        lecture(lectureParams);
        break;
      case 4:
       DQubitManager.setQubitState(0, States.DOWN);
        lectureParams = new string[,]
        {
          {
            "The Unit Vector and States", "The Unit Vector and States"
          },
          {
            "When the unit vector is pointing straight down like it is here, the qubit is in state |1>.\nQubits in state |1> are on.",
            "This two-state system is similar to what you would see in a classical computer, which has bits that may be on or off."
          }
        };
        lecture(lectureParams);
        break;
      case 5:
       DQubitManager.setQubitState(0, States.LEFT);
        lectureParams = new string[,]
        {
          {
            "The Superposition Principle", "The Superposition Principle"
          },
          {
            "However, qubits are not limited to only two states. Notice that the unit vector is now pointing to the left instead of up or down.",
            "Whenever the unit vector is not pointing straight up or down, the current state is some combination of |0> and |1>."
          }
        };
        lecture(lectureParams);
        break;
      case 6:
        //setQubitState(States.UP_LEFT, 0);
        lectureParams = new string[,]
        {
          {
            "The Superposition Principle", "The Superposition Principle"
          },
          {
            "We call this the Superposition Principle.\nWhile in superposition, a qubit is existing in multiple states simultaneously.",
            "Take a moment to observe these qubits. They are all in a superposition of states."
          }
        };

        if (state)
        {
          setupNextButton();
          messageTitle.text = lectureParams[0,0];
          message.text = lectureParams[1,0];
        }
        else
        {
          if (!DQubitManager.checkQubitExists(1))
           DQubitManager.createQubit(new Vector3(-1.6f, 1, 1), States.RIGHT);
          if (!DQubitManager.checkQubitExists(2))
           DQubitManager.createQubit(new Vector3(-1.6f, 1, -1), States.UP_RIGHT);
          if (!DQubitManager.checkQubitExists(3))
           DQubitManager.createQubit(new Vector3(-1.2f, 1, 2), States.DOWN_LEFT);
          if (!DQubitManager.checkQubitExists(4))
           DQubitManager.createQubit(new Vector3(-1.2f, 1, -2), States.LEFT_BACKWARD);

          setupNextButton();
          messageTitle.text = lectureParams[0,1];
          message.text = lectureParams[1,1];
        }

        break;
      case 7:
        if (DQubitManager.checkQubitExists(4))
         DQubitManager.destroyQubit(4);
        if (DQubitManager.checkQubitExists(3))
         DQubitManager.destroyQubit(3);
        if (DQubitManager.checkQubitExists(2))
         DQubitManager.destroyQubit(2);
        if (DQubitManager.checkQubitExists(1))
         DQubitManager.destroyQubit(1);

        lectureParams = new string[,]
        {
          {
            "The Superposition Principle", "The Superposition Principle"
          },
          {
            "Superposition allows qubits to represent significantly more values than classical bits, "
              + "which quantum computers leverage into more computing power.",
            "However, the Superposition Principle also presents some interesting challenges.\n"
              +"To explore this more, we will need to learn about measurement."
          }
        };
        lecture(lectureParams);
        break;
      case 8:
        lectureParams = new string[,]
        {
          {
            "The Measurement Tool", "The Measurement Tool"
          },
          {
            "There is a flashlight hanging at your waist. We will call this the Measurement Tool.",
            "Shining its beam on a qubit will measure the current state of the qubit."
          }
        };
        lecture(lectureParams);
        break;
      case 9:
        if (state)
        {
          message.text = "We'll talk more about what measurement means in a moment. For now, just try grabbing the flashlight.";
          if (isHoldingFlashlight)
          {
            Debug.Log("Changing state!");
            state = false;
          }
        }
        else
        {
          if (isHoldingFlashlight)
          {
           DQubitManager.setQubitLock(0, false);
            message.text = "The qubit has changed color to indicate it can be interacted with."
              + "\nPass the flashlight's beam over the qubit to measure it. Watch the unit vector carefully!";
            if (currentGate.CompareTo("Measure") == 0)
            {
             DQubitManager.setQubitState(0, States.UP);
             DQubitManager.setQubitLock(0, true);
              clean();
              incrementStep();
            }
          }
          else {
           DQubitManager.setQubitLock(0, true);
            message.text = "Oops, you let go of the flashlight. Pick it back up from your waist to continue.";
          }
        }
        break;
      case 10:
        lectureParams = new string[,]
        {
          {
            "The Superposition Principle II", "The Superposition Principle II"
          },
          {
            "Great job! Did you notice how the unit vector behaved when you measured the qubit?"
              + "\nAs soon as the qubit was measured, it fell out of superposition into state |0>.",
            "A qubit in superposition cannot remain in superposition if it is measured or observed."
          }
        };
        lecture(lectureParams);
        break;
      case 11:
        lectureParams = new string[,]
        {
          {
            "The Superposition Principle II", "The Superposition Principle II"
          },
          {
            "When a qubit in a superposition of states is measured, it will immediately collapse "
            + "into either state |0> or state |1>.",
            "It's impossible to know which state the superposition will collapse into, but we can make an educated guess."
          }
        };
        lecture(lectureParams);
        break;
      case 12:
        lectureParams = new string[,]
        {
          {
            "The Superposition Principle II", "The Superposition Principle II"
          },
          {
            "The state is determined by a probabalistic function dependent on "
              + "the unit vector's position within the Bloch-sphere.",
            "Basically, the closer the unit vector is to a particular state, the more likely it is that the qubit will collapse into that state when measured."
          }
        };
        lecture(lectureParams);
        break;
      case 13:
       DQubitManager.setQubitState(0, States.UP_RIGHT);
        lectureParams = new string[,]
        {
          {
            "Examples", "Examples"
          },
          {
            "Let's try out some examples.",
            "First, we'll try out a qubit with the unit vector closer to state |0>."
          }
        };
        lecture(lectureParams);
        break;
      case 14:
        if (state)
        {
          message.text = "Take a moment to consider what will happen when you measure this particualr qubit.";
          setupNextButton();
        }
        else
        {
         DQubitManager.setQubitLock(0, false);
          message.text = "When you are ready, shine the flashlight over the qubit to measure its state.";
          if (currentGate.CompareTo("Measure") == 0)
          {
            message.text = "The qubit collapsed to state |0>.";
           DQubitManager.setQubitLock(0, true);
           DQubitManager.setQubitState(0, States.UP);
            setupNextButton();
          }
        }
        break;
      case 15:
        if (state)
        {
         DQubitManager.setQubitState(0, States.DOWN_LEFT);
          message.text = "Let's try with a unit vector closer to state |1>.";
          setupNextButton();
        }
        else
        {
         DQubitManager.setQubitLock(0, false);
          message.text = "When you are ready, shine the flashlight over the qubit to measure its state.";
          if (currentGate.CompareTo("Measure") == 0)
          {
            message.text = "The qubit collapsed to state |1>, just as expected.";
           DQubitManager.setQubitLock(0, true);
           DQubitManager.setQubitState(0, States.DOWN);
            setupNextButton();
          }
        }
        break;
      case 16:
        if (state)
        {
         DQubitManager.setQubitState(0, States.DOWN_RIGHT); // DOWN_LEFT does not work! Why tho?
          message.text = "Let's measure a qubit with a similar superposition one more time.";
          setupNextButton();
        }
        else
        {
         DQubitManager.setQubitLock(0, false);
          message.text = "When you are ready, shine the flashlight over the qubit to measure its state.";
          if (currentGate.CompareTo("Measure") == 0)
          {
            message.text = "Note that this time the qubit collapsed to state |0> even though the "
              +"unit vector was closer to |1>.";
           DQubitManager.setQubitLock(0, false);
           DQubitManager.setQubitState(0, States.UP);
            setupNextButton();
          }
        }
        break;
      case 17:
        lectureParams = new string[,]
        {
          {
            "Examples", "Examples"
          },
          {
            "Remember that there is always a chance that the qubit could collapse into either state, "
              +"even if the unit vector is closer to a particular state.",
            "The only time a qubit is certain to collapse to a certain state is if the unit vector "
              +"is already pointing directly at that state."
          }
        };
        lecture(lectureParams);
        break;
      default:
        // some kind of error
        break;
    }
  }
}
