using System;

namespace Assets.Scripts.Goals
{
    public class DummyGoal : GoalComposite
    {
        public DummyGoal(MovingEntity owner) {
            Owner = owner;
            Name = "DummyGoal";
        }
    }
}
