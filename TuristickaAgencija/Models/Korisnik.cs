using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class Korisnik
    {
        public string KorisnickoIme { get; set; }      // jedinstveno
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public PolKorisnika Pol { get; set; }
        public string Email { get; set; }
        public DateTime DatumRodjenja { get; set; }      // čuvati u formatu dd/MM/yyyy
        public UlogaKorisnika Uloga { get; set; }
        public List<Aranzman> Aranzmani;
        // Lista svih rezervacija aranžmana bez obzira na njihov status (ako je korisnik Turista)
        // Lista svih kreiranih aranžmana(ako je korisnik Menadžer)

        public int Otkazao { get; set; }
        public bool Blokiran { get; set; }

        public Korisnik()
        {
            KorisnickoIme = "";
            Lozinka = "";
            Ime = "";
            Prezime = "";
            Pol = PolKorisnika.Muski;
            Email = "";
            DatumRodjenja = DateTime.Now;
            Uloga = UlogaKorisnika.Turista;
            Aranzmani = new List<Aranzman>();
            Otkazao = 0;
            Blokiran = false;
        }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, PolKorisnika pol,
            string email, string datumRodjenja, UlogaKorisnika uloga, int otkazao, bool blokiran)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;

            if (datumRodjenja.Contains('.'))
                datumRodjenja = datumRodjenja.Replace('.', '/');
            string[] niz = datumRodjenja.Split('/');
            int dan = Int32.Parse(niz[0]);
            int mesec = Int32.Parse(niz[1]);
            int godina = Int32.Parse(niz[2]);
            DateTime datum = new DateTime(godina, mesec, dan);

            DatumRodjenja = datum;
            Uloga = uloga;
            Aranzmani = new List<Aranzman>();

            Otkazao = otkazao;
            Blokiran = blokiran;
        }
    }
}