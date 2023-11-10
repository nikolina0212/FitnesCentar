using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class PosetilacController : Controller
    {
        // GET: Posetilac
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrijavaNaTrening(string naziv)
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Naziv.Equals(naziv))
                {
                     if (!g.SpisakPosetilaca.Contains(korisnik.KorisnickoIme))
                     {
                            if (g.MaxBrPosetilaca > g.SpisakPosetilaca.Count)
                            {
                                foreach (GrupniTrening gt in grupniTreninzi.ToList())
                                {
                                    if (gt.Naziv.Equals(naziv))
                                    {
                                        grupniTreninzi.Remove(gt);
                                    }

                                }

                                g.SpisakPosetilaca.Add(korisnik.KorisnickoIme);
                                grupniTreninzi.Add(g);
                                Data.SacuvajTrening(grupniTreninzi);
                                TempData["poruka"] = $"Uspesno ste se prijavili na trening {g.Naziv}!";
                                return View("Index");
                            }
                            else
                            {
                                TempData["poruka"] = $"Na grupni trening {g.Naziv} je prijavljen maksimalan broj posetilaca!";
                                return View("Index");

                            }
                            
                     }
                    else
                    {
                            TempData["poruka"] = $"Vec ste prijavljeni na trening {g.Naziv}!";
                            return View("Index");
                        
                    }

                }

            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult IstorijaTreninga()
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
               
                if(g.SpisakPosetilaca.Contains(korisnik.KorisnickoIme) && g.VremeTreninga<DateTime.Now && g.Obrisan == false)
                {
                    pronadjen.Add(g);
                }

            }

            return View(pronadjen);
        }

        public ActionResult Pretrazi(string naziv, string tipTreninga, string fitnesCentar)
        {

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.SpisakPosetilaca.Contains(korisnik.KorisnickoIme) && g.VremeTreninga < DateTime.Now && g.Obrisan == false)
                {
                    pronadjen.Add(g);
                }
            }

            List<GrupniTrening> pretrazeni = new List<GrupniTrening>();
            if (String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(tipTreninga) && String.IsNullOrEmpty(fitnesCentar))
            {
                TempData["poruka"] = $"Morate uneti parametar za pretragu!";
                return View("IstorijaTreninga", pronadjen);
            }
            else if (!String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(tipTreninga) && String.IsNullOrEmpty(fitnesCentar))
            {
                foreach (GrupniTrening g in pronadjen)
                {
                    if (g.Naziv.ToLower().Contains(naziv) || g.Naziv.ToUpper().Contains(naziv) || g.Naziv.Contains(naziv))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }
            else if (String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(tipTreninga) && String.IsNullOrEmpty(fitnesCentar))
            {

                foreach (GrupniTrening g in pronadjen)
                {
                    if (g.TipTreninga.ToLower().Contains(tipTreninga) || g.TipTreninga.ToUpper().Contains(tipTreninga) || g.TipTreninga.Contains(tipTreninga))
                    {
                        pretrazeni.Add(g);
                    }
                }

            }
            else if (String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(tipTreninga) && !String.IsNullOrEmpty(fitnesCentar))
            {
                foreach (GrupniTrening g in pronadjen)
                {
                    if (g.FitnesCentar.Naziv.ToLower().Contains(fitnesCentar) || g.FitnesCentar.Naziv.ToUpper().Contains(fitnesCentar) || g.FitnesCentar.Naziv.Contains(fitnesCentar))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }
            else if (!String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(tipTreninga) && !String.IsNullOrEmpty(fitnesCentar))
            {
                foreach (GrupniTrening g in pronadjen)
                {
                    if ((g.Naziv.ToLower().Contains(naziv) || g.Naziv.ToUpper().Contains(naziv) || g.Naziv.Contains(naziv)) && (g.FitnesCentar.Naziv.ToLower().Contains(fitnesCentar) || g.FitnesCentar.Naziv.ToUpper().Contains(fitnesCentar) || g.FitnesCentar.Naziv.Contains(fitnesCentar)))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }
            else if (String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(tipTreninga) && !String.IsNullOrEmpty(fitnesCentar))
            {
                foreach (GrupniTrening g in pronadjen)
                {
                    if ((g.TipTreninga.ToLower().Contains(tipTreninga) || g.TipTreninga.ToUpper().Contains(tipTreninga) || g.TipTreninga.Contains(tipTreninga)) && (g.FitnesCentar.Naziv.ToLower().Contains(fitnesCentar) || g.FitnesCentar.Naziv.ToUpper().Contains(fitnesCentar) || g.FitnesCentar.Naziv.Contains(fitnesCentar)))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }
            else if (!String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(tipTreninga) && String.IsNullOrEmpty(fitnesCentar))
            {
                foreach (GrupniTrening g in pronadjen)
                {
                    if ((g.Naziv.ToLower().Contains(naziv) || g.Naziv.ToUpper().Contains(naziv) || g.Naziv.Contains(naziv)) && (g.TipTreninga.ToLower().Contains(tipTreninga) || g.TipTreninga.ToUpper().Contains(tipTreninga) || g.TipTreninga.Contains(tipTreninga)))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }
            else if (!String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(tipTreninga) && !String.IsNullOrEmpty(fitnesCentar))
            {
                foreach (GrupniTrening g in pronadjen)
                {
                    if ((g.Naziv.ToLower().Contains(naziv) || g.Naziv.ToUpper().Contains(naziv) || g.Naziv.Contains(naziv)) && (g.TipTreninga.ToLower().Contains(tipTreninga) || g.TipTreninga.ToUpper().Contains(tipTreninga) || g.TipTreninga.Contains(tipTreninga)) && (g.FitnesCentar.Naziv.ToLower().Contains(fitnesCentar) || g.FitnesCentar.Naziv.ToUpper().Contains(fitnesCentar) || g.FitnesCentar.Naziv.Contains(fitnesCentar)))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }

            return View("IstorijaTreninga", pretrazeni);


        }

        public ActionResult SortirajPoNazivu(string nacin)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.SpisakPosetilaca.Contains(korisnik.KorisnickoIme) && g.VremeTreninga < DateTime.Now && g.Obrisan == false)
                {
                    pronadjen.Add(g);
                }
            }
            List<GrupniTrening> sortirani = new List<GrupniTrening>();
            foreach (GrupniTrening f in pronadjen)
            {
                if (nacin == "Opadajuce")
                {
                    pronadjen.Sort((x, y) => y.Naziv.CompareTo(x.Naziv));
                    sortirani = pronadjen;
                }
                else if (nacin == "Rastuce")
                {
                    pronadjen.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));
                    sortirani = pronadjen;
                }
                else
                {
                    return View("Index");
                }

            }
            return View("IstorijaTreninga", sortirani);
        }

        public ActionResult SortirajPoTipuTreninga(string nacin)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.SpisakPosetilaca.Contains(korisnik.KorisnickoIme) && g.VremeTreninga < DateTime.Now && g.Obrisan == false)
                {
                    pronadjen.Add(g);
                }
            }
           
            List<GrupniTrening> sortirani = new List<GrupniTrening>();
            foreach (GrupniTrening f in pronadjen)
            {
                if (nacin == "Opadajuce")
                {
                    pronadjen.Sort((x, y) => y.TipTreninga.CompareTo(x.TipTreninga));
                    sortirani = pronadjen;
                }
                else if (nacin == "Rastuce")
                {
                    pronadjen.Sort((x, y) => x.TipTreninga.CompareTo(y.TipTreninga));
                    sortirani = pronadjen;
                }
                else
                {
                    return View("Index");
                }

            }
            return View("IstorijaTreninga", sortirani);
        }

        public ActionResult SortirajPoDatumuIVremenu(string nacin)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.SpisakPosetilaca.Contains(korisnik.KorisnickoIme) && g.VremeTreninga < DateTime.Now && g.Obrisan == false)
                {
                    pronadjen.Add(g);
                }
            }
            List<GrupniTrening> sortirani = new List<GrupniTrening>();
            foreach (GrupniTrening f in pronadjen)
            {
                if (nacin == "Opadajuce")
                {
                    pronadjen.Sort((x, y) => y.VremeTreninga.CompareTo(x.VremeTreninga));
                    sortirani = pronadjen;
                }
                else if (nacin == "Rastuce")
                {
                    pronadjen.Sort((x, y) => x.VremeTreninga.CompareTo(y.VremeTreninga));
                    sortirani = pronadjen;
                }
                else
                {
                    return View("Index");
                }

            }
            return View("IstorijaTreninga", sortirani);
        }

        public ActionResult Komentar(string fitnesCentar, string adresa)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> treninziKorisnika = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.SpisakPosetilaca.Contains(korisnik.KorisnickoIme) && g.VremeTreninga < DateTime.Now && g.Obrisan == false)
                {
                    treninziKorisnika.Add(g);
                }
            }

            korisnik.TreninziPosetioca = treninziKorisnika;
            foreach (GrupniTrening g  in treninziKorisnika)
            {
                if (g.FitnesCentar.Naziv==fitnesCentar && g.FitnesCentar.Adresa==adresa)
                {
                    ViewBag.FitnesCentar = fitnesCentar;
                    ViewBag.Adresa = adresa;
                    return View();
                }
               
                
            }
            TempData["poruka"] = $"Komentarisanje nije moguce! Nikada niste bili prijavljeni na trening u ovom fitnes centru!";
            return View("Index");


        }


    }
}
           


