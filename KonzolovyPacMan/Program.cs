//příprava herního pole

using System.Runtime.CompilerServices;

//rozměry pole, pevně dané
int vyska = 8;
int sirka = 30;


//Znaky prvků
string prekazkaZnak = "x";
string pokladZnak = "o";

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


//STAVBA POLE
//horní okraj pole +-----+
OkrajPoleHorniDolni();
   
Console.WriteLine();

//tělo pole
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
foreach (KeyValuePair<string, int[]> entry in seznamPrvku)
{
    int hodnotaX = entry.Value[1];
    int hodnotaY = entry.Value[0];

    if (entry.Key.StartsWith("X"))
    {
        pole[hodnotaY].Remove(hodnotaX, 1).Insert(hodnotaX, "X");
    }
    else
    {
        pole[hodnotaY].Remove(hodnotaX, 1).Insert(hodnotaX, "o");
    }

}

//vypsat obsah pole
foreach (var item in pole)
{
    Console.WriteLine(item.ToString());
}

//dolní okraj pole
OkrajPoleHorniDolni();


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