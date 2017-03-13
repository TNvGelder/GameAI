using System;

namespace Assets.Scripts.Goals
{
    public abstract class Goal
    {
        protected MovingEntity Owner;
        protected Status status;
        protected int type;
        public string Name { get; set; }

        public virtual void Activate()
        {
            status = Status.Active;
        }
        public virtual Status Process()
        {
            if (status == Status.Inactive)
            {
                Activate();
            }

            return status;
        }
        public virtual void Terminate()
        {
            status = Status.Inactive;
        }

        public virtual bool HandleMessage(Message m)
        {
            return true;
        }

        public virtual void AddSubgoal(Goal g)
        {

        }

        public bool IsActive()
        {
            return status == Status.Active;
        }

        public bool IsCompleted ()
        {
            return status == Status.Completed;
        }

        public bool HasFailed()
        {
            return status == Status.Failed;
        }

        public int Get_Type()
        {
            return 0;
        }
    }
}
