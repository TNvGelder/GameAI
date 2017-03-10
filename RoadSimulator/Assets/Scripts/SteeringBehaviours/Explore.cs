using DataStructures.GraphStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours
{
    public class Explore : FollowPathBehaviour
    {

        public Explore(MovingEntity me) : base(me)
        {
            
        }


        private void chooseNextPos(int decisionAmount,Vector2D currentPos, Vector2D prevPos)
        {
            Dictionary<Vector2D,GraphNode<Vector2D>> nodes = ME.PathPlanner.Graph.Nodes;
            GraphNode<Vector2D> node;
        }

        public override Vector2D Calculate()
        {
            
            LinkedList<Vector2D> wayPoints = Path.WayPoints;
            if (wayPoints.Count > 1)
            {
                return base.Calculate();
            }else if( wayPoints.Count == 0)
            {
                Vector2D closestNodePos = ME.PathPlanner.GetClosestNodePosToPosition(ME.Pos);
                wayPoints.AddLast(closestNodePos);
                return Calculate();
            }else
            {
               // wayPoints.First;
                return Calculate();
            }
        }
    }
}
