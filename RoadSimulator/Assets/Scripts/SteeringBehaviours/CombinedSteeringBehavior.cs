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
            { typeof(ObstacleAvoidanceBehavior), 5f },
            { typeof(FleeBehaviour), 1f },
            { typeof(SeekBehaviour), 1f }
        };

        public WeightedSumPriorityCombinedSteeringBehavior(MovingEntity entity)
        {
            _entity = entity;
        }

        public Vector2D Calculate()
        {
            Vector2D SteeringForce = new Vector2D();

            foreach(var behavior in _entity.SteeringBehaviours)
            {
                SteeringForce += behavior.Calculate() * weights[behavior.GetType()];
            }

            return SteeringForce.Truncate(_entity.MaxSpeed);
        }
    }
}
