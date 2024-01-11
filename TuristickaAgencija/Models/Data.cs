using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace TuristickaAgencija.Models
{
    public class Data
    {
        public static float NajmanjaCena(Aranzman a)
        {
            if (a.Smestaj == null)
                return 0;
            if (a.Smestaj.Jedinice.Count == 0)
                return 0;
            
            return a.Smestaj.Jedinice.Min(x => x.Cena);
        }

        #region Aranzmani

        public static List<Aranzman> UcitajAranzmane(string path)
        {
            List<Aranzman> ret = new List<Aranzman>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Aranzman a = new Aranzman(tokens[0], (TipAranzmana)Enum.Parse(typeof(TipAranzmana), tokens[1]),
                    (TipPrevoza)Enum.Parse(typeof(TipPrevoza), tokens[2]), tokens[3],
                    tokens[4], tokens[5], null, tokens[6], Int32.Parse(tokens[7]),
                    tokens[8], tokens[9], tokens[10], null, bool.Parse(tokens[11]), tokens[12]);
                ret.Add(a);

                Korisnik menadzer = Korisnici.FindByUsername(tokens[12]);
                if (!menadzer.Aranzmani.Contains(a))
                {
                    menadzer.Aranzmani.Add(a);
                }
            }
            sr.Close();
            stream.Close();
            
            return ret;
        }

        public static void SacuvajAranzman(Aranzman a)
        {
            Aranzmani.ListaAranzmana.Add(a);
            Data.AzurirajAranzmane();
        }

        public static void AzurirajAranzmane()
        {
            List<Aranzman> lista = Aranzmani.ListaAranzmana;
            string path = HostingEnvironment.MapPath("~/App_Data/aranzmani.txt");
            string str = "";
            bool prvaLinija = true;
            foreach (Aranzman a in lista)
            {
                string datum1 = a.DatumPocetkaPutovanja.ToString("dd/MM/yyyy");
                datum1 = datum1.Replace('.', '/');
                string datum2 = a.DatumZavrsetkaPutovanja.ToString("dd/MM/yyyy");
                datum2 = datum2.Replace('.', '/');
                if (!prvaLinija)
                {
                    str += "\n";
                }
                str += a.Naziv + ";" + a.Tip.ToString() + ";" + a.Prevoz.ToString() + ";"
                + a.Lokacija + ";" + datum1 + ";" + datum2 + ";" + a.VremeNalazenja + ";"
                + a.MaxBrojPutnika.ToString() + ";" + a.OpisAranzmana + ";" + a.ProgramPutovanja + ";"
                + a.PosterAranzmana + ";" + a.Obrisan.ToString() + ";" + a.Kreirao;
                prvaLinija = false;

                Korisnik menadzer = Korisnici.FindByUsername(a.Kreirao);
                if (!menadzer.Aranzmani.Contains(a))
                {
                    menadzer.Aranzmani.Add(a);
                }
            }
            File.WriteAllText(path, str);

            AzurirajKorisnike();
        }

        #endregion

        #region MestaNalazenja

        public static List<MestoNalazenja> UcitajMestaNalazenja(string path)
        {
            List<MestoNalazenja> ret = new List<MestoNalazenja>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                MestoNalazenja mn = new MestoNalazenja(tokens[0], float.Parse(tokens[1]),
                    float.Parse(tokens[2]), tokens[3], bool.Parse(tokens[4]));
                ret.Add(mn);

                Aranzman a = Aranzmani.FindByNaziv(tokens[3]);
                a.MestoNalazenja = mn;
            }
            sr.Close();
            stream.Close();

            return ret;
        }

        public static void AzurirajMestaNalazenja()
        {
            List<MestoNalazenja> lista = MestaNalazenja.ListaMestaNalazenja;
            string path = HostingEnvironment.MapPath("~/App_Data/mestaNalazenja.txt");
            string str = "";
            bool prvaLinija = true;
            foreach (MestoNalazenja mn in lista)
            {
                if (!prvaLinija)
                {
                    str += "\n";
                }
                str += mn.Adresa + ";" + mn.GeografskaDuzina.ToString() + ";" + mn.GeografskaSirina.ToString() + ";"
                    + mn.NazivAranzmana + ";" + mn.Obrisano.ToString();
                prvaLinija = false;
            }
            File.WriteAllText(path, str);
        }

        #endregion

        #region Smestaji

        public static List<Smestaj> UcitajSmestaje(string path)
        {
            List<Smestaj> ret = new List<Smestaj>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Smestaj s = new Smestaj((TipSmestaja)Enum.Parse((typeof(TipSmestaja)), tokens[0]), 
                    tokens[1], Int32.Parse(tokens[2]), bool.Parse(tokens[3]),
                    bool.Parse(tokens[4]), bool.Parse(tokens[5]), bool.Parse(tokens[6]), tokens[7], 
                    bool.Parse(tokens[8]));
                ret.Add(s);

                Aranzman a = Aranzmani.FindByNaziv(tokens[7]);
                a.Smestaj = s;
            }
            sr.Close();
            stream.Close();

            return ret;
        }

        public static void AzurirajSmestaje()
        {
            List<Smestaj> lista = Smestaji.ListaSmestaja;
            string path = HostingEnvironment.MapPath("~/App_Data/smestaji.txt");
            string str = "";
            bool prvaLinija = true;
            foreach (Smestaj s in lista)
            {
                if (!prvaLinija)
                {
                    str += "\n";
                }
                str += s.Tip.ToString() + ";" + s.Naziv + ";" + s.BrojZvezdica.ToString() + ";"
                + s.Bazen.ToString() + ";" + s.SpaCentar.ToString() + ";" + s.PrilagodjenoZaInvalide.ToString() + ";" 
                + s.Wifi.ToString() + ";" + s.NazivAranzmana + ";" + s.Obrisan.ToString();
                prvaLinija = false;
            }
            File.WriteAllText(path, str);
        }

        #endregion

        #region SmestajneJedinice

        public static List<SmestajnaJedinica> UcitajSmestajneJedinice(string path)
        {
            List<SmestajnaJedinica> ret = new List<SmestajnaJedinica>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            // ukoliko je azurirano, mora se ponovo upisivati
            foreach (SmestajnaJedinica sj in SmestajneJedinice.ListaSmestajnihJedinica)
            {
                Smestaj smestaj = Smestaji.FindByNaziv(sj.NazivSmestaja);
                smestaj.Jedinice.Remove(sj);
            }

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                SmestajnaJedinica sj;
                if (tokens[5] != "")
                {
                    sj = new SmestajnaJedinica(Int32.Parse(tokens[0]), bool.Parse(tokens[1]),
                        float.Parse(tokens[2]), tokens[3], bool.Parse(tokens[4]), tokens[5], 
                        Int32.Parse(tokens[6]), bool.Parse(tokens[7]));

                    Korisnik k = Korisnici.FindByUsername(tokens[5]);
                    Aranzman a = Aranzmani.FindByNaziv((Smestaji.FindByNaziv(tokens[3])).NazivAranzmana);
                    if (!k.Aranzmani.Contains(a))
                    {
                        k.Aranzmani.Add(a);
                    }
                }
                else
                {
                    sj = new SmestajnaJedinica(Int32.Parse(tokens[0]), bool.Parse(tokens[1]),
                        float.Parse(tokens[2]), tokens[3], bool.Parse(tokens[4]), null, 
                        Int32.Parse(tokens[6]), bool.Parse(tokens[7]));
                }

                ret.Add(sj);

                Smestaj s = Smestaji.FindByNaziv(tokens[3]);
                if (!s.Jedinice.Contains(sj))
                    s.Jedinice.Add(sj);
                
            }
            sr.Close();
            stream.Close();

            SmestajneJedinice.ListaSmestajnihJedinica = ret;
            

            return ret;
        }

        public static void AzurirajSmestajneJedinice()
        {
            List<SmestajnaJedinica> lista = SmestajneJedinice.ListaSmestajnihJedinica;
            string path = HostingEnvironment.MapPath("~/App_Data/smestajneJedinice.txt");
            string str = "";
            bool prvaLinija = true;
            foreach (SmestajnaJedinica sj in lista)
            {
                if (!prvaLinija)
                {
                    str += "\n";
                }
                str += sj.DozvoljenBrojGostiju.ToString() + ";" + sj.Ljubimci.ToString() + ";"
                    + sj.Cena.ToString() + ";" + sj.NazivSmestaja + ";"
                    + sj.Zauzeta.ToString() + ";" + sj.KorisnickoIme + ";" + sj.Identifikator 
                    + ";" + sj.Obrisana.ToString();
                prvaLinija = false;
            }
            File.WriteAllText(path, str);
        }

        #endregion

        #region Komentari

        public static List<Komentar> UcitajKomentare(string path)
        {
            List<Komentar> ret = new List<Komentar>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Komentar k = new Komentar(tokens[0], tokens[1], tokens[2], Int32.Parse(tokens[3]), bool.Parse(tokens[4]),
                    bool.Parse(tokens[5]));
                ret.Add(k);
            }
            sr.Close();
            stream.Close();

            return ret;
        }

        public static void SacuvajKomentar(Komentar komentar)
        {
            Komentari.ListaKomentara.Add(komentar);
            AzurirajKomentare();
        }

        public static void AzurirajKomentare()
        {
            List<Komentar> lista = Komentari.ListaKomentara;
            string path = HostingEnvironment.MapPath("~/App_Data/komentari.txt");
            string str = "";
            bool prvaLinija = true;
            foreach (Komentar komentar in lista)
            {
                if (!prvaLinija)
                {
                    str += "\n";
                }
                str += komentar.Turista + ";" + komentar.Aranzman + ";" + komentar.Tekst + ";"
                + komentar.Ocena.ToString() + ";" + komentar.Odobren.ToString() + ";" + komentar.Odbijen.ToString();
                prvaLinija = false;
            }
            File.WriteAllText(path, str);
        }

        #endregion

        #region Korisnici

        public static List<Korisnik> UcitajKorisnike(string path)
        {
            List<Korisnik> ret = new List<Korisnik>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Korisnik k = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3],
                    (PolKorisnika)Enum.Parse(typeof(PolKorisnika), tokens[4]), tokens[5], tokens[6],
                    (UlogaKorisnika)Enum.Parse(typeof(UlogaKorisnika), tokens[7]), Int32.Parse(tokens[8]),
                    bool.Parse(tokens[9]));
                ret.Add(k);
            }
            sr.Close();
            stream.Close();

            return ret;
        }

        public static void SacuvajKorisnika(Korisnik user)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            string datum = user.DatumRodjenja.ToString("dd/MM/yyyy");
            datum = datum.Replace('.', '/');

            sw.Write("\n" + user.KorisnickoIme + ";" + user.Lozinka + ";" + user.Ime + ";" + user.Prezime + ";"
                + user.Pol + ";" + user.Email + ";" + datum + ";"
                + user.Uloga.ToString() + ";" + user.Otkazao.ToString() + ";" + user.Blokiran.ToString());

            sw.Close();
            stream.Close();

            Korisnici.ListaKorisnika = Data.UcitajKorisnike("~/App_Data/korisnici.txt");
            AzurirajAranzmane();
        }

        public static void IzmeniKorisnika(Korisnik k)
        {
            List<Korisnik> lista = Korisnici.ListaKorisnika;
            Korisnik korisnik = Korisnici.FindByUsername(k.KorisnickoIme);

            if (korisnik == null)
                return;
            
            korisnik.Lozinka = k.Lozinka;
            korisnik.Ime = k.Ime;
            korisnik.Prezime = k.Prezime;
            korisnik.Pol = k.Pol;
            korisnik.Email = k.Email;
            korisnik.DatumRodjenja = k.DatumRodjenja;
            korisnik.Uloga = k.Uloga;
            korisnik.Otkazao = k.Otkazao;
            korisnik.Blokiran = k.Blokiran;

            Korisnici.ListaKorisnika = lista;
            AzurirajKorisnike();
        }

        public static void AzurirajKorisnike()
        {
            List<Korisnik> lista = Korisnici.ListaKorisnika;
            string path = HostingEnvironment.MapPath("~/App_Data/korisnici.txt");
            //string obrisan;
            string str = "";
            bool prvaLinija = true;
            foreach (Korisnik k in lista)
            {
                string datum = k.DatumRodjenja.ToString("dd/MM/yyyy");
                datum = datum.Replace('.', '/');
                //obrisan = s.Obrisan ? "Da" : "Ne";
                if (!prvaLinija)
                {
                    str += "\n";
                }
                str += k.KorisnickoIme + ";" + k.Lozinka + ";" + k.Ime + ";" + k.Prezime + ";"
                    + k.Pol.ToString() + ";" + k.Email + ";" + datum + ";"
                    + k.Uloga.ToString() + ";" + k.Otkazao.ToString() + ";" + k.Blokiran.ToString();
                prvaLinija = false;
            }
            File.WriteAllText(path, str);
            
        }

        #endregion

        #region Rezervacije

        public static List<Rezervacija> UcitajRezervacije(string path)
        {
            List<Rezervacija> ret = new List<Rezervacija>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Rezervacija r = new Rezervacija(tokens[0], tokens[1],
                    (StatusRezervacije)Enum.Parse(typeof(StatusRezervacije), tokens[2]), tokens[3], Int32.Parse(tokens[4]));
                ret.Add(r);

                Korisnik turista = Korisnici.FindByUsername(tokens[1]);
                if (tokens[2] == "Aktivna")
                {
                    if (!turista.Aranzmani.Contains(Aranzmani.FindByNaziv(r.Aranzman)))
                    {
                        turista.Aranzmani.Add(Aranzmani.FindByNaziv(r.Aranzman));
                    }
                }
                else
                {
                    // Ako ne postoji aktivna rezervacija odredjenog aranzmana za ovog turistu
                    bool postoji = false;
                    foreach (Rezervacija rez in Rezervacije.ListaRezervacija)
                    {
                        if (rez.Status == StatusRezervacije.Aktivna && rez.Aranzman == tokens[3]
                            && rez.Turista == tokens[1])
                        {
                            postoji = true;
                            break;
                        }
                    }
                    if (!postoji)
                    {
                        if (turista.Aranzmani.Contains(Aranzmani.FindByNaziv(r.Aranzman)))
                        {
                            turista.Aranzmani.Remove(Aranzmani.FindByNaziv(r.Aranzman));
                        }
                    }
                }
            }
            sr.Close();
            stream.Close();

            return ret;
        }

        public static void AzurirajRezervacije()
        {
            List<Rezervacija> lista = Rezervacije.ListaRezervacija;
            string path = HostingEnvironment.MapPath("~/App_Data/rezervacije.txt");
            //string obrisan;
            string str = "";
            bool prvaLinija = true;
            foreach (Rezervacija r in lista)
            {
                //obrisan = s.Obrisan ? "Da" : "Ne";
                if (!prvaLinija)
                {
                    str += "\n";
                }
                str += r.Identifikator + ";" + r.Turista + ";" + r.Status.ToString() + ";" + r.Aranzman + ";"
                    + r.SmestajnaJedinica.ToString() /*+ obrisan*/;
                prvaLinija = false;

                Korisnik turista = Korisnici.FindByUsername(r.Turista);
                if (r.Status.ToString() == "Aktivna")
                {
                    if (!turista.Aranzmani.Contains(Aranzmani.FindByNaziv(r.Aranzman)))
                    {
                        turista.Aranzmani.Add(Aranzmani.FindByNaziv(r.Aranzman));
                    }
                }
                else
                {
                    // Ako ne postoji aktivna rezervacija odredjenog aranzmana za ovog turistu
                    bool postoji = false;
                    foreach (Rezervacija rez in Rezervacije.ListaRezervacija)
                    {
                        if (rez.Status == StatusRezervacije.Aktivna && rez.Aranzman == r.Aranzman
                            && rez.Turista == r.Turista)
                        {
                            postoji = true;
                            break;
                        }
                    }
                    if (!postoji)
                    {
                        if (turista.Aranzmani.Contains(Aranzmani.FindByNaziv(r.Aranzman)))
                        {
                            turista.Aranzmani.Remove(Aranzmani.FindByNaziv(r.Aranzman));
                        }
                    }
                }
            }
            File.WriteAllText(path, str);
        }

        #endregion
    }
}