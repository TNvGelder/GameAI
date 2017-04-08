using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Path
    {
        public bool Finished { get; set; }
        public Vector2D CurrentWayPoint { get; set; }
        public LinkedList<Vector2D> WayPoints { get; set; }

        public Path(LinkedList<Vector2D> wayPoints)
        {
            Finished = true;
            WayPoints = wayPoints;
            NextWayPoint();
        }

        public bool IsArrived(MovingEntity entity)
        {
            if (CurrentWayPoint == null)
            {
                return true;
            }
            Vector2D diff = CurrentWayPoint - entity.Pos;
            return (Math.Abs(diff.X) < 1 && Math.Abs(diff.Y) < 1);
        }

        public bool NextWayPoint()
        {
            int count = WayPoints.Count;
            if (count > 0)
            {
                Finished = false;
                CurrentWayPoint = WayPoints.First.Value;
                WayPoints.RemoveFirst();
                return true;
            }
            CurrentWayPoint = null;
            Finished = true;

            return false;
        }
    }
}
