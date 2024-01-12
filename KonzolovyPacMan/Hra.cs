using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonzolovyPacMan
{
    internal class Hra //AI: Hra může být objekt, který se bude vytvářet v hlavním programu a jako parametr přijme seznam levelů vytvořený v hlavním programu
    {
       private readonly List<Level> SeznamLevelu = new List<Level>();
       private int indexSoucasnehoLevelu = 0;
        string[] hraciPole;
        public Hra (List<Level> levely) //konstruktor objektu Hra
        {
            SeznamLevelu = levely;
        }

        public void Play()
        {
            while(true) { //vnější loop, který zajistí, že se po dokončení jednoho levelu načte další level 
            Level aktualniLevel = SeznamLevelu[indexSoucasnehoLevelu];

            hraciPole = aktualniLevel.NactiObsahLevelu(aktualniLevel.ZiskejZakladniPole());

            aktualniLevel.VypisPole(hraciPole);

            while (true) //vnitřní loop, v něm se dokončují jednotlivé levely
            {

                aktualniLevel.ProvedStisknutiKlavesy(hraciPole);


                hraciPole = aktualniLevel.ZmenPoziciHrace(hraciPole);


                aktualniLevel.VypisPole(hraciPole);

                if (aktualniLevel.VyhralHrac()) //kontrola, jestli je počet drahokamů == 0
                {
                    Console.Clear();
                    Console.WriteLine("Vyhráli jste! Ukončete stisknutím Q.");
                    if (indexSoucasnehoLevelu < SeznamLevelu.Count - 1)
                    {
                        Console.WriteLine("Nebo pokračujte na další level stisknutím Enter.");
                        if (aktualniLevel.ZjistiKlavesu() == ConsoleKey.Enter)
                        {
                            Console.Clear();
                            indexSoucasnehoLevelu++;
                            break; //vystoupí z vnitřního while loopu. Před tím jsem tady znovu načítala level a konstruovala pole
                            }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            }
        }

       
    }
}
