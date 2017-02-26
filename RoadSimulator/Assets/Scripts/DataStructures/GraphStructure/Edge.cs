using System;
using System.Collections.Generic;
using DataStructures.GraphStructure;

namespace DataStructures.GraphStructure
{

    //The Edge class provides data about a connected node and what the cost is to reach the node using that edge.
    public class Edge<T> : IComparable<Edge<T>>
    {
        public GraphNode<T> Destination { get; set; }
        public double Cost { get; set; }

        public Edge(GraphNode<T> destination, double cost)
        {
            Destination = destination;
            Cost = cost;
        }


        public int CompareTo(Edge<T> otherEdge)
        {
            double otherCost = otherEdge.Cost;
            int result;
            if (Cost < otherCost)
            {
                result = -1;
            }
            else if (Cost > otherCost)
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            return result;
        }
    }
}