using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.SteeringBehaviours
{
    public class FollowPathBehaviour : SteeringBehaviour
    {
        protected SeekBehaviour seekBehaviour;
        public Path Path { get; set; }

        public FollowPathBehaviour(MovingEntity me) : base(me)
        {
            seekBehaviour = new SeekBehaviour(me, new Vector2D());
        }

        public override Vector2D Calculate()
        {
            
            if (Path.IsArrived(ME))
            {
                Path.NextWayPoint();
            }else if (Path.WayPoints.Count > 0)
            {
                Path.Finished = false;
            }
            if (!Path.Finished)
            {
                seekBehaviour.Target = Path.CurrentWayPoint;
                return seekBehaviour.Calculate();
            }else
            {
                return new Vector2D();
            }
        }
    }
}
