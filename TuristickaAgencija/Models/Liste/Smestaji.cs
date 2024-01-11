using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class Smestaji
    {
        public static List<Smestaj> ListaSmestaja = new List<Smestaj>();

        public static Smestaj FindByNaziv(string naziv)
        {
            return ListaSmestaja.Find(item => item.Naziv == naziv);
        }

        public static int BrojSlobodnihJedinica(Smestaj smestaj)
        {
            int ret = 0;

            foreach (SmestajnaJedinica sj in smestaj.Jedinice)
            {
                if (!sj.Zauzeta)
                    ret++;
            }

            return ret;
        }

        public static List<Smestaj> Copy(List<Smestaj> smestaji)
        {
            List<Smestaj> ret = new List<Smestaj>();

            foreach (Smestaj s in smestaji)
            {
                ret.Add(s);
            }

            return ret;
        }
    }
}