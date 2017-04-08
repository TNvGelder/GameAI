using DataStructures.GraphStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataStructures.GraphStructure
{
    public abstract class PathFindingStrategy<T>
    {
        protected Graph<T> graph;

        public PathFindingStrategy(Graph<T> graph)
        {
            this.graph = graph;
        }


        public abstract void FindShortestPath(T startValue, T endValue);
    }
}
