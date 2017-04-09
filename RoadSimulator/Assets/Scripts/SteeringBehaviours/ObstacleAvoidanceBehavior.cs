using Assets.Scripts.DataStructures;
using System;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours
{
    public class ObstacleAvoidanceBehavior : SteeringBehaviour
    {
        private const double MinDetectionBoxLength = 1.5;

        public ObstacleAvoidanceBehavior(MovingEntity me) : base(me)
        {
        }

        public override Vector2D Calculate()
        {
            var world = ME.MyWorld;

            var m_dDBoxLength = MinDetectionBoxLength +
                  (ME.Speed() / ME.MaxSpeed) *
                  MinDetectionBoxLength;

            world.TagObstaclesWithinViewRange(ME, m_dDBoxLength);

            MovingEntity ClosestIntersectingObstacle = null;

            double DistToClosestIP = double.MaxValue;

            Vector2D LocalPosOfClosestObstacle = null;

            foreach (var cur in world.GetMovingEntities())
            {
                if (cur.Tagged)
                {
                    //Debug.Log(ME.ID + " tagged " + cur.ID);
                    Vector2D LocalPos = PointToLocalSpace(cur.Pos,
                                             ME.Heading,
                                             ME.Side,
                                             ME.Pos);

                    if (LocalPos.X >= 0)
                    {
                        double ExpandedRadius = cur.BRadius + ME.BRadius;

                        //if the distance from the x axis to the object's position is less
                        //than its radius + half the width of the detection box then there
                        //is a potential intersection.
                        if (LocalPos.Y < ExpandedRadius)
                        {
                            double cX = LocalPos.X;
                            double cY = LocalPos.Y;

                            //we only need to calculate the sqrt part of the above equation once
                            double SqrtPart = Math.Sqrt(ExpandedRadius * ExpandedRadius - cY * cY);

                            double ip = cX - SqrtPart;

                            if (ip <= 0.0)
                            {
                                ip = cX + SqrtPart;
                            }

                            //test to see if this is the closest so far. If it is keep a
                            //record of the obstacle and its local coordinates

                            if (ip < DistToClosestIP)
                            {
                                DistToClosestIP = ip;

                                ClosestIntersectingObstacle = cur;

                                LocalPosOfClosestObstacle = LocalPos;
                            }
                        }
                    }
                }
            }

            //if we have found an intersecting obstacle, calculate a steering 
            //force away from it
            Vector2D SteeringForce = new Vector2D();

            if (ClosestIntersectingObstacle != null)
            {
                //the closer the agent is to an object, the stronger the 
                //steering force should be
                double multiplier = 5.0 + (m_dDBoxLength - LocalPosOfClosestObstacle.X) /
                                    m_dDBoxLength;


                //calculate the lateral force
                SteeringForce.Y = (float)((ClosestIntersectingObstacle.BRadius -
                                   LocalPosOfClosestObstacle.Y) * multiplier);

                //apply a braking force proportional to the obstacles distance from
                //the vehicle. 
                const double BrakingWeight = 2;

                SteeringForce.X = (float)((ClosestIntersectingObstacle.BRadius -
                                   LocalPosOfClosestObstacle.X) *
                                   BrakingWeight);
            }

            //finally, convert the steering vector from local to world space
            return VectorToWorldSpace(SteeringForce,
                                      ME.Heading,
                                      ME.Side);
        }

        public Vector2D PointToLocalSpace(Vector2D point, Vector2D AgentHeading, Vector2D AgentSide, Vector2D AgentPosition)
        {
            //make a copy of the point
            Vector2D TransPoint = point;

            double Tx = -AgentPosition.Dot(AgentHeading);
            double Ty = -AgentPosition.Dot(AgentSide);

            //create the transformation matrix
            var matTransform = new Matrix(AgentHeading.X, AgentSide.Y, 0, 
                                          AgentHeading.Y, AgentSide.Y, 0, 
                                          (float)Tx, (float)Ty, 0);

            return matTransform * TransPoint;
        }


        public Vector2D VectorToWorldSpace(Vector2D vec,
                                     Vector2D AgentHeading,
                                     Vector2D AgentSide)
        {
            //make a copy of the point
            Vector2D TransVec = vec.Clone();

            return Matrix.Rotate(AgentHeading, AgentSide) * (TransVec);
        }
    }
}
