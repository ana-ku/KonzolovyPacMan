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
            Pozice novaPozice = player.SouradniceHrace; //novaPozice podstoupi změny dle UserInputu, pokud nebude na nové pozici překážka, stanou se z ní nové player.SouradniceHrace. Pokud tam bude překážka, souřadnice hráče se neaktualizují.

            //Přiřazení nových souřadnic, přejímaných z informací z UserInput

            switch (userInput.SmerPohybu)
            {
                case "Y-":
                    if (novaPozice.Y > 0)
                    {
                        novaPozice.Y--;
                    }
                    //kontroluje, jestli hráč nevyjel z canvasu
                    break;
                case "Y+":
                    if (novaPozice.Y < vyska - 1)
                    {
                        novaPozice.Y++;
                    }
                    break;
                case "X-":
                    if (novaPozice.X > 1)
                    {
                        novaPozice.X--;
                    }
                    break;

                case "X+":
                    if (novaPozice.X < sirka)
                    {
                        novaPozice.X++;
                    }
                    break;
            }


            //Kontrola, jestli není na nově přistupovaném poli znak X

            string radek = hraciPole[novaPozice.Y];

            string znak = radek[novaPozice.X].ToString();

            if (znak != PrekazkaZnak) //pohnu hráčem
            {
                //Vymažu hráče na předchozí pozici
                NastavZnak(hraciPole, player.SouradniceHrace, " ");
                
                //Vepíšu hráče na současnou pozici
                NastavZnak(hraciPole, novaPozice, HracZnak);

            
                if (znak == PokladZnak)
                {
                    PocetDrahokamu--;
                }
                player.SouradniceHrace = novaPozice;
            }
           

            return hraciPole;

        }
        public bool VyhralHrac() //AI: jednodušší kontrola podmínky ve hře
        {
            return PocetDrahokamu == 0; //hodnota bude false, dokud nebude splněná return podmínka
        }
        public void NastavZnak(string[] hraciPole, Pozice pozice, string znak)
        {
            hraciPole[pozice.Y] = hraciPole[pozice.Y].Remove(pozice.X, 1).Insert(pozice.X, znak);

        }
    }
}
