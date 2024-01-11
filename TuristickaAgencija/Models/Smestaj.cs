using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class Smestaj
    {
        public TipSmestaja Tip { get; set; }
        public string Naziv { get; set; }
        public int BrojZvezdica { get; set; }       // za hotele
        public bool Bazen { get; set; }
        public bool SpaCentar { get; set; }
        public bool PrilagodjenoZaInvalide { get; set; }
        public bool Wifi { get; set; }
        public List<SmestajnaJedinica> Jedinice;

        public string NazivAranzmana { get; set; }
        public bool Obrisan { get; set; }

        public Smestaj()
        {
            Tip = TipSmestaja.Vila;
            Naziv = "";
            BrojZvezdica = 0;
            Bazen = false;
            SpaCentar = false;
            PrilagodjenoZaInvalide = false;
            Wifi = false;
            Jedinice = new List<SmestajnaJedinica>();
            Obrisan = false;
        }

        public Smestaj(TipSmestaja tip, string naziv, int brojZvezdica, bool bazen, bool spaCentar, 
            bool prilagodjenoZaInvalide, bool wifi, string nazivAranzmana, bool obrisan)
        {
            Tip = tip;
            Naziv = naziv;
            BrojZvezdica = brojZvezdica;
            Bazen = bazen;
            SpaCentar = spaCentar;
            PrilagodjenoZaInvalide = prilagodjenoZaInvalide;
            Wifi = wifi;
            NazivAranzmana = nazivAranzmana;

            Jedinice = new List<SmestajnaJedinica>();
            Obrisan = obrisan;
        }
    }
}