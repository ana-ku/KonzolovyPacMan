using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace KonzolovyPacMan
{
    internal class Level
    {
        public int IndexLevelu;
        public int Vyska;
        public int Sirka;
        public string PrekazkaZnak;
        public PoziceHrace PocatecniSouradniceHrace;
        public string CestaKSouboru;
        public string[] PolePrekazekDrahokamu;
        readonly string PokladZnak = "o";
        readonly string HracZnak = "P";
        public PoziceHrace SouradniceHrace = new PoziceHrace();
        public int PocetDrahokamu = 0;
        Dictionary<string, int[]> SeznamPrvku = new Dictionary<string, int[]>();


        int predchoziSouradnice0;
        int predchoziSouradnice1;


        public Level(string cestaKSouboru)
        {
            CestaKSouboru = cestaKSouboru;
            string[] obsahSouboru = File.ReadAllLines(CestaKSouboru);

            //Vytáhnout informace ze souboru
            //1. Index levelu

            string indexLeveluString = Regex.Match(obsahSouboru[0], @"\d+").Value;
            IndexLevelu = int.Parse(indexLeveluString);

            //2. Výška pole

            string vyskaPoleString = Regex.Match(obsahSouboru[1], @"\d+").Value;
            Vyska = int.Parse(vyskaPoleString);

            //3. Šířka pole

            string sirkaPoleString = Regex.Match(obsahSouboru[2], @"\d+").Value;
            Sirka = int.Parse(sirkaPoleString);

            //4. Počáteční pozice hráče

            string[] pocatPoziceSplit = obsahSouboru[3].Split(':');
            string[] pocatPozice = pocatPoziceSplit[1].Split(' ');
            int[] cislaPocatPozice = Array.ConvertAll(pocatPozice, s => int.Parse(s));
            PocatecniSouradniceHrace = new PoziceHrace(cislaPocatPozice[0], cislaPocatPozice[1]);


            //5. Znak pro překážku
            string prekazkaZnak = Regex.Match(obsahSouboru[4], @":(.*)").Groups[1].Value;
            PrekazkaZnak = prekazkaZnak;

            //6. Překážky a drahokamy
            List<string> seznamPrekazekDrahokamu = obsahSouboru.ToList();
            seznamPrekazekDrahokamu.RemoveRange(0, 6);
            PolePrekazekDrahokamu = seznamPrekazekDrahokamu.ToArray(); 

        }

       
        public void NapisZnak(string znak)
        {
            Console.Write(znak);
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

        public string[] ZmenPoziciHrace(string[] hraciPole)
        {
            int y = SouradniceHrace.Y;
            int x = SouradniceHrace.X;
            string radek = hraciPole[y];
            char znak = radek[x];
            if (znak.Equals('o'))
            {
                PocetDrahokamu--;
            }

            hraciPole[y] = hraciPole[y].Remove(x, 1).Insert(x, HracZnak);
            return hraciPole;
        }
        public string[] NactiObsahLevelu(string[] zakladniPole)
        {
            SeznamPrvku.Clear();

            //připravit slovník
            foreach (var item in PolePrekazekDrahokamu)
            {
                string[] info = item.Split(':');
                string[] cislaString = info[1].Split(' ');
                int[] cislaInt = Array.ConvertAll(cislaString, s => int.Parse(s));

                SeznamPrvku.Add(info[0], new int[] { cislaInt[0], cislaInt[1] });
            }
            //Vyměnit za překážky/poklady
            foreach (KeyValuePair<string, int[]> entry in SeznamPrvku)
            {
                int hodnotaX = entry.Value[0] + 1;
                int hodnotaY = entry.Value[1];


                if (entry.Key.StartsWith('X'))
                {
                    zakladniPole[hodnotaY] = zakladniPole[hodnotaY].Remove(hodnotaX, 1).Insert(hodnotaX, PrekazkaZnak);
                }
                else
                {
                    PocetDrahokamu++;
                    zakladniPole[hodnotaY] = zakladniPole[hodnotaY].Remove(hodnotaX, 1).Insert(hodnotaX, PokladZnak);
                }

            }
            //Umístit hráče na výchozí pozici
            zakladniPole[PocatecniSouradniceHrace.Y] = zakladniPole[PocatecniSouradniceHrace.Y].Remove(PocatecniSouradniceHrace.X, 1).Insert(PocatecniSouradniceHrace.X, HracZnak);

            SouradniceHrace.X = PocatecniSouradniceHrace.X;
            SouradniceHrace.Y = PocatecniSouradniceHrace.Y;

            return zakladniPole;

        }


        public void VypisPole(string[] hraciPole)
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
            VypisInfo();
        }
        public void VypisInfo()
        {
            Console.WriteLine("\nZbývající počet drahokamů: " + PocetDrahokamu);
        }

        public ConsoleKey ZjistiKlavesu()
        {
            var key = Console.ReadKey(true);
            return key.Key;
        }


        public void ProvedStisknutiKlavesy(string[] hraciPole)
        {
            //Uložím si předchozí souřadnice
            predchoziSouradnice0 = SouradniceHrace.X;
            predchoziSouradnice1 = SouradniceHrace.Y;

            //Změna souřadnic 

            ConsoleKey stisknutaKlavesa = ZjistiKlavesu();

            if (stisknutaKlavesa == ConsoleKey.Q)
            {
                System.Environment.Exit(0);
            }
            if (stisknutaKlavesa == ConsoleKey.UpArrow && SouradniceHrace.Y > 0)
            {
                SouradniceHrace.Y--;
            }
            if (stisknutaKlavesa == ConsoleKey.DownArrow && SouradniceHrace.Y < Vyska - 1)
            {
                SouradniceHrace.Y++;
            }
            if (stisknutaKlavesa == ConsoleKey.LeftArrow && SouradniceHrace.X > 1)
            {
                SouradniceHrace.X--;
            }
            if (stisknutaKlavesa == ConsoleKey.RightArrow && SouradniceHrace.X < Sirka)
            {
                SouradniceHrace.X++;
            }

            //Kontrola, jestli není na nově přistupovaném poli znak X

            string radek = hraciPole[SouradniceHrace.Y];

            string znak = radek[SouradniceHrace.X].ToString();

            if (znak == PrekazkaZnak)
            {
                SouradniceHrace.Y = predchoziSouradnice1;
                SouradniceHrace.X = predchoziSouradnice0;
            }
        }

        public void VymazHrace(string[] hraciPole)
        {

            var x = SouradniceHrace.X;
            var y = SouradniceHrace.Y;
            hraciPole[y] = hraciPole[y].Remove(x, 1).Insert(x, " ");

        }

    }
}
