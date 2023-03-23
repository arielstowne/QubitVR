using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Custom classes for linear algebra required for vector rotations and gates
*
* History stores gate applications.
* Complex creates/stores/manipulates complex numbers
* Matrix creates/stores/manipulates matrices
*/
namespace QubitMath
{
    /** Statically holds pre-set qubit states as a matrix of complex numbers.*/
    public static class States
    {
        public static Matrix UP = new Matrix(new Complex[,] {{new Complex(1, 0)}, {new Complex(0, 0)}});
        public static Matrix DOWN = new Matrix(new Complex[,] {{new Complex(0, 0)}, {new Complex(1, 0)}});
        public static Matrix LEFT = new Matrix(new Complex[,] {{new Complex(0.70711f, 0)}, {new Complex(0, -0.70711f)}});
        public static Matrix RIGHT = new Matrix(new Complex[,] {{new Complex(0.70711f, 0)}, {new Complex(0, 0.70711f)}});
        public static Matrix FORWARD = new Matrix(new Complex[,] {{new Complex(0.70711f, 0)}, {new Complex(-0.70711f, 0)}});
        public static Matrix BACKWARD = new Matrix(new Complex[,] {{new Complex(0.70711f, 0)}, {new Complex(0.70711f, 0)}});

        public static Matrix LEFT_FORWARD = new Matrix(new Complex[,] {{new Complex(0.70711f, 0)}, {new Complex(-0.5f, -0.5f)}});
        public static Matrix LEFT_BACKWARD = new Matrix(new Complex[,] {{new Complex(0.70711f, 0)}, {new Complex(0.5f, -0.5f)}});
        public static Matrix RIGHT_FORWARD = new Matrix(new Complex[,] {{new Complex(0.70711f, 0)}, {new Complex(-0.5f, 0.5f)}});
        public static Matrix RIGHT_BACKWARD = new Matrix(new Complex[,] {{new Complex(0.70711f, 0)}, {new Complex(0.5f, 0.5f)}});

        public static Matrix UP_LEFT = new Matrix(new Complex[,] {{new Complex(0.85355f, 0.35355f)}, {new Complex(0.14645f, -0.35355f)}});
        public static Matrix UP_RIGHT = new Matrix(new Complex[,] {{new Complex(0.85355f, -0.35355f)}, {new Complex(0.14645f, 0.35355f)}});
        public static Matrix DOWN_LEFT = new Matrix(new Complex[,] {{new Complex(0.14645f, 0.35355f)}, {new Complex(0.85355f, -0.35355f)}});
        public static Matrix DOWN_RIGHT = new Matrix(new Complex[,] {{new Complex(0.14645f, -0.35355f)}, {new Complex(0.85355f, 0.35355f)}});
    }

    /** Stores gate applications that a user applies to a qubit.
    *
    * Can be used to check for a combination of applied gates, to implement an
    * undo feature, etc.
    */
    public class History
    {
        public int maxLength = 50;
        public int length;
        public String[] gates;
        public Matrix[] states;

        public History()
        {
            this.length = 0;
            this.gates = new String[maxLength];
            this.states = new Matrix[maxLength];
        }

        public void Add(string gate, Matrix state, Transform transform)
        {
            // Simple check for preventing OOBE.
            // TODO: Let's refactor this to use ArrayLists or implement array wrapping
            if (length == maxLength - 1)
              length = 0;
            this.gates[length] = gate;
            this.states[length] = state;
            this.length++;

            GameObject moduleManager = transform.root.gameObject;
            moduleManager.BroadcastMessage("setGate", this);
        }

        public void Clear()
        {
            for (int n=0; n<30; n++)
            {
                this.gates[n] = null;
                this.states[n] = null;
            }
            this.length = 0;
        }

        public void Print()
        {
            for (int n = 0; n < this.length; n++)
            {
                Debug.Log(this.gates[n] + ": [" + this.states[n].matrix[0, 0] + ", " + this.states[n].matrix[1, 0] + "]");
            }
        }
    }

    /** Simple Complex number class in the form a + bi where 'a' and 'b' are floats. */
    public class Complex
    {

        public float a, b;
        public Complex()
        {
            this.a = 0;
            this.b = 0;
        }

        public Complex(float a, float b)
        {
            this.a = a;
            this.b = b;
        }

        public Complex Add(Complex num)
        {
            Complex result = new Complex();
            result.a += (this.a + num.a);
            result.b += (this.b + num.b);
            return result;
        }

        public Complex Multiply(Complex num)
        {
            Complex result = new Complex();
            result.a += (this.a * num.a) - (this.b * num.b);
            result.b += (this.a * num.b) + (this.b * num.a);
            return result;
        }

        /** Rounds near-integer values to that integer.*/
        public void SmartRound()
        {
            // Rounds near-integer values to that integer.
            double guess_a = Math.Round((double)this.a);
            double guess_b = Math.Round((double)this.b);
            if (Math.Abs(this.a - guess_a) < 0.01)
                this.a = (float)guess_a;
            if (Math.Abs(this.b - guess_b) < 0.01)
                this.b = (float)guess_b;
        }

        public override String ToString() {
            return a + " + " + b + "i";
        }
    }

    /** Represents a matrix with a Complex[n, m] array. */
    public class Matrix
    {
        public Complex[,] matrix;
        public int rows, cols;

        public Matrix(Complex[,] matrix)
        {
            this.matrix = matrix;
            this.rows = matrix.GetLength(0);
            this.cols = matrix.GetLength(1);
        }

        public Matrix(int n, int m)
        {
            this.matrix = new Complex[n, m];
            this.rows = n;
            this.cols = m;

            for(int i = 0; i < n; i++)
                for(int j = 0; j < m; j++)
                    matrix[i, j] = new Complex();
        }

        /** Multiplies the current matrix by another matrix and returns a new matrix with the result.
        * @param other the matrix being used as the multiplier
        * @return result if the matrices are incompatible, returns null.
        */
        public Matrix Multiply(Matrix other)
        {
            if(cols != other.rows)
            {
                Debug.Log("Incompatible matrices: cannot multiply. Matrix A has "+rows+" rows and "+cols+" columns. Matrix B has "+other.rows+" rows and "+other.cols+" columns.");
                return null;
            }

            Matrix result = new Matrix(rows, other.cols);

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < other.cols; j++)
                {
                    for(int k = 0; k < cols; k++)
                    {
                        Complex mult = matrix[i, k].Multiply(other.matrix[k, j]);
                        result.matrix[i, j] = result.matrix[i, j].Add(mult);
                    }
                }
            }

            return result;
        }

        /** Finds the complex conjugate of the current matrix and returns a new matrix with the result. */
        public Matrix ComplexConjugate()
        {
            Matrix result = new Matrix(rows, cols);

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    result.matrix[i, j].a = matrix[i,j].a;
                    result.matrix[i, j].b = matrix[i,j].b * -1;
                }
            }

            return result;
        }

        /** Finds the transpose of the current matrix and returns the result. */
        public Matrix Transpose()
        {
            int n = matrix.GetLength(0), m = matrix.GetLength(1);
            Matrix result = new Matrix(cols, rows);

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    result.matrix[j, i].a = matrix[i, j].a;
                    result.matrix[j, i].b = matrix[i, j].b;
                }
            }

            return result;
        }

        /** Finds the transpose followed by the complex conjugate of the current matrix and returns the result. */
        public Matrix ConjugateTranspose()
        {
            return this.Transpose().ComplexConjugate();
        }

        /** Used when the matrix is 1 x 1 */
        public float GetSingleRealIntVal() {
            if(matrix.GetLength(0) != 1 || matrix.GetLength(1) != 1)
            {
               Debug.Log("Matrix does not hold a single value.");
               // can this be negative?
               return -1;
            }

            // double check this part
            return matrix[0,0].a;
        }

        public override String ToString() {

            String result = "";

            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                result += "[ ";
                bool first = true;

                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    if(!first)
                    {
                        result+=", ";
                    }
                    result += matrix[i, j].ToString();
                    first = false;

                }

                result+="]\n";
            }

            return result;
        }

        /** Used for .csv reporting */
        public String ReportToString() {

            String result = "";

            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                result += "[ ";
                bool first = true;

                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    if(!first)
                    {
                        result+=", ";
                    }
                    result += matrix[i, j].ToString();
                    first = false;

                }

                result+="]";
            }

            return result;
        }
    }
}
