using Assets.Scripts.Goals.ThinkStrategies;
using Assets.Scripts.SteeringBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Goals
{
    public class Think : GoalComposite
    {
        public List<IThinkStrategy> ThinkStrategies = new List<IThinkStrategy>
        {
            new FuelStrategy()
        };

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
                AddSubgoal(Determine());
            }

            base.Activate();
        }

        internal void InsertPriorityGoal(GoalComposite goal, Goal priorityGoal)
        {
            goal.Subgoals.Insert(0, priorityGoal);
            goal.Subgoals[1].Terminate();
        }

        public Goal Determine()
        {
            if (Owner.IsCop)
            {
                return new Patrol(Owner);
            }
            else if (!IsAnyoneDoing<Seek>())
            {
                var entities = World.Instance.GetMovingEntities();
                MovingEntity target = null;

                do
                {
                    target = entities[World.instance.Random.Next(0, entities.Count - 1)];
                } while (target == null || target == Owner);

                return new Seek(Owner, target);
            } else if (!IsAnyoneDoing<Explore>())
            {
                return new Explore(Owner);
            }
            else if (!IsAnyoneDoing<WorkBankGoHome>())
            {
                return new WorkBankGoHome(Owner);
            }
            else if (!IsAnyoneDoing<RobBank>() && !Owner.IsCop)
            {
                return new RobBank(Owner);
            }
            else
            {
                var r = World.instance.Random.Next(1000, 2000);
                if(r < 1500)
                {
                    return new Explore(Owner);
                } else
                {
                    return new WorkBankGoHome(Owner);
                }
            }
        }
        
        public bool IsAnyoneDoing<T>()
        {
            return World.Instance.GetMovingEntities().Any(x => x.Think.Subgoals.Count > 0 && x.Think.Subgoals[0] is T);
        }

        public  void SetGoal(Goal goal)
        {
            RemoveAllSubgoals();
            Subgoals.Add(goal);
        }

        public void AddMoveToPosition(Vector2D target)
        {
            SetGoal(new MoveToPosition(Owner, target));
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
