using Assets.Scripts.SteeringBehaviours;
using System.Collections.Generic;

namespace Assets.Scripts.Goals
{
    public class ExploreGoal : Goal
    {
        public ExploreGoal(MovingEntity owner) {
            Owner = owner;
            Name = "Explore";
        }

        public override void Activate()
        {
            Owner.SteeringBehaviours.Add(new Scripts.SteeringBehaviours.ExploreBehavior(Owner));

            base.Activate();
        }

        public override void Terminate()
        {
            Owner.RemoveBehaviour(typeof(Scripts.SteeringBehaviours.ExploreBehavior));

            base.Terminate();
        }
    }
}
