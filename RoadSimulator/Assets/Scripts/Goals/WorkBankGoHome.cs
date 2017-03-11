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
            Goal goal = null;

            if (!HasWorked)
            {
                AddSubgoal(GoTo("Work"));
                HasWorked = true;
            } else if (!HasGoneToBank)
            {
                AddSubgoal(GoTo("Bank"));
                HasGoneToBank = true;
            } else if (!HasGoneHome)
            {
                AddSubgoal(GoTo("Home"));
                HasGoneHome = true;
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

            return new MoveToPosition(Owner, target, "Going " + tag);
        }

        public override Status Process()
        {
            return base.Process();
        }
    }
}
