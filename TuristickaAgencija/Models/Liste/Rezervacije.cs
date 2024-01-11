using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class Rezervacije
    {
        public static List<Rezervacija> ListaRezervacija = new List<Rezervacija>();

        public static Rezervacija FindByID(string id)
        {
            return ListaRezervacija.Find(item => item.Identifikator == id);
        }
    }
}