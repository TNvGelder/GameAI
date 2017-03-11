using Assets.Scripts.SteeringBehaviours;
using System;
using System.Linq;

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

            if (Subgoals.Count == 0)
            {
                Subgoals.Add(Determine());
            }

            base.Activate();
        }

        public Goal Determine()
        {
            var r = World.instance.Random.Next(1000, 2000);

            if (r < 1500)
            {
                return new Explore(Owner);
            } else
            {
                return new WorkBankGoHome(Owner);
            }
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
                var goal = new GetFuel(Owner);

                var subGoal = Subgoals[0] as GoalComposite;
                if (subGoal != null)
                {
                    if (subGoal.Subgoals.Any() && !(subGoal.Subgoals[0] is GetFuel))
                    {
                        subGoal.Subgoals.Insert(0, goal);
                    }
                } else
                {
                    RemoveAllSubgoals();
                    Subgoals.Add(goal);
                }

                return Status.Active;
            }

            return s;
        }
    }
}
