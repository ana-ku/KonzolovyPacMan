using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonzolovyPacMan
{
    internal class GameBoard
    {
        public UserInput userInput = new UserInput();

        public Player player = new Player();

        public string PokladZnak = "o";

        public string HracZnak = "P";

        public string PrekazkaZnak;

        public string[] PolePrekazekDrahokamu;

        public int PocetDrahokamu = 0;

        public Pozice PocatecniSouradniceHrace;

        List<GameObject> SeznamPrvku = new List<GameObject>();

        public string[] NactiObsahLevelu(string[] zakladniPole)
        {
            SeznamPrvku.Clear();

            //připravit slovník
            foreach (var item in PolePrekazekDrahokamu) // z každého prvku pole vytvořím nový objekt
            {
                GameObject herniObjekt = new GameObject();
                string[] info = item.Split(':');
                string[] cislaString = info[1].Split(' ');
                int[] cislaInt = Array.ConvertAll(cislaString, s => int.Parse(s));
                herniObjekt.Znak = info[0];
                herniObjekt.Pozice = new Pozice(cislaInt[0] + 1, cislaInt[1]);
                SeznamPrvku.Add(herniObjekt);
            }
            //Vyměnit za překážky/poklady
            foreach (GameObject entry in SeznamPrvku)
            {
                int hodnotaX = entry.Pozice.X;
                int hodnotaY = entry.Pozice.Y;

                if (entry.Znak.StartsWith("X"))
                {
                    entry.JePrekazka = true; //pokud je znak X, bude překážka neprůchozí
                }

                //Vložit poklady a překážky do pole.
                zakladniPole[hodnotaY] = zakladniPole[hodnotaY].Remove(hodnotaX, 1).Insert(hodnotaX, entry.JePrekazka ? PrekazkaZnak : PokladZnak);

                //pokud klíč neobsahuje X, znamená to, že obsahuje "O", tj značku pro drahokam. Z toho můžu vypočítat počet drahokamů
                
                if (!entry.JePrekazka)
                {
                    PocetDrahokamu++;
                }

            }
            //Umístit hráče na výchozí pozici
            zakladniPole[PocatecniSouradniceHrace.Y] = zakladniPole[PocatecniSouradniceHrace.Y].Remove(PocatecniSouradniceHrace.X, 1).Insert(PocatecniSouradniceHrace.X, HracZnak);

            player.SouradniceHrace.X = PocatecniSouradniceHrace.X;
            player.SouradniceHrace.Y = PocatecniSouradniceHrace.Y;

            return zakladniPole;

        }
        public string[] ZmenPoziciHrace(string[] hraciPole, int vyska, int sirka)
        {
            //Uložím si předchozí souřadnice
            player.PredchoziSouradniceHrace.X = player.SouradniceHrace.X;
            player.PredchoziSouradniceHrace.Y = player.SouradniceHrace.Y;

            int a = player.PredchoziSouradniceHrace.X;
            int b = player.PredchoziSouradniceHrace.Y;

            int y = player.SouradniceHrace.Y;
            int x = player.SouradniceHrace.X;

            //Přiřazení nových souřadnic, přejímaných z informací z UserInput

            switch (userInput.SmerPohybu)
            {
                case "Y-":
                    if (y > 0)
                    {
                        y--;
                    }
                    //kontroluje, jestli hráč nevyjel z canvasu
                    break;
                case "Y+":
                    if (y < vyska - 1)
                    {
                        y++;
                    }
                    break;
                case "X-":
                    if (x > 1)
                    {
                        x--;
                    }
                    break;

                case "X+":
                    if (x < sirka)
                    {
                        x++;
                    }
                    break;
            }


            //Kontrola, jestli není na nově přistupovaném poli znak X

            string radek = hraciPole[y];

            string znak = radek[x].ToString();

            if (znak != PrekazkaZnak) //pohnu hráčem
            {
                //Vymažu současnou pozici
                hraciPole[b] = hraciPole[b].Remove(a, 1).Insert(a, " ");


                //pak provedu změnu pozice

                hraciPole[y] = hraciPole[y].Remove(x, 1).Insert(x, HracZnak);

                if (znak == PokladZnak)
                {
                    PocetDrahokamu--;
                }
                player.SouradniceHrace.X = x;
                player.SouradniceHrace.Y = y;
            }
            else
            {
                player.SouradniceHrace.X = a;
                player.SouradniceHrace.Y = b;
            }

            return hraciPole;

        }
        public bool VyhralHrac() //AI: jednodušší kontrola podmínky ve hře
        {
            return PocetDrahokamu == 0; //hodnota bude false, dokud nebude splněná return podmínka
        }
    }
}
