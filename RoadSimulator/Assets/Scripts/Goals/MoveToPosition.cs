using Assets.Scripts.SteeringBehaviours;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Goals
{
    public class MoveToPosition : Goal
    {
        private FollowPathBehaviour followPath;
        private Vector2D Target;

        public MoveToPosition(MovingEntity owner, Vector2D target) {
            Owner = owner;
            Target = target;
            Name = "MoveToPosition";
        }

        public override void Activate()
        {
            LinkedList<Vector2D> waypoints;
            Owner.PathPlanner.CreatePathToPosition(Target, out waypoints);
            followPath = new FollowPathBehaviour(Owner);
            followPath.Path = new Path(waypoints);
            Owner.SteeringBehaviours.Add(followPath);

            base.Activate();
        }

        public override void Terminate()
        {
            Owner.RemoveBehaviour(typeof(Scripts.SteeringBehaviours.FollowPathBehaviour));

            base.Terminate();
        }

        public override Status Process()
        {
            if (Owner.IsAtPosition(Target))
            {
                status = Status.Completed;
            }

            return base.Process();
        }
    }
}
