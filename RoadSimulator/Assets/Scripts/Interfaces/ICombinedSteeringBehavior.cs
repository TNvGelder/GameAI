using System;

namespace Assets.Scripts.SteeringBehaviours
{
    public interface ICombinedSteeringBehavior
    {
        Vector2D Calculate();
    }
}