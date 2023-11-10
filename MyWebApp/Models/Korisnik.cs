using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Korisnik
    {
        public Korisnik(string korisnickoIme, Uloga uloga)
        {
            KorisnickoIme = korisnickoIme;
            this.Uloga = uloga;
        }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string email, string datumRodjenja, Uloga uloga, List<GrupniTrening> treninziPosetioca, List<GrupniTrening> treninziTrenera, FitnesCentar kTrener, List<FitnesCentar> fitnesCentri, bool prijavljen, bool blokiran)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            TreninziPosetioca = treninziPosetioca;
            TreninziTrenera = treninziTrenera;
            KTrener = kTrener;
            FitnesCentri = fitnesCentri;
            Prijavljen = false;
            Blokiran = false;
        }
        public Korisnik() { }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string email, string datumRodjenja, Uloga uloga, FitnesCentar kTrener, bool prijavljen, bool blokiran)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            KTrener = kTrener;
            Prijavljen = prijavljen;
            Blokiran = blokiran;
        }

        public Korisnik(string korisnickoIme)
        {
            KorisnickoIme = korisnickoIme;
        }

        public String KorisnickoIme { get; set; }
        public String Lozinka { get; set; }
        public String Ime { get; set; }
        public String Prezime { get; set; }
        public String Pol { get; set; }
        public String Email { get; set; }
        public String DatumRodjenja { get; set; }
        public Uloga Uloga { get; set; }
        public List<GrupniTrening> TreninziPosetioca { get; set; }
        public List<GrupniTrening> TreninziTrenera { get; set; }
        public FitnesCentar KTrener { get; set; }
        public List<FitnesCentar> FitnesCentri { get; set; }
        public bool Prijavljen { get; set; }
        public bool Blokiran { get; set; }
    }
}