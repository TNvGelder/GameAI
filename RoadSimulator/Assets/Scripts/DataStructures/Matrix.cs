using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataStructures
{
    public class Matrix
    {
        public float[,] matrix;

        // f, 0, 0
        // 0, f, 0
        // 0, 0, f
        public Matrix(float f = 1.0f) : this(f, 0, 0, 0, f, 0, 0, 0, f)
        { }

        //public Matrix(float m11, float m12,
        //            float m21, float m22)
        //{
        //    matrix = new float[2, 2];
        //    matrix[0, 0] = m11;
        //    matrix[0, 1] = m12;
        //    matrix[1, 0] = m21;
        //    matrix[1, 1] = m22;
        //}

        public Matrix(
                    float f00, float f01, float f02,
                    float f10, float f11, float f12,
                    float f20, float f21, float f22 = 1)
        {
            matrix = new float[3, 3];
            matrix[0, 0] = f00; matrix[0, 1] = f01; matrix[0, 2] = f02;
            matrix[1, 0] = f10; matrix[1, 1] = f11; matrix[1, 2] = f12;
            matrix[2, 0] = f20; matrix[2, 1] = f21; matrix[2, 2] = f22;
        }

        public Matrix(Vector2D v) : this((float)v.X, 0, 0, (float)v.Y, 0, 0, 0, 0, 0)
        { }

        public static Matrix Scale(float s)
        {
            Matrix m = new Matrix(s);
            return m;
        }

        public static Matrix Translate(float x, float y)
        {
            return new Matrix(
                1, 0, x,
                0, 1, y,
                0, 0, 1
            );
        }

        public static Matrix Rotate(float degrees)
        {
            float rad = GetRad(degrees);
            Matrix rm = new Matrix((float)Math.Cos(rad), (float)-Math.Sin(rad), 0,
                (float)Math.Sin(rad), (float)Math.Cos(rad), 0,
                0, 0, 1);
            return rm;
        }


        //create a rotation matrix from a 2D vector
        public static Matrix Rotate(Vector2D fwd, Vector2D side)
        {
            return new Matrix(fwd.X, fwd.Y, 0,
                                             side.X, side.Y, 0,
                                             0, 0, 1);
        }


        public static float GetRad(float degrees)
        {
            return (float)Math.PI / (180 / degrees);
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            for (var x = 0; x < m1.matrix.GetLength(0); x++)
            {
                for (var y = 0; y < m1.matrix.GetLength(1); y++)
                {
                    m1.matrix[x, y] = m1.matrix[x, y] + m2.matrix[x, y];
                }
            }

            return m1;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            for (var x = 0; x < m1.matrix.GetLength(0); x++)
            {
                for (var y = 0; y < m1.matrix.GetLength(1); y++)
                {
                    m1.matrix[x, y] = m1.matrix[x, y] - m2.matrix[x, y];
                }
            }

            return m1;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            // 5 0 0   2 0 0    10 0 0
            // 5 0 0   0 2 0    10 0 0
            // 1 0 0   0 0 1     2 0 0

            Matrix m = new Matrix();

            for (var x = 0; x < m1.matrix.GetLength(0); x++)
            {
                for (var y = 0; y < m1.matrix.GetLength(1); y++)
                {
                    float sum = 0f;

                    for (var v = 0; v < m1.matrix.GetLength(0); v++)
                    {
                        sum += m1.matrix[v, y] * m2.matrix[x, v];
                    }

                    m.matrix[x, y] = sum;
                }
            }

            return m;
        }


        public static Matrix operator *(Matrix m1, float f)
        {
            for (var x = 0; x < m1.matrix.GetLength(0); x++)
            {
                for (var y = 0; y < m1.matrix.GetLength(1); y++)
                {
                    m1.matrix[x, y] = m1.matrix[x, y] * f;
                }
            }

            return m1;
        }

        public static Matrix operator *(float f, Matrix m1)
        {
            return m1 * f;
        }


        public static Vector2D ToVector2D(Matrix m1)
        {
            return new Vector2D(m1.matrix[0, 0], m1.matrix[1, 0]);
        }

        public static Vector2D operator *(Matrix m1, Vector2D v)
        {
            Matrix m = new Matrix(v) * m1;
            Console.WriteLine(m);
            return ToVector2D(m);
        }

        public override string ToString()
        {
            var res = "";
            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    res += matrix[x, y] + ",";
                }
                res += "\r\n";
            }

            return res;
        }
    }
}
