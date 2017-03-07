using DataStructures.GraphStructure;
using DataStructures.PriorityQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataStructures.GraphStructure.PathFindingStrategies
{
    public class DijkstraStrategy<T> : PathFindingStrategy<T>
    {
        public DijkstraStrategy(Graph<T> graph) : base(graph)
        {
        }

        /// <summary>
        /// Algorithm to find the shortest weighted path in a graph. (Shortest distance)
        /// This method uses a startvalue as well as an end value that need to be given.
        /// If the endpoint is reached in the algorithm the distance to nodes further away will not be determined.
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        public override void FindShortestPath(T startValue, T endValue)
        {
            BinaryHeap<Edge<T>> priorityQueue = new BinaryHeap<Edge<T>>();

            if (!graph.Nodes.ContainsKey(startValue) || !graph.Nodes.ContainsKey(endValue))
            {
                throw new Exception();
            }
            foreach (GraphNode<T> node in graph.Nodes.Values)
            {
                node.Reset();
            }

            GraphNode<T> start = graph.Nodes[startValue];
            priorityQueue.Add(new Edge<T>(start, 0));
            start.Distance = 0;
            int nodesSeen = 0;
            while (!priorityQueue.IsEmpty)
            {
                Edge<T> minPath = priorityQueue.DeleteMin();
                GraphNode<T> node = minPath.Destination;

                if (node.Scratch == 0)//Not yet processed node
                {
                    node.Scratch = 1;
                    nodesSeen++;
                    if (node.Value.Equals(endValue))
                    {
                        return;
                    }
                    foreach (Edge<T> e in node.Adjacent)
                    {
                        GraphNode<T> adjacentNode = e.Destination;
                        double edgeCost = e.Cost;

                        if (adjacentNode.Distance > node.Distance + edgeCost)
                        {
                            adjacentNode.Distance = node.Distance + edgeCost;
                            adjacentNode.Prev = node;
                            priorityQueue.Add(new Edge<T>(adjacentNode, adjacentNode.Distance));
                        }
                    }
                }
            }

        }
    }
}
