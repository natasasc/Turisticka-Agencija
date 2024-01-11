using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class MestoNalazenja
    {
        public string Adresa { get; set; }     // u formatu: ulica i broj, mesto/grad, poštanski broj
        public float GeografskaDuzina { get; set; }
        public float GeografskaSirina { get; set; }

        public string NazivAranzmana { get; set; }
        public bool Obrisano { get; set; }

        public MestoNalazenja()
        {
            Adresa = "";
            GeografskaDuzina = 0;
            GeografskaSirina = 0;
            NazivAranzmana = "";
            Obrisano = false;
        }

        public MestoNalazenja(string adresa, float geografskaDuzina, float geografskaSirina, string nazivAranzmana,
            bool obrisano)
        {
            Adresa = adresa;
            GeografskaDuzina = geografskaDuzina;
            GeografskaSirina = geografskaSirina;
            NazivAranzmana = nazivAranzmana;
            Obrisano = obrisano;
        }
    }
}