using Assets.Scripts.SteeringBehaviours;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    public class PatrolGoal : GoalComposite
    {
        public PatrolGoal(MovingEntity owner) {
            Owner = owner;
            Name = "Patrol";
        }

        public override void Activate()
        {
            AddSubgoal(DestinationTravelService.GoTo(Owner, "GasStation", "GasStation"));
            AddSubgoal(DestinationTravelService.GoTo(Owner, "Home", "Home"));
            AddSubgoal(DestinationTravelService.GoTo(Owner, "Work", "Work"));
            AddSubgoal(DestinationTravelService.GoTo(Owner, "Bank", "Bank"));

            base.Activate();
        }

        public override Status Process()
        {
            return base.Process();
        }
    }
}
