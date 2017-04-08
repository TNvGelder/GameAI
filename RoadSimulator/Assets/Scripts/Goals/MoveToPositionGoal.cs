using Assets.Scripts.SteeringBehaviours;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Goals
{
    public class MoveToPositionGoal : Goal
    {
        private FollowPathBehaviour followPath;
        private Vector2D Target;

        public MoveToPositionGoal(MovingEntity owner, Vector2D target) {
            Owner = owner;
            Target = target;
            Name = "MoveToPosition";
        }

        public MoveToPositionGoal(MovingEntity owner, Vector2D target, string name) : this(owner, target)
        {
            Name = name;
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
