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

            Level aktualniLevel;

            GameBoard gameBoard = new GameBoard();

            Canvas canvas = new Canvas();

            while(true) { //vnější loop, který zajistí, že se po dokončení jednoho levelu načte další level 
            
            aktualniLevel = SeznamLevelu[indexSoucasnehoLevelu];

                //přerozdělení parametrů z třídy Level do ostatních 2 tříd GameBoard a Canvas

            gameBoard.PolePrekazekDrahokamu = aktualniLevel.PolePrekazekDrahokamu;
            gameBoard.PocatecniSouradniceHrace = aktualniLevel.PocatecniSouradniceHrace;
            gameBoard.PrekazkaZnak = aktualniLevel.PrekazkaZnak;
            canvas.Vyska = aktualniLevel.Vyska;
            canvas.Sirka = aktualniLevel.Sirka;

            //Získám hrací pole s playerem a překážkami

            hraciPole = gameBoard.NactiObsahLevelu(canvas.ZiskejZakladniPole()); 

            canvas.VypisPole(hraciPole, gameBoard.PocetDrahokamu);

            while (true) //vnitřní loop, v něm se dokončují jednotlivé levely
            {

                gameBoard.userInput.ProvedStisknutiKlavesy();

                hraciPole = gameBoard.ZmenPoziciHrace(hraciPole, canvas.Vyska, canvas.Sirka);

                canvas.VypisPole(hraciPole, gameBoard.PocetDrahokamu);

                    if (gameBoard.VyhralHrac()) //kontrola, jestli je počet drahokamů == 0
                    {

                    Console.Clear();

                    Console.WriteLine("Vyhráli jste! Ukončete stisknutím Q.");
                    if (indexSoucasnehoLevelu < SeznamLevelu.Count - 1)
                    {

                        Console.WriteLine("Nebo pokračujte na další level stisknutím Enter.");
                        if (gameBoard.userInput.ZjistiKlavesu() == ConsoleKey.Enter)
                        {
                            Console.Clear();
                            indexSoucasnehoLevelu++;
                            break; //vystoupí z vnitřního while loopu.
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
