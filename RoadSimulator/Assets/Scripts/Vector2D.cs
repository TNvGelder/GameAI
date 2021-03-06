﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Vector2D : IEquatable<Vector2D>
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2D() : this(0, 0)
        {
        }

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(Vector2 unityVector)
        {
            X = unityVector.x;
            Y = unityVector.y;
        }

        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
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

        public static Vector2D operator *(Vector2D a, float b)
        {
            return new Vector2D(a.X * b, a.Y * b);
        }

        public static Vector2D operator /(Vector2D a, float b)
        {
            return new Vector2D(b != 0 ? a.X / b : a.X, b != 0 ? a.Y / b : a.Y);
        }


        public Vector2D Normalize()
        {
            return this / Length();
        }

        public Vector2D Truncate(float maX)
        {
            if (Length() > maX)
            {
                return Normalize() * maX;
            }

            return this;
        }

        public bool Equals(Vector2D other)
        {
            return (nearlyEqual(X, other.X) && nearlyEqual(Y, other.Y));
        }

        private bool nearlyEqual(float a, float b)
        {
            float diff = Math.Abs(a - b);
            return a == b || diff < 0.01f;

        }

        public override int GetHashCode()
        {
            int result = (int)X;
            result = 31 * result + (int)Y;
            return result;
        }

        public Vector2D Clone()
        {
            return new Vector2D(this.X, this.Y);
        }

        public override string ToString()
        {
            return String.Format("({0},{1})", X, Y);
        }

        public float Sign(Vector2D toTarget)
        {
            return Y * toTarget.X > X * toTarget.Y ? -1 : 1;
        }

        public float Dot(Vector2D toTarget)
        {
            return X * toTarget.X + Y * toTarget.Y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2((float)X, (float)Y);
        }

        public Vector2D Perp()
        {
            return new Vector2D(-Y, X);
        }

        public double Vec2DDistanceSq(Vector2D v2)
        {
            double ySeparation = v2.Y - this.Y;
            double xSeparation = v2.X - this.X;

            return ySeparation * ySeparation + xSeparation * xSeparation;
        }
    }
}
