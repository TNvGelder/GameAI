using Assets.Scripts.SteeringBehaviours;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    public class WorkBankGoHomeGoal : GoalComposite
    {
        public FollowPathBehaviour followPath { get; private set; }
        private GameObject targetObj;

        public WorkBankGoHomeGoal(MovingEntity owner) {
            Owner = owner;
            Name = "WorkBankGoHome";
        }

        public override void Activate()
        {
            AddSubgoal(DestinationTravelService.GoTo(Owner,"Work"));
            AddSubgoal(DestinationTravelService.GoTo(Owner, "Bank", out targetObj));
            AddSubgoal(DestinationTravelService.GoTo(Owner, "Home"));

            base.Activate();
        }

        public override void OnSubGoalFinish(Goal subgoal)
        {
            var goTo = subgoal as MoveToPositionGoal;
            if (goTo != null)
            {
                if (goTo.Name == "Going to Bank") {
                    
                    World.Instance.GetEntity<Bank>(targetObj).AddMoney(200);
                }
            }
        }


        public override Status Process()
        {
            return base.Process();
        }
    }
}
