using Assets.Scripts.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{

    /// <summary>
    /// Service for creating goals towards a destination.
    /// </summary>
    public class DestinationTravelService
    {

        public static MoveToPositionGoal GoTo(MovingEntity traveler, GameObject destination, string text)
        {
            Vector2D target = new Vector2D(destination.transform.position.x, destination.transform.position.y);
            return new MoveToPositionGoal(traveler, target, text);
        }

        public static MoveToPositionGoal GoTo(MovingEntity traveler, string tag, string text)
        {
            GameObject chosen;
            return GoTo(traveler, tag, text, out chosen);
        }

        public static MoveToPositionGoal GoTo(MovingEntity traveler, string tag)
        {
            GameObject chosen;
            return GoTo(traveler, tag, out chosen);
        }

        public static MoveToPositionGoal GoTo(MovingEntity traveler, string tag, out GameObject chosenGameObj)
        {

            return GoTo(traveler, tag, "Going to " + tag, out chosenGameObj);
        }

        public static MoveToPositionGoal GoTo(MovingEntity traveler, string tag, string text, out GameObject chosenGameObj)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            Vector2D target;
            Vector2D travelerPos = traveler.Pos;
            GameObject obj = objects[0];
            target = new Vector2D(obj.transform.position.x, obj.transform.position.y);
            if (objects.Length> 1)
            {
                float nearestDistance = int.MaxValue;
                foreach (GameObject gameObj in objects)
                {
                    Vector2D gameObjPos = new Vector2D(gameObj.transform.position.x, gameObj.transform.position.y);
                    float distance = (gameObjPos - travelerPos).Length();
                    if (distance < nearestDistance)
                    {
                        target = gameObjPos;
                        obj = gameObj;
                        nearestDistance = distance;
                    }
                }
            }
            chosenGameObj = obj;

            return new MoveToPositionGoal(traveler, target, text);
        }

    }
}
