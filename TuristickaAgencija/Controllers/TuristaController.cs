using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuristickaAgencija.Models;

namespace TuristickaAgencija.Controllers
{
    public class TuristaController : Controller
    {
        // GET: Turista
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Rezervisi()
        {
            SmestajnaJedinica sj = SmestajneJedinice.FindByID(Request["Identifikator"]);

            sj.Zauzeta = true;
            sj.KorisnickoIme = ((Korisnik)Session["user"]).KorisnickoIme;
            Data.AzurirajSmestajneJedinice();

            Rezervacija rez = Rezervacije.ListaRezervacija.Find(x => (x.Turista == ((Korisnik)Session["user"]).KorisnickoIme
                && x.SmestajnaJedinica == sj.Identifikator));

            if (rez == null)
            {
                string id = Rezervacija.GetID();
                rez = new Rezervacija(id, ((Korisnik)Session["user"]).KorisnickoIme,
                    StatusRezervacije.Aktivna, (Smestaji.FindByNaziv(sj.NazivSmestaja)).NazivAranzmana, sj.Identifikator);
                Rezervacije.ListaRezervacija.Add(rez);
                Data.AzurirajRezervacije();
            }
            else
            {
                rez.Status = StatusRezervacije.Aktivna;
                Data.AzurirajRezervacije();
            }
            
            string temp = (SmestajneJedinice.FindSmestaj(sj.NazivSmestaja)).NazivAranzmana;
            Aranzman a = Aranzmani.FindByNaziv(temp);

            Korisnik k = Korisnici.FindByUsername(sj.KorisnickoIme);
            if (!k.Aranzmani.Contains(a))
            {
                k.Aranzmani.Add(a);
            }

            return View("Index");
        }

        public ActionResult MojeRezervacije()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Otkazi()
        {
            SmestajnaJedinica sj = SmestajneJedinice.FindByID(Request["Identifikator"]);

            Rezervacija r = Rezervacije.ListaRezervacija.Find(x => (x.SmestajnaJedinica == sj.Identifikator
                && x.Turista == sj.KorisnickoIme));

            Korisnik k = Korisnici.FindByUsername(((Korisnik)Session["user"]).KorisnickoIme);
            k.Otkazao++;
            Data.AzurirajKorisnike();

            sj.Zauzeta = false;
            sj.KorisnickoIme = "";
            Data.AzurirajSmestajneJedinice();

            r.Status = StatusRezervacije.Otkazana;
            Data.AzurirajRezervacije();

            int brojRezSJuAranzmanu = 0;
            foreach (SmestajnaJedinica item in SmestajneJedinice.ListaSmestajnihJedinica)
            {
                Korisnik korisnik = Korisnici.FindByUsername(((Korisnik)Session["user"]).KorisnickoIme);
                if (korisnik.KorisnickoIme == item.KorisnickoIme
                    && SmestajneJedinice.FindSmestaj(sj.NazivSmestaja).NazivAranzmana
                        == SmestajneJedinice.FindSmestaj(item.NazivSmestaja).NazivAranzmana)
                {
                    brojRezSJuAranzmanu++;
                }
            }

            if (brojRezSJuAranzmanu == 0)   // ako nema nijedna rezervacija ovog korisnika u datom aranzmanu
            {
                Korisnik u = Korisnici.FindByUsername(((Korisnik)Session["user"]).KorisnickoIme);
                u.Aranzmani.Remove(Aranzmani.FindByNaziv((Smestaji.FindByNaziv(sj.NazivSmestaja)).NazivAranzmana));
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult OstaviKomentar()
        {
            SmestajnaJedinica jedinica = SmestajneJedinice.FindByID(Request["Identifikator"]);
            return View("Komentar", jedinica);
        }

        public ActionResult Komentar()
        {
            return View();
        }

        public ActionResult Komentar(SmestajnaJedinica jedinica)
        {
            return View(jedinica);
        }

        [HttpPost]
        public ActionResult Komentarisi(Komentar komentar)
        {
            SmestajnaJedinica sj = SmestajneJedinice.FindByID(Request["Identifikator"]);

            if (komentar.Tekst == null || komentar.Tekst == "")
            {
                ViewBag.Message = "Popunite prazna polja!";
                return View("Komentar");
            }

            if (komentar.Ocena != 1 && komentar.Ocena != 2 && komentar.Ocena != 3
                && komentar.Ocena != 4 && komentar.Ocena != 5)
            {
                ViewBag.Message = "Ocena mora biti neki broj od 1 do 5!";
                return View("Komentar");
            }

            komentar.Odobren = false;


            Data.SacuvajKomentar(komentar);

            return View("MojeRezervacije");
        }

        [HttpPost]
        public ActionResult SortiranjeRezervacija()
        {
            List<Rezervacija> rezervacije = new List<Rezervacija>();
            rezervacije = Rezervacije.ListaRezervacija;

            string vrsta, nacin;
            vrsta = Request["sortvrsta"] != null ? Request["sortvrsta"] : "";
            nacin = Request["sortnacin"] != null ? Request["sortnacin"] : "";

            if (vrsta != "dozvoljenom broju gostiju" && vrsta != "ceni")
            {
                ViewBag.Message = "Odaberite neku od ponudjenih vrsta sortiranja!";
                return View("MojeRezervacije");
            }

            if (nacin != "rastuce" && nacin != "opadajuce")
            {
                ViewBag.Message = "Odaberite neki od ponudjenih nacina sortiranja!";
                return View("MojeRezervacije");
            }

            List<Rezervacija> SortedList = new List<Rezervacija>();

            if (vrsta == "dozvoljenom broju gostiju")
            {
                

                SortedList = rezervacije.OrderBy(o => 
                    SmestajneJedinice.FindByID(o.SmestajnaJedinica.ToString()).DozvoljenBrojGostiju).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = rezervacije.OrderByDescending(o =>
                    SmestajneJedinice.FindByID(o.SmestajnaJedinica.ToString()).DozvoljenBrojGostiju).ToList();
                }
            }
            else    // po ceni
            {
                SortedList = rezervacije.OrderBy(o =>
                    SmestajneJedinice.FindByID(o.SmestajnaJedinica.ToString()).Cena).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = rezervacije.OrderByDescending(o =>
                    SmestajneJedinice.FindByID(o.SmestajnaJedinica.ToString()).Cena).ToList();
                }
            }

            Rezervacije.ListaRezervacija = SortedList;
            return View("MojeRezervacije");
        }

        [HttpPost]
        public ActionResult PretragaRezervacija()
        {
            List<Rezervacija> rezervacije = Rezervacije.ListaRezervacija.ToList();  // kopija
            List<Rezervacija> rezervacijeFilter = rezervacije.ToList();     // kopija

            string donjiBrojGostiju = Request["donjiBrojGostiju"] != null ? Request["donjiBrojGostiju"] : "";
            string gornjiBrojGostiju = Request["gornjiBrojGostiju"] != null ? Request["gornjiBrojGostiju"] : "";
            string ljubimci = Request["ljubimci"] != null ? Request["ljubimci"] : "";
            string cena = Request["cena"] != null ? Request["cena"] : "";

            ViewBag.TrazenaRezervacija = null;
            
            if (!Int32.TryParse(donjiBrojGostiju, out int result1) && donjiBrojGostiju != "")
            {
                ViewBag.Message = "Los format donje granice broja gostiju. Mora biti ceo broj!";
                return View("MojeRezervacije");
            }

            if (!Int32.TryParse(gornjiBrojGostiju, out int result2) && gornjiBrojGostiju != "")
            {
                ViewBag.Message = "Los format gornje granice broja gostiju. Mora biti ceo broj!";
                return View("MojeRezervacije");
            }

            if (ljubimci.ToLower() != "da" && ljubimci.ToLower() != "ne" && ljubimci != "")
            {
                ViewBag.Message = "Los format ljubimaca.";
                return View("MojeRezervacije");
            }

            if (cena != "")
            {
                if (!cena.Contains('-'))
                {
                    ViewBag.Message = "Los format cene.";
                    return View("MojeRezervacije");
                }

                string[] granice = cena.Split('-');

                if (granice.Count() != 2)
                {
                    ViewBag.Message = "Los format cene.";
                    return View("MojeRezervacije");
                }
                if (!float.TryParse(granice[0], out float min) || !float.TryParse(granice[1], out float max))
                {
                    ViewBag.Message = "Los format cene.";
                    return View("MojeRezervacije");
                }

                foreach (Rezervacija r in rezervacije.ToList())
                {
                    SmestajnaJedinica sj = SmestajneJedinice.FindByID(r.SmestajnaJedinica.ToString());
                    if ((sj.Cena > max || sj.Cena < min) && rezervacijeFilter.Contains(r))
                        rezervacijeFilter.Remove(r);
                }
            }

            foreach (Rezervacija r in rezervacije.ToList())
            {
                if (((Korisnik)Session["user"]).KorisnickoIme != r.Turista)
                    rezervacijeFilter.Remove(r);
            }

            if (donjiBrojGostiju != "")
            {
                foreach (Rezervacija r in rezervacije.ToList())
                {
                    SmestajnaJedinica sj = SmestajneJedinice.FindByID(r.SmestajnaJedinica.ToString());
                    if (sj.DozvoljenBrojGostiju < result1 && rezervacijeFilter.Contains(r))
                        rezervacijeFilter.Remove(r);
                }
            }

            if (gornjiBrojGostiju != "")
            {
                foreach (Rezervacija r in rezervacije.ToList())
                {
                    SmestajnaJedinica sj = SmestajneJedinice.FindByID(r.SmestajnaJedinica.ToString());
                    if (sj.DozvoljenBrojGostiju > result2 && rezervacijeFilter.Contains(r))
                        rezervacijeFilter.Remove(r);
                }
            }

            if (ljubimci != "")
            {
                bool temp;
                if (ljubimci.ToLower() == "da")
                {
                    temp = true;
                }
                else
                {
                    temp = false;
                }

                foreach (Rezervacija r in rezervacije.ToList())
                {
                    SmestajnaJedinica sj = SmestajneJedinice.FindByID(r.SmestajnaJedinica.ToString());
                    if (sj.Ljubimci != temp && rezervacijeFilter.Contains(r))
                        rezervacijeFilter.Remove(r);
                }
            }

            if (rezervacijeFilter.Count == 0)
                ViewBag.Message = "Nema rezervacija za prikazivanje.";
            else
                ViewBag.TrazenaRezervacija = rezervacijeFilter;

            return View("MojeRezervacije");
        }
    }
}