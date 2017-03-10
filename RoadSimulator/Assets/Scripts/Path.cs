using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Path
    {
        private LinkedList<Vector2D> wayPoints;
        private Vector2D currentWayPoint;
        private bool finished = true;
        public bool Finished { get {
                return finished;
           } set { finished = value; } }
        public Vector2D CurrentWayPoint
        {
            get
            {
                return currentWayPoint;
            }
            set { currentWayPoint = value; }
        }

        public LinkedList<Vector2D> WayPoints {
            get
            {
                return wayPoints;
            }
            set {
                wayPoints = value;
            }
        }

        public Path(LinkedList<Vector2D> wayPoints)
        {
            this.wayPoints = wayPoints;
            NextWayPoint();
        }

        

        public bool IsArrived(MovingEntity entity)
        {
            Vector2D diff = currentWayPoint - entity.Pos;
            return (Math.Abs(diff.X) < 1 && Math.Abs(diff.Y) < 1);
        }

        public bool NextWayPoint()
        {
            int count = wayPoints.Count;
            if (count > 0)
            {
                finished = false;
                currentWayPoint = wayPoints.First.Value;
                wayPoints.RemoveFirst();
                return true;
            }
            currentWayPoint = null;
            finished = true;

            return false;
        }
    }
}
