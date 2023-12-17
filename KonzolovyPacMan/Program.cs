using Microsoft.VisualBasic.Devices;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Text.RegularExpressions;
using System;
//Rozměry pole, pevně dané
int vyska = 8;
int sirka = 30;

//Znaky prvků
string prekazkaZnak = "x";
string pokladZnak = "o";
string hracZnak = "P";

//Příprava k načtení souboru s překážkami/poklady
int pocetLevelu = 3;
string level = "level";
string[] levely = new string[pocetLevelu];
string cesta = @"../../../";

for(int i = 0; i<pocetLevelu; i++)
{
    levely[i] = cesta + level + (i.ToString()) + ".txt";
}

//Obsah levelu
int soucasnyLevel = 0;
Dictionary<string, int[]> seznamPrvku = new Dictionary<string, int[]>();
int pocetDrahokamu = 0;

//Player
int[] pocatecniSouradniceHrace = new int[2] { 5, 5 };
int[] souradniceHrace = new int[2];
int predchoziSouradnice0;
int predchoziSouradnice1;


//STAVBA POLE
//Horní okraj pole
OkrajPoleHorniDolni(); //nemění se
   
Console.WriteLine();

//Tělo pole
string[] pole = new string[vyska];
for (int j = 0; j < vyska; j++)
{   
    for (int i = 0; i <= sirka; i++)
    {
        if (i == 0)
        {
            pole[j] += "|";
        }
        if (i == sirka)
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

//Naplnit level
NacistObsahLevelu();




//Umístit hráče na výchozí pozici

PoziceHrace(pocatecniSouradniceHrace[1], pocatecniSouradniceHrace[0]);


//Vypsat obsah pole

foreach (var item in pole)
{
    Console.WriteLine(item.ToString());
}

//dolní okraj pole
OkrajPoleHorniDolni();

Console.WriteLine("\nZbývající počet drahokamů: " + pocetDrahokamu);
Console.WriteLine("Level " + (soucasnyLevel +1) + "/" + pocetLevelu);

//HRA! 

while (true) {


    //Odstranit hráče ze současné pozice
    VymazatHrace();

    //Stisknutí klávesy - vypočte nový index pozice hráče
    StisknutiKlavesy();

    //Změna pozice hráče - znovu umístí hráče 
    PoziceHrace(souradniceHrace[1], souradniceHrace[0]);

    Console.Clear();

    if (pocetDrahokamu == 0)
    {
        Console.Clear();
        Console.WriteLine("Vyhráli jste! Ukončete stisknutím Q.");
        if (soucasnyLevel < pocetLevelu - 1)
        {
        Console.WriteLine("Nebo pokračujte na další level stisknutím Enter.");
        if (ZjistitKlavesu() == ConsoleKey.Enter)
        {
            Console.Clear();
            soucasnyLevel++;
            VymazatPredchoziLevel();
            NacistObsahLevelu();
            PoziceHrace(pocatecniSouradniceHrace[1], pocatecniSouradniceHrace[0]);
            }
        }
    }
    

    OkrajPoleHorniDolni(); 

    Console.WriteLine();

    foreach (var item in pole)
    {
        Console.WriteLine(item.ToString());
    }
    OkrajPoleHorniDolni();

    Console.WriteLine("\nZbývající počet drahokamů: " + pocetDrahokamu);
    Console.WriteLine("Level " + (soucasnyLevel +1) + "/" + pocetLevelu);


}


//METODY
void VymazatHrace()
{

    var x = souradniceHrace[1];
    var y = souradniceHrace[0];
    pole[y] = pole[y].Remove(x, 1).Insert(x, " ");

}

void PoziceHrace(int x, int y)
{
    souradniceHrace[1] = x;
    souradniceHrace[0] = y;
  
    //pokud je na nové pozici drahokam
    string radek = pole[y];
    char znak = radek[x];
    if(znak.Equals('o'))
    {
        pocetDrahokamu--;
    }
    
    pole[y] = pole[y].Remove(x, 1).Insert(x, hracZnak);
    
}
void NapisZnak(string znak)
{
   Console.Write(znak);
}

void OkrajPoleHorniDolni()
{
    NapisZnak("+");
    for (int i = 0; i < sirka; i++)
    {
        NapisZnak("-");
    }
    NapisZnak("+");
}


//
ConsoleKey ZjistitKlavesu()
{
    var key = Console.ReadKey(true);
    return key.Key;
}

void StisknutiKlavesy() {

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
if (stisknutaKlavesa == ConsoleKey.DownArrow && souradniceHrace[0] < vyska -1)
{
    souradniceHrace[0]++;
}
if (stisknutaKlavesa == ConsoleKey.LeftArrow && souradniceHrace[1] > 1)
{
    souradniceHrace[1]--;
}
if (stisknutaKlavesa == ConsoleKey.RightArrow && souradniceHrace[1] < sirka)
{
    souradniceHrace[1]++;
}
 
//Kontrola, jestli není na nově přistupovaném poli znak X

    string radek = pole[souradniceHrace[0]];
    char znak = radek[souradniceHrace[1]];
    if(znak == 'x')
    {
        souradniceHrace[1] = predchoziSouradnice1;
        souradniceHrace[0] = predchoziSouradnice0;
    }
}
void NacistObsahLevelu()
{

    pocetDrahokamu = 0;
    seznamPrvku.Clear();

    string[] radkyLevelu = File.ReadAllLines(levely[soucasnyLevel]);
    
    //připravit slovník
    foreach (var item in radkyLevelu)
    {
        string[] info = item.Split(':');
        string[] cislaString = info[1].Split(' ');
        int[] cislaInt = Array.ConvertAll(cislaString, s => int.Parse(s));
        seznamPrvku.Add(info[0], new int[] { cislaInt[0], cislaInt[1] }); //Vytvoření slovníku s překážkami a poklady
    }
    //Vyměnit za překážky/poklady
    foreach (KeyValuePair<string, int[]> entry in seznamPrvku)
    {
        int hodnotaX = entry.Value[1];
        int hodnotaY = entry.Value[0];


        if (entry.Key.StartsWith("X"))
        {
            pole[hodnotaY] = pole[hodnotaY].Remove(hodnotaX, 1).Insert(hodnotaX, prekazkaZnak);
        }
        else
        {
            pocetDrahokamu++;
            pole[hodnotaY] = pole[hodnotaY].Remove(hodnotaX, 1).Insert(hodnotaX, pokladZnak);
        }

    }

}
void VymazatPredchoziLevel() 
{
    for (int i = 0; i <pole.Length;i++)
    {
        pole[i] = pole[i].Replace(prekazkaZnak, " ").Replace(pokladZnak, " ");
    }
}

Console.ReadLine();