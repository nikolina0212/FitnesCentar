using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            List<FitnesCentar> fitnesCentri = Data.UcitajFitnesCentre("~/App_Data/FitnesCentri.txt");
            HttpContext.Current.Application["fitnesCentri"] = fitnesCentri;
            List<GrupniTrening> grupniTreninzi = Data.UcitajTreninge("~/App_Data/GrupniTreninzi.txt");
            HttpContext.Current.Application["grupniTreninzi"] = grupniTreninzi;
            List<Komentar> komentari = Data.UcitajKomentare("~/App_Data/Komentari.txt");
            HttpContext.Current.Application["komentari"] = komentari;
            List<Korisnik> korisnici = Data.UcitajKorisnike("~/App_Data/Korisnici.txt");
            HttpContext.Current.Application["korisnici"] = korisnici;
            List<Komentar> komentariZaVlasnika = Data.UcitajKomentare("~/App_Data/KomentariZaVlasnika.txt");
            HttpContext.Current.Application["komentariZaVlasnika"] = komentariZaVlasnika;
        }
    }
}
