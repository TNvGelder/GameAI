using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.SteeringBehaviours
{
    public class Separation : SteeringBehaviour
    {

        public Separation(MovingEntity me) : base(me)
        {
        }

        public override Vector2D Calculate()
        {
            Vector2D steeringForce = new Vector2D();
            World world = World.Instance;
            world.TagObstaclesWithinViewRange(ME, ME.ViewDistance);
            foreach (var cur in world.GetMovingEntities())
            {
                if (cur.Tagged)
                {
                    Vector2D ToAgent = ME.Pos - cur.Pos;
                    steeringForce += ToAgent.Normalize() / ToAgent.Length();
                }
            }
            return steeringForce;

        }
    }
}
