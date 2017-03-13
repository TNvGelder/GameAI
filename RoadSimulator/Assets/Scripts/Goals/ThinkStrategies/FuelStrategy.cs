using System.Linq;

namespace Assets.Scripts.Goals.ThinkStrategies
{
    public class FuelStrategy : IThinkStrategy
    {
        public void Evaluate(MovingEntity Owner, Think think)
        {
            if (Owner.Fuel < 30.0 && think.Subgoals[0].GetType() != typeof(GetFuel))
            {
                var goal = new GetFuel(Owner);

                var subGoal = think.Subgoals[0] as GoalComposite;
                if (subGoal != null)
                {
                    if (subGoal.Subgoals.Any() && !(subGoal.Subgoals[0] is GetFuel))
                    {
                        think.InsertPriorityGoal(subGoal, goal);
                    }
                }
                else
                {
                    think.SetGoal(goal);
                }
            }
        }
    }
}
