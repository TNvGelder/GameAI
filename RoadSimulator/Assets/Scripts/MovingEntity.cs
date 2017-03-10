﻿using Assets.Scripts.DataStructures;
using Assets.Scripts.SteeringBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovingEntity : BaseGameEntity
    {
        public Vector2D Velocity { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxTurnRate { get; set; }
        public Vector2D Heading { get; set; }
        public Vector2D Side { get; set; }
        private List<ISteeringBehavior> steeringBehaviours;
        public List<ISteeringBehavior> SteeringBehaviours { get { return steeringBehaviours; } }
        public ICombinedSteeringBehavior CombinedSteeringBehavior { get; set; }
        public bool Tagged { get; internal set; }
        public float BRadius { get; set; }
        private PathPlanner pathPlanner;
        public PathPlanner PathPlanner { get { return pathPlanner; } }

        public MovingEntity(GameObject gameObject, Vector2D pos, Vector2D size, PathPlanner pathPlanner) : base(gameObject, pos, size)
        {
            this.pathPlanner = pathPlanner;
            pathPlanner.Me = this;
            Mass = 1;
            MaxSpeed = 10;
            BRadius = 2;
            MaxTurnRate = 9999999999999;
            steeringBehaviours = new List<ISteeringBehavior>();
            CombinedSteeringBehavior = new WeightedSumPriorityCombinedSteeringBehavior(this);

            if (Velocity == null)
                Velocity = new Vector2D();

            if (Side == null)
                Side = new Vector2D();

            if (Pos == null)
                Pos = new Vector2D();

            if (Heading == null)
                Heading = new Vector2D();

            Tagged = true;
        }

        public void removeBehaviour(Type behaviourType)
        {
            for (int i = 0; i < steeringBehaviours.Count; i++)
            {
                if (steeringBehaviours[i].GetType() == behaviourType)
                {
                    steeringBehaviours.RemoveAt(i);
                    return;
                }
            }
        }

        public override void Update(float timeElapsed)
        {
            if (steeringBehaviours.Count == 0)
            {
                return;
            }

            Vector2D SteeringForce = new Vector2D();

            SteeringForce = CombinedSteeringBehavior.Calculate();

            //Acceleration = Force/Mass
            Vector2D acceleration = SteeringForce / Mass;

            //update velocity
            Velocity += (acceleration * timeElapsed);

            //make sure vehicle does not exceed maximum velocity
            Velocity = Velocity.Truncate(MaxSpeed);

            //update the position

            this.Pos += Velocity * timeElapsed;
            GameObject.transform.position = new Vector2((float)this.Pos.X,(float) this.Pos.Y);

            //update the heading if the vehicle has a velocity greater than a very small
            //value
            if (Velocity.LengthSquared() > 0.00000001)
            {
                Heading = Velocity.Normalize();
                RotateHeadingToFacePosition(SteeringForce);
            }
        }

        // afgekeken van https://github.com/wangchen/Programming-Game-AI-by-Example-src
        public void RotateHeadingToFacePosition(Vector2D target)
        {
            Vector2D toTarget = ((target - this.Pos).Normalize());

            //first determine the angle between the heading vector and the target
            double angle = Math.Cos(Heading.Dot(toTarget));

            //return true if the player is facing the target
            if (angle < 0.00001) return;

            //clamp the amount to turn to the max turn rate
            if (angle > MaxTurnRate) angle = MaxTurnRate;

            //The next few lines use a rotation matrix to rotate the player's heading
            //vector accordingly
            Matrix RotationMatrix = new Matrix();

            //notice how the direction of rotation has to be determined when creating
            //the rotation matrix
            RotationMatrix *= Matrix.Rotate((float)(angle * Heading.Sign(toTarget)));
            Heading = Matrix.ToVector2D(RotationMatrix * new Matrix(Heading));
            RotationMatrix *= new Matrix(Velocity);

            GameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, Heading.ToVector2());

            //finally recreate m_vSide
            Side = Heading.Perp();
        }

        public bool CanWalkBetween(Vector2D pos1, Vector2D pos2)
        {
            return true;
        }


        public float Speed()
        {
            return Velocity.Length();
        }

        public override string ToString()
        {
            return String.Format("{0}", Velocity);
        }
    }
}
