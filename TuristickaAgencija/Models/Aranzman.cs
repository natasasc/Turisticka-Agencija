using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class Aranzman
    {
        public string Naziv { get; set; }
        public TipAranzmana Tip { get; set; }
        public TipPrevoza Prevoz { get; set; }
        public string Lokacija { get; set; }
        public DateTime DatumPocetkaPutovanja { get; set; }      // čuvati u formatu dd/MM/yyyy
        public DateTime DatumZavrsetkaPutovanja { get; set; }    // čuvati u formatu dd/MM/yyyy
        public MestoNalazenja MestoNalazenja { get; set; }
        public string VremeNalazenja { get; set; }             // čuvati u formatu HH:mm
        public int MaxBrojPutnika { get; set; }
        public string OpisAranzmana { get; set; }
        public string ProgramPutovanja { get; set; }
        public string PosterAranzmana { get; set; }             // putanja do slike
        public Smestaj Smestaj { get; set; }

        public bool Obrisan { get; set; }
        public string Kreirao { get; set; }

        public Aranzman()
        {
            Naziv = "";
            Tip = TipAranzmana.NajamApartmana;
            Prevoz = TipPrevoza.Ostalo;
            Lokacija = "";
            DatumPocetkaPutovanja = DateTime.Now;
            DatumZavrsetkaPutovanja = DateTime.Now;
            MestoNalazenja = new MestoNalazenja();
            VremeNalazenja = "";
            MaxBrojPutnika = 0;
            OpisAranzmana = "";
            ProgramPutovanja = "";
            PosterAranzmana = "";
            Obrisan = false;
            Kreirao = "";
        }

        public Aranzman(string naziv, TipAranzmana tip, TipPrevoza prevoz, string lokacija, string dpp, string dzp,
            MestoNalazenja mestoNalazenja, string vremeNalazenja, int maxBrojPutnika, string opis, string program,
            string poster, Smestaj smestaj, bool obrisan, string kreirao)
        {
            Naziv = naziv;
            Tip = tip;
            Prevoz = prevoz;
            Lokacija = lokacija;

            string[] niz = dpp.Split('/');
            int dan = Int32.Parse(niz[0]);
            int mesec = Int32.Parse(niz[1]);
            int godina = Int32.Parse(niz[2]);

            DatumPocetkaPutovanja = new DateTime(godina, mesec, dan);

            niz = dzp.Split('/');
            dan = Int32.Parse(niz[0]);
            mesec = Int32.Parse(niz[1]);
            godina = Int32.Parse(niz[2]);

            DatumZavrsetkaPutovanja = new DateTime(godina, mesec, dan);

            MestoNalazenja = mestoNalazenja;
            VremeNalazenja = vremeNalazenja;
            MaxBrojPutnika = maxBrojPutnika;
            OpisAranzmana = opis;
            ProgramPutovanja = program;
            PosterAranzmana = poster;
            Smestaj = smestaj;
            Obrisan = obrisan;
            Kreirao = kreirao;
        }
    }
}