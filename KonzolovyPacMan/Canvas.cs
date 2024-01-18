using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonzolovyPacMan
{
    internal class Canvas
    {
        public int Vyska;
        public int Sirka;
        

        //Vykreslení základního pole
        public string[] ZiskejZakladniPole()
        {
            string[] zakladniPole = new string[Vyska];
            for (int j = 0; j < Vyska; j++)
            {
                for (int i = 0; i <= Sirka; i++)
                {
                    if (i == 0)
                    {
                        zakladniPole[j] += "|";
                    }
                    if (i == Sirka)
                    {
                        zakladniPole[j] += "|";
                    }

                    //jinak přidám prázdné " " 
                    else
                    {
                        zakladniPole[j] += " ";
                    }
                }

            }
            return zakladniPole;
        }

       
        public void VypisPole(string[] hraciPole, int pocetDrahokamu)
        {
            Console.Clear();
            VypisOkrajPoleHorniDolni();
            Console.WriteLine();
            foreach (var item in hraciPole)
            {
                Console.WriteLine(item.ToString());
            }

            VypisOkrajPoleHorniDolni();
            Console.WriteLine();
            VypisInfo(pocetDrahokamu);
        }

        public void VypisInfo(int pocetDrahokamu)
        {
            Console.WriteLine("\nZbývající počet drahokamů: " + pocetDrahokamu);
        }

        public void VypisOkrajPoleHorniDolni()
        {
            NapisZnak("+");
            for (int i = 0; i < Sirka; i++)
            {
                NapisZnak("-");
            }
            NapisZnak("+");
        }
        public void NapisZnak(string znak)
        {
            Console.Write(znak);
        }

    }
}
