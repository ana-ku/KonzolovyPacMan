using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonzolovyPacMan
{
    internal struct PoziceHrace
    {
        public int X { get; set; }
        public int Y { get; set; }

        public PoziceHrace( int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
