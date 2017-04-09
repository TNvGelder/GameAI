using Assets.Scripts.Goals.ThinkStrategies;
using Assets.Scripts.SteeringBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Goals
{
    public class ThinkGoal : GoalComposite
    {
        

        public List<IThinkStrategy> ThinkStrategies = new List<IThinkStrategy>
        {
            new FuelStrategy()
        };

        public ThinkGoal(MovingEntity owner)
        {
            Owner = owner;
            Name = "Think";
        }

        public override void Activate()
        {
            Owner.SteeringBehaviours.Add(new ObstacleAvoidanceBehavior(Owner));
            Owner.SteeringBehaviours.Add(new Separation(Owner));
            if (Subgoals.Count == 0)
            {
                AddSubgoal(Determine());
            }

            base.Activate();
        }

        internal void InsertPriorityGoal(GoalComposite goal, Goal priorityGoal)
        {
            goal.Subgoals.Insert(0, priorityGoal);

            if (goal.Subgoals.Count > 1)
            {
                goal.Subgoals[1].Terminate();
            }
        }

        public Goal Determine()
        {
            if (Owner.IsCop)
            {
                return new PatrolGoal(Owner);
            }
            else if (!IsAnyoneDoing<SeekGoal>())
            {
                var entities = World.Instance.GetMovingEntities();
                MovingEntity target = null;

                do
                {
                    target = entities[World.Instance.Random.Next(0, entities.Count - 1)];
                } while (target == null || target == Owner);

                return new SeekGoal(Owner, target);
            } else if (!IsAnyoneDoing<ExploreGoal>())
            {
                return new ExploreGoal(Owner);
            }
            else if (!IsAnyoneDoing<WorkBankGoHomeGoal>())
            {
                return new WorkBankGoHomeGoal(Owner);
            }
            else
            {
                var r = World.Instance.Random.Next(1000, 2000);
                if(r < 1500)
                {
                    return new ExploreGoal(Owner);
                } else
                {
                    return new WorkBankGoHomeGoal(Owner);
                }
            }
        }
        
        public static bool IsAnyoneDoing<T>()
        {
            return World.Instance.GetMovingEntities().Any(x =>
            {
                if (x.Think == null || x.Think.Subgoals == null)
                    return false;

                return x.Think.Subgoals.Count > 0 && x.Think.Subgoals[0] is T;
            });
        }

        public  void SetGoal(Goal goal)
        {
            RemoveAllSubgoals();
            Subgoals.Add(goal);
        }

        public void AddMoveToPosition(Vector2D target)
        {
            SetGoal(new MoveToPositionGoal(Owner, target));
        }

        public override Status Process()
        {
            var s = base.Process();

            ThinkStrategies.ForEach(x => x.Evaluate(Owner, this));

            if (s == Status.Completed || s == Status.Failed)
            {
                status = Status.Inactive;
                s = Status.Inactive;
            }

            return s;
        }
    }
}
