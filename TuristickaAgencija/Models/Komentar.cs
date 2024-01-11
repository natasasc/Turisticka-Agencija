using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class Komentar
    {
        public string Turista { get; set; }     // cuva se korisnicko ime
        public string Aranzman { get; set; }    // cuva se naziv
        public string Tekst { get; set; }
        public int Ocena { get; set; }      // na skali od 1 do 5

        public bool Odobren { get; set; }
        public bool Odbijen { get; set; }

        public Komentar()
        {
            Turista = "";
            Aranzman = "";
            Tekst = "";
            Ocena = 0;
            Odobren = false;
            Odbijen = false;
        }

        public Komentar(string usernameTuriste, string nazivAranzmana, string tekst, int ocena, bool odobren, bool odbijen)
        {
            Turista = usernameTuriste;
            Aranzman = nazivAranzmana;
            Tekst = tekst;
            Ocena = ocena;
            Odobren = odobren;
            Odbijen = odbijen;
        }
    }
}