using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.PriorityQueue;

namespace DataStructures.GraphStructure
{
    /// <summary>
    /// The graph class can be used to store values in a graph and to look for the shortest path between points in the graph.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Graph<T>
    {
        private Dictionary<T, GraphNode<T>> _nodeDictionary = new Dictionary<T, GraphNode<T>>();

        //Adds an edge between two nodes with the given cost.
        public void AddEdge(T start, T destination, double cost)
        {
            GraphNode<T> startNode = getNode(start);
            GraphNode<T> destNode = getNode(destination);
            startNode.Adjacent.Add(new Edge<T>(destNode, cost));
        }

        
        /// <summary>
        /// Algorithm to find the shortest weighted path in a graph. (Shortest distance)
        /// This method uses a startvalue as well as an end value that need to be given.
        /// If the endpoint is reached in the algorithm the distance to nodes further away will not be determined.
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        private void Dijkstra(T startValue, T endValue)
        {
            
            BinaryHeap<Edge<T>> priorityQueue = new BinaryHeap<Edge<T>>();

            if (!_nodeDictionary.ContainsKey(startValue) || !_nodeDictionary.ContainsKey(endValue))
            {
                throw new Exception();
            }
            foreach (GraphNode<T> node in _nodeDictionary.Values)
            {
                node.Reset();
            }
            
            GraphNode<T> start = _nodeDictionary[startValue];
            priorityQueue.Add(new Edge<T>(start, 0));
            start.Distance = 0;
            int nodesSeen = 0;
            while (!priorityQueue.IsEmpty && nodesSeen < _nodeDictionary.Count)
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

        public bool ContainsValue(T value)
        {
            return _nodeDictionary.ContainsKey(value);
        }

        //Returns all the values from the shortest path from the startValue to endValue. The linkedlist will start with the startValue.
        //Returns an empty linkedlist if there is no possible path.
        public LinkedList<T> GetPath(T startValue, T endValue)
        {
            Dijkstra(startValue, endValue);
            LinkedList<T> result = new LinkedList<T>();
            GraphNode<T> currentNode = getNode(endValue);
            if (currentNode.Prev != null)//There is a path between startValue and endValue
            {
                result.AddFirst(currentNode.Value);
                while (currentNode.Prev != null)
                {
                    currentNode = currentNode.Prev;
                    result.AddFirst(currentNode.Value);
                }
            }
            
            return result;
        }

        //Returns the node or creates the node if it does not exist.
        private GraphNode<T> getNode(T value)
        {
            if (_nodeDictionary.ContainsKey(value))
            {
                return _nodeDictionary[value];
            }
            else
            {
                GraphNode<T> node = new GraphNode<T>(value);
                _nodeDictionary[value] = node;
                return node;
            }
        }
    }
}