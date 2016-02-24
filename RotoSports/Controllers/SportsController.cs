using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RotoSports.Controllers
{
    public class SportsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}