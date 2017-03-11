namespace Assets.Scripts.Goals
{
    public class VisitShop : GoalComposite
    {
        public VisitShop(MovingEntity owner, Vector2D target) {
            Owner = owner;
            Name = "VisitShop";
        }

        public override void Activate()
        {
            base.Activate();
        }
    }
}
