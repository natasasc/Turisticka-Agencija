using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class SmestajnaJedinica
    {
        public int DozvoljenBrojGostiju { get; set; }
        public bool Ljubimci { get; set; }
        public float Cena { get; set; }

        public string NazivSmestaja { get; set; }
        public bool Zauzeta { get; set; }
        public string KorisnickoIme { get; set; }
        public int Identifikator { get; set; }
        public bool Obrisana { get; set; }

        public SmestajnaJedinica()
        {
            DozvoljenBrojGostiju = 0;
            Ljubimci = false;
            Cena = 0;
            NazivSmestaja = "";
            Zauzeta = false;
            KorisnickoIme = "";
            Identifikator = 0;
            Obrisana = false;
        }

        public SmestajnaJedinica(int dozvoljenBrojGostiju, bool ljubimci, float cena, string nazivSmestaja, 
            bool zauzeta, string korisnickoIme, int id, bool obrisana)
        {
            DozvoljenBrojGostiju = dozvoljenBrojGostiju;
            Ljubimci = ljubimci;
            Cena = cena;
            NazivSmestaja = nazivSmestaja;
            Zauzeta = zauzeta;
            KorisnickoIme = korisnickoIme;
            Identifikator = id;
            Obrisana = obrisana;
        }

        private Object thisLock;
        public int GetID()
        {
            thisLock = new Object();
            int ret = 0;

            lock (thisLock)
            {
                foreach (SmestajnaJedinica sj in SmestajneJedinice.ListaSmestajnihJedinica)
                {
                    if (sj.Identifikator > ret)
                        ret = sj.Identifikator;
                }
            }

            return ++ret;
        }
    }
}