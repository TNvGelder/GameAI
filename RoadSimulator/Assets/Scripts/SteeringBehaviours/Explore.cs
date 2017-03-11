using DataStructures.GraphStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.SteeringBehaviours
{
    public class Explore : FollowPathBehaviour
    {
        private Random rnd;

        public Explore(MovingEntity me) : base(me)
        {
            rnd = new Random();
        }


        private void chooseNextPos(int decisionAmount,Vector2D currentPos, Vector2D prevPos)
        {
            Dictionary<Vector2D,GraphNode<Vector2D>> nodes = ME.PathPlanner.Graph.Nodes;

            if (nodes.ContainsKey(currentPos) && nodes.ContainsKey(prevPos) && decisionAmount > 0)
            {
                GraphNode<Vector2D> node = nodes[currentPos];
                GraphNode<Vector2D> prevNode = nodes[prevPos];
                int count = node.Adjacent.Count;
                if (count > 0)
                {
                    int i = rnd.Next(count - 1);

                    Edge<Vector2D> edge = node.Adjacent[i];
                    if (edge.Destination == prevNode && count > 1)
                    {
                        edge = node.Adjacent[Math.Abs(i - 1)];
                    }
                    Vector2D nextPos = edge.Destination.Value;
                    Path.WayPoints.AddLast(edge.Destination.Value);
                    chooseNextPos(decisionAmount - 1, nextPos, currentPos);
                }
            }
        }

        public override Vector2D Calculate()
        {
            
            LinkedList<Vector2D> wayPoints = Path.WayPoints;
            if (wayPoints.Count > 1)
            {
                return base.Calculate();
            }else if( wayPoints.Count == 0)
            {
                Vector2D closestNodePos = ME.PathPlanner.GetClosestNodePosToPosition(ME.Pos);
                wayPoints.AddLast(closestNodePos);
                return Calculate();
            }else
            {
                if (Path.CurrentWayPoint == null)
                {
                    Path.CurrentWayPoint = Path.WayPoints.First.Value;
                }
                chooseNextPos(3, Path.WayPoints.First.Value, Path.CurrentWayPoint);
               // wayPoints.First;
                return Calculate();
            }
        }
    }
}
