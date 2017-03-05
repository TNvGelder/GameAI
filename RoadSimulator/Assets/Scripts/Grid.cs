using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Grid
    {
        public Grid()
        {
            Cells = new List<GridCell>();
        }

        public List<GridCell> Cells { get; set; }
    }
}
