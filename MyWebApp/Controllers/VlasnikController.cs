using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class VlasnikController : Controller
    {
        // GET: Vlasnik
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PregledFitnesCentara()
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVlasnika = new List<FitnesCentar>();

            foreach(var f in fitnesCentri)
            {
                if (f.Vlasnik.KorisnickoIme == korisnik.KorisnickoIme && f.Obrisan==false)
                {
                    fitnesCentriVlasnika.Add(f);
                }
            }

            korisnik.FitnesCentri = fitnesCentriVlasnika;
            return View(fitnesCentriVlasnika);
        }

        [HttpPost]
        public ActionResult RegistracijaTrenera(string fitnesCentar, string adresa)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            ViewBag.Naziv = fitnesCentar;
            ViewBag.Adresa = adresa;
            return View();
        }

        [HttpPost]
        public ActionResult RegistrujTrenera(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string email, string datumRodjenja, string fitnesCentar, string adresa)
        {
           
            if (korisnickoIme == null && lozinka == null && ime == null && prezime == null && email == null && datumRodjenja == null)
            {
                TempData["porukaaa"] = $"Za uspesnu registraciju neophodno je ispravno popuniti sva polja!";
                return View("RegistracijaTrenera");
            }
            else if (korisnickoIme == null)
            {
                TempData["poruka1"] = "Polje korisnicko ime je obavezno!";
                return View("RegistracijaTrenera");

            }
            else if (lozinka == null)
            {
                TempData["poruka2"] = "Polje lozinka je obavezno!";
                return View("RegistracijaTrenera");

            }
            else if (ime == null)
            {
                TempData["poruka3"] = "Polje ime je obavezno!";
                return View("RegistracijaTrenera");

            }
            else if (prezime == null)
            {
                TempData["poruka4"] = "Polje prezime je obavezno!";
                return View("RegistracijaTrenera");

            }
            else if (String.IsNullOrEmpty(pol.ToString()))
            {
                TempData["poruka5"] = "Polje pol je obavezno!";
                return View("RegistracijaTrenera");

            }
            else if (email == null)
            {
                TempData["poruka6"] = "Polje email je obavezno!";
                return View("RegistracijaTrenera");

            }
            else if (String.IsNullOrEmpty(datumRodjenja))
            {
                TempData["poruka7"] = "Polje datum rodjenja je obavezno!";
                return View("RegistracijaTrenera");
            }
            else
            {

                List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
                foreach (Korisnik k in korisnici.ToList())
                {
                    if (k.KorisnickoIme == korisnickoIme && k.Uloga == Uloga.TRENER && k.KTrener.Naziv == fitnesCentar && k.KTrener.Adresa==adresa && k.Blokiran==false)
                    {
                        TempData["porukaa"] = $"Trener je vec registrovan u Vasem fitnes centru!";
                        return View("RegistracijaTrenera");
                    }
                    if (k.KorisnickoIme == korisnickoIme && k.Uloga == Uloga.TRENER && k.Blokiran==false)
                    {
                        TempData["porukaa"] = $"Trener ne moze biti istovremeno zaposlen u vise fitnes centara!";
                        return View("RegistracijaTrenera");
                    }
                    if (k.KorisnickoIme == korisnickoIme && k.Blokiran==false)
                    { 
                        TempData["porukaa"] = $"Korisnik sa korisnickim imenom: {korisnickoIme} vec postoji. Pokusajte ponovo!";
                        return View("RegistracijaTrenera");
                    }
                  
                }
                
                Korisnik korisnik = new Korisnik(korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja, Uloga.TRENER, new FitnesCentar(fitnesCentar, adresa), false, false);
                korisnici.Add(korisnik);
                Data.SacuvajKorisnika(korisnik);
                TempData["porukaa"] = $"Registrovali ste trenera: {ime} {prezime}";
                return View("Index");

            }

        }

        [HttpPost]
        public ActionResult Kreiraj(string naziv, string adresa, string mesecnaCl,string godisnjaCl, string cenaTr, string cenaGrupnogTr, string cenaTrSaPT)
        {
   
            if (String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(adresa) && String.IsNullOrEmpty(mesecnaCl) && String.IsNullOrEmpty(godisnjaCl) && String.IsNullOrEmpty(cenaTr) && String.IsNullOrEmpty(cenaGrupnogTr) && String.IsNullOrEmpty(cenaTrSaPT))
            {
                TempData["porukaaa"] = $"Za uspesno dodavanje fitnes centra neophodno je ispravno popuniti sva polja!";
                return RedirectToAction("Kreiranje", "Vlasnik");
            }
            else if (String.IsNullOrEmpty(naziv))
            {
                TempData["poruka1"] = "Polje naziv je obavezno!";
                return RedirectToAction("Kreiranje", "Vlasnik");

            }
            else if (String.IsNullOrEmpty(adresa))
            {
                TempData["poruka2"] = "Polje adresa je obavezno!";
                return RedirectToAction("Kreiranje", "Vlasnik");

            }
            else if (String.IsNullOrEmpty(mesecnaCl))
            {
                TempData["poruka3"] = "Polje mesecna clanarina je obavezno!";
                return RedirectToAction("Kreiranje", "Vlasnik");

            }
            else if (String.IsNullOrEmpty(godisnjaCl))
            {
                TempData["poruka4"] = "Polje godisnja clanarina je obavezno!";
                return RedirectToAction("Kreiranje", "Vlasnik");

            }
            else if (String.IsNullOrEmpty(cenaTr))
            {
                TempData["poruka5"] = "Polje cena treninga je obavezno!";
                return RedirectToAction("Kreiranje", "Vlasnik");

            }
            else if (String.IsNullOrEmpty(cenaGrupnogTr))
            {
                TempData["poruka6"] = "Polje cena grupnog treninga je obavezno!";
                return RedirectToAction("Kreiranje", "Vlasnik");

            }
            else if (String.IsNullOrEmpty(cenaTrSaPT))
            {
                TempData["poruka7"] = "Polje cena treninga sa personalnim trenerom je obavezno!";
                return RedirectToAction("Kreiranje", "Vlasnik");

            }
            else
            {
                Korisnik korisnik = (Korisnik)Session["korisnik"];
                List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
                List<FitnesCentar> postoji = new List<FitnesCentar>();
                foreach (var f in fitnesCentri)
                {
                    if (f.Naziv == naziv && f.Adresa == adresa && f.Obrisan==false)
                    {
                        postoji.Add(f);
                    }
                }

                if (postoji.Count == 0)
                {
                    int godOtv = DateTime.Now.Year;
                    FitnesCentar fitnesCentar = new FitnesCentar(naziv, adresa, godOtv, new Korisnik(korisnik.KorisnickoIme, Uloga.VLASNIK), Double.Parse(mesecnaCl), Double.Parse(godisnjaCl), Double.Parse(cenaTr), Double.Parse(cenaGrupnogTr), Double.Parse(cenaTrSaPT), false);
                    fitnesCentri.Add(fitnesCentar);
                    Data.SacuvajFitnesCentar(fitnesCentar);
                    TempData["poruka"] = $"Uspesno kreiran fitnes centar {naziv}!";
                    return RedirectToAction("Index", "Vlasnik");

                }
                else
                {
                    TempData["poruka"] = "Fitnes centar vec postoji.Pokusajte ponovo!";
                    return RedirectToAction("Kreiranje", "Vlasnik");
                }
            }
           

        }
        public ActionResult Kreiranje()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IzmenaFitnesCentra(string nazivFC, string adresa)
        {
            
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pronadjen = new List<FitnesCentar>();
            foreach(var f in fitnesCentri)
            {
                if (f.Naziv == nazivFC && f.Adresa==adresa)
                {
                    pronadjen.Add(f);
                }
            }
            return View(pronadjen);
        }


        [HttpPost]
        public ActionResult IzmeniNaziv(string naziv, string fitnesC, string adresaC)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pronadjen = new List<FitnesCentar>();
            FitnesCentar izmenjen = new FitnesCentar();

            if (!String.IsNullOrEmpty(naziv))
            {
                foreach (var f in fitnesCentri)
                {
                    if (f.Naziv == fitnesC && f.Adresa == adresaC && f.Obrisan==false)
                    {
                        pronadjen.Add(f);
                        izmenjen = f;
                    }
   
                }

                    fitnesCentri.Remove(izmenjen);
                    izmenjen.Naziv = naziv;
                    fitnesCentri.Add(izmenjen);
                    Data.IzmenaFitnesCentara(fitnesCentri);


                List<Komentar>komentari= (List<Komentar>)HttpContext.Application["komentari"];
                foreach(Komentar k in komentari.ToList())
                {
                    if(k.FitnesCentar.Naziv==fitnesC && k.FitnesCentar.Adresa == adresaC && k.FitnesCentar.Obrisan==false)
                    {
                        komentari.Remove(k);
                        k.FitnesCentar.Naziv = naziv;
                        komentari.Add(k);
                        Data.IzmeniKomentare(komentari);
                    }
                }

               
                List<Korisnik> korisnici= (List<Korisnik>)HttpContext.Application["korisnici"];
                foreach(Korisnik k in korisnici.ToList())
                {
                    if(k.KTrener.Naziv==fitnesC && k.KTrener.Adresa == adresaC && k.KTrener.Obrisan==false)
                    {
                        korisnici.Remove(k);
                        k.KTrener.Naziv = naziv;
                        korisnici.Add(k);
                        Data.IzmeniKorisnika(korisnici);
                    }
                }

               
                List<Komentar> komentariZaVlasnika = (List<Komentar>)HttpContext.Application["komentariZaVlasnika"];
                foreach (Komentar k in komentariZaVlasnika.ToList())
                {
                    if (k.FitnesCentar.Naziv == fitnesC && k.FitnesCentar.Adresa == adresaC && k.FitnesCentar.Obrisan==false)
                    {
                        komentariZaVlasnika.Remove(k);
                        k.FitnesCentar.Naziv = naziv;
                        komentariZaVlasnika.Add(k);
                        Data.IzmeniKomentareZaVlasnika(komentariZaVlasnika);
                    }
                }


                List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
                foreach (GrupniTrening g in grupniTreninzi.ToList())
                {
                    if (g.FitnesCentar.Naziv == fitnesC && g.FitnesCentar.Adresa == adresaC && g.FitnesCentar.Obrisan==false)
                    {
                        grupniTreninzi.Remove(g);
                        g.FitnesCentar.Naziv = naziv;
                        grupniTreninzi.Add(g);
                        Data.SacuvajTrening(grupniTreninzi);
                    }
                }


                return View("IzmenaFitnesCentra", pronadjen);
                
            }

            foreach (var f in fitnesCentri)
            {
                if (f.Naziv == fitnesC && f.Adresa == adresaC)
                {
                    pronadjen.Add(f);
                }
            }

            TempData["poruka"] = "Polje naziv ne sme biti prazno!";
            return View("IzmenaFitnesCentra", pronadjen);
        }

        [HttpPost]
        public ActionResult IzmeniAdresu(string adresa, string fitnesC, string adresaC)
        {

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pronadjen = new List<FitnesCentar>();
            FitnesCentar izmenjen = new FitnesCentar();
            if (!String.IsNullOrEmpty(adresa))
            {
                foreach (var f in fitnesCentri)
                {
                    if (f.Naziv == fitnesC && f.Adresa == adresaC && f.Obrisan==false)
                    {
                        pronadjen.Add(f);
                        izmenjen = f;
                    }
                }

                fitnesCentri.Remove(izmenjen);
                izmenjen.Adresa = adresa;
                fitnesCentri.Add(izmenjen);
                Data.IzmenaFitnesCentara(fitnesCentri);


                List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
                foreach (Komentar k in komentari.ToList())
                {
                    if (k.FitnesCentar.Naziv == fitnesC && k.FitnesCentar.Adresa == adresaC && k.FitnesCentar.Obrisan==false)
                    {
                        komentari.Remove(k);
                        k.FitnesCentar.Adresa = adresa;
                        komentari.Add(k);
                        Data.IzmeniKomentare(komentari);
                    }
                }

              
                List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
                foreach (Korisnik k in korisnici.ToList())
                {
                    if (k.KTrener.Naziv == fitnesC && k.KTrener.Adresa == adresaC && k.KTrener.Obrisan==false)
                    {
                        korisnici.Remove(k);
                        k.KTrener.Adresa = adresa;
                        korisnici.Add(k);
                        Data.IzmeniKorisnika(korisnici);
                    }
                }

               
                List<Komentar> komentariZaVlasnika = (List<Komentar>)HttpContext.Application["komentariZaVlasnika"];
                foreach (Komentar k in komentariZaVlasnika.ToList())
                {
                    if (k.FitnesCentar.Naziv == fitnesC && k.FitnesCentar.Adresa == adresaC && k.FitnesCentar.Obrisan==false)
                    {
                        komentariZaVlasnika.Remove(k);
                        k.FitnesCentar.Adresa = adresa;
                        komentariZaVlasnika.Add(k);
                        Data.IzmeniKomentareZaVlasnika(komentariZaVlasnika);
                    }
                }

                List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
                foreach (GrupniTrening g in grupniTreninzi.ToList())
                {
                    if (g.FitnesCentar.Naziv == fitnesC && g.FitnesCentar.Adresa == adresaC && g.FitnesCentar.Obrisan==false)
                    {
                        grupniTreninzi.Remove(g);
                        g.FitnesCentar.Adresa = adresa;
                        grupniTreninzi.Add(g);
                        Data.SacuvajTrening(grupniTreninzi);
                    }
                }

               
                return View("IzmenaFitnesCentra", pronadjen);
            }

            foreach (var f in fitnesCentri)
            {
                if (f.Naziv == fitnesC && f.Adresa == adresaC)
                {
                    pronadjen.Add(f);
                }
            }

            TempData["poruka"] = "Polje adresa ne sme biti prazno!";
            return View("IzmenaFitnesCentra", pronadjen);

        }
        [HttpPost]
        public ActionResult IzmeniMesecnuClanarinu(string cmcl, string fitnesC, string adresaC)
        {

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pronadjen = new List<FitnesCentar>();
            FitnesCentar izmenjen = new FitnesCentar();
            if (!String.IsNullOrEmpty(cmcl))
            {
                foreach (var f in fitnesCentri)
                {
                    if (f.Naziv == fitnesC && f.Adresa == adresaC)
                    {
                        pronadjen.Add(f);
                        izmenjen = f;
                    }
                }

                fitnesCentri.Remove(izmenjen);
                izmenjen.CenaMesecneClanarine = Double.Parse(cmcl);
                fitnesCentri.Add(izmenjen);
                Data.IzmenaFitnesCentara(fitnesCentri);
                return View("IzmenaFitnesCentra", pronadjen);
            }

            foreach (var f in fitnesCentri)
            {
                if (f.Naziv == fitnesC && f.Adresa == adresaC)
                {
                    pronadjen.Add(f);
                }
            }

            TempData["poruka"] = "Polje mesecna clanarina ne sme biti prazno!";
            return View("IzmenaFitnesCentra", pronadjen);

        }
        [HttpPost]
        public ActionResult IzmeniGodisnjuClanarinu(string cgcl, string fitnesC, string adresaC)
        {

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pronadjen = new List<FitnesCentar>();
            FitnesCentar izmenjen = new FitnesCentar();
            if (!String.IsNullOrEmpty(cgcl))
            {
                foreach (var f in fitnesCentri)
                {
                    if (f.Naziv == fitnesC && f.Adresa == adresaC)
                    {
                        pronadjen.Add(f);
                        izmenjen = f;
                    }
                }

                fitnesCentri.Remove(izmenjen);
                izmenjen.CenaGodisnjeClanarine = Double.Parse(cgcl);
                fitnesCentri.Add(izmenjen);
                Data.IzmenaFitnesCentara(fitnesCentri);
                return View("IzmenaFitnesCentra", pronadjen);
            }

            foreach (var f in fitnesCentri)
            {
                if (f.Naziv == fitnesC && f.Adresa == adresaC)
                {
                    pronadjen.Add(f);
                }
            }

            TempData["poruka"] = "Polje godisnja clanarina ne sme biti prazno!";
            return View("IzmenaFitnesCentra", pronadjen);
        }
        [HttpPost]
        public ActionResult IzmeniCenuTreninga(string cenaT, string fitnesC, string adresaC)
        {

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pronadjen = new List<FitnesCentar>();
            FitnesCentar izmenjen = new FitnesCentar();
            if (!String.IsNullOrEmpty(cenaT))
            {
                foreach (var f in fitnesCentri)
                {
                    if (f.Naziv == fitnesC && f.Adresa == adresaC)
                    {
                        pronadjen.Add(f);
                        izmenjen = f;
                    }
                }

                fitnesCentri.Remove(izmenjen);
                izmenjen.CenaTreninga = Double.Parse(cenaT);
                fitnesCentri.Add(izmenjen);
                Data.IzmenaFitnesCentara(fitnesCentri);
                return View("IzmenaFitnesCentra", pronadjen);
            }

            foreach (var f in fitnesCentri)
            {
                if (f.Naziv == fitnesC && f.Adresa == adresaC)
                {
                    pronadjen.Add(f);
                }
            }

            TempData["poruka"] = "Polje cena treninga ne sme biti prazno!";
            return View("IzmenaFitnesCentra", pronadjen);


        }
        [HttpPost]
        public ActionResult IzmeniCenuGrupnogTreninga(string cenaGT, string fitnesC, string adresaC)
        {

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pronadjen = new List<FitnesCentar>();
            FitnesCentar izmenjen = new FitnesCentar();
            if (!String.IsNullOrEmpty(cenaGT))
            {
                foreach (var f in fitnesCentri)
                {
                    if (f.Naziv == fitnesC && f.Adresa == adresaC)
                    {
                        pronadjen.Add(f);
                        izmenjen = f;
                    }
                }

                fitnesCentri.Remove(izmenjen);
                izmenjen.CenaGrupnogTreninga = Double.Parse(cenaGT);
                fitnesCentri.Add(izmenjen);
                Data.IzmenaFitnesCentara(fitnesCentri);
                return View("IzmenaFitnesCentra", pronadjen);

            }

            foreach (var f in fitnesCentri)
            {
                if (f.Naziv == fitnesC && f.Adresa == adresaC)
                {
                    pronadjen.Add(f);
                }
            }

            TempData["poruka"] = "Polje cena grupnog treninga ne sme biti prazno!";
            return View("IzmenaFitnesCentra", pronadjen);

        }
        [HttpPost]
        public ActionResult IzmeniCenuGrupnogTreningaSaPT(string cenaGTsaPT, string fitnesC, string adresaC)
        {

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pronadjen = new List<FitnesCentar>();
            FitnesCentar izmenjen = new FitnesCentar();
            if (!String.IsNullOrEmpty(cenaGTsaPT))
            {
                foreach (var f in fitnesCentri)
                {
                    if (f.Naziv == fitnesC && f.Adresa == adresaC)
                    {
                        pronadjen.Add(f);
                        izmenjen = f;
                    }
                }

                fitnesCentri.Remove(izmenjen);
                izmenjen.CenaGrupnogTreningaSaPT = Double.Parse(cenaGTsaPT);
                fitnesCentri.Add(izmenjen);
                Data.IzmenaFitnesCentara(fitnesCentri);
                return View("IzmenaFitnesCentra", pronadjen);
            }

            foreach (var f in fitnesCentri)
            {
                if (f.Naziv == fitnesC && f.Adresa == adresaC)
                {
                    pronadjen.Add(f);
                }
            }

            TempData["poruka"] = "Polje cena grupnog treninga sa personalnim trenerom ne sme biti prazno!";
            return View("IzmenaFitnesCentra", pronadjen);

        }

        [HttpPost]
        public ActionResult Obrisi(string nazivFC, string adresa)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
           
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> predstojeciTr = new List<GrupniTrening>();
            foreach(GrupniTrening g in grupniTreninzi)
            {
                if(g.FitnesCentar.Naziv== nazivFC && g.FitnesCentar.Adresa == adresa && g.VremeTreninga>DateTime.Now)
                {
                    predstojeciTr.Add(g);
                }
            }

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnescentri"];
            FitnesCentar obrisati = new FitnesCentar();

            if (predstojeciTr.Count == 0)
            {
                foreach (var f in fitnesCentri)
                {
                    if (f.Naziv == nazivFC && f.Adresa == adresa)
                    {
                        obrisati = f;

                    }
                }

                fitnesCentri.Remove(obrisati);
                obrisati.Obrisan = true;
                fitnesCentri.Add(obrisati);
                Data.IzmenaFitnesCentara(fitnesCentri);

                List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
                foreach(Korisnik k in korisnici)
                {
                    if (k.KTrener.Naziv == nazivFC && k.KTrener.Adresa==adresa)
                    {
                        k.Blokiran = true;
                    }
                }
               
                Data.IzmeniKorisnika(korisnici);

            }
            else
            {
                TempData["poruka"] = "Ne mozete obrisati fitnes centar jer postoje treninzi koji se jos nisu odrzali!";
                return View("Index");

            }

            TempData["poruka"] = "Uspesno ste obrisali fitnes centar!";
            return View("Index");


        }


        public ActionResult Blokiranje(string nazivFC, string adresa)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            List<Korisnik> spisakTrenera = new List<Korisnik>();

            foreach(Korisnik k in korisnici)
            {
                if(k.KTrener.Naziv==nazivFC && k.KTrener.Adresa == adresa && k.Blokiran==false)
                {
                    spisakTrenera.Add(k);
                }
            }
            return View(spisakTrenera);
        }

        [HttpPost]
        public ActionResult BlokirajTrenera(string korisnickoIme)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik pronadjen = new Korisnik();

            foreach(Korisnik k in korisnici)
            {
                if (k.KorisnickoIme == korisnickoIme)
                {
                    pronadjen = k;
                }
            }

            pronadjen.Blokiran = true;
            Data.IzmeniKorisnika(korisnici);
            TempData["poruka"] = $"Korisnik {korisnickoIme} je uspjesno blokiran!";
            return View("Index");
        }

        public ActionResult Komentari(string fitnesCentar, string adresa, string tekstKomentara, string ocena)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Komentar> komentariZaVlasnika = (List<Komentar>)HttpContext.Application["komentariZaVlasnika"];
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            int id = komentari.Count + komentariZaVlasnika.Count + 1;

            if ((String.IsNullOrEmpty(tekstKomentara) && String.IsNullOrEmpty(ocena)) || (String.IsNullOrEmpty(tekstKomentara) && !String.IsNullOrEmpty(ocena)) || (!String.IsNullOrEmpty(tekstKomentara) && String.IsNullOrEmpty(ocena)) || float.Parse(ocena)<1 || float.Parse(ocena)>5)
            {
                TempData["poruka1"] = $"Da biste ostavili komentar neophodno je ispravno popuniti sva polja!";
                return View("Poruka");
            }
            else
            {
                Komentar k = new Komentar(new Korisnik(korisnik.KorisnickoIme), new FitnesCentar(fitnesCentar, adresa), tekstKomentara, float.Parse(ocena), id);
                Data.SacuvajKomentarZaVlasnika(k);
                komentariZaVlasnika.Add(k);
                TempData["poruka"] = $" {korisnik.KorisnickoIme} - Hvala na recenziji!";
                return View("Poruka");
            }
           
        }

        public ActionResult MojiKomentari(string fitnesCentar, string adresa)
        {
            List<Komentar> komentariZaVlasnika = (List<Komentar>)HttpContext.Application["komentariZaVlasnika"];
            List<Komentar> mojiKomentari = new List<Komentar>();
            foreach(Komentar k in komentariZaVlasnika)
            {
                if(k.FitnesCentar.Naziv==fitnesCentar && k.FitnesCentar.Adresa == adresa)
                {
                    mojiKomentari.Add(k);
                }
            }
            return View(mojiKomentari);
        }

       
        public ActionResult OdobriKomentar(string idKomentara)
        {
            List<Komentar> komentariZaVlasnika = (List<Komentar>)HttpContext.Application["komentariZaVlasnika"];
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            Komentar kom = new Komentar();
            foreach(Komentar k in komentariZaVlasnika)
            {
                if (k.Id==int.Parse(idKomentara))
                {
                    kom = k;
                }
            }
               
            komentari.Add(kom);
            Data.IzmeniKomentare(komentari);
            komentariZaVlasnika.Remove(kom);
            Data.IzmeniKomentareZaVlasnika(komentariZaVlasnika);
            TempData["poruka2"] = "Komentar odobren!";
            return View("Poruka");
        }

       
        public ActionResult OdbijKomentar(string idKomentara)
        {
            TempData["poruka2"] = "Komentar odbijen!";
            return View("Poruka");
        }




    }
}