using Coypu;
using Coypu.Drivers.Selenium;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Threading;
using System.Linq;

namespace NaSpacerDo.Tests.View
{
    public abstract class TestBase
    {
        public BrowserSession Browser { get; private set; }

        private const string TEMPMAILADDRESS = "https://temp-mail.org";

        [SetUp]
        public virtual void SetUp()
        {
            Browser = GetBrowser();
            Browser.Driver.ResizeTo(new Size(1920, 1280), Browser);
        }

        private BrowserSession GetBrowser()
        {
            var sessionConfiguration = new SessionConfiguration
            {
                AppHost = ConfigurationManager.AppSettings["host"].ToString(),
                Port = int.Parse(ConfigurationManager.AppSettings["port"].ToString()),
                WaitBeforeClick = new TimeSpan(0, 0, 0, 0, 500)
            };
            sessionConfiguration.Driver = typeof(SeleniumWebDriver);

            bool isPhantomJS = ConfigurationManager.AppSettings["browser"].ToString() == "phantom";
            if (isPhantomJS)
            {
                sessionConfiguration.Browser = Coypu.Drivers.Browser.PhantomJS;
            }
            else
            {
                sessionConfiguration.Browser = Coypu.Drivers.Browser.Chrome;
            }

            return new BrowserSession(sessionConfiguration);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Browser.Dispose();
        }

        protected void AddCompany(string name, string email = null, string phone = null, string www = null, string shortDescription = null,
            string city = null, string street = null, string addressNumber = null, string zipCode = null, string logoPath = null)
        {
            Browser.Visit("Company/Add");

            FillCompanyForm(name, email, phone, www, shortDescription, city, street, addressNumber, zipCode, logoPath);

            Browser.FindId("save_company").Click();
        }

        protected void FillCompanyForm(string name, string email, string phone, string www, string shortDescription, string city, string street, string addressNumber, string zipCode, string logoPath)
        {
            if (name != null)
            {
                Browser.FillIn("company_name").With(name);
            }
            if (email != null)
            {
                Browser.FillIn("company_email").With(email);
            }
            Browser.FillIn("company_phone").With(phone);
            Browser.FillIn("company_www").With(www);
            Browser.FillIn("company_short_description").With(shortDescription);
            Browser.FillIn("company_address_city").With(city);
            Browser.FillIn("company_address_street").With(street);
            Browser.FillIn("company_address_number").With(addressNumber);
            Browser.FillIn("company_address_zip_code").With(zipCode);
            if (logoPath != null)
            {
                Browser.FillIn("company_logo_path", new Options { ConsiderInvisibleElements = true, }).With(logoPath);
            }
        }

        protected void Register(string email, string password)
        {
            Browser.Visit("Account/Register");

            Browser.FillIn("email").With(email);
            Browser.FillIn("password").With(password);
            Browser.FillIn("password_confirm").With(password);

            Browser.ClickButton("register_btn");
        }

        protected string RegisterAndConfirm(string password)
        {
            using (BrowserSession browser = GetBrowser())
            {
                browser.Visit(TEMPMAILADDRESS);

                string mail = browser.FindId("mail").Value;
                Register(mail, password);
                string confirmationMailXpath = "id('mails')/tbody/tr/td[2]/a";
                WaitForElement(browser, confirmationMailXpath);
                browser.FindXPath(confirmationMailXpath).Click();
                WaitForLink(browser, "link");
                browser.ClickLink("link");
                return mail;
            }
        }

        private void WaitForElement(BrowserSession browser, string xpath)
        {
            int maxAttempts = 30;
            int i = 0;
            while (!browser.FindXPath(xpath).Exists())
            {
                Thread.Sleep(5000);
                i++;
                if (i > maxAttempts)
                {
                    throw new Exception("Nie znaleziono elementu");
                }
            }
        }

        private void WaitForLink(BrowserSession browser, string link)
        {
            int maxAttempts = 30;
            int i = 0;
            while (!browser.FindLink(link).Exists())
            {
                Thread.Sleep(5000);
                i++;
                if (i > maxAttempts)
                {
                    throw new Exception("Nie znaleziono linka");
                }
            }
        }

        protected void LogIn(string email, string password)
        {
            Browser.Visit("Account/Login");
            Browser.FillIn("email").With(email);
            Browser.FillIn("password").With(password);
            Browser.ClickButton("login_btn");
        }

        protected void LogOut()
        {
            Browser.FindId("logOffLink").Click();
        }

        public void WaitForAjax()
        {
            int activeRequests = GetActiveRequestsCount();
            while (activeRequests > 0)
            {
                Thread.Sleep(500);
                activeRequests = GetActiveRequestsCount();
            }
        }

        private int GetActiveRequestsCount()
        {
            return int.Parse(Browser.ExecuteScript("return $.active").ToString());
        }
    }
}
