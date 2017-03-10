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
