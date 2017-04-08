using Assets.Scripts.DataStructures;
using Assets.Scripts.Goals;
using Assets.Scripts.SteeringBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class MovingEntity : BaseGameEntity
    {
        public Vector2D Velocity { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxTurnRate { get; set; }
        public Vector2D Heading { get; set; }
        public Vector2D Side { get; set; }
        public List<ISteeringBehavior> SteeringBehaviours { get; set; }
        public ICombinedSteeringBehavior CombinedSteeringBehavior { get; set; }
        public PathPlanner PathPlanner { get; internal set; }
        public ThinkGoal Think { get; set; }
        public float Fuel { get; internal set; }
        public bool IsCop { get; internal set; }

        public MovingEntity(GameObject gameObject, Vector2D pos, Vector2D size, PathPlanner pathPlanner) : base(gameObject, pos, size)
        {
            PathPlanner = pathPlanner;
            PathPlanner.Me = this;
            Mass = 1;
            Fuel = MyWorld.Random.Next(30, 100); // random initial fuel value, between 30 and 100
            MaxSpeed = 10;
            BRadius = 2;
            MaxTurnRate = 9999999999999;
            SteeringBehaviours = new List<ISteeringBehavior>();
            CombinedSteeringBehavior = new WeightedSumPriorityCombinedSteeringBehavior(this);

            if (gameObject.name == "Police")
            {
                IsCop = true;
            }

            if (Velocity == null)
                Velocity = new Vector2D();

            if (Side == null)
                Side = new Vector2D();

            if (Pos == null)
                Pos = new Vector2D();

            if (Heading == null)
                Heading = new Vector2D();

            Tagged = true;

            Think = new ThinkGoal(this);
        }

        public ISteeringBehavior GetBehaviour(Type behaviourType)
        {

            for (int i = 0; i < SteeringBehaviours.Count; i++)
            {
                if (SteeringBehaviours[i].GetType() == behaviourType)
                {
                    return SteeringBehaviours[i];
                }
            }
            return null;
        }

        public void RemoveBehaviour(Type behaviourType)
        {
            for (int i = 0; i < SteeringBehaviours.Count; i++)
            {
                if (SteeringBehaviours[i].GetType() == behaviourType)
                {
                    SteeringBehaviours.RemoveAt(i);
                    return;
                }
            }
        }

        public override void Update(float timeElapsed)
        {
            if (Fuel <= 0)
            {
                return;
            }

            Think.Process();

            if (SteeringBehaviours.Count == 0)
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
            GameObject.transform.position = new Vector2(Pos.X, Pos.Y);

            // only decrease fuel when moving
            if (Velocity.Length() > 0.1)
            {
                Fuel -= 0.02f * 2/3;
            }

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
            Vector2D toTarget = ((target - Pos).Normalize());

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

        public bool IsAtPosition(Vector2D pos)
        {
            var tolerance = 4f;

            return Pos.Vec2DDistanceSq(pos) < tolerance * tolerance;
        }

        public override string ToString()
        {
            return string.Format("{0}", Velocity);
        }
    }
}
