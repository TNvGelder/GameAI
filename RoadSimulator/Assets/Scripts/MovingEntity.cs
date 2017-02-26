using Assets.Scripts.DataStructures;
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

        public ISteeringBehavior SB { get; set; }

        public MovingEntity(GameObject gameObject, Vector2D pos, Vector2D size,  World w) : base(gameObject, pos, size, w)
        {
            Mass = 30;
            MaxSpeed = 150;
            MaxTurnRate = 9999999999999;
            Velocity = new Vector2D();
        }

        public override void Update(float timeElapsed)
        {
            if (SB == null)
            {
                return;
            }

            Vector2D SteeringForce = SB.Calculate();
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

        public override string ToString()
        {
            return String.Format("{0}", Velocity);
        }
    }
}
