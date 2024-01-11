using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class Rezervacija
    {
        public string Identifikator { get; set; }       // jedinstven, 15 karaktera
        public string Turista { get; set; }
        public StatusRezervacije Status { get; set; }
        public string Aranzman { get; set; }
        public int SmestajnaJedinica { get; set; }

        public Rezervacija()
        {
            Identifikator = "";
            Turista = "";
            Status = StatusRezervacije.Otkazana;
            Aranzman = "";
            SmestajnaJedinica = 0;
        }

        public Rezervacija(string id, string turista, StatusRezervacije status, string aranzman, int sj )
        {
            Identifikator = id;
            Turista = turista;
            Status = status;
            Aranzman = aranzman;
            SmestajnaJedinica = sj;
        }

        private static Random random;
        public static string GetID()
        {
            string id;
            while (true)
            {
                random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                id = new string(Enumerable.Repeat(chars, 15).Select(s => s[random.Next(s.Length)]).ToArray());

                Rezervacija r = Rezervacije.ListaRezervacija.Find(x => x.Identifikator == id);
                if (r == null)
                    break;
            }
            
            return id;
        }
    }
}