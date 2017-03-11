using Assets.Scripts.SteeringBehaviours;
using DataStructures.GraphStructure;

namespace Assets.Scripts.Goals
{
    public class TraverseEdge : Goal
    {
        Edge<Vector2D> Edge;

        public TraverseEdge(MovingEntity owner, Edge<Vector2D> edge) {
            Owner = owner;
            Edge = edge;
            Name = "TraverseEdge";
        }

        public override void Activate()
        {
            Owner.SteeringBehaviours.Add(new ObstacleAvoidanceBehavior(Owner));
            Owner.SteeringBehaviours.Add(new SeekBehaviour(Owner, Edge.Destination.Value));
            Owner.SteeringBehaviours.Add(new ArriveBehavior(Owner, Edge.Destination.Value, Deceleration.Normal));

            base.Activate();
        }

        public override Status Process()
        {
            if (Owner.IsAtPosition(Edge.Destination.Value))
            {
                return Status.Completed;
            }

            return base.Process();
        }

        public override void Terminate()
        {
            Owner.RemoveBehaviour(typeof(ObstacleAvoidanceBehavior));
            Owner.RemoveBehaviour(typeof(SeekBehaviour));
            Owner.RemoveBehaviour(typeof(ArriveBehavior));

            base.Terminate();
        }
    }
}
