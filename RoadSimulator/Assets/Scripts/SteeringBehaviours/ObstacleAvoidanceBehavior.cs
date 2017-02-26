﻿using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SteeringBehaviours
{
    public class ObstacleAvoidanceBehavior : SteeringBehaviour
    {
        public ObstacleAvoidanceBehavior(MovingEntity me) : base(me)
        {
        }

        // https://github.com/wangchen/Programming-Game-AI-by-Example-src/blob/master/Buckland_Chapter3-Steering%20Behaviors/SteeringBehaviors.cpp#L859
        public override Vector2D Calculate()
        {
            var world = this.ME.MyWorld;

            var m_dDBoxLength = world.MinDetectionBoxLength +
                  (this.ME.Speed() / this.ME.MaxSpeed) *
                  world.MinDetectionBoxLength;

            // todo: hier moeten we alleen entities in de buurt pakken
            List<MovingEntity> movingEntities = world.entities;

            MovingEntity ClosestIntersectingObstacle = null;

            double DistToClosestIP = double.MaxValue;

            Vector2D LocalPosOfClosestObstacle = null;

            foreach (var cur in movingEntities)
            {
                // Tagged moet aan/uit gezet worden wanneer we kijken welke entities in de buurt zitten
                // nu is het altijd true
                if (cur.Tagged)
                {
                    Vector2D LocalPos = PointToLocalSpace(cur.Pos,
                                             ME.Heading,
                                             ME.Side,
                                             ME.Pos);

                    if (LocalPos.X >= 0)
                    {
                        // het is ff de vraag hoe we bounded radius moeten bepalen...
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
                double multiplier = 1.0 + (m_dDBoxLength - LocalPosOfClosestObstacle.X) /
                                    m_dDBoxLength;


                //calculate the lateral force
                SteeringForce.Y = (float)((ClosestIntersectingObstacle.BRadius -
                                   LocalPosOfClosestObstacle.Y) * multiplier);

                //apply a braking force proportional to the obstacles distance from
                //the vehicle. 
                const double BrakingWeight = 0.2;

                SteeringForce.X = (float)((ClosestIntersectingObstacle.BRadius -
                                   LocalPosOfClosestObstacle.X) *
                                   BrakingWeight);
            }

            //finally, convert the steering vector from local to world space

            // hier komt altijd (0,0) uit omdat Heading en Side 0,0 zijn.
            // in VectorToWorldSpace doe je dus in principe SteeringForce * 0
            return VectorToWorldSpace(SteeringForce,
                                      ME.Heading,
                                      ME.Side);
        }

        // https://github.com/wangchen/Programming-Game-AI-by-Example-src/blob/aa5379f3c4dc142577c7f010ebfb705e78754c5c/Common/2D/Transformations.h#L141
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


        // https://github.com/wangchen/Programming-Game-AI-by-Example-src/blob/aa5379f3c4dc142577c7f010ebfb705e78754c5c/Common/2D/Transformations.h#L118
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