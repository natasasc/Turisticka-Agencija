using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class SmestajneJedinice
    {
        public static List<SmestajnaJedinica> ListaSmestajnihJedinica = new List<SmestajnaJedinica>();

        public static Smestaj FindSmestaj(string nazivSmestaja)
        {
            return Smestaji.FindByNaziv(nazivSmestaja);
        }

        public static SmestajnaJedinica FindByID(string id)
        {
            int broj = Int32.Parse(id);
            return ListaSmestajnihJedinica.Find(x => x.Identifikator == broj);
        }
    }
}