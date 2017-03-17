using System;

namespace Assets.Scripts.SteeringBehaviours
{
    public interface ICombinedSteeringBehavior
    {
        Vector2D Calculate();
        void EnableBehaviour(Type behaviour);
        void DisableBehaviour(Type behaviour);
        bool IsEnabled(Type behaviour);
    }
}