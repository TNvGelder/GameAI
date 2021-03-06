﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Goals
{
    public class CatchRobberGoal : SeekGoal
    {
        public CatchRobberGoal(MovingEntity owner, MovingEntity target, string name = "Catch Robber") : base(owner, target, name)
        {
        }

        public override Status Process()
        {
            if (Owner.IsAtPosition(Target.Pos))
            {
                status = Status.Completed;
            }

            return base.Process();
        }
    }
}
