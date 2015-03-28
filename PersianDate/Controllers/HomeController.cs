using PersianDate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersianDate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string persianDateFrom, DateTime? gregorianDateFrom)
        {

            DateConvert model = new DateConvert();

            if (!string.IsNullOrEmpty(persianDateFrom))
            {
                model.gregorianDateConverted = (PersianDateTime.Parse(persianDateFrom)).DateTime;
            }
            if (gregorianDateFrom != null)
            {
                model.persianDateConverted = new PersianDateTime((DateTime)gregorianDateFrom);
            }

            return View(model);
        }
    }
}