using Assets.Scripts.SteeringBehaviours;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    public class GetFuelGoal : Goal
    {
        public FollowPathBehaviour followPath { get; private set; }
        public Vector2D Target { get; set; }

        public GetFuelGoal(MovingEntity owner) {
            Owner = owner;
            Name = "Get Fuel";
        }

        public override void Activate()
        {
            Target = null;
            float nearestDistance = int.MaxValue;

            foreach(var obj in GameObject.FindGameObjectsWithTag("GasStation"))
            {
                var gasStationPos = new Vector2D(obj.transform.position.x, obj.transform.position.y);
                var ownerPos = new Vector2D(Owner.Pos.X, Owner.Pos.Y);
                var distance = (gasStationPos - ownerPos).Length();

                if (distance < nearestDistance)
                {
                    Target = gasStationPos;
                    nearestDistance = distance;
                }
            }

            LinkedList<Vector2D> waypoints;
            Owner.PathPlanner.CreatePathToPosition(Target, out waypoints);
            followPath = new FollowPathBehaviour(Owner);
            followPath.Path = new Path(waypoints);

            Owner.SteeringBehaviours.Add(followPath);

            base.Activate();
        }

        public override void Terminate()
        {
            Owner.RemoveBehaviour(typeof(FollowPathBehaviour));

            base.Terminate();
        }

        public override Status Process()
        {
            if (Target != null && Owner.IsAtPosition(Target))
            {
                Owner.Fuel = 100;
                status = Status.Completed;
            }

            return base.Process();
        }
    }
}
