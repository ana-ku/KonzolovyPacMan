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

List<Level> SeznamLevelu = NacistSeznamLevelu(root);

Hra hra = new Hra(SeznamLevelu);

hra.Play();

static List<Level> NacistSeznamLevelu(string root)
{
    var soubory = Directory.EnumerateFiles(root);
    return soubory.Select(soubor => new Level(soubor)).ToList();

}

Console.ReadLine();