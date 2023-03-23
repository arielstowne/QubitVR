using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Section2_Assessment_Questions;

/** Depreciated script  for the scoreboard feature from Mod 1 Sect 2 */
public class Scoreboard_Manager : MonoBehaviour
{
    public GameObject scoreboard;
    GameObject attempts_text, title_text;
    GameObject[] rows = new GameObject[4];
    GameObject[] check_marks = new GameObject[4];
    GameObject[] x_marks = new GameObject[4];
    GameObject[,] gates = new GameObject[4, 3];
    Vector3 on = new Vector3(0.055f, 0, 0.019f);
    Vector3 off = new Vector3(-0.055f, 0, -0.019f);

    public Material H, NOT, S, T, gray, green, red, yellow, black;
    bool[,] enabledGates = new bool[4, 3];
    bool blink = true, pauseBlink = false;
    int blinkTickCounter = 0, attempt = -1;


    void Start()
    {
        // Set references to text objects on scoreboard
        attempts_text = scoreboard.transform.GetChild(0).GetChild(1).gameObject;
        title_text = scoreboard.transform.GetChild(0).GetChild(0).gameObject;

        // Populate all 4 row, xmark, and checkmark references for easy access
        for (int n=0; n < 4; n++)
        {
            rows[n] = scoreboard.transform.GetChild(0).GetChild(2).GetChild(n).gameObject;
            check_marks[n] = rows[n].transform.GetChild(1).gameObject;
            x_marks[n] = rows[n].transform.GetChild(2).gameObject;
        }

        // Populate all 12 gate references for easy access
        for (int row=0; row < 4; row++)
            for (int gate=0; gate < 3; gate++)
                gates[row, gate] = rows[row].transform.GetChild(3 + gate).gameObject;

        // Initially disable all gates on the scoreboard
        for (int n=0; n < 4; n++)
            for (int m=0; m < 3; m++)
                disableGate(gates[n, m]);
    }

    public void Initialize(Question question)
    {
        // Set all rows and gates active
        for (int n=0; n < question.attempts; n++)
            rows[n].SetActive(true);

        for (int n=0; n < question.numGates; n++)
            for (int m=0; m < question.attempts; m++)
                gates[m, n].SetActive(true);
    }

    public void resetScoreboard()
    {
        attempt = -1;
        pauseBlink = false;

        // disable all enabled gates
        for (int n=0; n < 4; n++)
        {
            for (int m=0; m < 3; m++)
            {
                if (enabledGates[n, m])
                {
                    disableGate(gates[n, m]);
                    enabledGates[n, m] = false;
                }
            }
        }

        // unload check and x
        for (int n=0; n < 4; n++)
        {
            check_marks[n].SetActive(false);
            x_marks[n].SetActive(false);
        }

        // set all borders to black
        for (int n=0; n < 4; n++)
            setBorderColor(rows[n], black);

        // unload all gates
        for (int n=0; n < 4; n++)
            for (int m=0; m < 3; m++)
                gates[n, m].SetActive(false);

        // unload all rows
        for (int n=0; n < 4; n++)
            rows[n].SetActive(false);
    }

    public void updateTitle(int step, Question question)
    {
        // Update scoreboard title text
        title_text.GetComponent<TextMeshPro>().SetText(
            "Question " + step + "  |  " + question.numSolutions + " Solution" + (question.numSolutions == 1 ? "" : "s"));
    }

    public void updateAttempts(int attemptsRemaining)
    {
        // Update scoreboard attempts text
        attempts_text.GetComponent<TextMeshPro>().SetText("Attempts Remaining:  " + attemptsRemaining);
        attempt++;
    }

    public void enableGate(string gate, int gatesApplied)
    {
        // Obtain the specific gate on the scoreboard to change, then apply appropriate changes.
        GameObject piece = gates[attempt, gatesApplied-1];
        enabledGates[attempt, gatesApplied-1] = true;
        piece.transform.Translate(on, Space.World);

        if (gate == "H")
        {
            piece.GetComponent<Renderer>().material = H;
            piece.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().SetText("H");
        }
        else if (gate == "NOT")
        {
            piece.GetComponent<Renderer>().material = NOT;
            piece.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().SetText("X");
        }
        else if (gate == "S")
        {
            piece.GetComponent<Renderer>().material = S;
            piece.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().SetText("S");
        }
        else if (gate == "T")
        {
            piece.GetComponent<Renderer>().material = T;
            piece.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().SetText("T");
        }
    }

    private void disableGate(GameObject piece)
    {
        // Set the specific 'piece' or gate to disabled state (gray, textless, and moved back)
        piece.transform.Translate(off, Space.World);
        piece.GetComponent<Renderer>().material = gray;
        piece.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().SetText("");
    }

    public void setRowBorder(string color)
    {
        // calls setBorderColor and enables an X or Check depending on color
        if (color == "green")
        {
            setBorderColor(rows[attempt-1], green);
            check_marks[attempt-1].SetActive(true);
        }
        else if (color == "red")
        {
            setBorderColor(rows[attempt-1], red);
            x_marks[attempt-1].SetActive(true);
        }
    }

    private void setBorderColor(GameObject row, Material color)
    {
        // Sets all components of the border to 'color'
        for (int n=0; n < 4; n++)
            row.transform.GetChild(0).GetChild(n).gameObject.GetComponent<Renderer>().material = color;
    }

    public void pauseBlinking()
    {
        // Function to prevent edge-case of attempt being 4 after finishing a 4 attempt question.
        if (attempt < 4)
            setBorderColor(rows[attempt], black);
        pauseBlink = true;
    }

    private void Blink()
    {
        // Function to toggle between black and yellow border
        if (attempt < 0 || pauseBlink)
            return;

        if (blink)
            setBorderColor(rows[attempt], black);
        else
            setBorderColor(rows[attempt], yellow);

        blink = !blink;
    }

    void Update()
    {
        // Border blinks between yellow and black every 45 frames
        if (blinkTickCounter > 45)
        {
            blinkTickCounter = 0;
            Blink();
        }

        blinkTickCounter++;
    }
}
