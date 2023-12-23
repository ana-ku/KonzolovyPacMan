using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonzolovyPacMan
{
    internal class Level
    {
        public int CisloLevelu;
        public int Vyska;
        public int Sirka;
        public char PrekazkaZnak;
        public int[] PocatecniSouradniceHrace = new int[2];
        public string NazevSouboru;
        readonly string Cesta = @"../../../";
        string pokladZnak = "o";
        string hracZnak = "P";
        public int[] souradniceHrace = new int[2];
        public int pocetDrahokamu = 0;
        int predchoziSouradnice0;
        int predchoziSouradnice1;


        public Level(int cisloLevelu, int vyska, int sirka, char prekazkaZnak, int[] pocatecniSouradniceHrace, string nazevSouboru)
        {
            CisloLevelu = cisloLevelu;
            Vyska = vyska;
            Sirka = sirka;
            PrekazkaZnak = prekazkaZnak;
            PocatecniSouradniceHrace = pocatecniSouradniceHrace;
            NazevSouboru = nazevSouboru;
        }
        Dictionary<string, int[]> seznamPrvku = new Dictionary<string, int[]>();


        public string VytvorCestuKSouboru()
        {
            return Cesta + NazevSouboru;
        }


        public void NapisZnak(string znak)
        {
            Console.Write(znak);
        }

        public void OkrajPoleHorniDolni()
        {
            NapisZnak("+");
            for (int i = 0; i < Sirka; i++)
            {
                NapisZnak("-");
            }
            NapisZnak("+");
        }

       public string[] ZakladniPole()
        {
            string[] pole = new string[Vyska];
            for (int j = 0; j < Vyska; j++)
            {
                for (int i = 0; i <= Sirka; i++)
                {
                    if (i == 0)
                    {
                        pole[j] += "|";
                    }
                    if (i == Sirka)
                    {
                        pole[j] += "|";
                    }

                    //jinak přidám prázdné " " 
                    else
                    {
                        pole[j] += " ";
                    }
                }

            }
            return pole;
        }

        public string[] PoziceHrace(string[] pole, int x, int y)
        {
            
            string radek = pole[y];
            char znak = radek[x];
            if (znak.Equals('o'))
            {
                pocetDrahokamu--;
            }

            pole[y] = pole[y].Remove(x, 1).Insert(x, hracZnak);
            return pole;
        }
        public string[] NactiObsahLevelu(string[] pole)
        {
            seznamPrvku.Clear();

            string[] radkyLevelu = File.ReadAllLines(VytvorCestuKSouboru());

            //připravit slovník
            foreach (var item in radkyLevelu)
            {
                string[] info = item.Split(':');
                string[] cislaString = info[1].Split(' ');
                int[] cislaInt = Array.ConvertAll(cislaString, s => int.Parse(s));

                seznamPrvku.Add(info[0], new int[] { cislaInt[0], cislaInt[1] });
            }
            //Vyměnit za překážky/poklady
            foreach (KeyValuePair<string, int[]> entry in seznamPrvku)
            {
                int hodnotaX = entry.Value[1];
                int hodnotaY = entry.Value[0];


                if (entry.Key.StartsWith('X'))
                {
                    pole[hodnotaY] = pole[hodnotaY].Remove(hodnotaX, 1).Insert(hodnotaX, PrekazkaZnak.ToString());
                }
                else
                {
                    pocetDrahokamu++;
                    pole[hodnotaY] = pole[hodnotaY].Remove(hodnotaX, 1).Insert(hodnotaX, pokladZnak);
                }

            }
            //Umístit hráče na výchozí pozici
            pole[PocatecniSouradniceHrace[0]] = pole[PocatecniSouradniceHrace[0]].Remove(PocatecniSouradniceHrace[1], 1).Insert(PocatecniSouradniceHrace[1], hracZnak);

            souradniceHrace[0] = PocatecniSouradniceHrace[0];
            souradniceHrace[1] = PocatecniSouradniceHrace[1];
            
            return pole;

        }

       
        public void VypisPole(string[] pole)
        {
            Console.Clear();
            OkrajPoleHorniDolni();
            Console.WriteLine();
            foreach (var item in pole)
            {
                Console.WriteLine(item.ToString());
            }
            OkrajPoleHorniDolni();
            Console.WriteLine();
            VypisInfo();
        }
        public void VypisInfo()
        {
            Console.WriteLine("\nZbývající počet drahokamů: " + pocetDrahokamu);
        }

        public ConsoleKey ZjistitKlavesu()
        {
            var key = Console.ReadKey(true);
            return key.Key;
        }


        public void StisknutiKlavesy(string[] pole)
        {
            //Uložím si předchozí souřadnice
            predchoziSouradnice0 = souradniceHrace[0];
            predchoziSouradnice1 = souradniceHrace[1];

            //Změna souřadnic 

            ConsoleKey stisknutaKlavesa = ZjistitKlavesu();

            if (stisknutaKlavesa == ConsoleKey.Q)
            {
                System.Environment.Exit(0);
            }
            if (stisknutaKlavesa == ConsoleKey.UpArrow && souradniceHrace[0] > 0)
            {
                souradniceHrace[0]--;
            }
            if (stisknutaKlavesa == ConsoleKey.DownArrow && souradniceHrace[0] < Vyska - 1)
            {
                souradniceHrace[0]++;
            }
            if (stisknutaKlavesa == ConsoleKey.LeftArrow && souradniceHrace[1] > 1)
            {
                souradniceHrace[1]--;
            }
            if (stisknutaKlavesa == ConsoleKey.RightArrow && souradniceHrace[1] < Sirka)
            {
                souradniceHrace[1]++;
            }

            //Kontrola, jestli není na nově přistupovaném poli znak X

            string radek = pole[souradniceHrace[0]];

            char znak = radek[souradniceHrace[1]];

            if (znak == PrekazkaZnak)
            {
                souradniceHrace[1] = predchoziSouradnice1;
                souradniceHrace[0] = predchoziSouradnice0;
            }
        }

        public void VymazatHrace(string[] pole)
        {

            var x = souradniceHrace[1];
            var y = souradniceHrace[0];
            pole[y] = pole[y].Remove(x, 1).Insert(x, " ");

        }

    }
}
