using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class Komentari
    {
        public static List<Komentar> ListaKomentara = new List<Komentar>();

        public static List<Komentar> FindByAranzman(string naziv)
        {
            List<Komentar> ret = new List<Komentar>();

            foreach (Komentar k in ListaKomentara)
            {
                if (k.Aranzman == naziv)
                    ret.Add(k);
            }

            return ret;
        }
    }
}