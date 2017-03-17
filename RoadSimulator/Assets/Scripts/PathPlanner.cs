using DataStructures.GraphStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class PathPlanner
    {
        public Graph<Vector2D> graph;
        public Graph<Vector2D> Graph { get { return graph; } }
        public MovingEntity Me;

        public PathPlanner(Graph<Vector2D> graph)
        {
            this.graph = graph;
        }

        public Vector2D GetClosestNodePosToPosition(Vector2D pos)
        {
            GraphNode<Vector2D> node = GetClosestNodeToPosition(pos);
            if (node != null)
            {
                return node.Value;
            }else
            {
                return null;
            }
        }

        private GraphNode<Vector2D> GetClosestNodeToPosition(Vector2D pos)
        {
            if (Me == null)
            {
                return null;
            }
            float closestSoFar = float.MaxValue;
            Vector2D closestNodePos = null;

            foreach (Vector2D nodePos in graph.Nodes.Keys)
            {
                float distance = (pos - nodePos).LengthSquared();
                if (distance < closestSoFar && Me.CanWalkBetween(pos, nodePos))
                {
                    closestNodePos = nodePos;
                    closestSoFar = distance;
                }
            }

            if (closestNodePos != null)
            {
                return graph.Nodes[closestNodePos];
            }
            return null;
        }

        public bool CreatePathToPosition(Vector2D TargetPos, out LinkedList<Vector2D> path)
        {
            path = new LinkedList<Vector2D>();

            GraphNode<Vector2D> closestNodeToMe = GetClosestNodeToPosition(Me.Pos);
            GraphNode<Vector2D> closestNodeToTarget = GetClosestNodeToPosition(TargetPos);
            if (closestNodeToMe != null && closestNodeToTarget != null)
            {
                path = graph.GetPath(closestNodeToMe.Value, closestNodeToTarget.Value);
                if (path.Count > 0)
                {
                    path.AddLast(TargetPos);
                }
                return (path.Count > 0);

            }

            return false;
        }

    }
}
