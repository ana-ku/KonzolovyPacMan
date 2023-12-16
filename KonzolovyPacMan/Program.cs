using Microsoft.VisualBasic.Devices;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
//Rozměry pole, pevně dané
int vyska = 8;
int sirka = 30;


//Znaky prvků
string prekazkaZnak = "x";
string pokladZnak = "o";
string hracZnak = "P";

//Soupis prvků (souřadnice Y,X)
Dictionary<string, int[]> seznamPrvku = new Dictionary<string, int[]>() {
    { "X1", new int[] { 0, 5 } },
    { "X2", new int[] { 1, 5 } },
    { "X3", new int[] { 1, 20 } },
    { "X4", new int[] { 1, 21 } },
    { "X5", new int[] { 1, 22 } },
    { "X6", new int[] { 2, 12 } },
    { "X7", new int[] { 2, 22 } },
    { "X8", new int[] { 3, 22 } },
    { "X9", new int[] { 4, 4 } },
    { "X10", new int[] { 5, 4 } },
    { "X11", new int[] { 6, 4 } },
    { "X12", new int[] { 6, 18 } },
    { "X13", new int[] { 6, 19 } },
    { "X14", new int[] { 6, 20 } },
    { "o1", new int[] { 0, 2 } },
    { "o2", new int[] { 0, 25 } },
    { "o3", new int[] { 1, 9 } },
    { "o4", new int[] { 3, 23 } },
    { "o5", new int[] { 4, 2 } },
    { "o6", new int[] { 4, 17 } },
    { "o7", new int[] { 7, 2 } },
    { "o8", new int[] { 7, 19 } },
};

//player
int[] pocatecniSouradniceHrace = new int[2] { 5, 5 };
int[] souradniceHrace = new int[2];
int predchoziSouradnice0;
int predchoziSouradnice1;


//STAVBA POLE
//horní okraj pole
OkrajPoleHorniDolni(); //nemění se
   
Console.WriteLine();



//TĚLO POLE
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
//vyměnit za překážky/poklady
int pocetDrahokamu = 0;
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
        Console.WriteLine("Vyhráli jste! Ukončete stisknutím Q.");
    }
    else 
    { 

    OkrajPoleHorniDolni(); 

    Console.WriteLine();

    foreach (var item in pole)
    {
        Console.WriteLine(item.ToString());
    }
    OkrajPoleHorniDolni();

    Console.WriteLine("\nZbývající počet drahokamů: " + pocetDrahokamu);
    }
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

    predchoziSouradnice0 = souradniceHrace[0];
    predchoziSouradnice1 = souradniceHrace[1];
    
    
    //pokud není na políčku X

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

    string radek = pole[souradniceHrace[0]];
    char znak = radek[souradniceHrace[1]];
    if(znak == 'x')
    {
        souradniceHrace[1] = predchoziSouradnice1;
        souradniceHrace[0] = predchoziSouradnice0;
    }
}
void PrestatPoslouchatKlavesy()
{

}

Console.ReadLine();