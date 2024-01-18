using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonzolovyPacMan
{
    internal class UserInput
    {
        public string SmerPohybu;

        public ConsoleKey ZjistiKlavesu()
        {
            var key = Console.ReadKey(true);
            return key.Key;
        }


        public void ProvedStisknutiKlavesy()
        {
            
            ConsoleKey stisknutaKlavesa = ZjistiKlavesu();

            if (stisknutaKlavesa == ConsoleKey.Q)
            {
                System.Environment.Exit(0);
            }
            if (stisknutaKlavesa == ConsoleKey.UpArrow)
            {
                SmerPohybu = "Y-";
            }
            if (stisknutaKlavesa == ConsoleKey.DownArrow)
            {
                SmerPohybu = "Y+";
            }
            if (stisknutaKlavesa == ConsoleKey.LeftArrow)
            {
                SmerPohybu = "X-";
            }
            if (stisknutaKlavesa == ConsoleKey.RightArrow)
            {
                SmerPohybu = "X+";
            }
           
        }
    }
}
