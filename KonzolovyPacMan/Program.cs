using Microsoft.VisualBasic.Devices;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Text.RegularExpressions;
using System;
using KonzolovyPacMan;
using System.Security.Authentication.ExtendedProtection;


//Všechny levely
Level level0 = new Level(0, 8, 35, 'V', new int[2] { 0, 4 }, "level0.txt");
Level level1 = new Level(1, 9, 54, 'J', new int[2] { 2, 30 }, "level1.txt");
Level level2 = new Level(2, 14, 20, 'H', new int[2] { 11, 15 }, "level2.txt");

//Seznam levelů
List<Level> seznamLevelu = new List<Level> {level0, level1, level2};


int predchoziSouradnice0;
int predchoziSouradnice1;
int soucasnyLevelInt = 0;

//HRA! 

Level aktualniLevel = NacistLevel(soucasnyLevelInt);

string[] pole = VypsatLevel(aktualniLevel);

while (true) {
aktualniLevel.VymazatHrace(pole);


aktualniLevel.StisknutiKlavesy(pole);


pole = aktualniLevel.PoziceHrace(pole, aktualniLevel.souradniceHrace[1], aktualniLevel.souradniceHrace[0]);


aktualniLevel.VypisPole(pole);

if(aktualniLevel.pocetDrahokamu == 0)
    {
        Console.Clear();
        Console.WriteLine("Vyhráli jste! Ukončete stisknutím Q.");
        if (soucasnyLevelInt < seznamLevelu.Count -1)
        {
            Console.WriteLine("Nebo pokračujte na další level stisknutím Enter.");
            if (aktualniLevel.ZjistitKlavesu() == ConsoleKey.Enter)
            {
                Console.Clear();
                soucasnyLevelInt++;
                aktualniLevel = NacistLevel(soucasnyLevelInt);
                pole = VypsatLevel(NacistLevel(soucasnyLevelInt));
            }
        }
    }
}

Level NacistLevel(int cislo) {

    Level aktualniLevel = seznamLevelu[cislo];
    return aktualniLevel;
}

string[] VypsatLevel(Level level)
{
    string[] pole = level.NactiObsahLevelu(level.ZakladniPole());

    level.VypisPole(pole);
    return pole;

}

Console.ReadLine();