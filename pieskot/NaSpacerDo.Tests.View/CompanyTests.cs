using NUnit.Framework;
using Coypu;
using Coypu.Drivers.Selenium;
using System.Threading;
using System.Configuration;
using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace NaSpacerDo.Tests.View
{
    [TestFixture]
    public class CompanyTests : TestBase
    {
        [Test]
        public void CanAddNewCompany()
        {
            string password = "1234qwer";
            string mail = RegisterAndConfirm(password);
            LogIn(mail, password);

            string companyName = $"test name {Guid.NewGuid()}";
            string email = mail;
            string phone = "123123123";
            string www = "www.test.pl";
            string shortDescription = "short description short description short description";
            string city = "Some City";
            string street = "Some street";
            string addressNumber = "55";
            string zipCode = "12-123";
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = Path.Combine(folderPath, "TestFiles/logo1.png");

            AddCompany(companyName, email, phone, www, shortDescription, city, street, addressNumber, zipCode, logoPath);

            Assert.IsTrue(Browser.HasContent(companyName), "company name");
            Assert.IsTrue(Browser.FindId("company_logo").Exists(), "company logo");
            Assert.IsTrue(Browser.HasContent(email), "company email");
            Assert.IsTrue(Browser.HasContent(phone), "company phone");
            Assert.IsTrue(Browser.HasContent(www), "company www");
            Assert.IsTrue(Browser.HasContent(shortDescription), "company short description");
            Assert.IsTrue(Browser.HasContent(city), "company city");
            Assert.IsTrue(Browser.HasContent(street), "company street");
            Assert.IsTrue(Browser.HasContent(addressNumber), "company address number");
            Assert.IsTrue(Browser.HasContent(zipCode), "company zip code");
        }

        [Test]
        public void CanEditCompany()
        {
            string password = "1234qwer";
            string mail = RegisterAndConfirm(password);
            LogIn(mail, password);

            string companyName = $"test name 2 {Guid.NewGuid()}";
            string email = mail;
            string phone = "123123123";
            string www = "www.test.pl";
            string shortDescription = "short description short description short description";
            string city = "Some City";
            string street = "Some street";
            string addressNumber = "55";
            string zipCode = "12-123";
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = Path.Combine(folderPath, "TestFiles/logo1.png");

            AddCompany(companyName, email, phone, www, shortDescription, city, street, addressNumber, zipCode, logoPath);

            Browser.FindId("edit_company").Click();

            string editCompanyName = $"edited test name 2 {Guid.NewGuid()}";
            string editEmail = "test@editedtest.pl";
            string editPhone = "453453434";
            string editWww = "www.editedtest.pl";
            string editShortDescription = "edited short description short description short description";
            string editCity = "edited Some City";
            string editStreet = "edited Some street";
            string editAddressNumber = "5555";
            string editZipCode = "44-345";
            string editLogoPath = Path.Combine(folderPath, "TestFiles/logo2.png");

            FillCompanyForm(editCompanyName, editEmail, editPhone, editWww, editShortDescription, editCity, editStreet, editAddressNumber, editZipCode, editLogoPath);
            Browser.FindId("save_company").Click();

            Assert.IsTrue(Browser.HasContent(editCompanyName), "company name");
            Assert.IsTrue(Browser.FindId("company_logo").Exists(), "company logo");
            Assert.IsTrue(Browser.HasContent(editEmail), "company email");
            Assert.IsTrue(Browser.HasContent(editEmail), "company phone");
            Assert.IsTrue(Browser.HasContent(editWww), "company www");
            Assert.IsTrue(Browser.HasContent(editShortDescription), "company short description");
            Assert.IsTrue(Browser.HasContent(editCity), "company city");
            Assert.IsTrue(Browser.HasContent(editStreet), "company street");
            Assert.IsTrue(Browser.HasContent(editAddressNumber), "company address number");
            Assert.IsTrue(Browser.HasContent(editZipCode), "company zip code");
        }

        [Test]
        public void CanAddCompanyWithoutLogoAndAddLogoInEdit()
        {
            string password = "1234qwer";
            string mail = RegisterAndConfirm(password);
            LogIn(mail, password);

            string companyName = $"test name 2 {Guid.NewGuid()}";
            string email = mail;
            string phone = "123123123";
            string www = "www.test.pl";
            string shortDescription = "short description short description short description";
            string city = "Some City";
            string street = "Some street";
            string addressNumber = "55";
            string zipCode = "12-123";
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = Path.Combine(folderPath, "TestFiles/logo1.png");

            AddCompany(companyName, email, phone, www, shortDescription, city, street, addressNumber, zipCode, null);

            Browser.FindId("edit_company").Click();

            Browser.FillIn("company_logo_path", new Options { ConsiderInvisibleElements = true, }).With(logoPath);

            Browser.FindId("save_company").Click();

            Assert.IsTrue(Browser.HasContent(companyName), "company name");
            Assert.IsTrue(Browser.FindId("company_logo").Exists(), "company logo");
        }

        [Test]
        public void EditCompany_NotEditLogoImage_LogoStillExist()
        {
            string password = "1234qwer";
            string mail = RegisterAndConfirm(password);
            LogIn(mail, password);

            string companyName = $"test name {Guid.NewGuid()}";
            string email = mail;
            string phone = "123123123";
            string www = "www.test.pl";
            string shortDescription = "short description short description short description";
            string city = "Some City";
            string street = "Some street";
            string addressNumber = "55";
            string zipCode = "12-123";
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = Path.Combine(folderPath, "TestFiles/logo1.png");

            AddCompany(companyName, email, phone, www, shortDescription, city, street, addressNumber, zipCode, logoPath);

            Browser.FindId("edit_company").Click();

            string editCompanyName = $"edited test name 2 {Guid.NewGuid()}";
            string editEmail = "test@editedtest.pl";
            string editPhone = "453453434";
            string editWww = "www.editedtest.pl";
            string editShortDescription = "edited short description short description short description";
            string editCity = "edited Some City";
            string editStreet = "edited Some street";
            string editAddressNumber = "5555";
            string editZipCode = "44-345";

            FillCompanyForm(editCompanyName, editEmail, editPhone, editWww, editShortDescription, editCity, editStreet, editAddressNumber, editZipCode, null);
            Browser.FindId("save_company").Click();

            Assert.IsTrue(Browser.FindId("company_logo").Exists(), "company logo");
        }

        [Test]
        public void AddCompany_WithExistingName_MessageAboutInvalidName()
        {
            string password = "1234qwer";
            string mail = RegisterAndConfirm(password);
            LogIn(mail, password);

            string companyName = $"test name {Guid.NewGuid()}";
            string email = mail;
            string phone = "123123123";
            string www = "www.test.pl";
            string shortDescription = "short description short description short description";
            string city = "Some City";
            string street = "Some street";
            string addressNumber = "55";
            string zipCode = "12-123";
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = Path.Combine(folderPath, "TestFiles/logo1.png");

            // dodajemy prawidłowy obiekt aby mieć pewność że jakiś jest w bazie
            AddCompany(companyName, email, phone, www, shortDescription, city, street, addressNumber, zipCode, logoPath);

            Browser.Visit("Company/Add");
            FillCompanyForm(companyName, email, phone, www, shortDescription, city, street, addressNumber, zipCode, logoPath);

            Thread.Sleep(3000);

            Browser.FindId("save_company").Click();

            var errorMessages = Browser.FindAllCss("span.field-validation-error");
            Assert.That(errorMessages.Count(), Is.EqualTo(1));
            Assert.That(errorMessages.First().Text, Is.EqualTo("Obiekt o podanej nazwie już istnieje"));
        }

        [Test]
        public void AddCompany_WithTooShortName_MessageAboutInvalidName()
        {
            string password = "1234qwer";
            string mail = RegisterAndConfirm(password);
            LogIn(mail, password);

            Browser.Visit("Company/Add");

            string companyName = $"ab";
            string email = mail;
            string phone = "123123123";
            string www = "www.test.pl";
            string shortDescription = "short description short description short description";
            string city = "Some City";
            string street = "Some street";
            string addressNumber = "55";
            string zipCode = "12-123";
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = Path.Combine(folderPath, "TestFiles/logo1.png");

            FillCompanyForm(companyName, email, phone, www, shortDescription, city, street, addressNumber, zipCode, logoPath);

            Browser.FindId("save_company").Click();

            var errorMessages = Browser.FindAllCss("span.field-validation-error");
            Assert.That(errorMessages.Count(), Is.EqualTo(1));
            Assert.That(errorMessages.First().Text, Is.StringContaining("Nazwa powinna mieć przynajmiej "));
        }

        [Test]
        public void AddCompany_WithInvalidEmail_MessageAboutInvalidEmail()
        {
            string password = "1234qwer";
            string mail = RegisterAndConfirm(password);
            LogIn(mail, password);

            Browser.Visit("Company/Add");

            string companyName = $"test name {Guid.NewGuid()}";
            string email = "testtest.pl";
            string phone = "123123123";
            string www = "www.test.pl";
            string shortDescription = "short description short description short description";
            string city = "Some City";
            string street = "Some street";
            string addressNumber = "55";
            string zipCode = "12-123";
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = Path.Combine(folderPath, "TestFiles/logo1.png");

            FillCompanyForm(companyName, email, phone, www, shortDescription, city, street, addressNumber, zipCode, logoPath);

            Browser.FindId("save_company").Click();

            var errorMessages = Browser.FindAllCss("span.field-validation-error");
            Assert.That(errorMessages.Count(), Is.EqualTo(1));
            Assert.That(errorMessages.First().Text, Is.EqualTo("Podany adres e-mail jest nieprawidłowy"));
        }

        [Test]
        public void AddCompany_WithInvalidFileExtension_MessageAboutInvalidFileExtension()
        {
            string password = "1234qwer";
            string mail = RegisterAndConfirm(password);
            LogIn(mail, password);

            Browser.Visit("Company/Add");

            string companyName = $"test name {Guid.NewGuid()}";
            string email = mail;
            string phone = "123123123";
            string www = "www.test.pl";
            string shortDescription = "short description short description short description";
            string city = "Some City";
            string street = "Some street";
            string addressNumber = "55";
            string zipCode = "12-123";
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = Path.Combine(folderPath, "TestFiles/test.txt");

            FillCompanyForm(companyName, email, phone, www, shortDescription, city, street, addressNumber, zipCode, logoPath);

            Browser.FindId("save_company").Click();

            var errorMessages = Browser.FindAllCss("span.field-validation-error");
            Assert.That(errorMessages.Count(), Is.EqualTo(1));
            Assert.That(errorMessages.First().Text, Is.StringContaining("Nieprawidłowy plik. Dostępne są tylko pliki z rozszerzeniami"));
        }

        [Test]
        public void AddCompany_WithoutRequiredValues_MessagesAboutRequiredValues()
        {
            string password = "1234qwer";
            string mail = RegisterAndConfirm(password);
            LogIn(mail, password);
            Browser.Visit("Company/Add");

            Browser.FindId("save_company").Click();

            var errorMessages = Browser.FindAllCss("span.field-validation-error");
            Assert.That(errorMessages.Count(), Is.EqualTo(2));
            Assert.That(errorMessages.First().Text, Is.StringContaining("jest obowiązkowe"));
            Assert.That(errorMessages.Last().Text, Is.StringContaining("jest obowiązkowe"));
        }

        [Test]
        public void EditCompany_TwoUsers_OnlyOwnerCanUpdateCompany()
        {
            string password = "1234qwer";
            string user1Login = $"user1{Guid.NewGuid()}";
            string user2Login = $"user2{Guid.NewGuid()}";
            string user1Email = RegisterAndConfirm(password);
            string user2Email = RegisterAndConfirm(password);

            LogIn(user1Email, password);

            string companyName = $"test name {Guid.NewGuid()}";
            string email = user1Email;
            string phone = "123123123";
            string www = "www.test.pl";
            string shortDescription = "short description short description short description";
            string city = "Some City";
            string street = "Some street";
            string addressNumber = "55";
            string zipCode = "12-123";
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = Path.Combine(folderPath, "TestFiles/logo1.png");

            AddCompany(companyName, email, phone, www, shortDescription, city, street, addressNumber, zipCode, logoPath);

            LogOut();
            LogIn(user2Email, password);
            Browser.Visit($"Company/Show/{companyName}");
            Browser.FindId("edit_company").Click();

            Assert.That(Browser.HasContent("Nie masz uprawnień do edycji tego obiektu"));
        }
    }
}
