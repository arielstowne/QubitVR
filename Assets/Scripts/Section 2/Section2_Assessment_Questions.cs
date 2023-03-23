using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QubitMath;

/** Used as  a question bank for Section 2 Assessment */
namespace Section2_Assessment_Questions
{
    /** Individual question with start, target, number of gates, number of solns, solns array, and attempts.*/
    public class Question
    {
        public Matrix start, target;
        public int numGates, numSolutions, attempts;
        public string[,] solutions;

        public Question(Matrix start, Matrix target, int numGates, int numSolutions, string[,] solutions)
        {
            this.start = start;
            this.target = target;
            this.numGates = numGates;
            this.numSolutions = numSolutions;
            this.solutions = solutions;
            this.attempts = 1; // 1 Attempt per question.
        }
    }

    /** Assessment object is statically instantiated with pre-definied question bank
    *
    * Currently holds three question types: easy (1 gate), medium (2 gates), hard (3 gates)
    * Current has six of each question type.
    */
    public class Assessment
    {
        public Question[] singleGateQuestions = new Question[6];
        public Question[] doubleGateQuestions = new Question[6];
        public Question[] tripleGateQuestions = new Question[6];

        public Assessment()
        {
            // SINGLE GATE QUESTIONS

            string[,] solution1_1 = {{"NOT"}};
            this.singleGateQuestions[0] = new Question(States.UP, States.DOWN, 1, 1, solution1_1);

            string[,] solution1_2 = {{"H"}};
            this.singleGateQuestions[1] = new Question(States.UP, States.BACKWARD, 1, 1, solution1_2);

            string[,] solution1_3 = {{"S"}};
            this.singleGateQuestions[2] = new Question(States.BACKWARD, States.RIGHT, 1, 1, solution1_3);

            string[,] solution1_4 = {{"T"}};
            this.singleGateQuestions[3] = new Question(States.LEFT, States.LEFT_BACKWARD, 1, 1, solution1_4);

            string[,] solution1_5 = {{"H"}};
            this.singleGateQuestions[4] = new Question(States.BACKWARD, States.UP, 1, 1, solution1_5);

            string[,] solution1_6 = {{"NOT"}, {"H"}};
            this.singleGateQuestions[5] = new Question(States.RIGHT, States.LEFT, 1, 2, solution1_6);

            // ===============================================================================================
            // DOUBLE GATE QUESTIONS

            string[,] solution2_1 = {{"NOT", "H"}};
            this.doubleGateQuestions[0] = new Question(States.DOWN, States.BACKWARD, 2, 1, solution2_1);

            string[,] solution2_2 = {{"H", "S"}};
            this.doubleGateQuestions[1] = new Question(States.UP, States.RIGHT, 2, 1, solution2_2);

            string[,] solution2_3 = {{"NOT", "H"}};
            this.doubleGateQuestions[2] = new Question(States.UP, States.FORWARD, 2, 1, solution2_3);

            string[,] solution2_4 = {{"T", "NOT"}, {"T", "H"}};
            this.doubleGateQuestions[3] = new Question(States.RIGHT_BACKWARD, States.LEFT, 2, 2, solution2_4);

            string[,] solution2_5 = {{"H", "S"}, {"NOT", "S"}};
            this.doubleGateQuestions[4] = new Question(States.RIGHT, States.BACKWARD, 2, 2, solution2_5);

            string[,] solution2_6 = {{"H", "NOT"}};
            this.doubleGateQuestions[5] = new Question(States.FORWARD, States.UP, 2, 1, solution2_6);

            // ===============================================================================================
            // TRIPLE GATE QUESTIONS

            string[,] solution3_1 = {{"NOT", "S", "H"}, {"S", "H", "NOT"}, {"H", "S", "H"}};
            this.tripleGateQuestions[0] = new Question(States.RIGHT, States.UP, 3, 3, solution3_1);

            string[,] solution3_2 = {{"H", "S", "T"}, {"H", "T", "S"}};
            this.tripleGateQuestions[1] = new Question(States.DOWN_RIGHT, States.BACKWARD, 3, 2, solution3_2);

            string[,] solution3_3 = {{"S", "T", "H"}, {"T", "S", "H"}};
            this.tripleGateQuestions[2] = new Question(States.RIGHT, States.DOWN_RIGHT, 3, 2, solution3_3);

            string[,] solution3_4 = {{"H", "S", "NOT"}};
            this.tripleGateQuestions[3] = new Question(States.UP, States.LEFT_BACKWARD, 3, 1, solution3_4);

            string[,] solution3_5 = {{"NOT", "H", "T"}};
            this.tripleGateQuestions[4] = new Question(States.DOWN, States.RIGHT_BACKWARD, 3, 1, solution3_5);

            string[,] solution3_6 = {{"H", "T", "NOT"}};
            this.tripleGateQuestions[5] = new Question(States.DOWN, States.RIGHT_FORWARD, 3, 1, solution3_6);
        }
    }
}
