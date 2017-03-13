using Assets.Scripts.SteeringBehaviours;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    public class WorkBankGoHome : GoalComposite
    {
        public FollowPathBehaviour followPath { get; private set; }

        public WorkBankGoHome(MovingEntity owner) {
            Owner = owner;
            Name = "WorkBankGoHome";
        }

        public override void Activate()
        {
            AddSubgoal(GoTo("Work"));
            AddSubgoal(GoTo("Bank"));
            AddSubgoal(GoTo("Home"));

            base.Activate();
        }

        public Goal GoTo(string tag)
        {
            var obj = GameObject.FindGameObjectsWithTag(tag)[0];
            var target = new Vector2D(obj.transform.position.x, obj.transform.position.y);

            return new MoveToPosition(Owner, target, "Going to " + tag);
        }

        public override Status Process()
        {
            return base.Process();
        }
    }
}
