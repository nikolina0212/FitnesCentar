using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MyWebApp.Models
{
    public class Data
    {
        public static List<FitnesCentar> UcitajFitnesCentre(string path)
        {
            List<FitnesCentar> fitnesCentri = new List<FitnesCentar>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                FitnesCentar fc = new FitnesCentar(tokens[0], tokens[1], int.Parse(tokens[2]), new Korisnik(tokens[3],(Uloga)Enum.Parse(typeof(Uloga), tokens[4])), double.Parse(tokens[5]), double.Parse(tokens[6]), double.Parse(tokens[7]), double.Parse(tokens[8]), double.Parse(tokens[9]), bool.Parse(tokens[10]));
                fitnesCentri.Add(fc);
            }

            sr.Close();
            stream.Close();
            return fitnesCentri;
        }

        public static List<GrupniTrening> UcitajTreninge(string path)
        {
            List<GrupniTrening> grupniTreninzi = new List<GrupniTrening>();
            path = HostingEnvironment.MapPath(path);
            using (var sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] tokens = line.Split(';');

                    GrupniTrening grTrening= new GrupniTrening(tokens[0], tokens[1], new FitnesCentar(tokens[2], tokens[3]), int.Parse(tokens[4]), DateTime.ParseExact(tokens[5], "dd/MM/yyyy HH:mm", null), int.Parse(tokens[6]), new List<string>(), new Korisnik(tokens[8]), bool.Parse(tokens[9]));

                    if (tokens[6] != String.Empty)
                    {
                        var posetioci = tokens[7].Split(',');
                        foreach (var item in posetioci)
                        {
                            grTrening.SpisakPosetilaca.Add(item);
                        }

                    }

                    string id = grTrening.Naziv;
                    grTrening.Naziv = id;
                    grupniTreninzi.Add(grTrening);
                }
                sr.Close();
            }
            return grupniTreninzi;
        }

        public static void SacuvajTrening(List<GrupniTrening> grupniTreninzi)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/GrupniTreninzi.txt");
            List<Korisnik> korisnici = UcitajKorisnike("~/App_Data/Korisnici.txt");

            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (GrupniTrening item in grupniTreninzi)
                {
                    string posetilac = String.Empty;
               
                    if (item.SpisakPosetilaca != null)
                    {
                        if (item.SpisakPosetilaca.Count > 0)
                        {
                            foreach (var p in item.SpisakPosetilaca)
                            {
                                posetilac += p;
                                posetilac += ",";

                            }
                            posetilac = posetilac.Remove(posetilac.Length - 1);
                        }
                    }
                  
                    string gt = item.Naziv + ";" + item.TipTreninga + ";" + item.FitnesCentar.Naziv + ";" + item.FitnesCentar.Adresa + ";" + item.TrajanjeTreninga + ";" + item.VremeTreninga.ToString("dd/MM/yyyy HH:mm") + ";" + item.MaxBrPosetilaca + ";" + posetilac + ";" + item.Trener.KorisnickoIme + ";" + item.Obrisan.ToString();
                    sw.WriteLine(gt);
                }

            }
        }

        public static List<Komentar> UcitajKomentare(string path)
        {
            List<Komentar> komentari = new List<Komentar>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Komentar kom = new Komentar(new Korisnik(tokens[0]), new FitnesCentar(tokens[1], tokens[2]), tokens[3], float.Parse(tokens[4]), int.Parse(tokens[5]));
                komentari.Add(kom);
            }

            sr.Close();
            stream.Close();

            return komentari;
        }
        public static void SacuvajKorisnika(Korisnik k)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(k.KorisnickoIme + ";" + k.Lozinka + ";" + k.Ime + ";" + k.Prezime + ";" +
                k.Pol + ";" + k.Email + ";" + (DateTime.Parse(k.DatumRodjenja)).ToString("dd/MM/yyyy") + ";" + k.Uloga.ToString() + ";" + k.KTrener.Naziv + ";" + k.KTrener.Adresa + ";" + k.Prijavljen.ToString() + ";" + k.Blokiran.ToString());

            sw.Close();
            stream.Close();
        }

        public static List<Korisnik> UcitajKorisnike(string path)
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                int count = tokens.Count();
                Korisnik kor = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4].ToString(), tokens[5], tokens[6], (Uloga)Enum.Parse(typeof(Uloga), tokens[7]), new FitnesCentar(tokens[8], tokens[9]), bool.Parse(tokens[10]), bool.Parse(tokens[11]));
                korisnici.Add(kor);
            }

            sr.Close();
            stream.Close();

            return korisnici;
        }

        public static void IzmeniKorisnika(List<Korisnik> korisnici)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/korisnici.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Korisnik k in korisnici)
            {
                sw.WriteLine(k.KorisnickoIme + ";" + k.Lozinka + ";" + k.Ime + ";" + k.Prezime + ";" +
                k.Pol + ";" + k.Email + ";" + (DateTime.Parse(k.DatumRodjenja)).ToString("dd/MM/yyyy") + ";" + k.Uloga.ToString() + ";" + k.KTrener.Naziv + ";"  + k.KTrener.Adresa + ";" + k.Prijavljen.ToString() + ";" + k.Blokiran.ToString());
            }

            sw.Close();
            stream.Close();

        }

        public static void SacuvajFitnesCentar(FitnesCentar f)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/FitnesCentri.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(f.Naziv + ";" + f.Adresa + ";" + f.GodinaOtvaranja.ToString() + ";" + f.Vlasnik.KorisnickoIme + ";" + f.Vlasnik.Uloga.ToString() + ";" +
                f.CenaMesecneClanarine.ToString() + ";" + f.CenaGodisnjeClanarine.ToString() + ";" + f.CenaTreninga.ToString() + ";" + f.CenaGrupnogTreninga.ToString() + ";" + f.CenaGrupnogTreningaSaPT.ToString() + ";" + f.Obrisan.ToString());
            sw.Close();
            stream.Close();
        }

        public static void IzmenaFitnesCentara(List<FitnesCentar> fitnesCentri)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/FitnesCentri.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (FitnesCentar f in fitnesCentri)
            {
                sw.WriteLine(f.Naziv + ";" + f.Adresa + ";" + f.GodinaOtvaranja.ToString() + ";" + f.Vlasnik.KorisnickoIme + ";" + f.Vlasnik.Uloga.ToString() + ";" +
               f.CenaMesecneClanarine.ToString() + ";" + f.CenaGodisnjeClanarine.ToString() + ";" + f.CenaTreninga.ToString() + ";" + f.CenaGrupnogTreninga.ToString() + ";" + f.CenaGrupnogTreningaSaPT.ToString() + ";" + f.Obrisan.ToString());
            }

            sw.Close();
            stream.Close();

        }

        public static void IzmeniKomentare(List<Komentar> komentari)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Komentari.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Komentar kom in komentari)
            {
                sw.WriteLine( kom.Posetilac.KorisnickoIme + ";" + kom.FitnesCentar.Naziv + ";" + kom.FitnesCentar.Adresa + ";" + kom.TekstKomentara + ";" + kom.Ocena + ";"+ kom.Id);
            }

            sw.Close();
            stream.Close();

        }

        public static void IzmeniKomentareZaVlasnika(List<Komentar> komentari)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/KomentariZaVlasnika.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Komentar kom in komentari)
            {
                sw.WriteLine(kom.Posetilac.KorisnickoIme + ";" + kom.FitnesCentar.Naziv + ";" + kom.FitnesCentar.Adresa + ";" + kom.TekstKomentara + ";" + kom.Ocena + ";" + kom.Id);
            }

            sw.Close();
            stream.Close();

        }

        public static void SacuvajKomentarZaVlasnika(Komentar kom)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/KomentariZaVlasnika.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine( kom.Posetilac.KorisnickoIme + ";" + kom.FitnesCentar.Naziv + ";" + kom.FitnesCentar.Adresa + ";" + kom.TekstKomentara + ";" + kom.Ocena + ";" + kom.Id);

            sw.Close();
            stream.Close();
        }

    }
}