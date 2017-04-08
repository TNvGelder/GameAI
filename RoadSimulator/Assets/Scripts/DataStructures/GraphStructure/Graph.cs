using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.PriorityQueue;
using Assets.Scripts.DataStructures.GraphStructure;
using Assets.Scripts.DataStructures.GraphStructure.PathFindingStrategies;

namespace DataStructures.GraphStructure
{
    /// <summary>
    /// The graph class can be used to store values in a graph and to look for the shortest path between points in the graph.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Graph<T>
    {
        public Dictionary<T, GraphNode<T>> Nodes { get; protected set; }

        public PathFindingStrategy<T> PathFindingStrategy { get; set; }

        public Graph()
        {
            PathFindingStrategy =  new DijkstraStrategy<T>(this);
            Nodes = new Dictionary<T, GraphNode<T>>();
        }

        //Adds an edge between two nodes with the given cost.
        public void AddEdge(T start, T destination, double cost)
        {
            GraphNode<T> startNode = getNode(start);
            GraphNode<T> destNode = getNode(destination);
            startNode.Adjacent.Add(new Edge<T>(destNode, cost));
        }

        public bool ContainsValue(T value)
        {
            return Nodes.ContainsKey(value);
        }

        //Returns all the values from the shortest path from the startValue to endValue. The linkedlist will start with the startValue.
        //Returns an empty linkedlist if there is no possible path.
        public LinkedList<T> GetPath(T startValue, T endValue)
        {
            PathFindingStrategy.FindShortestPath(startValue, endValue);
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
            if (Nodes.ContainsKey(value))
            {
                return Nodes[value];
            }
            else
            {
                GraphNode<T> node = new GraphNode<T>(value);
                Nodes[value] = node;
                return node;
            }
        }
    }
}