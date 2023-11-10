using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class GrupniTrening
    {
        public GrupniTrening(string naziv, string tipTreninga, FitnesCentar fitnesCentar, int trajanjeTreninga, DateTime vremeTreninga, int maxBrPosetilaca, List<string> spisakPosetilaca, Korisnik trener, bool obrisan)
        {
            Naziv = naziv;
            TipTreninga = tipTreninga;
            FitnesCentar = fitnesCentar;
            TrajanjeTreninga = trajanjeTreninga;
            VremeTreninga = vremeTreninga;
            MaxBrPosetilaca = maxBrPosetilaca;
            SpisakPosetilaca = spisakPosetilaca;
            Trener = trener;
            Obrisan = obrisan;
        }
        public GrupniTrening() { }


        public String Naziv { get; set; }
        public String TipTreninga { get; set; }
        public FitnesCentar FitnesCentar { get; set; }
        public int TrajanjeTreninga { get; set; } 
        public DateTime VremeTreninga { get; set; }
        public int MaxBrPosetilaca { get; set; }
        public List<string> SpisakPosetilaca { get; set; }
        public Korisnik Trener { get; set; }
        public bool Obrisan { get; set; }
    }
}