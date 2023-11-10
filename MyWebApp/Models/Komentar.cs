using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Komentar
    {
        public Korisnik Posetilac { get; set; }
        public FitnesCentar FitnesCentar { get; set; }
        public String TekstKomentara { get; set; }
        public float Ocena { get; set; } 
        public int Id { get; set; }

        public Komentar() { }

        public Komentar(Korisnik posetilac, FitnesCentar fitnesCentar, string tekstKomentara, float ocena, int id)
        {
            Posetilac = posetilac;
            FitnesCentar = fitnesCentar;
            TekstKomentara = tekstKomentara;
            Ocena = ocena;
            Id = id;
        }
    }
}