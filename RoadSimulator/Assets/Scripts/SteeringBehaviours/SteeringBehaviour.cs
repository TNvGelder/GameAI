using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.SteeringBehaviours
{
    public abstract class SteeringBehaviour : ISteeringBehavior
    {
        public MovingEntity ME { get; set; }
        public abstract Vector2D Calculate();

        public SteeringBehaviour(MovingEntity me)
        {
            ME = me;
        }
    }
}
