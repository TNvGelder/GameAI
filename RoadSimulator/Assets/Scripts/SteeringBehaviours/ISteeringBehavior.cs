using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.SteeringBehaviours
{
    public interface ISteeringBehavior
    {
        MovingEntity ME { get; set; }
        Vector2D Calculate();
    }
}
