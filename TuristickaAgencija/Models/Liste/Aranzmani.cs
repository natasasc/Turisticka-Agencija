using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace TuristickaAgencija.Models
{
    public class Aranzmani
    {
        public static List<Aranzman> ListaAranzmana = new List<Aranzman>();
        
        public static Aranzman FindByNaziv(string naziv)
        {
            return ListaAranzmana.Find(item => item.Naziv == naziv);
        }

        public static List<Aranzman> Copy(List<Aranzman> aranzmani)
        {
            List<Aranzman> ret = new List<Aranzman>();

            foreach (Aranzman a in aranzmani)
            {
                ret.Add(a);
            }

            return ret;
        }

        //public static Aranzman UpdateAranzman(Aranzman aranzman)
        //{
        //    Aranzman existingAranzman = FindByNaziv(aranzman.Naziv);
        //    existingAranzman.FirstName = aranzman.FirstName;
        //    existingAranzman.LastName = aranzman.LastName;
        //    return existingAranzman;
        //}
        //private static int GenerateId()
        //{
        //    return Math.Abs(Guid.NewGuid().GetHashCode());
        //}
    }
}