using System.Collections.Generic;

namespace Assets.Scripts.SteeringBehaviours
{
    public class FollowPathBehaviour : SteeringBehaviour
    {
        protected SeekBehaviour seekBehaviour;
        public Path Path { get; set; }

        public FollowPathBehaviour(MovingEntity me) : base(me)
        {
            seekBehaviour = new SeekBehaviour(me, new Vector2D());
            Path = new Path(new LinkedList<Vector2D>());
        }

        public override Vector2D Calculate()
        {
            if (Path.IsArrived(ME))
            {
                if (Path.WayPoints.Count > 0)
                {
                    Path.NextWayPoint();
                }
                else
                {
                    Path.Finished = true;
                }
                
            } else if (Path.WayPoints.Count > 0)
            {
                Path.Finished = false;
            }

            if (!Path.Finished)
            {
                seekBehaviour.Target = Path.CurrentWayPoint;
                return seekBehaviour.Calculate();
            } else
            {
                return new Vector2D();
            }
        }
    }
}
