using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.SteeringBehaviours
{
    public class WeightedSumPriorityCombinedSteeringBehavior : ICombinedSteeringBehavior
    {
        private MovingEntity _entity;
        public Dictionary<Type, float> weights = new Dictionary<Type, float>()
        {
            { typeof(ObstacleAvoidanceBehavior), 2f },
            { typeof(FleeBehaviour), 1f },
            { typeof(SeekBehaviour), 1f },
            { typeof(SeekMovingEntityBehaviour), 1f },
            { typeof(Explore), 1f },
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
                //if (!enabled.ContainsKey(type))
                //{
                //    enabled[type] = true;
                //}
                //if (enabled[type])
                //{
                //    SteeringForce += behavior.Calculate() * weight;
                //}
                SteeringForce += behavior.Calculate() * weight;
            }

            return SteeringForce.Truncate(_entity.MaxSpeed);
        }
    }
}
