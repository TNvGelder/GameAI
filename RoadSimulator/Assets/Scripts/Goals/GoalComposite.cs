using System;
using System.Collections.Generic;

namespace Assets.Scripts.Goals
{
    public abstract class GoalComposite : Goal
    {
        public List<Goal> Subgoals = new List<Goal>();
        public virtual void OnSubGoalFinish(Goal subgoal) { }

        public override void AddSubgoal(Goal g)
        {
            Subgoals.Add(g);
        }

        public override Status Process()
        {
            status = ProcessSubgoals();
            return status;
        }

        public Status ProcessSubgoals()
        {
            while(Subgoals.Count > 0 && (Subgoals[0].IsCompleted() || Subgoals[0].HasFailed()))
            {
                var s = Subgoals[0];
                Subgoals[0].Terminate();
                Subgoals.Remove(Subgoals[0]);
                OnSubGoalFinish(s);
            }

            if (status == Status.Inactive)
            {
                Activate();
            }

            if (Subgoals.Count > 0)
            {
                Status s = Subgoals[0].Process();

                if (s == Status.Completed && Subgoals.Count > 1)
                {
                    return Status.Active;
                }

                return s;
            }

            return Status.Completed;
        }

        public override void Terminate()
        {
            base.Terminate();
        }

        public void RemoveAllSubgoals()
        {
            while(Subgoals.Count > 0)
            {
                var composite = Subgoals[0] as GoalComposite;

                if (composite != null)
                {
                    composite.RemoveAllSubgoals();
                }

                Subgoals[0].Terminate();
                Subgoals.Remove(Subgoals[0]);
            }
        }

        public string GetDisplayText(string padding = "")
        {
            var r = padding + UI.ColorizeText(Name, "white") + "\r\n";
            foreach (var sGoal in Subgoals)
            {
                if (sGoal is GoalComposite)
                {
                    r += ((GoalComposite)sGoal).GetDisplayText("  " + padding) + "\r\n";
                } else
                {
                    r += UI.ColorizeText(padding + "- " + sGoal.Name, sGoal.IsActive() ? "blue" : "white") + "\r\n";
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
