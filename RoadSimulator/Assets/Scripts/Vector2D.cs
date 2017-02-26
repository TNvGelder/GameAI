using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D() : this(0, 0)
        {
        }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(Vector2 unityVector)
        {
            X = unityVector.x;
            Y = unityVector.y;
        }

        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public double LengthSquared()
        {
            return (this.X * this.X) + (this.Y * this.Y);
        }

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2D operator *(Vector2D a, double b)
        {
            return new Vector2D(a.X * b, a.Y * b);
        }

        public static Vector2D operator /(Vector2D a, double b)
        {
            return new Vector2D(b != 0 ? a.X / b : a.X, b != 0 ? a.Y / b : a.Y);
        }

        public Vector2D Normalize()
        {
            return this / Length();
        }

        public Vector2D Truncate(double maX)
        {
            if (Length() > maX)
            {
                return Normalize() * maX;
            }

            return this;
        }

        public Vector2D Clone()
        {
            return new Vector2D(this.X, this.Y);
        }

        public override string ToString()
        {
            return String.Format("({0},{1})", X, Y);
        }
    }
}
