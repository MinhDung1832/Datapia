using DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Datapia.DataAccess;
using Datapia.Models;

namespace Datapia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Overview(string wh_code)
        {
            if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
            {
                

                return View();
            }
            Common.SaveSession("Home", "Overview");
            return RedirectToAction("Login", "Account");
        }

        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}