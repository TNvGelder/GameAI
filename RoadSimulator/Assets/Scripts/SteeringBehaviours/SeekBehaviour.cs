using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.SteeringBehaviours
{
    public class SeekBehaviour : SteeringBehaviour
    {
        public MovingEntity Target { get; set; }

        public SeekBehaviour(MovingEntity me, MovingEntity target) : base(me)
        {
            Target = target;
        }

        public override Vector2D Calculate()
        {
            Vector2D DesiredVelocity = ((Target.Pos.Clone() - ME.Pos.Clone())
                                        .Normalize()) * ME.MaxSpeed;

            return DesiredVelocity - ME.Velocity;
        }
    }
}
