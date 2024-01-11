using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuristickaAgencija.Models;

namespace TuristickaAgencija.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProsliAranzmani()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OdredjeniAranzman()
        {
            Aranzman a = Aranzmani.FindByNaziv(Request["naziv"]);
            ViewBag.Aranzman = a;
            return View();
        }

        public ActionResult PretragaISortiranje()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sortiranje()
        {
            List<Aranzman> aranzmani = new List<Aranzman>();
            aranzmani = Aranzmani.ListaAranzmana;

            string vrsta, nacin;
            vrsta = Request["sortvrsta"] != null ? Request["sortvrsta"] : "";
            nacin = Request["sortnacin"] != null ? Request["sortnacin"] : "";

            if (vrsta != "nazivu" && vrsta != "polasku" && vrsta != "povratku")
            {
                ViewBag.Message = "Odaberite neku od ponudjenih vrsta sortiranja!";
                return View("PretragaISortiranje");
            }

            if (nacin != "rastuce" && nacin != "opadajuce")
            {
                ViewBag.Message = "Odaberite neki od ponudjenih nacina sortiranja!";
                return View("PretragaISortiranje");
            }

            List<Aranzman> SortedList = new List<Aranzman>();

            if (vrsta == "nazivu")
            {
                SortedList = aranzmani.OrderBy(o => o.Naziv).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = aranzmani.OrderByDescending(o => o.Naziv).ToList();
                }
            }
            else if (vrsta == "polasku")
            {
                SortedList = aranzmani.OrderBy(o => o.DatumPocetkaPutovanja).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = aranzmani.OrderByDescending(o => o.DatumPocetkaPutovanja).ToList();
                }
            }
            else    // vrsta = "povratku"
            {
                SortedList = aranzmani.OrderBy(o => o.DatumZavrsetkaPutovanja).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = aranzmani.OrderByDescending(o => o.DatumZavrsetkaPutovanja).ToList();
                }
            }

            Aranzmani.ListaAranzmana = SortedList;
            return View("PretragaISortiranje");
        }

        [HttpPost]
        public ActionResult Pretraga()
        {
            List<Aranzman> aranzmani = Aranzmani.Copy(Aranzmani.ListaAranzmana);
            List<Aranzman> aranzmanFilter = Aranzmani.Copy(aranzmani);

            foreach (Aranzman a in aranzmani.ToList())
            {
                if (a.Obrisan && aranzmanFilter.Contains(a))
                    aranzmanFilter.Remove(a);
            }

            string donjiDatumPocetak = Request["donjiDatumPocetak"] != null ? Request["donjiDatumPocetak"] : "";
            string gornjiDatumPocetak = Request["gornjiDatumPocetak"] != null ? Request["gornjiDatumPocetak"] : "";
            string donjiDatumZavrsetak = Request["donjiDatumZavrsetak"] != null ? Request["donjiDatumZavrsetak"] : "";
            string gornjiDatumZavrsetak = Request["gornjiDatumZavrsetak"] != null ? Request["gornjiDatumZavrsetak"] : "";
            string prevoz = Request["prevoz"] != null ? Request["prevoz"] : "";
            string aranzman = Request["aranzman"] != null ? Request["aranzman"] : "";
            string naziv = Request["naziv"] != null ? Request["naziv"] : "";

            ViewBag.TrazeniAranzman = null;
            
            if (prevoz != TipPrevoza.Autobus.ToString() && prevoz != TipPrevoza.AutobusIAvion.ToString()
                && prevoz != TipPrevoza.Avion.ToString() && prevoz != TipPrevoza.Individualan.ToString()
                && prevoz != TipPrevoza.Ostalo.ToString() && prevoz != "")
            {
                ViewBag.Message = "Los format tipa prevoza. Odaberite neku od ponudjenih vrsta prevoza!";
                return View("PretragaISortiranje");
            }

            if (aranzman != TipAranzmana.AllInclusive.ToString() && aranzman != TipAranzmana.NajamApartmana.ToString()
                && aranzman != TipAranzmana.NocenjeSaDoruckom.ToString() && aranzman != TipAranzmana.Polupansion.ToString()
                && aranzman != TipAranzmana.PunPansion.ToString() && aranzman != "")
            {
                ViewBag.Message = "Los format tipa aranzmana. Odaberite neku od ponudjenih vrsta aranzmana!";
                return View("PretragaISortiranje");
            }

            if (donjiDatumPocetak != "")
            {
                DateTime datum = DateTime.Parse(donjiDatumPocetak);

                foreach (Aranzman a in aranzmani.ToList())
                {
                    if (a.DatumPocetkaPutovanja < datum && aranzmanFilter.Contains(a))
                        aranzmanFilter.Remove(a);
                }
            }

            if (gornjiDatumPocetak != "")
            {
                DateTime datum = DateTime.Parse(gornjiDatumPocetak);

                foreach (Aranzman a in aranzmani.ToList())
                {
                    if (a.DatumPocetkaPutovanja > datum && aranzmanFilter.Contains(a))
                        aranzmanFilter.Remove(a);
                }
            }

            if (donjiDatumZavrsetak != "")
            {
                DateTime datum = DateTime.Parse(donjiDatumZavrsetak);

                foreach (Aranzman a in aranzmani.ToList())
                {
                    if (a.DatumZavrsetkaPutovanja < datum && aranzmanFilter.Contains(a))
                        aranzmanFilter.Remove(a);
                }
            }

            if (gornjiDatumZavrsetak != "")
            {
                DateTime datum = DateTime.Parse(gornjiDatumZavrsetak);

                foreach (Aranzman a in aranzmani.ToList())
                {
                    if (a.DatumZavrsetkaPutovanja > datum && aranzmanFilter.Contains(a))
                        aranzmanFilter.Remove(a);
                }
            }

            if (prevoz != "")
            {
                foreach (Aranzman a in aranzmani.ToList())
                {
                    if (!a.Prevoz.ToString().Equals(prevoz) && aranzmanFilter.Contains(a))
                    {
                        aranzmanFilter.Remove(a);
                    }
                }
            }

            if (aranzman != "")
            {
                foreach (Aranzman a in aranzmani.ToList())
                {
                    if (!a.Tip.ToString().Equals(aranzman) && aranzmanFilter.Contains(a))
                    {
                        aranzmanFilter.Remove(a);
                    }
                }
            }

            if (naziv != "")
            {
                foreach (Aranzman a in aranzmani.ToList())
                {
                    if (!a.Naziv.Contains(naziv) && aranzmanFilter.Contains(a))
                    {
                        aranzmanFilter.Remove(a);
                    }
                }
            }
            
            if (aranzmanFilter.Count == 0)
                ViewBag.Message = "Nema aranzmana za prikazivanje.";
            else
                ViewBag.TrazeniAranzman = aranzmanFilter;

            return View("PretragaISortiranje");
        }

        public ActionResult Komentari()
        {
            return View();
        }

        public ActionResult Smestaji()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SortiranjeSmestaja()
        {
            List<Smestaj> smestaji = new List<Smestaj>();
            smestaji = Models.Smestaji.ListaSmestaja;

            string vrsta, nacin;
            vrsta = Request["sortvrsta"] != null ? Request["sortvrsta"] : "";
            nacin = Request["sortnacin"] != null ? Request["sortnacin"] : "";

            if (vrsta != "nazivu" && vrsta != "ukupnom broju smestajnih jedinica" 
                && vrsta != "broju slobodnih smestajnih jedinica")
            {
                ViewBag.Message = "Odaberite neku od ponudjenih vrsta sortiranja!";
                return View("Smestaji");
            }

            if (nacin != "rastuce" && nacin != "opadajuce")
            {
                ViewBag.Message = "Odaberite neki od ponudjenih nacina sortiranja!";
                return View("Smestaji");
            }

            List<Smestaj> SortedList = new List<Smestaj>();

            if (vrsta == "nazivu")
            {
                SortedList = smestaji.OrderBy(o => o.Naziv).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = smestaji.OrderByDescending(o => o.Naziv).ToList();
                }
            }
            else if (vrsta == "ukupnom broju smestajnih jedinica")
            {
                SortedList = smestaji.OrderBy(o => o.Jedinice.Count).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = smestaji.OrderByDescending(o => o.Jedinice.Count).ToList();
                }
            }
            else    // vrsta = "broju slobodnih smestajnih jedinica"
            {
                SortedList = smestaji.OrderBy(o => Models.Smestaji.BrojSlobodnihJedinica(o)).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = smestaji.OrderByDescending(o => Models.Smestaji.BrojSlobodnihJedinica(o)).ToList();
                }
            }

            Models.Smestaji.ListaSmestaja = SortedList;
            return View("Smestaji");
        }

        [HttpPost]
        public ActionResult PretragaSmestaja()
        {
            List<Smestaj> smestaji = Models.Smestaji.Copy(Models.Smestaji.ListaSmestaja);
            List<Smestaj> smestajFilter = Models.Smestaji.Copy(smestaji);

            string naziv = Request["naziv"] != null ? Request["naziv"] : "";
            string tip = Request["tip"] != null ? Request["tip"] : "";
            string bazen = Request["bazen"] != null ? Request["bazen"] : "";
            string spaCentar = Request["spaCentar"] != null ? Request["spaCentar"] : "";
            string prilagodjenoZaInvalide = Request["prilagodjenoZaInvalide"] != null ? Request["prilagodjenoZaInvalide"] : "";
            string wifi = Request["wifi"] != null ? Request["wifi"] : "";
            
            ViewBag.TrazeniSmestaj = null;

            if (tip != TipSmestaja.Hotel.ToString() && tip != TipSmestaja.Motel.ToString()
                && tip != TipSmestaja.Vila.ToString() && tip != "")
            {
                ViewBag.Message = "Los format tipa smestaja. Odaberite neku od ponudjenih vrsta smestaja!";
                return View("Smestaji");
            }

            if (bazen.ToLower() != "da" && bazen.ToLower() != "ne" && bazen != "")
            {
                ViewBag.Message = "Los format bazena. (da/ne)";
                return View("Smestaji");
            }

            if (spaCentar.ToLower() != "da" && spaCentar.ToLower() != "ne" && spaCentar != "")
            {
                ViewBag.Message = "Los format spa centra. (da/ne)";
                return View("Smestaji");
            }

            if (prilagodjenoZaInvalide.ToLower() != "da" && prilagodjenoZaInvalide.ToLower() != "ne" 
                && prilagodjenoZaInvalide != "")
            {
                ViewBag.Message = "Los format prilagodjenosti za invalide. (da/ne)";
                return View("Smestaji");
            }

            if (wifi.ToLower() != "da" && wifi.ToLower() != "ne" && wifi != "")
            {
                ViewBag.Message = "Los format wifi-a. (da/ne)";
                return View("Smestaji");
            }

            if (naziv != "")
            {
                foreach (Smestaj s in smestaji)
                {
                    if (!s.Naziv.Contains(naziv) && smestajFilter.Contains(s))
                        smestajFilter.Remove(s);
                }
            }

            if (tip != "")
            {
                foreach (Smestaj s in smestaji)
                {
                    if (!s.Tip.ToString().Equals(tip) && smestajFilter.Contains(s))
                        smestajFilter.Remove(s);
                }
            }

            if (bazen != "")
            {
                bool postoji;
                if (bazen.ToLower() == "da")
                    postoji = true;
                else
                    postoji = false;

                foreach (Smestaj s in smestaji)
                {
                    if (!s.Bazen.Equals(postoji) && smestajFilter.Contains(s))
                        smestajFilter.Remove(s);
                }
            }

            if (spaCentar != "")
            {
                bool postoji;
                if (spaCentar.ToLower() == "da")
                    postoji = true;
                else
                    postoji = false;

                foreach (Smestaj s in smestaji)
                {
                    if (!s.SpaCentar.Equals(postoji) && smestajFilter.Contains(s))
                        smestajFilter.Remove(s);
                }
            }

            if (prilagodjenoZaInvalide != "")
            {
                bool postoji;
                if (prilagodjenoZaInvalide.ToLower() == "da")
                    postoji = true;
                else
                    postoji = false;

                foreach (Smestaj s in smestaji)
                {
                    if (!s.PrilagodjenoZaInvalide.Equals(postoji) && smestajFilter.Contains(s))
                        smestajFilter.Remove(s);
                }
            }

            if (wifi != "")
            {
                bool postoji;
                if (wifi.ToLower() == "da")
                    postoji = true;
                else
                    postoji = false;

                foreach (Smestaj s in smestaji)
                {
                    if (!s.Wifi.Equals(postoji) && smestajFilter.Contains(s))
                        smestajFilter.Remove(s);
                }
            }
            
                if (smestajFilter.Count == 0)
                    ViewBag.Message = "Nema smestaja za prikazivanje.";
                else
                    ViewBag.TrazeniSmestaj = smestajFilter;
            
            return View("Smestaji");
        }

        public ActionResult Registracija()
        {
            Korisnik k = new Korisnik();
            return View(k);
        }

        [HttpPost]
        public ActionResult Registracija(Korisnik turista)
        {
            if (turista.KorisnickoIme == null || turista.Lozinka == null || turista.Ime == null || turista.Prezime == null
                || turista.Email == null || turista.DatumRodjenja == null)
            {
                ViewBag.Message = "Sva polja su obavezna!";
                return View();
            }
            

            List<Korisnik> users = Korisnici.ListaKorisnika;
            foreach (Korisnik k in users)
            {
                if (k.KorisnickoIme == turista.KorisnickoIme)
                {
                    ViewBag.Message = $"Korisnik {turista.KorisnickoIme} vec postoji!";
                    return View();
                }
            }

            if (Request["Pol"] != "Muski" && Request["Pol"] != "Zenski")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za pol!";
                return View();
            }

            turista.Uloga = UlogaKorisnika.Turista;
            turista.Otkazao = 0;
            turista.Blokiran = false;
            users.Add(turista);
            Korisnici.ListaKorisnika = users;
            Data.SacuvajKorisnika(turista);

            return View("Login");
        }

        public ActionResult Login()
        {
            Korisnik k = (Korisnik)Session["user"];
            return View(k);
        }

        [HttpPost]
        public ActionResult Login(Korisnik korisnik)
        {
            if (korisnik == null || korisnik.KorisnickoIme.Equals(""))
                return View();

            //ViewBag.User = korisnik;
            List<Korisnik> users = Korisnici.ListaKorisnika;
            Korisnik user = users.Find(u => u.KorisnickoIme.Equals(korisnik.KorisnickoIme)
                && u.Lozinka.Equals(korisnik.Lozinka));

            if (user == null)
            {
                ViewBag.Message = "Podaci su lose uneti!";
                return View();
            }

            if (user.Blokiran)
            {
                ViewBag.Message = "Uneti korisnik je blokiran!";
                return View();
            }

            Session["user"] = user;

            return View("Index");
        }

        public ActionResult Profil()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IzmeniProfil(Korisnik korisnik)
        {
            if (korisnik.Lozinka == null || Request["Lozinka2"] == null || korisnik.Ime == null || korisnik.Prezime == null
                || korisnik.Email == null || korisnik.DatumRodjenja == null)
            {
                ViewBag.Message = "Sva polja su obavezna!";
                return View("Profil");
            }

            if (korisnik.DatumRodjenja.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
            {
                ViewBag.Message = "Popunite polje za datum!";
                return View("Profil");
            }

            Korisnik k = Korisnici.FindByUsername(korisnik.KorisnickoIme);

            if (Request["Pol"] != "Muski" && Request["Pol"] != "Zenski")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za pol!";
                return View("Profil");
            }

            if (korisnik.Lozinka != Request["Lozinka2"])
            {
                ViewBag.Message = "Lozinke se ne poklapaju!";
                return View("Profil");
            }
            
            Data.IzmeniKorisnika(korisnik);

            return View("Profil");
        }
    }
}