using Assets.Scripts.SteeringBehaviours;
using System;

namespace Assets.Scripts.Goals
{
    public class Think : GoalComposite
    {
        public Think(MovingEntity owner)
        {
            Owner = owner;
            Name = "Think";
        }

        public override void Activate()
        {
            //Owner.SteeringBehaviours.Add(new ObstacleAvoidanceBehavior(Owner));

            if (Subgoals.Count == 0) { 
                Subgoals.Add(new Explore(Owner));
            }

            base.Activate();
        }

        public void AddMoveToPosition(Vector2D target)
        {
            RemoveAllSubgoals();
            Subgoals.Add(new MoveToPosition(Owner, target));
        }

        public override Status Process()
        {
            var s = base.Process();

            if (s == Status.Completed || s == Status.Failed)
            {
                status = Status.Inactive;
            }

            if (Owner.Fuel < 30.0 && Subgoals[0].GetType() != typeof(GetFuel))
            {
                RemoveAllSubgoals();
                Subgoals.Add(new GetFuel(Owner));
                return Status.Active;
            }

            return s;
        }
    }
}
