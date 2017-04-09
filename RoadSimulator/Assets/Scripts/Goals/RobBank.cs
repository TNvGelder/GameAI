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
        private GameObject targetObj;

        public RobBank(MovingEntity owner, Bank bank) {
            Owner = owner;
            Name = "Rob bank";
            targetObj = bank.GameObject;
        }

        public override void Activate()
        {
            AddSubgoal(DestinationTravelService.GoTo(Owner, targetObj, "Rob the bank"));
            AddSubgoal(DestinationTravelService.GoTo(Owner, "Home", "Go Home"));

            base.Activate();
        }

        public override void OnSubGoalFinish(Goal subgoal)
        {
            var goTo = subgoal as MoveToPositionGoal;
            if (goTo != null)
            {
                if (goTo.Name == "Rob the bank")
                {
                    World.Instance.GetEntity<Bank>(targetObj).GetRobbed(Owner);
                }
            }
        }

        public override Status Process()
        {
            return base.Process();
        }
    }
}
