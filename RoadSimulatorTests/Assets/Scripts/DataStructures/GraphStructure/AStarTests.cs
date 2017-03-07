using Assets.Scripts;
using Assets.Scripts.DataStructures.GraphStructure.PathFindingStrategies;
using DataStructures.GraphStructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneAPI.DataStructures.GraphStructure.Tests
{
    [TestClass()]
    public class AStarTests
    {

        [TestMethod()]
        public void AStarWithoutNodes()
        {
            Graph<Vector2D> g = new Graph<Vector2D>();
            g.PathFindingStrategy = new AStarStrategy(g);
            bool succes = true;
            try
            {
                g.GetPath(new Vector2D(), new Vector2D());
            }
            catch
            {
                succes = false;
            }
            Assert.IsTrue(!succes);
        }

        [TestMethod()]
        public void AStarWithNoPossiblePath()
        {
            Graph<Vector2D> g = new Graph<Vector2D>();
            g.PathFindingStrategy = new AStarStrategy(g);
            g.AddEdge(new Vector2D(), new Vector2D(2, 0), 3);
            g.AddEdge(new Vector2D(4, 0), new Vector2D(6, 0), 3);
            LinkedList<Vector2D> path = g.GetPath(new Vector2D(), new Vector2D(6, 0));
            Assert.IsTrue(path.Count == 0);
        }

        [TestMethod()]
        public void AStarWithMultiplePaths()
        {
            Graph<Vector2D> g = new Graph<Vector2D>();
            g.PathFindingStrategy = new AStarStrategy(g);
            g.AddEdge(new Vector2D(), new Vector2D(4, 0), 3);
            g.AddEdge(new Vector2D(4, 0), new Vector2D(4, 7), 3);
            g.AddEdge(new Vector2D(), new Vector2D(-4, 0), 1);
            g.AddEdge(new Vector2D(-4, 0), new Vector2D(-4, 7), 1);
            g.AddEdge(new Vector2D(-4, 7), new Vector2D(4, 7), 1);

            LinkedList<Vector2D> path = g.GetPath(new Vector2D(), new Vector2D(4, 7));
            LinkedList<Vector2D> expected = new LinkedList<Vector2D>();

            expected.AddFirst(new Vector2D(4, 7));
            expected.AddFirst(new Vector2D(4, 0));
            expected.AddFirst(new Vector2D());
            Assert.IsTrue(path.SequenceEqual(expected));
        }
    }
}
