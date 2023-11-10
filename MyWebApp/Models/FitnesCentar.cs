using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class FitnesCentar
    {
        public FitnesCentar(string naziv, string adresa, int godinaOtvaranja, Korisnik vlasnik, double cenaMesecneClanarine, double cenaGodisnjeClanarine, double cenaTreninga, double cenaGrupnogTreninga, double cenaGrupnogTreningaSaPT, bool obrisan)
        {
            Naziv = naziv;
            Adresa = adresa;
            GodinaOtvaranja = godinaOtvaranja;
            Vlasnik = vlasnik;
            CenaMesecneClanarine = cenaMesecneClanarine;
            CenaGodisnjeClanarine = cenaGodisnjeClanarine;
            CenaTreninga = cenaTreninga;
            CenaGrupnogTreninga = cenaGrupnogTreninga;
            CenaGrupnogTreningaSaPT = cenaGrupnogTreningaSaPT;
            Obrisan = obrisan;
        }
        public FitnesCentar() { }

        public FitnesCentar(string naziv)
        {
            Naziv = naziv;
        }

        public FitnesCentar(string naziv, string adresa)
        {
            Naziv = naziv;
            Adresa = adresa;
        }

        public String Naziv { get; set; }
        public String Adresa { get; set; } // ulica i broj, grad, postanski broj 
        public int GodinaOtvaranja { get; set; }
        public Korisnik Vlasnik { get; set; }
        public double CenaMesecneClanarine { get; set; }
        public double CenaGodisnjeClanarine { get; set; }
        public double CenaTreninga { get; set; }
        public double CenaGrupnogTreninga { get; set; }
        public double CenaGrupnogTreningaSaPT { get; set; }
        public bool Obrisan { get; set; }

    }
}