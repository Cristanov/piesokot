using Coypu;
using NUnit.Framework;
using System;
using System.Drawing;
using System.Threading;

namespace NaSpacerDo.Tests.View
{
    [TestFixture]
    public class AccountTests : TestBase
    {
        [Test]
        public void CanRegisterUser()
        {
            string email = $"some@email{Guid.NewGuid()}.com";

            Register(email, "1234abcd");

            Assert.That(Browser.HasContent("Rejestracja zakończona pomyślnie"));
        }

        [Test]
        public void RegisterUser_RegisterWithExistingEmail_ErrorMessage()
        {
            string email = $"some@email{Guid.NewGuid()}.com";

            Register(email, "1234qwer");
            Register(email, "1234qwer");

            Assert.That(Browser.HasContent("is already taken"));
        }

        [Test]
        public void RegisterUser_WithoutEmailConfirmation_CantLog()
        {
            string email = $"some@email{Guid.NewGuid()}.com";
            string password = "123qwe";

            Register(email, password);
            LogIn(email, password);

            Assert.That(Browser.HasContent("Nie potwierdzono adresu email"));
        }

        [Test]
        public void CanRegisterUserWithEmailConfirmationAndCanLog()
        {
            string password = "1234qwer";
            string mail = RegisterAndConfirm(password);
            LogIn(mail, password);

            Assert.That(Browser.HasContent(mail));
        }
    }
}
