# CODE REVIEW

## Načítání ze souborů

```csharp
//Všechny levely
Level level0 = new Level(0, 8, 35, 'V', new int[2] { 0, 4 }, "level0.txt");
Level level1 = new Level(1, 9, 54, 'J', new int[2] { 2, 30 }, "level1.txt");
Level level2 = new Level(2, 14, 20, 'H', new int[2] { 11, 15 }, "level2.txt");
```

Tím, že v kódu definuješ vlastnosti jako *číslo*, *výška* či *šířka* levelu, tak máš pořád část levelu definovanou v kódu -- v souboru máš definovány jen objekty. Tvůj potenciální level designér by tak pořád musel být schopen editovat a přeložit Tvůj projekt. Ideálním řešením by bylo, kdyby veškeré informace o levelech byly obsažené v souborech a aby program nebylo třeba vůbec překládat, když třeba level designér vyrobí a přidá nový level.

Typicky se načítání ze souborů dělá tak, že se projdou všechny soubory v dané složce (typicky něco jako `data/levels/`) a každý nalezený soubor se načtě do objektu (Level) a ten se pak uloží do nějaké kolekce (List<Level>).  ***//Hotovo, zkusila jsem všechno naházet do jednoho txt souboru***

-----

## Typ souřadnice

```csharp
public int[] PocatecniSouradniceHrace = new int[2];
```

Používat pole pro pozici není vhodné z několika důvodů:

1. Do proměnné typu `int[]` lze přiřadit pole o jakékoliv velikosti, tedy např. i velikosti 1. Typy proměnných v jazycích mají plnit tu roli, že zabraňují přiřazení nevhodné hodnoty do proměnné. Kdybych do `PocatecniSouradniceHrace `přiřadil  `new int[1]`, má proměnná nevhodnou hodnotu.

2. Pole je referenční typ. Časem by se Ti opět mohlo stát, že někde omylem přiřadíš dvěma proměnným stejný objekt v paměti a budeš se divit, že po změně jedné se změní i druhá.

3. Používání výrazů jako `Pozice[0]` není intuitivní. Nemusí být každému jasné, zda `X` odpovídá `0` a `Y` odpovídá `1` či naopak.

Vhodnější by bylo si vytvořit strukturu `Pozice`, která v sobě bude mít `X` a `Y`. Struktura je narozdíl od třídy hodnotový typ, takže se nemusíš bát, že se stane to, že změníš jednu proměnnou a záhadně se změní i druhá -- při přiřazení se vždy vytvoří nová kopie. ***//structs jsme v kurzu nebrali :(. Použila jsem to tady jako způsob ukládání informací o pozici hráče.***

```csharp
public struct Pozice
{
    public int X {get; set;}
    public int Y {get; set;}
    public Pozice(int x, int y) //konstruktor
    {
        X = x;
        Y = y;
    }
}
```

-----

## Zdrojové soubory

Dobrou praxí je mít každou třídu v jednom souboru jmenujícím se stejně, jako třída. Bylo by tedy vhodné namísto `Class1.cs` mít `Level.cs`. **//Jasně, to dá rozum!**

-----

## Názvy metod a proměnných

Z názvu metody (tzn. už z prvního řádku) by mělo bý patrné, co daná metoda dělá. Stejně tak u proměnných by něko být zřejmé, k čemu se používá. Uvedu pár příkladů z Tvého kódu, které by šly vylepšit.

- `int soucasnyLevelInt`
  
  - Tím `Int` na konci jsi nejspíš chtěla dostatečně odlišit proměnné obsahující samotný objekt  `Level` od proměné obsahující číslo levelu. V tom případě by bylo nejlepší použít rovnou název `cisloSoucasnehoLevelu` nebo `indexSoucasnehoLevelu` pokud chceš zdůraznit, že jde o sekvenci začínající `0` a ne `1`.

- `public void OkrajPoleHorniDolni()` 
  
  - Na první pohled není zřejmé, že metoda něco vypisuje. Lepší by bylo `VypisOkrajPoleHorniDolni`.

- `public string[] PoziceHrace(string[] pole, int x, int y)`

- `public void StisknutiKlavesy(string[] pole)`
  
  - Obecně by název metody měl začínat slovesem (*vrať*, *vypočti*, *zjisti*, *změň* atd.). 
  - `pole` je příliš obecný název a navíc jde v podstatě o název typu, což taky není vhodné dávat do názvů. Lepší by bylo nazvat to např. `HraciPole`, `HerniDeska` apod. //***upravila jsem***

-----

## Celková architektura programu

- Určitě by se dalo vymyslet více tříd, než samotný `Level`. Zamysli se nad tím, jak Tě napadlo, že zrovna `Level` by mohl být třída. Třeba bude ta stejná úvaha platit i pro jiné prvky v programu a uděláš z nich také třídy. ***//Zamyslím se nad tím, pořád asi úplně nechápu funkci tříd a objektů...***

- - Jedním z důvodů je např. to, že kdybychom chtěli přidat možnost resetovat současný level, tak budeš muset znovu načíst ze souboru daný level, protože v tom současném (již rozehraném) už mohou chybět některé diamanty.

- Ta proměnná `string[] pole` je reference, takže na řádku `38` metoda `PoziceHrace` vrátí totéž pole, do kterého ten výsledek přiřazuješ. To přiřazení (`pole =`) je tam tudíž zbytečné a matoucí. **//Chápu to, ale nevím, jak to změnit, když zároveň potřebuji mít proměnnou pole, se kterou pak pracují další metody. Zkusila jsem to upravit, aby se alespoň oddělilo vypsání pole do konzoly a načtení pole do proměnné.**

- Na řádku `38` pasuješ do metody `PoziceHrace` parametry s pozicí hráče, ale ty jsou k dispozici přímo ve třídě `Level`. Volání lze tedy zjednodušit na `aktualniLevel.PoziceHrace(pole)`. **//Jooo, upravila jsem. Krásně elegantní.**

- Uvnitř while cyklu se tedy v podstatě opakuje vzorec:
  
  - `aktualniLevel.VymazatHrace(pole);`
  - `aktualniLevel.StisknutiKlavesy(pole);`
  - `aktualniLevel.PoziceHrace(pole);`
  - `aktualniLevel.VypisPole(pole);`
  - Takový opakující se vzorec úplně vybízí k úvahám, zda by ten parametr `pole` nemohl být rovnou součástí toho objektu na levé straně. *//**Tomu nerozumím. Objektu aktualniLevel, nebo připravit nový, další objekt - hrací pole? Nebo ho mít jako jednu z vlastností objektu Aktualní level, a díky tomu bych ho nemusela posílat těm jednotlivým metodám jako argument? Zkusím se nad tím zamyslet.  ***

-----

## Ostatní

Obecně je dobré být ve všem konzistentní:

- Názvy
- Odsazení uvnitř těla bloků
- Otevírání a zavírání všech závorek
- Všude stejný typ mezer (tzn. buď všude mezery, nebo všude taby) a taky všude stejnou šířku odsazení ***//Ano,  zkusím na to dávat pozor. Já vždycky dávala přednost obsahu před formou :).***
