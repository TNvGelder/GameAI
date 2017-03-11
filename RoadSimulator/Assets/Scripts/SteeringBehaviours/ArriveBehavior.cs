using System;

namespace Assets.Scripts.SteeringBehaviours
{
    public enum Deceleration
    {
        Slow = 3,
        Normal = 2,
        Fast = 1
    };

    public class ArriveBehavior : SteeringBehaviour
    {
        public Vector2D Target { get; set; }
        public Deceleration Deceleration { get; set; }

        public ArriveBehavior(MovingEntity me, Vector2D target, Deceleration deceleration) : base(me)
        {
            Target = target;
            Deceleration = deceleration;
        }

        public override Vector2D Calculate()
        {
            var toTarget = Target - ME.Pos;

            var dist = toTarget.Length();

            if (dist > 0)
            {
                var tweaker = 0.3;

                var speed = dist / ((int)Deceleration) * tweaker;

                speed = Math.Min(speed, ME.MaxSpeed);

                var desiredVelocity = toTarget * (float)speed / dist;

                return desiredVelocity - ME.Velocity;
            }

            return new Vector2D();
        }
    }
}
