using Assets.Scripts.SteeringBehaviours;
using System.Collections.Generic;

namespace Assets.Scripts.Goals
{
    public class Explore : Goal
    {
        public Explore(MovingEntity owner) {
            Owner = owner;
            Name = "Explore";
        }

        public override void Activate()
        {
            Owner.SteeringBehaviours.Add(new Scripts.SteeringBehaviours.Explore(Owner));
            //Owner.SteeringBehaviours.Add(followPath);

            base.Activate();
        }

        public override void Terminate()
        {
            Owner.RemoveBehaviour(typeof(Scripts.SteeringBehaviours.Explore));

            base.Terminate();
        }
    }
}
