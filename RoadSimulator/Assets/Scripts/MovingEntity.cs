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

        public ISteeringBehavior SB { get; set; }

        public MovingEntity(GameObject gameObject, Vector2D pos, Vector2D size,  World w) : base(gameObject, pos, size, w)
        {
            Mass = 30;
            MaxSpeed = 150;
            Velocity = new Vector2D();
        }

        public override void Update(float timeElapsed)
        {
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
                Velocity = Velocity.Normalize();
            }
        }

        public override string ToString()
        {
            return String.Format("{0}", Velocity);
        }
    }
}
