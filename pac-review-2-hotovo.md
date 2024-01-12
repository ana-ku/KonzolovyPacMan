# CODE REVIEW: Pacman (8.1.2024)

## Načítání ze souborů

```csharp
string root = @"../../../data/";
List<Level> seznamLevelu = new List<Level>() ;
var soubory = from soubor in Directory.EnumerateFiles(root) select soubor;
foreach (var soubor in soubory)
{
  Level level = new Level(soubor);
  seznamLevelu.Add(level);
}
```

Jo, takhle nějak jsem to myslel a zatím nám to stačí.

*//Juchů!*

-----

## Serializace, deserializace

> jak můžu string, který získám z načtení souboru, uložit do objektu? rozsekat ho stejný způsobem, jakým jsem to dělala dosud?

Vygoogli si pojem *C# XML serialization*. Pomocí *serializace* se dají převádět objekty do řetězce (které je pak snadné uložit do souboru) a pomocí *deserializace* je zase možné vytvářet objekty z řetězců (které můžeš načítat z uložených souborů).

//*Nechám tedy teď být*

-----

## ZmenPoziciHrace

> Chápu to, ale nevím, jak to změnit, když zároveň potřebuji mít proměnnou pole, se kterou pak pracují další metody. Zkusila jsem to upravit, aby se alespoň oddělilo vypsání pole do konzoly a načtení pole do proměnné.
> 
> > Ta proměnná `string[] pole` je reference, takže na řádku `38` metoda `PoziceHrace` vrátí totéž pole, do kterého ten výsledek přiřazuješ. To přiřazení (`pole =`) je tam tudíž zbytečné a matoucí.

Zkusím to jinak. Podívej se na následující řádky tvého kódu:

```csharp
aktualniLevel.VymazHrace(hraciPole);
hraciPole = aktualniLevel.ZmenPoziciHrace(hraciPole);
```

Obě tyto metody zjevně dělají v podstatě tu stejnou činnost -- nějak změní hrací pole. Proč ale metoda `VymazHrace` nic nevrací (tzn. má návratový typ `void`), ale metoda `ZmenPoziciHrace` vrací typ `string[]`?

*//Sloučila jsem metody do jediné, tj., před změnou pozice hráče se nejdřív vymaže jeho znak ze současné pozice. Nebo by bylo i good practise, aby `VymazHrace`vracel také string array?*

## Typ souřadnice

```csharp
struct PoziceHrace ...
```

Teď sice používáš typ (`PoziceHrace`) pro pozici hráče, ale nadále používáš typ `int[]` pro pozici prvků. Výhodnější by bylo použít jeden obecný typ `Pozice` pro pozici všech objektů, tedy jak hráče tak prvků.

*//Hotovo, ještě jsem trochu zkrátila umisťování značek na pole, aby se tam neopakoval kód.* 

-----

## Třídy a objekty

To nejdůležitější jsem nestihl...
