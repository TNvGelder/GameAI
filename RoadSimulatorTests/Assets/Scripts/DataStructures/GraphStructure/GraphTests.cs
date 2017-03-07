using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataStructures.GraphStructure;
using System.Linq;

namespace DroneAPI.DataStructures.GraphStructure.Tests
{
    [TestClass()]
    public class GraphTests
    {
        [TestMethod()]
        public void AddEdgeTest()
        {
            Graph<int> g = new Graph<int>();
            g.AddEdge(4,7,2);
            Assert.IsTrue(g.ContainsValue(4) && g.ContainsValue(7));
        }

        [TestMethod()]
        public void GetPathTestWithoutNodes()
        {
            Graph<int> g = new Graph<int>();
            bool succes = true;
            try
            {
                g.GetPath(3, 5);
            }
            catch
            {
                succes = false;
            }
            Assert.IsTrue(!succes);
        }

        [TestMethod()]
        public void GetPathTestWithNoPossiblePath()
        {
            Graph<int> g = new Graph<int>();
            g.AddEdge(3, 4, 3);
            g.AddEdge(5, 6, 3);
            LinkedList<int> path = g.GetPath(3, 5);
            Assert.IsTrue(path.Count == 0);
        }

        [TestMethod()]
        public void GetPathTestWithMultiplePaths()
        {
            Graph<string> g = new Graph<string>();
            g.AddEdge("A", "B", 3);
            g.AddEdge("A", "C", 5);
            g.AddEdge("B", "C", 1);
            g.AddEdge("C", "D", 3);
            g.AddEdge("B", "D", 7);
            LinkedList<string> path = g.GetPath("A", "D");
            LinkedList<string> expected = new LinkedList<string>();
            expected.AddFirst("D");
            expected.AddFirst("C");
            expected.AddFirst("B");
            expected.AddFirst("A");
            Assert.IsTrue(path.SequenceEqual(expected));
        }
        

    }
}