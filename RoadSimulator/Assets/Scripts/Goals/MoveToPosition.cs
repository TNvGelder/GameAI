using Assets.Scripts.SteeringBehaviours;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Goals
{
    public class MoveToPosition : GoalComposite
    {
        private Explore playerExplore;
        private FollowPathBehaviour followPath;
        private Vector2D Target;

        public MoveToPosition(MovingEntity owner, Vector2D target) {
            Owner = owner;
            Target = target;
            Name = "MoveToPosition";
        }

        public override void Activate()
        {
            Owner.CombinedSteeringBehavior.DisableBehaviour(typeof(Explore));
            Owner.CombinedSteeringBehavior.EnableBehaviour(typeof(FollowPathBehaviour));
            LinkedList<Vector2D> waypoints;
            Owner.PathPlanner.CreatePathToPosition(Target, out waypoints);
            followPath = new FollowPathBehaviour(Owner);
            followPath.Path = new Path(waypoints);
            Owner.SteeringBehaviours.Add(new FollowPathBehaviour(Owner));

            base.Activate();
        }
    }
}
