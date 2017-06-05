using log4net;
using NaSpacerDo.Enums;
using NaSpacerDo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaSpacerDo.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILog log) : base(log)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action for adding new object
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ChangeCurrentCulture(int id)
        {
            CultureHelper.Language = (Language)id;
            Session["CurrentCulture"] = id;

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}