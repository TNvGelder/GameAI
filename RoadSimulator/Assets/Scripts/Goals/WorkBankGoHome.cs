using Assets.Scripts.SteeringBehaviours;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    public class WorkBankGoHome : GoalComposite
    {
        bool HasWorked = false;
        bool HasGoneToBank = false;
        bool HasGoneHome = false;

        public FollowPathBehaviour followPath { get; private set; }

        public WorkBankGoHome(MovingEntity owner) {
            Owner = owner;
            Name = "WorkBankGoHome";
        }

        public override void Activate()
        {
            if (!HasWorked)
            {
                HasWorked = true;
                AddSubgoal(GoTo("Work"));
            } else if (!HasGoneToBank)
            {
                HasGoneToBank = true;
                AddSubgoal(GoTo("Bank"));
            } else if (!HasGoneHome)
            {
                HasGoneHome = true;
                AddSubgoal(GoTo("Home"));
            } else
            {
                return;
            }

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
