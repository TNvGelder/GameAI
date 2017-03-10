namespace Assets.Scripts.SteeringBehaviours
{
    public class SeekMovingEntityBehaviour : SteeringBehaviour
    {
        public MovingEntity Target { get; set; }

        public SeekMovingEntityBehaviour(MovingEntity me, MovingEntity target) : base(me)
        {
            Target = target;
        }

        public override Vector2D Calculate()
        {
            Vector2D DesiredVelocity = ((Target.Pos.Clone() - ME.Pos.Clone())
                                        .Normalize()) * ME.MaxSpeed;

            return DesiredVelocity - ME.Velocity;
        }
    }
}
