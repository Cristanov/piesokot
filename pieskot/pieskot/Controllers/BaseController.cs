using log4net;
using NaSpacerDo.App_Start;
using NaSpacerDo.Enums;
using NaSpacerDo.Helpers;
using System;
using System.Web.Mvc;

namespace NaSpacerDo.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILog log;

        public BaseController(ILog log)
        {
            this.log = log;
        }

        protected override void ExecuteCore()
        {
            Language language = Language.Polish;
            if (Session == null || Session["CurrentCulture"] == null)
            {
                Enum.TryParse(System.Configuration.ConfigurationManager.AppSettings["LanguageId"], out language);
                Session["CurrentCulture"] = language;
            }
            else
            {
                language = (Language)Session["CurrentCulture"];
            }

            CultureHelper.Language = (Language)language;

            base.ExecuteCore();
        }

        protected override bool DisableAsyncSupport
        {
            get { return false; }
        }

        // GET: Base
        public void Error(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Error(message);
        }

        public void Error(string message)
        {
            Alert(message, Constants.Alerts.Error);
        }

        public void Warning(string message)
        {
            Alert(message, Constants.Alerts.Warning);
        }

        public void Success(string message)
        {
            Alert(message, Constants.Alerts.Success);
        }

        public void Info(string message)
        {
            Alert(message, Constants.Alerts.Info);
        }

        private void Alert(string message, Constants.Alerts type)
        {
            string key = type.ToString();
            if (!TempData.Keys.Contains(key))
                TempData.Add(key, "• " + message);
            else
                TempData[key] = TempData[key].ToString() + "<br />• " + message;
        }
    }
}