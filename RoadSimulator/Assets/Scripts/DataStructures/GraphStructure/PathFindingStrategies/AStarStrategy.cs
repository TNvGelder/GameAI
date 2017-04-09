using Assets.Scripts.DataStructures.GraphStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStructures.GraphStructure;
using DataStructures.PriorityQueue;


namespace Assets.Scripts.DataStructures.GraphStructure.PathFindingStrategies
{
    public class AStarStrategy : PathFindingStrategy<Vector2D>
    {
        /// <summary>
        /// The expected distance will be multiplied with this ratio. A higher ratio will make the algorithm faster.
        /// A lower ratio will make it more optimal.
        /// </summary>
        public float MinimumDistanceFactor { get; set; }

        public AStarStrategy(Graph<Vector2D> graph) : base(graph)
        {
            MinimumDistanceFactor = 2F;
        }

        public override void FindShortestPath(Vector2D startValue, Vector2D endValue)
        {
            BinaryHeap<Edge<Vector2D>> priorityQueue = new BinaryHeap<Edge<Vector2D>>();

            if (!graph.Nodes.ContainsKey(startValue) || !graph.Nodes.ContainsKey(endValue))
            {
                throw new Exception();
            }
            foreach (GraphNode<Vector2D> node in graph.Nodes.Values)
            {
                node.Reset();
            }

            GraphNode<Vector2D> start = graph.Nodes[startValue];
            GraphNode<Vector2D> goal = graph.Nodes[endValue];
            priorityQueue.Add(new Edge<Vector2D>(start, 0));
            start.Distance = 0;
            World world = World.Instance;
            HashSet<Vector2D> consideredEdges = new HashSet<Vector2D>();
            
            while (!priorityQueue.IsEmpty)
            {
                Edge<Vector2D> minPath = priorityQueue.DeleteMin();
                GraphNode<Vector2D> node = minPath.Destination;

                if (node.Scratch == 0)
                {
                    node.Scratch = 1;
                    if (node.Value.Equals(endValue))
                    {
                        if (world.IsSearchingPlayerPath)
                        {
                            world.ConsideredEdges = consideredEdges;
                        }
                        return;
                    }
                    foreach (Edge<Vector2D> e in node.Adjacent)
                    {
                        if (world.IsSearchingPlayerPath)
                        {
                            consideredEdges.Add(node.Value);
                            consideredEdges.Add(e.Destination.Value);
                        }
                        GraphNode<Vector2D> adjacentNode = e.Destination;
                        double edgeCost = e.Cost + heuristic(e.Destination, goal);
                        if (adjacentNode.Distance > node.Distance + edgeCost)
                        {
                            adjacentNode.Distance = node.Distance + edgeCost;
                            adjacentNode.Prev = node;
                            Edge<Vector2D> newEdge = new Edge<Vector2D>(adjacentNode, adjacentNode.Distance);
                            priorityQueue.Add(newEdge);
                        }
                    }
                }
            }

            

        }

        /// <summary>
        /// finds 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private float heuristic(GraphNode<Vector2D> node, GraphNode<Vector2D> goal)
        {
            
            float dx = Math.Abs(node.Value.X - goal.Value.X);
            float dy = Math.Abs(node.Value.Y - goal.Value.Y);
            return MinimumDistanceFactor * (dx + dy);
        }

    }
}
