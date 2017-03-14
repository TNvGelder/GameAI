using Assets.Scripts.SteeringBehaviours;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    public class Patrol : GoalComposite
    {
        private FollowPathBehaviour followPath;

        public Patrol(MovingEntity owner) {
            Owner = owner;
            Name = "Patrol";
        }

        public override void Activate()
        {
            GoTo("Bank", "Bank");
            GoTo("GasStation", "GasStation");
            GoTo("Home", "Home");
            GoTo("Work", "Work");

            base.Activate();
        }

        public override void OnSubGoalFinish(Goal subgoal)
        {
            var goTo = subgoal as MoveToPosition;
            if (goTo != null)
            {
                if (goTo.Name == "Rob Bank")
                {
                    World.Instance.GetEntity<Bank>().GetRobbed();
                }
            }
        }

        public void GoTo(string tag, string text)
        {
            var objects = GameObject.FindGameObjectsWithTag(tag);

            foreach(var obj in objects)
            {
                var target = new Vector2D(obj.transform.position.x, obj.transform.position.y);

                AddSubgoal(new MoveToPosition(Owner, target, text));
            }
        }

        public override Status Process()
        {
            return base.Process();
        }
    }
}
