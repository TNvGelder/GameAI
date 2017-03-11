using System;

namespace Assets.Scripts.Goals
{
    public class Think : GoalComposite
    {
        public Think(MovingEntity owner)
        {
            Owner = owner;
            Name = "Think";
        }

        public override void Activate()
        {
            if (Subgoals.Count > 0)
            {
                base.RemoveAllSubgoals();
            }

            Subgoals.Add(new MoveToPosition(Owner, new Scripts.Vector2D(-5.67f, -11.42f)));

            base.Activate();
        }
    }
}
