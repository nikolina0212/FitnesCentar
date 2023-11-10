using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> listaCentara = new List<FitnesCentar>();
            foreach(FitnesCentar f in fitnesCentri.ToList())
            {
                if (f.Obrisan == false)
                {
                    listaCentara.Add(f);
                }
            }
            listaCentara.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));
            var sortirani = listaCentara;
            ViewBag.Prijavljen = korisnik;
            return View(sortirani);
        }

        [HttpPost]
        public ActionResult Pretrazi(string naziv, string adresa, string minGod, string maxGod)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            ViewBag.Prijavljen = korisnik;
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> listaCentara = new List<FitnesCentar>();
            foreach (FitnesCentar f in fitnesCentri.ToList())
            {
                if (f.Obrisan == false)
                {
                    listaCentara.Add(f);
                }
            }
            List<FitnesCentar> sortirani = new List<FitnesCentar>();
            listaCentara.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));
            sortirani = listaCentara;
            List<FitnesCentar> pretrazeni = new List<FitnesCentar>();
            if (String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(adresa) && String.IsNullOrEmpty(minGod) && String.IsNullOrEmpty(maxGod))
            {
                TempData["poruka"] = $"Morate uneti parametar za pretragu!";
                return View("Index", sortirani);
            }
            else if (!String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(adresa) && String.IsNullOrEmpty(minGod) && String.IsNullOrEmpty(maxGod))
            {
                foreach (FitnesCentar f in listaCentara)
                {
                    if (f.Naziv.ToLower().Contains(naziv) || f.Naziv.ToUpper().Contains(naziv) || f.Naziv.Contains(naziv))
                    {
                        pretrazeni.Add(f);
                    }
                }
            }
            else if (String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(adresa) && String.IsNullOrEmpty(minGod) && String.IsNullOrEmpty(maxGod))
            {

                foreach (FitnesCentar f in listaCentara)
                {
                    string[] arr = f.Adresa.Split(',');
                    if (adresa.Contains(arr[0]) || adresa.ToLower().Contains(arr[0]) || adresa.ToUpper().Contains(arr[0]) || f.Adresa.ToLower().Contains(adresa) || f.Adresa.ToUpper().Contains(adresa) || f.Adresa.Contains(adresa))
                    {
                        pretrazeni.Add(f);
                    }
                }
            }
            else if (String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(adresa) && !String.IsNullOrEmpty(minGod) && !String.IsNullOrEmpty(maxGod))
            {
                foreach (FitnesCentar f in listaCentara)
                {
                    if (f.GodinaOtvaranja >= int.Parse(minGod) && f.GodinaOtvaranja <= int.Parse(maxGod))
                    {
                        pretrazeni.Add(f);
                    }
                }
            }
            else if (!String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(adresa) && String.IsNullOrEmpty(minGod) && String.IsNullOrEmpty(maxGod))
            {
                foreach (FitnesCentar f in listaCentara)
                {
                    if ((f.Naziv.ToLower().Contains(naziv) || f.Naziv.ToUpper().Contains(naziv) || f.Naziv.Contains(naziv)) && (f.Adresa.ToLower().Contains(adresa) || f.Adresa.ToUpper().Contains(adresa) || f.Adresa.Contains(adresa)))
                    {
                        pretrazeni.Add(f);
                    }
                }
            }
            else if (!String.IsNullOrEmpty(naziv) && String.IsNullOrEmpty(adresa) && !String.IsNullOrEmpty(minGod) && !String.IsNullOrEmpty(maxGod))
            {
                foreach (FitnesCentar f in listaCentara)
                {
                    if ((f.Naziv.ToLower().Contains(naziv) || f.Naziv.ToUpper().Contains(naziv) || f.Naziv.Contains(naziv)) && (f.GodinaOtvaranja >= int.Parse(minGod) && f.GodinaOtvaranja <= int.Parse(maxGod)))
                    {
                        pretrazeni.Add(f);
                    }
                }
            }
            else if (String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(adresa) && !String.IsNullOrEmpty(minGod) && !String.IsNullOrEmpty(maxGod))
            {
                foreach (FitnesCentar f in listaCentara)
                {
                    if ((f.Adresa.ToLower().Contains(adresa) || f.Adresa.ToUpper().Contains(adresa) || f.Adresa.Contains(adresa)) && (f.GodinaOtvaranja >= int.Parse(minGod) && f.GodinaOtvaranja <= int.Parse(maxGod)))
                    {
                        pretrazeni.Add(f);
                    }
                }
            }
            else if (!String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(adresa) && !String.IsNullOrEmpty(minGod) && !String.IsNullOrEmpty(maxGod))
            {
                foreach (FitnesCentar f in listaCentara)
                {
                    if ((f.Naziv.ToLower().Contains(naziv) || f.Naziv.ToUpper().Contains(naziv)) || f.Naziv.Contains(naziv) || (f.Adresa.ToLower().Contains(adresa) || f.Adresa.ToUpper().Contains(adresa) || f.Adresa.Contains(adresa)) && (f.GodinaOtvaranja >= int.Parse(minGod) && f.GodinaOtvaranja <= int.Parse(maxGod)))
                    {
                        pretrazeni.Add(f);
                    }
                }
            }
            return View("Index", pretrazeni);
        }
       
        public ActionResult SortirajPoNazivu(string nacin)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            ViewBag.Prijavljen = korisnik;
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> listaCentara = new List<FitnesCentar>();
            foreach (var f in fitnesCentri.ToList())
            {
                if (f.Obrisan == false)
                {
                    listaCentara.Add(f);
                }
            }
            List<FitnesCentar> sortirani = new List<FitnesCentar>();
            foreach (FitnesCentar f in listaCentara)
            {
                if (nacin == "Opadajuce")
                {
                    listaCentara.Sort((x, y) => y.Naziv.CompareTo(x.Naziv));
                    sortirani = listaCentara;
                }
                else if (nacin == "Rastuce")
                {
                    listaCentara.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));
                    sortirani = listaCentara;
                }
                else
                {
                    return View("Index");
                }

            }
            return View("Index", sortirani);
        }

        public ActionResult SortirajPoAdresi(string nacin)
        {

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            ViewBag.Prijavljen = korisnik;
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> listaCentara = new List<FitnesCentar>();
            foreach (var f in fitnesCentri.ToList())
            {
                if (f.Obrisan == false)
                {
                    listaCentara.Add(f);
                }
            }
            List<FitnesCentar> sortirani = new List<FitnesCentar>();
            foreach (FitnesCentar f in listaCentara)
            {
                if (nacin == "Opadajuce")
                {
                    listaCentara.Sort((x, y) => y.Adresa.CompareTo(x.Adresa));
                    sortirani = listaCentara;
                }
                else if (nacin == "Rastuce")
                {
                    listaCentara.Sort((x, y) => x.Adresa.CompareTo(y.Adresa));
                    sortirani = listaCentara;
                }
                else
                {
                    return View("Index");
                }

            }
            return View("Index", sortirani);
        }

        public ActionResult SortirajPoGodini(string nacin)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            ViewBag.Prijavljen = korisnik;
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> listaCentara = new List<FitnesCentar>();
            foreach (var f in fitnesCentri.ToList())
            {
                if (f.Obrisan == false)
                {
                    listaCentara.Add(f);
                }
            }
            List<FitnesCentar> sortirani = new List<FitnesCentar>();
            foreach (FitnesCentar f in listaCentara)
            {
                if (nacin == "Opadajuce")
                {
                    listaCentara.Sort((x, y) => y.GodinaOtvaranja.CompareTo(x.GodinaOtvaranja));
                    sortirani = listaCentara;
                }
                else if (nacin == "Rastuce")
                {
                    listaCentara.Sort((x, y) => x.GodinaOtvaranja.CompareTo(y.GodinaOtvaranja));
                    sortirani = listaCentara;
                }
                else
                {
                    return View("Index");
                }

            }
            return View("Index", sortirani);
        }

        public ActionResult Detalji(string naziv, string adresa)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            ViewBag.Prijavljen = korisnik;
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> listaCentara = new List<FitnesCentar>();
            foreach (var f in fitnesCentri.ToList())
            {
                if (f.Obrisan == false)
                {
                    listaCentara.Add(f);
                }
            }
            List<FitnesCentar> pronadjen = new List<FitnesCentar>();
            foreach (FitnesCentar f in listaCentara)
            {
                if (!String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(adresa))
                {
                    if (f.Naziv.Contains(naziv) && f.Adresa.Contains(adresa))
                    {
                        pronadjen.Add(f);
                    }

                }

            }
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> treninzi = new List<GrupniTrening>();

            foreach (GrupniTrening g in grupniTreninzi)
            {
                if (g.FitnesCentar.Naziv == naziv && g.FitnesCentar.Adresa == adresa && g.FitnesCentar.Obrisan == false && g.VremeTreninga > DateTime.Now && g.Obrisan==false)
                {
                    treninzi.Add(g);


                }

                ViewBag.Treninzi = treninzi;
            }

            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            List<Komentar> kom = new List<Komentar>();

            foreach (Komentar k in komentari)
            {
                if (!String.IsNullOrEmpty(naziv) && !String.IsNullOrEmpty(adresa))
                {
                    if (k.FitnesCentar.Naziv.Contains(naziv) && k.FitnesCentar.Adresa.Contains(adresa) && k.FitnesCentar.Obrisan == false)
                    {
                        kom.Add(k);
                    }

                }
                ViewBag.Komentari = kom;

            }



            return View("Detalji", pronadjen);
        }


    }

}

