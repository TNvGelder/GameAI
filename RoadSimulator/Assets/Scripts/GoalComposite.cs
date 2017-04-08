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
            ActivateIfInactive();

            status = ProcessSubgoals();

            return status;
        }

        public Status ProcessSubgoals()
        {
            while(Subgoals.Count > 0 && (Subgoals[0].IsCompleted() || Subgoals[0].HasFailed()))
            {
                var s = Subgoals[0];
                s.Terminate();
                Subgoals.Remove(s);
                OnSubGoalFinish(s);
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
            foreach(var goal in Subgoals)
            {
                goal.Terminate();
            }

            base.Terminate();
        }

        public void RemoveAllSubgoals()
        {
            while(Subgoals.Count > 0)
            {
                Subgoals[0].Terminate();
                Subgoals.Remove(Subgoals[0]);
            }
        }

        /**
         * Iterates over all subgoals (and their subgoals)
         * and formats them nicely so that it can be displayed to the user
         */
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
