using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class TrenerController : Controller
    {
        // GET: Trener
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dodaj(string naziv, string tipTreninga,string trajanjeTreninga, string datumIVreme, string maxPosetilaca)
        {
            if (String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(tipTreninga) && String.IsNullOrEmpty(trajanjeTreninga) && String.IsNullOrEmpty(datumIVreme) && String.IsNullOrEmpty(maxPosetilaca))
            {
                TempData["porukaaa"] = $"Za uspesno dodavanje grupnog treninga neophodno je ispravno popuniti sva polja!";
                return RedirectToAction("Index");
            }
            else if (String.IsNullOrEmpty(naziv))
            {
                TempData["poruka1"] = "Polje naziv je obavezno!";
                return RedirectToAction("Index");

            }
            else if (String.IsNullOrEmpty(tipTreninga))
            {
                TempData["poruka2"] = "Polje tip treninga je obavezno!";
                return RedirectToAction("Index");

            }
            else if (String.IsNullOrEmpty(trajanjeTreninga))
            {
                TempData["poruka5"] = "Polje trajanje treninga je obavezno!";
                return RedirectToAction("Index");
            }
            else if (String.IsNullOrEmpty(datumIVreme))
            {
                TempData["poruka6"] = "Polje datum i vreme je obavezno!";
                return RedirectToAction("Index");

            }
            else if (String.IsNullOrEmpty(maxPosetilaca))
            {
                TempData["poruka7"] = "Polje maksimalan broj posetilaca je obavezno!";
                return RedirectToAction("Index");

            }
            else
            {
                if (DateTime.Parse(datumIVreme) < DateTime.Now.AddDays(3))
                {
                    TempData["poruka"] = "Trening morate zakazati minimum 3 dana unapred!";
                    return View("Index");
                }

                Korisnik korisnik = (Korisnik)Session["korisnik"];
                List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
                foreach (GrupniTrening g in grupniTreninzi.ToList())
                {
                    if ((g.Naziv.ToLower().Equals(naziv) || g.Naziv.ToUpper().Equals(naziv) || g.Naziv.Equals(naziv)) && g.Obrisan==false)
                    {
                        TempData["poruka"] = $"Trening naziva: {naziv} vec postoji!";
                        return View("Index");
                    }

                }
      
                
                GrupniTrening gt = new GrupniTrening(naziv, tipTreninga, new FitnesCentar(korisnik.KTrener.Naziv, korisnik.KTrener.Adresa), int.Parse(trajanjeTreninga), DateTime.Parse(datumIVreme), int.Parse(maxPosetilaca), new List<string>(), new Korisnik(korisnik.KorisnickoIme), false);
                grupniTreninzi.Add(gt);
                
                Data.SacuvajTrening(grupniTreninzi);

                TempData["poruka"] = $"Trening uspesno dodat!";
                return View();


            }


        }

        public ActionResult PregledGrupnihTreninga()
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> treninziTrenera = new List<GrupniTrening>();

            foreach(GrupniTrening g in grupniTreninzi)
            {
                if (g.Trener.KorisnickoIme == korisnik.KorisnickoIme && g.Obrisan == false)
                {
                    treninziTrenera.Add(g);
                }
            }

            return View(treninziTrenera);
        }

        public ActionResult PregledZavrsenihTreninga()
        {

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> prosliTreninziTrenera = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Trener.KorisnickoIme == korisnik.KorisnickoIme && g.VremeTreninga < DateTime.Now && g.Obrisan == false)
                {
                    prosliTreninziTrenera.Add(g);
                }
            }
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            foreach (FitnesCentar f in fitnesCentri)
            {
                if (f.Naziv == korisnik.KTrener.Naziv && f.Adresa == korisnik.KTrener.Adresa && f.Obrisan==false)
                {
                    TempData["poruka1"] = f.GodinaOtvaranja;
                }
            }

            return View(prosliTreninziTrenera);

          
        }

        public ActionResult IzmenaTreninga(string naziv)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();
            foreach(GrupniTrening g in grupniTreninzi)
            {
                if (g.Naziv == naziv && g.Obrisan==false)
                {
                    pronadjen.Add(g);
                }
            }
            return View(pronadjen);
        }

        public ActionResult IzmeniTipTreninga(string tipTreninga, string naziv)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();
            GrupniTrening izmenjen = new GrupniTrening();
            if (!String.IsNullOrEmpty(tipTreninga))
            {
                foreach (GrupniTrening g in grupniTreninzi.ToList())
                {
                    if (g.Naziv == naziv)
                    {
                        pronadjen.Add(g);
                        izmenjen = g;
                       

                    }
                }

                grupniTreninzi.Remove(izmenjen);
                izmenjen.TipTreninga = tipTreninga;
                grupniTreninzi.Add(izmenjen);
                Data.SacuvajTrening(grupniTreninzi);
                return View("IzmenaTreninga", pronadjen);
            }
            
                foreach (GrupniTrening g in grupniTreninzi)
                {
                    if (g.Naziv == naziv)
                    {
                        pronadjen.Add(g);
                    }
                }



                TempData["poruka"] = $"Polje tip treninga ne sme biti prazno!";
                return View("IzmenaTreninga", pronadjen);

            

        }

        public ActionResult IzmeniNaziv(string naziv, string nazivTr)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();
            GrupniTrening izmenjen = new GrupniTrening();
            if (!String.IsNullOrEmpty(nazivTr))
            {
              
                foreach (GrupniTrening g in grupniTreninzi.ToList())
                {
                    if (g.Naziv == naziv)
                    {
                        pronadjen.Add(g);
                        izmenjen = g;


                    }
                }
                foreach (GrupniTrening g in grupniTreninzi.ToList())
                {
                    if ((g.Naziv.ToLower().Equals(nazivTr) || g.Naziv.ToUpper().Equals(nazivTr) || g.Naziv.Equals(nazivTr)) && g.Obrisan == false)
                    {
                        TempData["poruka"] = $"Trening naziva: {nazivTr} vec postoji!";
                        return View("IzmenaTreninga",pronadjen);
                    }

                }

                grupniTreninzi.Remove(izmenjen);
                izmenjen.Naziv=nazivTr;
                grupniTreninzi.Add(izmenjen);
                Data.SacuvajTrening(grupniTreninzi);
                return View("IzmenaTreninga", pronadjen);
            }

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Naziv == naziv)
                {
                    pronadjen.Add(g);
                }
            }



            TempData["poruka"] = $"Polje naziv ne sme biti prazno!";
            return View("IzmenaTreninga", pronadjen);
        }

        public ActionResult IzmeniTrajanjeTreninga(string naziv, string trajanjeTreninga)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();
            GrupniTrening izmenjen = new GrupniTrening();
            if (!String.IsNullOrEmpty(trajanjeTreninga))
            {
                foreach (GrupniTrening g in grupniTreninzi.ToList())
                {
                    if (g.Naziv == naziv)
                    {
                        pronadjen.Add(g);
                        izmenjen = g;


                    }
                }

                grupniTreninzi.Remove(izmenjen);
                izmenjen.TrajanjeTreninga=int.Parse(trajanjeTreninga);
                grupniTreninzi.Add(izmenjen);
                Data.SacuvajTrening(grupniTreninzi);
                return View("IzmenaTreninga", pronadjen);
            }

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Naziv == naziv)
                {
                    pronadjen.Add(g);
                }
            }



            TempData["poruka"] = $"Polje trajanje treninga ne sme biti prazno!";
            return View("IzmenaTreninga", pronadjen);
        }

        public ActionResult IzmeniDatumIVreme(string naziv, string datumVreme)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();
            GrupniTrening izmenjen = new GrupniTrening();
            if (!String.IsNullOrEmpty(datumVreme))
            {
                foreach (GrupniTrening g in grupniTreninzi.ToList())
                {
                    if (g.Naziv == naziv)
                    {
                        pronadjen.Add(g);
                        izmenjen = g;


                    }
                }

                if (DateTime.Parse(datumVreme) < DateTime.Now.AddDays(3))
                {
                    TempData["poruka"] = "Trening morate zakazati minimum 3 dana unapred!";
                    return View("IzmenaTreninga", pronadjen);
                }

                grupniTreninzi.Remove(izmenjen);
                izmenjen.VremeTreninga = DateTime.Parse(datumVreme);
                grupniTreninzi.Add(izmenjen);
                Data.SacuvajTrening(grupniTreninzi);
                return View("IzmenaTreninga", pronadjen);
            }

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Naziv == naziv)
                {
                    pronadjen.Add(g);
                }
            }



            TempData["poruka"] = $"Polje datum i vreme treninga ne sme biti prazno!";
            return View("IzmenaTreninga", pronadjen);
        }

        public ActionResult IzmeniMaksimalanBrojPosetilaca(string naziv, string maxBrPosetilaca)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> pronadjen = new List<GrupniTrening>();
            GrupniTrening izmenjen = new GrupniTrening();
            if (!String.IsNullOrEmpty(maxBrPosetilaca))
            {
                foreach (GrupniTrening g in grupniTreninzi.ToList())
                {
                    if (g.Naziv == naziv)
                    {
                        pronadjen.Add(g);
                        izmenjen = g;


                    }
                }

                grupniTreninzi.Remove(izmenjen);
                izmenjen.MaxBrPosetilaca = int.Parse(maxBrPosetilaca);
                grupniTreninzi.Add(izmenjen);
                Data.SacuvajTrening(grupniTreninzi);
                return View("IzmenaTreninga", pronadjen);
            }

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Naziv == naziv)
                {
                    pronadjen.Add(g);
                }
            }



            TempData["poruka"] = $"Polje maksimalan broj posetilaca treninga ne sme biti prazno!";
            return View("IzmenaTreninga", pronadjen);
        }

        public ActionResult Obrisi(string naziv)
        {

            Korisnik korisnik = (Korisnik)Session["korisnik"];

            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            GrupniTrening obrisati = new GrupniTrening();
            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Naziv == naziv && g.Obrisan==false)
                {
                    obrisati = g;

                }
            }
            if (obrisati.SpisakPosetilaca.Count == 0)
            {
                grupniTreninzi.Remove(obrisati);
                obrisati.Obrisan = true;
                grupniTreninzi.Add(obrisati);
                Data.SacuvajTrening(grupniTreninzi);
                TempData["poruka"] = $"Uspesno ste obrisali trening {naziv}";
                return View("Dodaj");
            }
            else
            {
                TempData["poruka"] = $"Brisanje treninga {naziv} nije moguce jer postoje posetioci prijavljeni na taj trening!";
                return View("Dodaj");
            }
            
        }

        public ActionResult PregledPosetilaca(string naziv)
        {
           
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Session["grupniTreninzi"] = grupniTreninzi;

            GrupniTrening grupniTrening = new GrupniTrening();
            foreach(GrupniTrening g in grupniTreninzi)
            {
                if (g.Naziv == naziv)
                {
                    grupniTrening = g;
                }
            }

           
            ViewBag.Message = grupniTrening;
            return View();
        }
      
        public ActionResult Pretrazi(string naziv, string tipTreninga, string minDatVreme, string maxDatVreme)
        {
            
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> prosliTreninziTrenera = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Trener.KorisnickoIme == korisnik.KorisnickoIme && g.VremeTreninga < DateTime.Now && g.Obrisan == false)
                {
                    prosliTreninziTrenera.Add(g);
                }
            }
            
           
            List<GrupniTrening> pretrazeni = new List<GrupniTrening>();
            if (String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(tipTreninga) && String.IsNullOrEmpty(minDatVreme) && String.IsNullOrEmpty(maxDatVreme))
            {
                TempData["poruka"] = $"Morate uneti parametar za pretragu!";
                return View("PregledZavrsenihTreninga", prosliTreninziTrenera);
            }
            else if (!String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(tipTreninga) && String.IsNullOrEmpty(minDatVreme) && String.IsNullOrEmpty(maxDatVreme))
            {
                foreach (GrupniTrening g in prosliTreninziTrenera)
                {
                    if (g.Naziv.ToLower().Contains(naziv) || g.Naziv.ToUpper().Contains(naziv) || g.Naziv.Contains(naziv))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }
            else if (String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(tipTreninga) && String.IsNullOrEmpty(minDatVreme) && String.IsNullOrEmpty(maxDatVreme))
            {

                foreach (GrupniTrening g in prosliTreninziTrenera)
                {
                     if (g.TipTreninga.ToLower().Contains(tipTreninga)|| g.TipTreninga.ToUpper().Contains(tipTreninga) || g.TipTreninga.Contains(tipTreninga))
                     {
                            pretrazeni.Add(g);
                     }
                }
               
            }
            else if (String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(tipTreninga) && !String.IsNullOrEmpty(minDatVreme) && !String.IsNullOrEmpty(maxDatVreme))
            {
                foreach (GrupniTrening g in prosliTreninziTrenera)
                {
                    if (g.VremeTreninga >= DateTime.Parse(minDatVreme) && g.VremeTreninga <= DateTime.Parse(maxDatVreme))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }
            else if (!String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(tipTreninga) && !String.IsNullOrEmpty(minDatVreme) && !String.IsNullOrEmpty(maxDatVreme))
            {
                foreach (GrupniTrening g in prosliTreninziTrenera)
                {
                    if ((g.Naziv.ToLower().Contains(naziv) || g.Naziv.ToUpper().Contains(naziv) || g.Naziv.Contains(naziv)) && (g.VremeTreninga >= DateTime.Parse(minDatVreme) && g.VremeTreninga <= DateTime.Parse(maxDatVreme)))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }
            else if (String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(tipTreninga) && !String.IsNullOrEmpty(minDatVreme) && !String.IsNullOrEmpty(maxDatVreme))
            {
                foreach (GrupniTrening g in prosliTreninziTrenera)
                {
                    if ((g.TipTreninga.ToLower().Contains(tipTreninga) || g.TipTreninga.ToUpper().Contains(tipTreninga) || g.TipTreninga.Contains(tipTreninga)) && (g.VremeTreninga >= DateTime.Parse(minDatVreme) && g.VremeTreninga <= DateTime.Parse(maxDatVreme)))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }
            else if (!String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(tipTreninga) && String.IsNullOrEmpty(minDatVreme) && String.IsNullOrEmpty(maxDatVreme))
            {
                foreach (GrupniTrening g in prosliTreninziTrenera)
                {
                    if ((g.Naziv.ToLower().Contains(naziv) || g.Naziv.ToUpper().Contains(naziv) || g.Naziv.Contains(naziv)) && (g.TipTreninga.ToLower().Contains(tipTreninga) || g.TipTreninga.ToUpper().Contains(tipTreninga) || g.TipTreninga.Contains(tipTreninga)))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }
            else if (!String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(tipTreninga) && !String.IsNullOrEmpty(minDatVreme) && !String.IsNullOrEmpty(maxDatVreme))
            {
                foreach (GrupniTrening g in prosliTreninziTrenera)
                {
                    if ((g.Naziv.ToLower().Contains(naziv) || g.Naziv.ToUpper().Contains(naziv) || g.Naziv.Contains(naziv)) && (g.TipTreninga.ToLower().Contains(tipTreninga) || g.TipTreninga.ToUpper().Contains(tipTreninga) || g.TipTreninga.Contains(tipTreninga)) && (g.VremeTreninga>= DateTime.Parse(minDatVreme) && g.VremeTreninga <= DateTime.Parse(maxDatVreme)))
                    {
                        pretrazeni.Add(g);
                    }
                }
            }

            return View("PregledZavrsenihTreninga", pretrazeni);


        }

        public ActionResult SortirajPoNazivu(string nacin)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> prosliTreninziTrenera = new List<GrupniTrening>();
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            foreach (FitnesCentar f in fitnesCentri)
            {
                if (f.Naziv == korisnik.KTrener.Naziv && f.Adresa == korisnik.KTrener.Adresa)
                {
                    TempData["poruka1"] = f.GodinaOtvaranja;
                }
            }
            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Trener.KorisnickoIme == korisnik.KorisnickoIme && g.VremeTreninga < DateTime.Now && g.Obrisan == false)
                {
                    prosliTreninziTrenera.Add(g);
                }
            }
            List<GrupniTrening> sortirani = new List<GrupniTrening>();
            foreach(GrupniTrening f in prosliTreninziTrenera)
            {
                if (nacin == "Opadajuce")
                {
                    prosliTreninziTrenera.Sort((x, y) => y.Naziv.CompareTo(x.Naziv));
                    sortirani = prosliTreninziTrenera;
                }
                else if (nacin == "Rastuce")
                {
                    prosliTreninziTrenera.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));
                    sortirani = prosliTreninziTrenera;
                }
                else
                {
                    return View("Index");
                }

            }
            return View("PregledZavrsenihTreninga", sortirani);
        }

        public ActionResult SortirajPoTipuTreninga(string nacin)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> prosliTreninziTrenera = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Trener.KorisnickoIme == korisnik.KorisnickoIme && g.VremeTreninga < DateTime.Now && g.Obrisan == false)
                {
                    prosliTreninziTrenera.Add(g);
                }
            }
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            foreach (FitnesCentar f in fitnesCentri)
            {
                if (f.Naziv == korisnik.KTrener.Naziv && f.Adresa == korisnik.KTrener.Adresa)
                {
                    TempData["poruka1"] = f.GodinaOtvaranja;
                }
            }
            List<GrupniTrening> sortirani = new List<GrupniTrening>();
            foreach (GrupniTrening f in prosliTreninziTrenera)
            {
                if (nacin == "Opadajuce")
                {
                    prosliTreninziTrenera.Sort((x, y) => y.TipTreninga.CompareTo(x.TipTreninga));
                    sortirani = prosliTreninziTrenera;
                }
                else if (nacin == "Rastuce")
                {
                    prosliTreninziTrenera.Sort((x, y) => x.TipTreninga.CompareTo(y.TipTreninga));
                    sortirani = prosliTreninziTrenera;
                }
                else
                {
                    return View("Index");
                }

            }
            return View("PregledZavrsenihTreninga", sortirani);
        }

        public ActionResult SortirajPoDatumuIVremenu(string nacin)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> prosliTreninziTrenera = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.Trener.KorisnickoIme == korisnik.KorisnickoIme && g.VremeTreninga < DateTime.Now && g.Obrisan == false)
                {
                    prosliTreninziTrenera.Add(g);
                }
            }
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            foreach (FitnesCentar f in fitnesCentri)
            {
                if (f.Naziv == korisnik.KTrener.Naziv && f.Adresa == korisnik.KTrener.Adresa)
                {
                    TempData["poruka1"] = f.GodinaOtvaranja;
                }
            }
            List<GrupniTrening> sortirani = new List<GrupniTrening>();
            foreach (GrupniTrening f in prosliTreninziTrenera)
            {
                if (nacin == "Opadajuce")
                {
                    prosliTreninziTrenera.Sort((x, y) => y.VremeTreninga.CompareTo(x.VremeTreninga));
                    sortirani = prosliTreninziTrenera;
                }
                else if (nacin == "Rastuce")
                {
                    prosliTreninziTrenera.Sort((x, y) => x.VremeTreninga.CompareTo(y.VremeTreninga));
                    sortirani = prosliTreninziTrenera;
                }
                else
                {
                    return View("Index");
                }

            }
            return View("PregledZavrsenihTreninga", sortirani);
        }


    }
}