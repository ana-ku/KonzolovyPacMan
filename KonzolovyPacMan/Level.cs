using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace KonzolovyPacMan
{
    internal class Level
    {
        public int IndexLevelu;

        public int Vyska;

        public int Sirka;

        public string PrekazkaZnak;

        public Pozice PocatecniSouradniceHrace;

        public string CestaKSouboru;

        string[] obsahSouboru;

        public string[] PolePrekazekDrahokamu;

        public Level(string cestaKSouboru)
        {
            CestaKSouboru = cestaKSouboru;
            obsahSouboru = File.ReadAllLines(CestaKSouboru);

            InicializovatLevel(); //AI: v konstruktoru použít metodu pro inicializování levelu namísto konkrétního kódu pro inicializaci levelu
        }

        private void InicializovatLevel()
        {
            //1. Index levelu
            
            string indexLeveluString = Regex.Match(obsahSouboru[0], @"\d+").Value;
            IndexLevelu = int.Parse(indexLeveluString);

            //2. Výška pole

            string vyskaPoleString = Regex.Match(obsahSouboru[1], @"\d+").Value;
            Vyska = int.Parse(vyskaPoleString);

            //3. Šířka pole

            string sirkaPoleString = Regex.Match(obsahSouboru[2], @"\d+").Value;
            Sirka = int.Parse(sirkaPoleString);

            //4. Počáteční pozice hráče

            string[] pocatPoziceSplit = obsahSouboru[3].Split(':');
            string[] pocatPozice = pocatPoziceSplit[1].Split(' ');
            int[] cislaPocatPozice = Array.ConvertAll(pocatPozice, s => int.Parse(s));
            PocatecniSouradniceHrace = new Pozice(cislaPocatPozice[0], cislaPocatPozice[1]);
            

            //5. Znak pro překážku
            string prekazkaZnak = Regex.Match(obsahSouboru[4], @":(.*)").Groups[1].Value;
            PrekazkaZnak = prekazkaZnak;

            //6. Překážky a drahokamy
            List<string> seznamPrekazekDrahokamu = obsahSouboru.ToList();
            //převést pole stringů do seznamu
            seznamPrekazekDrahokamu.RemoveRange(0, 6);
            //odstranit prvních 6 řádků, aby zbyly jen souřadnice překážek/drahokamů a jejich znaky
            
            PolePrekazekDrahokamu = seznamPrekazekDrahokamu.ToArray();
        }
   

    }
}
