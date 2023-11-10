using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class AutentifikacijaController : Controller
    {
        // GET: Autentifikacija
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registracija()
        {
            Korisnik korisnik = null;
            Session["korisnik"] = korisnik;
            return View(Session["korisnik"]);
        }

        [HttpPost]
        public ActionResult Registracija(Korisnik korisnik)
        {
            if (korisnik.KorisnickoIme == null && korisnik.Lozinka == null && korisnik.Ime == null && korisnik.Prezime == null && korisnik.Email == null && korisnik.DatumRodjenja == null)
            {
                TempData["poruka"] = $"Za uspesnu registraciju neophodno je ispravno popuniti sva polja!";
                return View();
            }
            else if (korisnik.KorisnickoIme == null)
            {
                TempData["poruka1"] = "Polje korisnicko ime je obavezno!";
                return View();

            }
            else if (korisnik.Lozinka == null)
            {
                TempData["poruka2"] = "Polje lozinka je obavezno!";
                return View();

            }
            else if (korisnik.Ime == null)
            {
                TempData["poruka3"] = "Polje ime je obavezno!";
                return View();

            }
            else if (korisnik.Prezime == null)
            {
                TempData["poruka4"] = "Polje prezime je obavezno!";
                return View();

            }
            else if (String.IsNullOrEmpty(korisnik.Pol.ToString()))
            {
                TempData["poruka5"] = "Polje pol je obavezno!";
                return View();

            }
            else if (korisnik.Email == null)
            {
                TempData["poruka6"] = "Polje email je obavezno!";
                return View();

            }
            else if (String.IsNullOrEmpty(korisnik.DatumRodjenja))
            {
                TempData["poruka7"] = "Polje datum rodjenja je obavezno!";
                return View();

            }
            else
            {
                List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
                foreach (Korisnik k in korisnici.ToList())
                {
                    if (k.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        TempData["poruka"] = $"Korisnik sa username-om: {korisnik.KorisnickoIme} vec postoji!";
                        return View();
                    }


                }
                korisnik.Uloga = Uloga.POSETILAC;
                korisnik.KTrener = new FitnesCentar("/", "/");
                korisnik.Prijavljen = false;
                korisnik.Blokiran = false;
                korisnici.Add(korisnik);
                Session["korisnik"] = korisnik;
                Data.SacuvajKorisnika(korisnik);


            }

            return RedirectToAction("Index", "Autentifikacija");


        }

        [HttpPost]
        public ActionResult Index(string korisnickoIme, string lozinka)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            if (!String.IsNullOrEmpty(korisnickoIme) && !String.IsNullOrEmpty(lozinka))
            {
                foreach (Korisnik k in korisnici)
                {
                    if (k.KorisnickoIme == korisnickoIme && k.Lozinka == lozinka && k.Blokiran==false)
                    {
                            
                          k.Prijavljen = true;
                          Session["korisnik"] = k;
                          Data.IzmeniKorisnika(korisnici);
                          return RedirectToAction("Index", "Home"); 
                          
                    }


                }
                TempData["poruka"] = $"Korisnik sa tim korisnickim imenom i lozinkom ne postoji. Proverite svoje podatke!";
                return View("Index");
            }
            TempData["poruka"] = $"Polja korisnicko ime i lozinka ne smeju biti prazna!";
            return View("Index");

        }

        public ActionResult Logout(Korisnik k)
        {
            Session["korisnik"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult IzmeniProfil()
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            ViewBag.Korisnik = korisnik;
            return View();
        }

        [HttpPost]
        public ActionResult IzmeniKorisnickoIme(string korisnickoIme)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            string staro = korisnik.KorisnickoIme;
            if (!String.IsNullOrEmpty(korisnickoIme))
            {
                if (korisnickoIme == staro)
                {
                    return RedirectToAction("IzmeniProfil", "Autentifikacija");
                }
                else
                {
                    List<Korisnik> postoje = new List<Korisnik>();
                    foreach (Korisnik k in korisnici)
                    {
                        if (korisnickoIme == k.KorisnickoIme)
                        {
                            postoje.Add(k);
                        }
                    }

                    if (postoje.Count != 0)
                    {
                        TempData["poruka"] = $"Korisnik sa korisnickim imenom {korisnickoIme} vec postoji. Pokusajte ponovo!";
                        return RedirectToAction("IzmeniProfil", "Autentifikacija");
                    }
                    else
                    {
                        Korisnik izmenjen = new Korisnik();
                        foreach (Korisnik k in korisnici)
                        {
                            if (k.KorisnickoIme == staro)
                            {
                                izmenjen = k;
                            }
                        }
                        izmenjen.KorisnickoIme = korisnickoIme;

                        korisnici.Remove(korisnik);
                        korisnici.Add(izmenjen);
                        Data.IzmeniKorisnika(korisnici);
                        Session["korisnik"] = izmenjen;

                        List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
                        foreach( var f in fitnesCentri)
                        {
                            if (f.Vlasnik.KorisnickoIme == staro && f.Obrisan==false)
                            {
                                fitnesCentri.Remove(f);
                                f.Vlasnik.KorisnickoIme = izmenjen.KorisnickoIme;
                                fitnesCentri.Add(f);
                                Data.IzmenaFitnesCentara(fitnesCentri);
                            }
                        }


                        List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
                        List<Komentar> kom = new List<Komentar>();
                        foreach(Komentar k in komentari.ToList())
                        {
                            if (k.Posetilac.KorisnickoIme == staro)
                            {
                                komentari.Remove(k);
                                k.Posetilac.KorisnickoIme = izmenjen.KorisnickoIme;
                                komentari.Add(k);
                                Data.IzmeniKomentare(komentari);
                            }
                        }


                        List<Komentar> komentariZaVlasnika = (List<Komentar>)HttpContext.Application["komentariZaVlasnika"];
                        foreach (Komentar k in komentariZaVlasnika.ToList())
                        {
                            if (k.Posetilac.KorisnickoIme == staro)
                            {
                                komentariZaVlasnika.Remove(k);
                                k.Posetilac.KorisnickoIme = izmenjen.KorisnickoIme;
                                komentariZaVlasnika.Add(k);
                                Data.IzmeniKomentareZaVlasnika(komentariZaVlasnika);
                            }
                        }

                       
                        List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
                        foreach (GrupniTrening g in grupniTreninzi.ToList())
                        {
                            if (g.Trener.KorisnickoIme==staro)
                            {
                                grupniTreninzi.Remove(g);
                                g.Trener.KorisnickoIme = izmenjen.KorisnickoIme;
                                grupniTreninzi.Add(g);
                                Data.SacuvajTrening(grupniTreninzi);
                            }
                        }

                      
                        foreach (GrupniTrening g in grupniTreninzi)
                        {
                             foreach(var k in g.SpisakPosetilaca.ToList())
                             {
                                if (g.SpisakPosetilaca.Contains(staro))
                                {
                                    g.SpisakPosetilaca.Remove(staro);
                                    g.SpisakPosetilaca.Add(izmenjen.KorisnickoIme);
                                    Data.SacuvajTrening(grupniTreninzi);
                                }
                             }
                                
                            
                        }

                        return RedirectToAction("IzmeniProfil", "Autentifikacija");

                    }
                }

            }
            TempData["poruka"] = $"Polje korisnicko ime ne sme biti prazno!";
            return RedirectToAction("IzmeniProfil", "Autentifikacija");

        }


        [HttpPost]
        public ActionResult IzmeniLozinku(string lozinka)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            if (!String.IsNullOrEmpty(lozinka))
            {
                Korisnik izmenjen = new Korisnik();
                foreach (Korisnik k in korisnici)
                {
                    if (k.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        izmenjen = k;
                    }
                }
                izmenjen.Lozinka = lozinka;
                korisnici.Remove(korisnik);
                korisnici.Add(izmenjen);
                Data.IzmeniKorisnika(korisnici);
                Session["korisnik"] = izmenjen;
                TempData["poruka"] = $"Uspesno ste promenili lozinku!";
                return RedirectToAction("IzmeniProfil", "Autentifikacija");
   
            }
            TempData["poruka"] = $"Polje lozinka ne sme biti prazno!";
            return RedirectToAction("IzmeniProfil", "Autentifikacija");

        }
        [HttpPost]
        public ActionResult IzmeniIme(string ime)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            if (!String.IsNullOrEmpty(ime))
            {
                Korisnik izmenjen = new Korisnik();
                foreach (Korisnik k in korisnici)
                {
                    if (k.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        izmenjen = k;
                    }
                }
                izmenjen.Ime = ime;
                korisnici.Remove(korisnik);
                korisnici.Add(izmenjen);
                Data.IzmeniKorisnika(korisnici);
                Session["korisnik"] = izmenjen;
                return RedirectToAction("IzmeniProfil", "Autentifikacija");
            }

            TempData["poruka"] = $"Polje ime ne sme biti prazno!";
            return RedirectToAction("IzmeniProfil", "Autentifikacija");

        }

        public ActionResult IzmeniPrezime(string prezime)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            if (!String.IsNullOrEmpty(prezime))
            {
                Korisnik izmenjen = new Korisnik();
                foreach (Korisnik k in korisnici)
                {
                    if (k.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        izmenjen = k;
                    }
                }
                izmenjen.Prezime = prezime;
                korisnici.Remove(korisnik);
                korisnici.Add(izmenjen);
                Data.IzmeniKorisnika(korisnici);
                Session["korisnik"] = izmenjen;
                return RedirectToAction("IzmeniProfil", "Autentifikacija");
            }

            TempData["poruka"] = $"Polje prezime ne sme biti prazno!";
            return RedirectToAction("IzmeniProfil", "Autentifikacija");

        }

        [HttpPost]
        public ActionResult IzmeniPol(string pol)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            if (!String.IsNullOrEmpty(pol))
            {
                Korisnik izmenjen = new Korisnik();
                foreach (Korisnik k in korisnici)
                {
                    if (k.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        izmenjen = k;
                    }
                }
                izmenjen.Pol = pol;
                korisnici.Remove(korisnik);
                korisnici.Add(izmenjen);
                Data.IzmeniKorisnika(korisnici);
                Session["korisnik"] = izmenjen;
                return RedirectToAction("IzmeniProfil", "Autentifikacija");

            }

            TempData["poruka"] = $"Polje pol ne sme biti prazno!";
            return RedirectToAction("IzmeniProfil", "Autentifikacija");

        }
        [HttpPost]
        public ActionResult IzmeniEmail(string email)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            if (!String.IsNullOrEmpty(email))
            {
                Korisnik izmenjen = new Korisnik();
                foreach (Korisnik k in korisnici)
                {
                    if (k.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        izmenjen = k;
                    }
                }
                izmenjen.Email = email;
                korisnici.Remove(korisnik);
                korisnici.Add(izmenjen);
                Data.IzmeniKorisnika(korisnici);
                Session["korisnik"] = izmenjen;
                return RedirectToAction("IzmeniProfil", "Autentifikacija");
            }
            TempData["poruka"] = $"Polje email ne sme biti prazno!";
            return RedirectToAction("IzmeniProfil", "Autentifikacija");

        }

        [HttpPost]
        public ActionResult IzmeniDatumRodjenja(string datumRodjenja)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            if (!String.IsNullOrEmpty(datumRodjenja))
            {
                Korisnik izmenjen = new Korisnik();
                foreach (Korisnik k in korisnici)
                {
                    if (k.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        izmenjen = k;
                    }
                }
                izmenjen.DatumRodjenja = datumRodjenja;
                korisnici.Remove(korisnik);
                korisnici.Add(izmenjen);
                Data.IzmeniKorisnika(korisnici);
                Session["korisnik"] = izmenjen;
                return RedirectToAction("IzmeniProfil", "Autentifikacija");
            }

            TempData["poruka"] = $"Polje datum rodjenja ne sme biti prazno!";
            return RedirectToAction("IzmeniProfil", "Autentifikacija");

        }
        public ActionResult Reset()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
