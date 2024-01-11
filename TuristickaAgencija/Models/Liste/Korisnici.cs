using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class Korisnici
    {
        public static List<Korisnik> ListaKorisnika = new List<Korisnik>();

        public static Korisnik FindByUsername(string naziv)
        {
            return ListaKorisnika.Find(item => item.KorisnickoIme == naziv);
        }
    }
}