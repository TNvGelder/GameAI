using Assets.Scripts.SteeringBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    public class RobBank : GoalComposite
    {
        private FollowPathBehaviour followPath;
        private Vector2D Target;

        public RobBank(MovingEntity owner) {
            Owner = owner;
            Name = "Rob bank";
        }

        public override void Activate()
        {
            AddSubgoal(GoTo("Bank", "Rob the bank"));
            AddSubgoal(GoTo("Home", "Go Home"));

            base.Activate();
        }

        public override void OnSubGoalFinish(Goal subgoal)
        {
            var goTo = subgoal as MoveToPosition;
            if (goTo != null)
            {
                if (goTo.Name == "Rob the bank")
                {
                    World.Instance.GetEntity<Bank>().GetRobbed(Owner);
                }
            }
        }

        public Goal GoTo(string tag, string text)
        {
            var obj = GameObject.FindGameObjectsWithTag(tag)[0];
            var target = new Vector2D(obj.transform.position.x, obj.transform.position.y);

            return new MoveToPosition(Owner, target, text);
        }

        public override Status Process()
        {
            return base.Process();
        }
    }
}
