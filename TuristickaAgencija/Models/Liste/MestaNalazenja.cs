using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuristickaAgencija.Models
{
    public class MestaNalazenja
    {
        public static List<MestoNalazenja> ListaMestaNalazenja = new List<MestoNalazenja>();

        public static MestoNalazenja FindByAranzman(string naziv)
        {
            return ListaMestaNalazenja.Find(x => x.NazivAranzmana == naziv);
        }
    }
}