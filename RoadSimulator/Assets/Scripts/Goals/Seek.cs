using Assets.Scripts.SteeringBehaviours;
using System.Collections.Generic;

namespace Assets.Scripts.Goals
{
    public class Seek : Goal
    {
        private MovingEntity Target;

        public Seek(MovingEntity owner, MovingEntity target) {
            Owner = owner;
            Target = target;
            Name = "Seek";
        }

        public float OriginalSpeed { get; private set; }

        public override void Activate()
        {
            OriginalSpeed = Owner.MaxSpeed;
            Owner.MaxSpeed = (float)(Target.MaxSpeed - (Target.MaxSpeed * 0.1));
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
