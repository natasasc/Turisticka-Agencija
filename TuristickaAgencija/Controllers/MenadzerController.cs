using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuristickaAgencija.Models;

namespace TuristickaAgencija.Controllers
{
    public class MenadzerController : Controller
    {
        // GET: Menadzer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KreiranjeAranzmana()
        {
            return View();
        }

        public ActionResult KreiranjeSmestaja()
        {
            return View();
        }

        public ActionResult KreiranjeSmestajneJedinice()
        {
            return View();
        }

        public ActionResult KreirajAranzman(Aranzman aranzman, MestoNalazenja mn)
        {
            if (aranzman.Naziv == null || aranzman.Lokacija == null || aranzman.DatumPocetkaPutovanja == null 
                || aranzman.DatumZavrsetkaPutovanja == null || aranzman.OpisAranzmana == null 
                || aranzman.ProgramPutovanja == null || aranzman.PosterAranzmana == null
                || aranzman.VremeNalazenja == null || mn.Adresa == null)
            {
                ViewBag.Message = "Sva polja su obavezna!";
                return View("KreiranjeAranzmana");
            }
            
            List<Aranzman> aranzmani = Aranzmani.ListaAranzmana;
            foreach (Aranzman a in aranzmani)
            {
                if (a.Naziv == aranzman.Naziv)
                {
                    ViewBag.Message = $"Aranzman pod nazivom {a.Naziv} vec postoji!";
                    return View("KreiranjeAranzmana");
                }
            }

            if (Request["Tip"] != "NocenjeSaDoruckom" && Request["Tip"] != "Polupansion"
                && Request["Tip"] != "PunPansion" && Request["Tip"] != "AllInclusive"
                && Request["Tip"] != "NajamApartmana")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za tip aranzmana!";
                return View("KreiranjeAranzmana");
            }

            if (Request["Prevoz"] != "Autobus" && Request["Prevoz"] != "Avion"
                && Request["Prevoz"] != "AutobusIAvion" && Request["Prevoz"] != "Individualan"
                && Request["Prevoz"] != "Ostalo")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za tip prevoza!";
                return View("KreiranjeAranzmana");
            }

            if (!Int32.TryParse(Request["MaxBrojPutnika"], out int broj))
            {
                ViewBag.Message = "Pogresan format maksimalnog broja putnika! Mora biti ceo broj.";
                return View("KreiranjeAranzmana");
            }

            if (aranzman.PosterAranzmana != "rovinj.jpeg" && aranzman.PosterAranzmana != "skiathos.jpg"
                    && aranzman.PosterAranzmana != "miami.jpg" && aranzman.PosterAranzmana != "znojmo.jpg")
            {
                ViewBag.Message = "Odaberite neki od ponudjenih postera aranzmana!";
                return View("KreiranjeAranzmana");
            }

            if (!float.TryParse(Request["GeografskaDuzina"], out float duzina))
            {
                ViewBag.Message = "Pogresan format geografske duzine! Mora biti realan broj.";
                return View("KreiranjeAranzmana");
            }

            if (!float.TryParse(Request["GeografskaSirina"], out float sirina))
            {
                ViewBag.Message = "Pogresan format geografske sirine! Mora biti realan broj.";
                return View("KreiranjeAranzmana");
            }

            if (Request["GeografskaDuzina"].Contains('.') || Request["GeografskaSirina"].Contains('.'))
            {
                ViewBag.Message = "Geografski polozaj se pise sa zarezom!";
                return View("KreiranjeAranzmana");
            }

            if (!aranzman.VremeNalazenja.Contains(':'))
            {
                ViewBag.Message = "Pogresan format vremena nalazenja! (HH:mm)";
                return View("KreiranjeAranzmana");
            }
            else
            {
                string[] niz = aranzman.VremeNalazenja.Split(':');

                if (niz.Count() != 2)
                {
                    ViewBag.Message = "Pogresan format vremena nalazenja! (HH:mm)";
                    return View("KreiranjeAranzmana");
                }
                else
                {
                    if (!Int32.TryParse(niz[0], out int sat))
                    {
                        ViewBag.Message = "Pogresan format vremena nalazenja! (HH:mm)";
                        return View("KreiranjeAranzmana");
                    }
                    else if (!Int32.TryParse(niz[1], out int minut))
                    {
                        ViewBag.Message = "Pogresan format vremena nalazenja! (HH:mm)";
                        return View("KreiranjeAranzmana");
                    }
                    else
                    {
                        if (sat < 0 || sat > 23 || minut < 0 || minut > 59)
                        {
                            ViewBag.Message = "Sati moraju biti 0-23, a minute 0-59!";
                            return View("KreiranjeAranzmana");
                        }
                    }
                }
            }

            mn.Obrisano = false;
            mn.NazivAranzmana = aranzman.Naziv;
            mn.GeografskaDuzina = duzina;
            mn.GeografskaSirina = sirina;
            MestaNalazenja.ListaMestaNalazenja.Add(mn);
            Data.AzurirajMestaNalazenja();

            aranzman.MestoNalazenja = mn;
            aranzman.Kreirao = ((Korisnik)Session["user"]).KorisnickoIme;
            aranzman.Obrisan = false;

            aranzmani.Add(aranzman);
            Aranzmani.ListaAranzmana = aranzmani;
            Data.AzurirajAranzmane();

            return View("PregledAranzmana");
        }

        public ActionResult KreirajSmestaj(Smestaj smestaj)
        {
            if (Request["Naziv"] == null || Request["Tip"] == null
                || Request["Bazen"] == null || Request["SpaCentar"] == null
                || Request["PrilagodjenoZaInvalide"] == null || Request["Wifi"] == null
                || Request["NazivAranzmana"] == null 
                || Request["Naziv"] == "" || Request["Tip"] == ""
                || Request["Bazen"] == "" || Request["SpaCentar"] == ""
                || Request["PrilagodjenoZaInvalide"] == "" || Request["Wifi"] == ""
                || Request["NazivAranzmana"] == "")
            {
                ViewBag.Message = "Sva polja su obavezna!";
                return View("KreiranjeSmestaja");
            }
            if (Request["Tip"] == "Hotel" && (Request["BrojZvezdica"] == "" || Request["BrojZvezdica"] == null))
            {
                ViewBag.Message = "Sva polja su obavezna!";
                return View("KreiranjeSmestaja");
            }
            else
            {
                smestaj.BrojZvezdica = 0;
            }

            List<Smestaj> smestaji = Smestaji.ListaSmestaja;
            foreach (Smestaj s in smestaji)
            {
                if (s.Naziv == smestaj.Naziv)
                {
                    ViewBag.Message = $"Smestaj pod nazivom {s.Naziv} vec postoji!";
                    return View("KreiranjeSmestaja");
                }
            }

            if (Request["Tip"] != "Hotel" && Request["Tip"] != "Motel" && Request["Tip"] != "Vila")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za tip smestaja!";
                return View("KreiranjeSmestaja");
            }

            smestaj.Tip = (TipSmestaja)Enum.Parse(typeof(TipSmestaja), Request["Tip"]);
            
            if (Request["BrojZvezdica"] != "1" && Request["BrojZvezdica"] != "2" && Request["BrojZvezdica"] != "3"
                    && Request["BrojZvezdica"] != "4" && Request["BrojZvezdica"] != "5" && Request["Tip"] == "Hotel")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za broj zvezdica!";
                return View("KreiranjeSmestaja");
            }

            if (Request["Tip"] == "Hotel")
                smestaj.BrojZvezdica = Int32.Parse(Request["BrojZvezdica"]);

            if (Request["Bazen"].ToLower() != "da" && Request["Bazen"].ToLower() != "ne")
            {
                ViewBag.Message = "Pogresan format bazena! (da/ne)";
                return View("KreiranjeSmestaja");
            }

            if (Request["Bazen"].ToLower() == "da")
            {
                smestaj.Bazen = true;
            }
            else
            {
                smestaj.Bazen = false;
            }

            if (Request["SpaCentar"].ToLower() != "da" && Request["SpaCentar"].ToLower() != "ne")
            {
                ViewBag.Message = "Pogresan format spa centra! (da/ne)";
                return View("KreiranjeSmestaja");
            }

            if (Request["SpaCentar"].ToLower() == "da")
            {
                smestaj.SpaCentar = true;
            }
            else
            {
                smestaj.SpaCentar = false;
            }

            if (Request["PrilagodjenoZaInvalide"].ToLower() != "da" && Request["PrilagodjenoZaInvalide"].ToLower() != "ne")
            {
                ViewBag.Message = "Pogresan format prilagodjenosti za invalide! (da/ne)";
                return View("KreiranjeSmestaja");
            }

            if (Request["PrilagodjenoZaInvalide"].ToLower() == "da")
            {
                smestaj.PrilagodjenoZaInvalide = true;
            }
            else
            {
                smestaj.PrilagodjenoZaInvalide = false;
            }

            if (Request["Wifi"].ToLower() != "da" && Request["Wifi"].ToLower() != "ne")
            {
                ViewBag.Message = "Pogresan format wifi-a! (da/ne)";
                return View("KreiranjeSmestaja");
            }

            if (Request["Wifi"].ToLower() == "da")
            {
                smestaj.Wifi = true;
            }
            else
            {
                smestaj.Wifi = false;
            }

            smestaj.Obrisan = false;

            Aranzman a = Aranzmani.FindByNaziv(smestaj.NazivAranzmana);
            if (a == null)
            {
                ViewBag.Message = $"Aranzman pod nazivom {smestaj.NazivAranzmana} ne postoji!";
                return View("KreiranjeSmestaja");
            }
            else
            {
                a.Smestaj = smestaj;
            }
            
            smestaji.Add(smestaj);
            Smestaji.ListaSmestaja = smestaji;
            Data.AzurirajSmestaje();
            
            return View("PregledSmestaja");
        }

        public ActionResult KreirajSmestajnuJedinicu(SmestajnaJedinica sj)
        {
            if (Request["DozvoljenBrojGostiju"] == null || Request["Ljubimci"] == null
                || Request["Cena"] == null || Request["NazivSmestaja"] == null
                || Request["DozvoljenBrojGostiju"] == "" || Request["Ljubimci"] == ""
                || Request["Cena"] == "" || Request["NazivSmestaja"] == "")
            {
                ViewBag.Message = "Sva polja su obavezna!";
                return View("KreiranjeSmestajneJedinice");
            }
            
            if (!Int32.TryParse(Request["DozvoljenBrojGostiju"], out int broj))
            {
                ViewBag.Message = "Los format dozvoljenog broja gostiju! Mora biti ceo broj.";
                return View("KreiranjeSmestajneJedinice");
            }

            if (Request["Ljubimci"].ToLower() != "da" && Request["Ljubimci"].ToLower() != "ne")
            {
                ViewBag.Message = "Los format ljubimaca!";
                return View("KreiranjeSmestajneJedinice");
            }

            if (!float.TryParse(Request["Cena"], out float cena))
            {
                ViewBag.Message = "Los format cene! Mora biti realan broj.";
                return View("KreiranjeSmestajneJedinice");
            }

            Smestaj s = Smestaji.FindByNaziv(sj.NazivSmestaja);

            if (s == null)
            {
                ViewBag.Message = $"Smestaj sa nazivom {sj.NazivSmestaja} ne postoji!";
                return View("KreiranjeSmestajneJedinice");
            }
            
            List<SmestajnaJedinica> jedinice = SmestajneJedinice.ListaSmestajnihJedinica;

            sj.Zauzeta = false;
            sj.KorisnickoIme = "";
            sj.Identifikator = sj.GetID();
            sj.Obrisana = false;

            sj.DozvoljenBrojGostiju = broj;
            if (Request["Ljubimci"].ToLower() == "da")
            {
                sj.Ljubimci = true;
            }
            else
            {
                sj.Ljubimci = false;
            }
            sj.Cena = cena;
            sj.NazivSmestaja = Request["NazivSmestaja"];

            jedinice.Add(sj);
            SmestajneJedinice.ListaSmestajnihJedinica = jedinice;
            Data.AzurirajSmestajneJedinice();

            Smestaji.FindByNaziv(sj.NazivSmestaja).Jedinice.Add(sj);

            return View("PregledSmestajnihJedinica");
        }

        public ActionResult PregledAranzmana()
        {
            return View();
        }

        public ActionResult PregledSmestaja()
        {
            return View();
        }

        public ActionResult PregledSmestajnihJedinica()
        {
            return View();
        }

        public ActionResult ModifikujAranzman()
        {
            Aranzman a = Aranzmani.FindByNaziv(Request["naziv"]);
            return View("ModifikovanjeAranzmana", a);
        }

        public ActionResult ModifikujSmestaj()
        {
            Smestaj s = Smestaji.FindByNaziv(Request["naziv"]);
            return View("ModifikovanjeSmestaja", s);
        }

        public ActionResult ModifikujSmestajnuJedinicu()
        {
            SmestajnaJedinica sj = SmestajneJedinice.FindByID(Request["Identifikator"]);
            return View("ModifikovanjeSmestajneJedinice", sj);
        }

        public ActionResult ModifikovanjeAranzmana()
        {
            return View();
        }

        public ActionResult ModifikovanjeSmestaja()
        {
            return View();
        }

        public ActionResult ModifikovanjeSmestajneJedinice()
        {
            return View();
        }

        public ActionResult ModifikovanjeAranzmana(Aranzman a)
        {
            return View(a);
        }

        public ActionResult ModifikovanjeSmestaja(Smestaj s)
        {
            return View(s);
        }

        public ActionResult ModifikovanjeSmestajneJedinice(Smestaj sj)
        {
            return View(sj);
        }

        public ActionResult Modifikuj(Aranzman aranzman, MestoNalazenja mn)
        {
            Aranzman a = Aranzmani.FindByNaziv(aranzman.Naziv);

            if (aranzman.Lokacija == null || aranzman.DatumPocetkaPutovanja == null
                || aranzman.DatumZavrsetkaPutovanja == null || aranzman.OpisAranzmana == null
                || aranzman.ProgramPutovanja == null || aranzman.PosterAranzmana == null
                || aranzman.VremeNalazenja == null || mn.Adresa == null)
            {
                ViewBag.Message = "Sva polja su obavezna!";
                return View("ModifikovanjeAranzmana", a);
            }
            
            if (Request["Tip"] != "NocenjeSaDoruckom" && Request["Tip"] != "Polupansion"
                && Request["Tip"] != "PunPansion" && Request["Tip"] != "AllInclusive"
                && Request["Tip"] != "NajamApartmana")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za tip aranzmana!";
                return View("ModifikovanjeAranzmana", a);
            }

            if (Request["Prevoz"] != "Autobus" && Request["Prevoz"] != "Avion"
                && Request["Prevoz"] != "AutobusIAvion" && Request["Prevoz"] != "Individualan"
                && Request["Prevoz"] != "Ostalo")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za tip prevoza!";
                return View("ModifikovanjeAranzmana", a);
            }

            if (!Int32.TryParse(Request["MaxBrojPutnika"], out int broj))
            {
                ViewBag.Message = "Pogresan format maksimalnog broja putnika! Mora biti ceo broj.";
                return View("ModifikovanjeAranzmana", a);
            }

            if (aranzman.PosterAranzmana != "rovinj.jpeg" && aranzman.PosterAranzmana != "skiathos.jpg")
            {
                ViewBag.Message = "Odaberite neki od ponudjenih postera aranzmana!";
                return View("ModifikovanjeAranzmana", a);
            }

            if (!float.TryParse(Request["GeografskaDuzina"], out float duzina))
            {
                ViewBag.Message = "Pogresan format geografske duzine! Mora biti realan broj.";
                return View("ModifikovanjeAranzmana", a);
            }

            if (!float.TryParse(Request["GeografskaSirina"], out float sirina))
            {
                ViewBag.Message = "Pogresan format geografske sirine! Mora biti realan broj.";
                return View("ModifikovanjeAranzmana", a);
            }

            if (!aranzman.VremeNalazenja.Contains(':'))
            {
                ViewBag.Message = "Pogresan format vremena nalazenja! (HH:mm)";
                return View("ModifikovanjeAranzmana", a);
            }
            else
            {
                string[] niz = aranzman.VremeNalazenja.Split(':');

                if (niz.Count() != 2)
                {
                    ViewBag.Message = "Pogresan format vremena nalazenja! (HH:mm)";
                    return View("ModifikovanjeAranzmana", a);
                }
                else
                {
                    if (!Int32.TryParse(niz[0], out int sat))
                    {
                        ViewBag.Message = "Pogresan format vremena nalazenja! (HH:mm)";
                        return View("ModifikovanjeAranzmana", a);
                    }
                    else if (!Int32.TryParse(niz[1], out int minut))
                    {
                        ViewBag.Message = "Pogresan format vremena nalazenja! (HH:mm)";
                        return View("ModifikovanjeAranzmana", a);
                    }
                    else
                    {
                        if (sat < 0 || sat > 23 || minut < 0 || minut > 59)
                        {
                            ViewBag.Message = "Sati moraju biti 0-23, a minute 0-59!";
                            return View("ModifikovanjeAranzmana", a);
                        }
                    }
                }
            }
            
            MestoNalazenja staro = MestaNalazenja.FindByAranzman(aranzman.Naziv);
            mn.NazivAranzmana = aranzman.Naziv;
            staro.Adresa = mn.Adresa;
            staro.GeografskaDuzina = mn.GeografskaDuzina;
            staro.GeografskaSirina = mn.GeografskaSirina;
            Data.AzurirajMestaNalazenja();
;
            Aranzman stari = Aranzmani.FindByNaziv(aranzman.Naziv);
            stari.Tip = aranzman.Tip;
            stari.Prevoz = aranzman.Prevoz;
            stari.Lokacija = aranzman.Lokacija;
            stari.DatumPocetkaPutovanja = aranzman.DatumPocetkaPutovanja;
            stari.DatumZavrsetkaPutovanja = aranzman.DatumZavrsetkaPutovanja;
            stari.MestoNalazenja = staro;
            stari.VremeNalazenja = aranzman.VremeNalazenja;
            stari.MaxBrojPutnika = aranzman.MaxBrojPutnika;
            stari.OpisAranzmana = aranzman.OpisAranzmana;
            stari.ProgramPutovanja = aranzman.ProgramPutovanja;
            stari.PosterAranzmana = aranzman.PosterAranzmana;

            Data.AzurirajAranzmane();

            return View("PregledAranzmana");
        }

        public ActionResult ModifikujDatiSmestaj(Smestaj smestaj)
        {
            Smestaj s = Smestaji.FindByNaziv(smestaj.Naziv);

            if ( Request["Tip"] == null || Request["Bazen"] == null || Request["SpaCentar"] == null
                || Request["PrilagodjenoZaInvalide"] == null || Request["Wifi"] == null || Request["Tip"] == ""
                || Request["Bazen"] == "" || Request["SpaCentar"] == ""
                || Request["PrilagodjenoZaInvalide"] == "" || Request["Wifi"] == "")
            {
                ViewBag.Message = "Sva polja su obavezna!";
                return View("ModifikovanjeSmestaja", s);
            }
            if (Request["Tip"] == "Hotel" && (Request["BrojZvezdica"] == "" || Request["BrojZvezdica"] == null))
            {
                ViewBag.Message = "Sva polja su obavezna!";
                return View("ModifikovanjeSmestaja", s);
            }

            if (Request["Tip"] != "Hotel" && Request["Tip"] != "Motel" && Request["Tip"] != "Vila")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za tip smestaja!";
                return View("ModifikovanjeSmestaja", s);
            }
            
            if (Request["BrojZvezdica"] != "1" && Request["BrojZvezdica"] != "2" && Request["BrojZvezdica"] != "3"
                    && Request["BrojZvezdica"] != "4" && Request["BrojZvezdica"] != "5" && Request["Tip"] == "Hotel")
            {
                ViewBag.Message = "Odaberite jednu od ponudjenih opcija za broj zvezdica!";
                return View("ModifikovanjeSmestaja", s);
            }

            if (Request["Bazen"].ToLower() != "da" && Request["Bazen"].ToLower() != "ne")
            {
                ViewBag.Message = "Pogresan format bazena! (da/ne)";
                return View("ModifikovanjeSmestaja", s);
            }

            if (Request["SpaCentar"].ToLower() != "da" && Request["SpaCentar"].ToLower() != "ne")
            {
                ViewBag.Message = "Pogresan format spa centra! (da/ne)";
                return View("ModifikovanjeSmestaja", s);
            }

            if (Request["PrilagodjenoZaInvalide"].ToLower() != "da" && Request["PrilagodjenoZaInvalide"].ToLower() != "ne")
            {
                ViewBag.Message = "Pogresan format prilagodjenosti za invalide! (da/ne)";
                return View("ModifikovanjeSmestaja", s);
            }

            if (Request["Wifi"].ToLower() != "da" && Request["Wifi"].ToLower() != "ne")
            {
                ViewBag.Message = "Pogresan format wifi-a! (da/ne)";
                return View("ModifikovanjeSmestaja", s);
            }

            smestaj.Tip = (TipSmestaja)Enum.Parse(typeof(TipSmestaja), Request["Tip"]);
            if (Request["Tip"] == "Hotel")
                smestaj.BrojZvezdica = Int32.Parse(Request["BrojZvezdica"]);
            else
                smestaj.BrojZvezdica = 0;

            if (Request["Bazen"].ToLower() == "da")
            {
                smestaj.Bazen = true;
            }
            else
            {
                smestaj.Bazen = false;
            }

            if (Request["SpaCentar"].ToLower() == "da")
            {
                smestaj.SpaCentar = true;
            }
            else
            {
                smestaj.SpaCentar = false;
            }

            if (Request["PrilagodjenoZaInvalide"].ToLower() == "da")
            {
                smestaj.PrilagodjenoZaInvalide = true;
            }
            else
            {
                smestaj.PrilagodjenoZaInvalide = false;
            }

            if (Request["Wifi"].ToLower() == "da")
            {
                smestaj.Wifi = true;
            }
            else
            {
                smestaj.Wifi = false;
            }

            Smestaj stari = Smestaji.FindByNaziv(smestaj.Naziv);
            stari.Tip = smestaj.Tip;
            stari.BrojZvezdica = smestaj.BrojZvezdica;
            stari.Bazen = smestaj.Bazen;
            stari.SpaCentar = smestaj.SpaCentar;
            stari.PrilagodjenoZaInvalide = smestaj.PrilagodjenoZaInvalide;
            stari.Wifi = smestaj.Wifi;
            Data.AzurirajSmestaje();
            
            return View("PregledSmestaja");
        }

        public ActionResult ModifikujDatuSmestajnuJedinicu(SmestajnaJedinica jedinica)
        {
            SmestajnaJedinica stara = SmestajneJedinice.FindByID(Request["Identifikator"]);

            if (Request["Ljubimci"] == null || Request["Cena"] == null 
                || Request["NazivSmestaja"] == null || Request["Ljubimci"] == ""
                || Request["Cena"] == "" || Request["NazivSmestaja"] == "")
            {
                ViewBag.Message = "Popunite prazna polja! (Dozvoljen broj gostiju se ne popunjava ako je smestajna jedinica rezervisana.)";
                return View("ModifikovanjeSmestajneJedinice", stara);
            }

            

            if (Request["Ljubimci"].ToLower() != "da" && Request["Ljubimci"].ToLower() != "ne")
            {
                ViewBag.Message = "Los format ljubimaca!";
                return View("ModifikovanjeSmestajneJedinice");
            }

            if (!float.TryParse(Request["Cena"], out float cena))
            {
                ViewBag.Message = "Los format cene! Mora biti realan broj.";
                return View("ModifikovanjeSmestajneJedinice", stara);
            }

            Smestaj s = Smestaji.FindByNaziv(jedinica.NazivSmestaja);

            if (s == null)
            {
                ViewBag.Message = $"Smestaj sa nazivom {jedinica.NazivSmestaja} ne postoji!";
                return View("KreiranjeSmestajneJedinice", stara);
            }

            foreach (Rezervacija r in Rezervacije.ListaRezervacija)
            {
                if (r.SmestajnaJedinica == jedinica.Identifikator)
                {
                    if (Aranzmani.FindByNaziv(Smestaji.FindByNaziv(jedinica.NazivSmestaja).NazivAranzmana)
                        .DatumPocetkaPutovanja > DateTime.Now)
                    {
                        if (Request["DozovljenBrojKreveta"] != null && Request["DozvoljenBrojKreveta"] != "")
                        {
                            ViewBag.Message = "Ne moze se menjati dozvoljen broj kreveta rezervisane smestajne jedinice!";
                            return View("ModifikovanjeSmestajneJedinice", stara);
                        }
                    }
                }
            }

            if (Request["DozvoljenBrojGostiju"] != "")
            {
                if (!Int32.TryParse(Request["DozvoljenBrojGostiju"], out int broj))
                {
                    ViewBag.Message = "Los format dozvoljenog broja gostiju! Mora biti ceo broj.";
                    return View("ModifikovanjeSmestajneJedinice", stara);
                }
                stara.DozvoljenBrojGostiju = broj;
            }

            if (Request["Ljubimci"].ToLower() == "da")
            {
                stara.Ljubimci = true;
            }
            else
            {
                stara.Ljubimci = false;
            }
            stara.Cena = cena;
            stara.NazivSmestaja = Request["NazivSmestaja"];
            
            Data.AzurirajSmestajneJedinice();

            return View("PregledSmestajnihJedinica");
        }

        public ActionResult ObrisiAranzman()
        {
            if (Aranzmani.FindByNaziv(Request["naziv"]).Smestaj != null)
            {
                bool promena = false;
                foreach (SmestajnaJedinica sj in SmestajneJedinice.ListaSmestajnihJedinica)
                {
                    if (sj.NazivSmestaja == Aranzmani.FindByNaziv(Request["naziv"]).Smestaj.Naziv)
                    {
                        sj.Obrisana = true;
                        promena = true;
                    }
                }
                if (promena)
                    Data.AzurirajSmestajneJedinice();

                Smestaj s = Smestaji.FindByNaziv(Aranzmani.FindByNaziv(Request["naziv"]).Smestaj.Naziv);
                s.Obrisan = true;
                Data.AzurirajSmestaje();
            }

            MestoNalazenja mn = MestaNalazenja.FindByAranzman(Request["naziv"]);
            mn.Obrisano = true;
            Data.AzurirajMestaNalazenja();

            Aranzman a = Aranzmani.FindByNaziv(Request["naziv"]);
            a.Obrisan = true;
            Data.AzurirajAranzmane();
            
            return View("PregledAranzmana");
        }

        public ActionResult ObrisiSmestaj()
        {
            if (Smestaji.FindByNaziv(Request["naziv"]).Jedinice != null)
            {
                bool promena = false;
                foreach (SmestajnaJedinica sj in SmestajneJedinice.ListaSmestajnihJedinica)
                {
                    if (sj.NazivSmestaja == Request["naziv"])
                    {
                        sj.Obrisana = true;
                        promena = true;
                    }
                }
                if (promena)
                    Data.AzurirajSmestajneJedinice();
            }

            Smestaj s = Smestaji.FindByNaziv(Request["naziv"]);
            s.Obrisan = true;
            Data.AzurirajSmestaje();

            return View("PregledSmestaja");
        }

        public ActionResult ObrisiSmestajnuJedinicu()
        {
            SmestajnaJedinica sj = SmestajneJedinice.FindByID(Request["Identifikator"]);
            sj.Obrisana = true;
            Data.AzurirajSmestajneJedinice();

            return View("PregledSmestajnihJedinica");
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
                return View("PregledAranzmana");
            }

            if (nacin != "rastuce" && nacin != "opadajuce")
            {
                ViewBag.Message = "Odaberite neki od ponudjenih nacina sortiranja!";
                return View("PregledAranzmana");
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
            return View("PregledAranzmana");
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
                return View("PregledAranzmana");
            }

            if (aranzman != TipAranzmana.AllInclusive.ToString() && aranzman != TipAranzmana.NajamApartmana.ToString()
                && aranzman != TipAranzmana.NocenjeSaDoruckom.ToString() && aranzman != TipAranzmana.Polupansion.ToString()
                && aranzman != TipAranzmana.PunPansion.ToString() && aranzman != "")
            {
                ViewBag.Message = "Los format tipa aranzmana. Odaberite neku od ponudjenih vrsta aranzmana!";
                return View("PregledAranzmana");
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

            return View("PregledAranzmana");
        }

        [HttpPost]
        public ActionResult SortiranjeSmestaja()
        {
            List<Smestaj> smestaji = new List<Smestaj>();
            smestaji = Smestaji.ListaSmestaja;

            string vrsta, nacin;
            vrsta = Request["sortvrsta"] != null ? Request["sortvrsta"] : "";
            nacin = Request["sortnacin"] != null ? Request["sortnacin"] : "";

            if (vrsta != "nazivu" && vrsta != "ukupnom broju smestajnih jedinica"
                && vrsta != "broju slobodnih smestajnih jedinica")
            {
                ViewBag.Message = "Odaberite neku od ponudjenih vrsta sortiranja!";
                return View("PregledSmestaja");
            }

            if (nacin != "rastuce" && nacin != "opadajuce")
            {
                ViewBag.Message = "Odaberite neki od ponudjenih nacina sortiranja!";
                return View("PregledSmestaja");
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

            Smestaji.ListaSmestaja = SortedList;
            return View("PregledSmestaja");
        }

        [HttpPost]
        public ActionResult PretragaSmestaja()
        {
            List<Smestaj> smestaji = Models.Smestaji.Copy(Models.Smestaji.ListaSmestaja);
            List<Smestaj> smestajFilter = Models.Smestaji.Copy(smestaji);

            foreach (Smestaj s in smestaji.ToList())
            {
                if (s.Obrisan && smestajFilter.Contains(s))
                    smestajFilter.Remove(s);
            }

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
                return View("PregledSmestaja");
            }

            if (bazen.ToLower() != "da" && bazen.ToLower() != "ne" && bazen != "")
            {
                ViewBag.Message = "Los format bazena. (da/ne)";
                return View("PregledSmestaja");
            }

            if (spaCentar.ToLower() != "da" && spaCentar.ToLower() != "ne" && spaCentar != "")
            {
                ViewBag.Message = "Los format spa centra. (da/ne)";
                return View("PregledSmestaja");
            }

            if (prilagodjenoZaInvalide.ToLower() != "da" && prilagodjenoZaInvalide.ToLower() != "ne"
                && prilagodjenoZaInvalide != "")
            {
                ViewBag.Message = "Los format prilagodjenosti za invalide. (da/ne)";
                return View("PregledSmestaja");
            }

            if (wifi.ToLower() != "da" && wifi.ToLower() != "ne" && wifi != "")
            {
                ViewBag.Message = "Los format wifi-a. (da/ne)";
                return View("PregledSmestaja");
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

            return View("PregledSmestaja");
        }

        [HttpPost]
        public ActionResult SortiranjeSJ()
        {
            List<SmestajnaJedinica> jedinice = new List<SmestajnaJedinica>();
            jedinice = SmestajneJedinice.ListaSmestajnihJedinica;

            string vrsta, nacin;
            vrsta = Request["sortvrsta"] != null ? Request["sortvrsta"] : "";
            nacin = Request["sortnacin"] != null ? Request["sortnacin"] : "";

            if (vrsta != "dozvoljenom broju gostiju" && vrsta != "ceni")
            {
                ViewBag.Message = "Odaberite neku od ponudjenih vrsta sortiranja!";
                return View("PregledSmestajnihJedinica");
            }

            if (nacin != "rastuce" && nacin != "opadajuce")
            {
                ViewBag.Message = "Odaberite neki od ponudjenih nacina sortiranja!";
                return View("PregledSmestajnihJedinica");
            }

            List<SmestajnaJedinica> SortedList = new List<SmestajnaJedinica>();

            if (vrsta == "dozvoljenom broju gostiju")
            {
                SortedList = jedinice.OrderBy(o => o.DozvoljenBrojGostiju).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = jedinice.OrderByDescending(o => o.DozvoljenBrojGostiju).ToList();
                }
            }
            else    // po ceni
            {
                SortedList = jedinice.OrderBy(o => o.Cena).ToList();
                if (nacin == "opadajuce")
                {
                    SortedList = jedinice.OrderByDescending(o => o.Cena).ToList();
                }
            }

            SmestajneJedinice.ListaSmestajnihJedinica = SortedList;
            return View("PregledSmestajnihJedinica");
        }

        [HttpPost]
        public ActionResult PretragaSJ()
        {
            List<SmestajnaJedinica> jedinice = SmestajneJedinice.ListaSmestajnihJedinica.ToList();  // kopija
            List<SmestajnaJedinica> jediniceFilter = jedinice.ToList();     // kopija

            string donjiBrojGostiju = Request["donjiBrojGostiju"] != null ? Request["donjiBrojGostiju"] : "";
            string gornjiBrojGostiju = Request["gornjiBrojGostiju"] != null ? Request["gornjiBrojGostiju"] : "";
            string ljubimci = Request["ljubimci"] != null ? Request["ljubimci"] : "";
            string cena = Request["cena"] != null ? Request["cena"] : "";

            ViewBag.TrazenaSJ = null;

            if (!Int32.TryParse(donjiBrojGostiju, out int result1) && donjiBrojGostiju != "")
            {
                ViewBag.Message = "Los format donje granice broja gostiju. Mora biti ceo broj!";
                return View("PregledSmestajnihJedinica");
            }

            if (!Int32.TryParse(gornjiBrojGostiju, out int result2) && gornjiBrojGostiju != "")
            {
                ViewBag.Message = "Los format gornje granice broja gostiju. Mora biti ceo broj!";
                return View("PregledSmestajnihJedinica");
            }

            if (ljubimci.ToLower() != "da" && ljubimci.ToLower() != "ne" && ljubimci != "")
            {
                ViewBag.Message = "Los format ljubimaca.";
                return View("PregledSmestajnihJedinica");
            }

            if (cena != "")
            {
                if (!cena.Contains('-'))
                {
                    ViewBag.Message = "Los format cene.";
                    return View("PregledSmestajnihJedinica");
                }

                string[] granice = cena.Split('-');

                if (granice.Count() != 2)
                {
                    ViewBag.Message = "Los format cene.";
                    return View("PregledSmestajnihJedinica");
                }
                if (!float.TryParse(granice[0], out float min) || !float.TryParse(granice[1], out float max))
                {
                    ViewBag.Message = "Los format cene.";
                    return View("PregledSmestajnihJedinica");
                }

                foreach (SmestajnaJedinica sj in jedinice.ToList())
                {
                    if ((sj.Cena > max || sj.Cena < min) && jediniceFilter.Contains(sj))
                        jediniceFilter.Remove(sj);
                }
            }

            if (donjiBrojGostiju != "")
            {
                foreach (SmestajnaJedinica sj in jedinice.ToList())
                {
                    if (sj.DozvoljenBrojGostiju < result1 && jediniceFilter.Contains(sj))
                        jediniceFilter.Remove(sj);
                }
            }

            if (gornjiBrojGostiju != "")
            {
                foreach (SmestajnaJedinica sj in jedinice.ToList())
                {
                    if (sj.DozvoljenBrojGostiju > result2 && jediniceFilter.Contains(sj))
                        jediniceFilter.Remove(sj);
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

                foreach (SmestajnaJedinica sj in jedinice.ToList())
                {
                    if (sj.Ljubimci != temp && jediniceFilter.Contains(sj))
                        jediniceFilter.Remove(sj);
                }
            }

            if (jediniceFilter.Count == 0)
                ViewBag.Message = "Nema smestajnih jedinica za prikazivanje.";
            else
                ViewBag.TrazenaSJ = jediniceFilter;

            return View("PregledSmestajnihJedinica");
        }

        public ActionResult MojeRezervacije()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OdredjenaRezervacija()
        {
            Rezervacija r = Rezervacije.FindByID(Request["Identifikator"]);
            ViewBag.OdredjenaRezervacija = r;
            return View();
        }

        public ActionResult MojiKomentari()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OdobriKomentar(Komentar k)
        {
            Komentar komentar = Komentari.ListaKomentara.Find(x => (x.Turista == k.Turista
                && x.Aranzman == k.Aranzman && x.Tekst == k.Tekst
                && x.Ocena == k.Ocena && x.Odobren == k.Odobren));

            komentar.Odobren = true;
            Data.AzurirajKomentare();

            return View("MojiKomentari");
        }

        [HttpPost]
        public ActionResult OdbijKomentar(Komentar k)
        {
            Komentar komentar = Komentari.ListaKomentara.Find(x => (x.Turista == k.Turista
                && x.Aranzman == k.Aranzman && x.Tekst == k.Tekst
                && x.Ocena == k.Ocena && x.Odobren == k.Odobren));

            komentar.Odbijen = true;
            Data.AzurirajKomentare();

            return View("MojiKomentari");
        }
    }
}