using System;
using System.Collections.Generic;

namespace Assets.Scripts.SteeringBehaviours
{
    public class WeightedSumPriorityCombinedSteeringBehavior : ICombinedSteeringBehavior
    {
        private MovingEntity _entity;
        public Dictionary<Type, float> weights = new Dictionary<Type, float>()
        {
            { typeof(ObstacleAvoidanceBehavior), 7f },
            { typeof(FleeBehaviour), 1f },
            { typeof(SeekBehaviour), 1f },
            { typeof(SeekMovingEntityBehaviour), 1f },
            { typeof(ExploreBehavior), 1f },
        };

        private Dictionary<Type, bool> enabled = new Dictionary<Type, bool>();

        public WeightedSumPriorityCombinedSteeringBehavior(MovingEntity entity)
        {
            _entity = entity;
        }

        public void EnableBehaviour(Type behaviour)
        {
            enabled[behaviour] = true;
        }

        public void DisableBehaviour(Type behaviour)
        {
            enabled[behaviour] = false;
        }

        public bool IsEnabled(Type behaviour)
        {
            return enabled.ContainsKey(behaviour) && enabled[behaviour];
        }

        public Vector2D Calculate()
        {
            Vector2D SteeringForce = new Vector2D();

            foreach(var behavior in _entity.SteeringBehaviours)
            {
                Type type = behavior.GetType();

                float weight = 1.0f;

                if (weights.ContainsKey(type))
                {
                    weight = weights[behavior.GetType()];
                }

                SteeringForce += behavior.Calculate() * weight;
            }

            return SteeringForce.Truncate(_entity.MaxSpeed);
        }
    }
}
