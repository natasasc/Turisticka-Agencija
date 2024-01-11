using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuristickaAgencija.Models;

namespace TuristickaAgencija.Controllers
{
    public class AdministratorController : Controller
    {
        // GET: Administrator
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registracija()
        {
            Korisnik k = new Korisnik();
            return View(k);
        }

        [HttpPost]
        public ActionResult Registracija(Korisnik menadzer)
        {
            if (menadzer.KorisnickoIme == null || menadzer.Lozinka == null || menadzer.Ime == null || menadzer.Prezime == null
                || menadzer.Email == null || menadzer.DatumRodjenja == null)
            {
                ViewBag.Message = "Sva polja su obavezna!";
                return View();
            }


            List<Korisnik> users = Korisnici.ListaKorisnika;
            foreach (Korisnik k in users)
            {
                if (k.KorisnickoIme == menadzer.KorisnickoIme)
                {
                    ViewBag.Message = $"Korisnik {menadzer.KorisnickoIme} vec postoji!";
                    return View();
                }
            }

            if (Request["Pol"] != "Muski" && Request["Pol"] != "Zenski")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za pol!";
                return View();
            }

            menadzer.Uloga = UlogaKorisnika.Menadzer;
            menadzer.Otkazao = 0;
            menadzer.Blokiran = false;
            users.Add(menadzer);
            Korisnici.ListaKorisnika = users;
            Data.SacuvajKorisnika(menadzer);

            return View("Index");
        }

        public ActionResult SviKorisnici()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SortiranjeKorisnika()
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            korisnici = Korisnici.ListaKorisnika;

            string vrsta, nacin;
            vrsta = Request["sortvrsta"] != null ? Request["sortvrsta"] : "";
            nacin = Request["sortnacin"] != null ? Request["sortnacin"] : "";

            if (vrsta != "imenu" && vrsta != "prezimenu" && vrsta != "ulozi")
            {
                ViewBag.Message = "Odaberite neku od ponudjenih vrsta sortiranja!";
                return View("SviKorisnici");
            }

            if (nacin != "rastuce" && nacin != "opadajuce")
            {
                ViewBag.Message = "Odaberite neki od ponudjenih nacina sortiranja!";
                return View("SviKorisnici");
            }

            List<Korisnik> SortedList = new List<Korisnik>();

            if (vrsta == "imenu")
            {
                SortedList = korisnici.OrderBy(o => o.Ime).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = korisnici.OrderByDescending(o => o.Ime).ToList();
                }
            }
            else if (vrsta == "prezimenu")
            {
                SortedList = korisnici.OrderBy(o => o.Prezime).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = korisnici.OrderByDescending(o => o.Prezime).ToList();
                }
            }
            else    // vrsta = "ulozi"
            {
                SortedList = korisnici.OrderBy(o => o.Uloga).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = korisnici.OrderByDescending(o => o.Uloga).ToList();
                }
            }

            Korisnici.ListaKorisnika = SortedList;
            return View("SviKorisnici");
        }

        [HttpPost]
        public ActionResult PretragaKorisnika()
        {
            List<Korisnik> korisnici = Korisnici.ListaKorisnika.ToList();
            List<Korisnik> korisnikFilter = korisnici.ToList();

            string ime = Request["Ime"] != null ? Request["Ime"] : "";
            string prezime = Request["Prezime"] != null ? Request["Prezime"] : "";
            string uloga = Request["Uloga"] != null ? Request["Uloga"] : "";

            ViewBag.TrazeniKorisnik = null;

            if (uloga != UlogaKorisnika.Turista.ToString() && uloga != UlogaKorisnika.Menadzer.ToString()
                && uloga != UlogaKorisnika.Administrator.ToString() && uloga != "")
            {
                ViewBag.Message = "Los format uloge korisnika. Odaberite neku od ponudjenih uloga!";
                return View("SviKorisnici");
            }

            if (ime != "")
            {
                foreach (Korisnik k in korisnici)
                {
                    if (!k.Ime.Contains(ime) && korisnikFilter.Contains(k))
                        korisnikFilter.Remove(k);
                }
            }

            if (prezime != "")
            {
                foreach (Korisnik k in korisnici)
                {
                    if (!k.Prezime.Contains(prezime) && korisnikFilter.Contains(k))
                        korisnikFilter.Remove(k);
                }
            }

            if (uloga != "")
            {
                foreach (Korisnik k in korisnici)
                {
                    if (k.Uloga.ToString() != uloga && korisnikFilter.Contains(k))
                        korisnikFilter.Remove(k);
                }
            }
            
            if (korisnikFilter.Count == 0)
                ViewBag.Message = "Nema korisnika za prikazivanje.";
            else
                ViewBag.TrazeniKorisnik = korisnikFilter;

            return View("SviKorisnici");
        }

        public ActionResult SumnjiviTuristi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Blokiraj()
        {
            Korisnik k = Korisnici.FindByUsername(Request["KorisnickoIme"]);
            k.Blokiran = true;
            Data.AzurirajKorisnike();

            return View("SumnjiviTuristi");
        }
    }
}