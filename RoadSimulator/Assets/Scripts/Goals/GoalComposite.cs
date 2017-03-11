using System;
using System.Collections.Generic;

namespace Assets.Scripts.Goals
{
    public class GoalComposite : Goal
    {
        public List<Goal> Subgoals = new List<Goal>();

        public override void AddSubgoal(Goal g)
        {
            Subgoals.Add(g);
        }

        public override Status Process()
        {
            return ProcessSubgoals();
        }

        public Status ProcessSubgoals()
        {
            while(Subgoals.Count > 0 && (Subgoals[0].IsCompleted() || Subgoals[0].HasFailed()))
            {
                Subgoals[0].Terminate();
                Subgoals.Remove(Subgoals[0]);
            }

            if (Subgoals.Count > 0)
            {
                Status s = Subgoals[0].Process();

                if (s == Status.Completed && Subgoals.Count > 1)
                {
                    return Status.Active;
                }

                return s;
            } else
            {
                return Status.Completed;
            }
        }

        public void RemoveAllSubgoals()
        {
            while(Subgoals.Count > 0)
            {
                Subgoals[0].Terminate();
                Subgoals.Remove(Subgoals[0]);
            }
        }

        public string GetDisplayText(string padding = "")
        {
            var r = padding + "- " + Name + "\r\n";
            foreach (var sGoal in Subgoals)
            {
                if (sGoal is GoalComposite)
                {
                    r += ((GoalComposite)sGoal).GetDisplayText("  " + padding) + "\r\n";
                } else
                {
                    r += "<color=" + (sGoal.IsActive() ? "blue" : "black") + "><size=20>" + sGoal.Name + "</size></color>";
                }
            }
            return r;
        }
    }

    public enum Status
    {
        Inactive,
        Active,
        Failed,
        Completed,
    };
}
