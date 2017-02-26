using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.SteeringBehaviours
{
    public class FleeBehaviour : SteeringBehaviour
    {
        public MovingEntity FleeFrom{ get; set; }

        public FleeBehaviour(MovingEntity me, MovingEntity fleeFrom) : base(me)
        {
            FleeFrom = fleeFrom;
        }

        public override Vector2D Calculate()
        {
            Vector2D DesiredVelocity = ((ME.Pos.Clone() - FleeFrom.Pos.Clone())
                                        .Normalize()) * ME.MaxSpeed;

            return DesiredVelocity - ME.Velocity;
        }
    }
}
