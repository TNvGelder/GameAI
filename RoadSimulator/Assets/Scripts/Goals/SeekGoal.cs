using Assets.Scripts.SteeringBehaviours;
using System.Collections.Generic;

namespace Assets.Scripts.Goals
{
    public class SeekGoal : Goal
    {
        public MovingEntity Target;

        public SeekGoal(MovingEntity owner, MovingEntity target, string name = "Seek") {
            Owner = owner;
            Target = target;
            Name = name;
        }

        public float OriginalSpeed { get; private set; }

        public override void Activate()
        {
            OriginalSpeed = Owner.MaxSpeed;
            Owner.MaxSpeed = (float)(Target.MaxSpeed * 0.9);
            Owner.SteeringBehaviours.Add(new Scripts.SteeringBehaviours.SeekMovingEntityBehaviour(Owner, Target));

            base.Activate();
        }

        public override void Terminate()
        {
            Owner.MaxSpeed = OriginalSpeed;
            Owner.RemoveBehaviour(typeof(Scripts.SteeringBehaviours.SeekMovingEntityBehaviour));

            base.Terminate();
        }
    }
}
