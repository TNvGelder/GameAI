using System;
using System.Collections.Generic;

namespace DataStructures.GraphStructure
{
    public class GraphNode<T>
    {
        public T Value { get; set; }
        public List<Edge<T>> Adjacent { get; set; }
        public GraphNode<T> Prev { get; set; }
        public double Distance { get; set; }
        public int Scratch { get; set; }

        public GraphNode(T value)
        {
            Value = value;
            Adjacent = new List<Edge<T>>();
        }

        public void Reset()
        {
            Distance = double.MaxValue;
            Prev = null;
            Scratch = 0;
        }
    }
}