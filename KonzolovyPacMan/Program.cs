using Microsoft.VisualBasic.Devices;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Text.RegularExpressions;
using System;
using KonzolovyPacMan;
using System.Security.Authentication.ExtendedProtection;
using System.Reflection.Emit;


//Projít všechny soubory v /data

string root = @"../../../data/";
List<Level> seznamLevelu = new List<Level>() ;
var soubory = from soubor in Directory.EnumerateFiles(root) select soubor;
foreach (var soubor in soubory)
{
    Level level = new Level(soubor);
    seznamLevelu.Add(level);
}


int predchoziSouradnice0;
int predchoziSouradnice1;
int indexSoucasnehoLevelu = 0;

//HRA! 

Level aktualniLevel = NactiLevel(indexSoucasnehoLevelu);

string[] hraciPole = aktualniLevel.NactiObsahLevelu(aktualniLevel.ZiskejZakladniPole());

aktualniLevel.VypisPole(hraciPole);

while (true)
{

    aktualniLevel.VymazHrace(hraciPole);


    aktualniLevel.ProvedStisknutiKlavesy(hraciPole);


    hraciPole = aktualniLevel.ZmenPoziciHrace(hraciPole);


    aktualniLevel.VypisPole(hraciPole);

    if (aktualniLevel.PocetDrahokamu == 0)
    {
        Console.Clear();
        Console.WriteLine("Vyhráli jste! Ukončete stisknutím Q.");
        if (indexSoucasnehoLevelu < seznamLevelu.Count - 1)
        {
            Console.WriteLine("Nebo pokračujte na další level stisknutím Enter.");
            if (aktualniLevel.ZjistiKlavesu() == ConsoleKey.Enter)
            {
                Console.Clear();
                indexSoucasnehoLevelu++;
                aktualniLevel = NactiLevel(indexSoucasnehoLevelu);
                hraciPole = NactiPole(NactiLevel(indexSoucasnehoLevelu));
                aktualniLevel.VypisPole(hraciPole);
            }
        }
    }
}

Level NactiLevel(int cislo)
{

    Level aktualniLevel = seznamLevelu[cislo];
    return aktualniLevel;
}

string[] NactiPole(Level level)
{
    string[] hraciPole = level.NactiObsahLevelu(level.ZiskejZakladniPole());
    return hraciPole;
}

Console.ReadLine();