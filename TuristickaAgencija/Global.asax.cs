using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
using System.Web.Routing;
using TuristickaAgencija.Models;

namespace TuristickaAgencija
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Korisnici.ListaKorisnika = Data.UcitajKorisnike("~/App_Data/korisnici.txt");
            Aranzmani.ListaAranzmana = Data.UcitajAranzmane("~/App_Data/aranzmani.txt");
            MestaNalazenja.ListaMestaNalazenja = Data.UcitajMestaNalazenja("~/App_Data/mestaNalazenja.txt");
            Smestaji.ListaSmestaja = Data.UcitajSmestaje("~/App_Data/smestaji.txt");
            
            SmestajneJedinice.ListaSmestajnihJedinica = Data.UcitajSmestajneJedinice("~/App_Data/smestajneJedinice.txt");
            Komentari.ListaKomentara = Data.UcitajKomentare("~/App_Data/komentari.txt");
            Rezervacije.ListaRezervacija = Data.UcitajRezervacije("~/App_Data/rezervacije.txt");
           
        }
    }
}
