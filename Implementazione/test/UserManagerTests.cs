using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using ParkingClient;

namespace ParkingClient.nUnitTests
{

[TestFixture]
public class UserManagerTests
{
    private UserManager userManager;
    private StringWriter consoleOutput;
    [SetUp]
    public void SetUp()
    {
        userManager = new UserManager();
    }

        [Test]
        public void TestUserRegistration()
        {// Simula l'input utente durante la registrazione
            string nome = "Aurora";
            string cognome = "Giulio";
            string telefono = "345905533";
            string email = "project@park.com";
            string pass = "pass1";

            // l'input simulato
            SimInput(nome, cognome, telefono, email, pass);

            // Es la registrazione dell'utente
            userManager.UserRegistration();

            // Verifica che i dati siano stati correttamente assegnati alle proprietà dell'utente
            Assert.AreEqual(nome, userManager.Nome);
            Assert.AreEqual(cognome, userManager.Cognome);
            Assert.AreEqual(telefono, userManager.Telefono);
            Assert.AreEqual(email.ToLower(), userManager.Email);
            Assert.AreEqual(pass, userManager.Pass);
        }

        // Metodo per simulare l'input utente durante i test
        private void SimInput(string nome, string cognome, string telefono, string email, string pass)
        {
            var inputStream = new System.IO.StringReader($"{nome}\n{cognome}\n{telefono}\n{email}\n{pass}\n");
            System.Console.SetIn(inputStream);
        }
        //Reg
        [Test]
        public void TestIsUtenteInserito_WithEmptyUser_ReturnsFalse()
        {
            // Creiamo un'istanza di UserManager senza dati
            var userManager = new UserManager();

            // Verifichiamo che il metodo IsUtenteInserito restituisca false
            bool result = userManager.IsUtenteInserito();

            Assert.IsFalse(result);
        }
        //check presenza
        [Test]
        public void TestIsUtenteInserito_WithUser_ReturnsTrue()
        {
            // Input simulato per il test
            string nome = "Aurora";
            string cognome = "Giulio";
            string telefono = "345905533";
            string email = "project@park.com";
            string pass = "pass1";

            // Creiamo un'istanza di UserManager con dati
            var userManager = new UserManager(nome, cognome, telefono, email, pass);

            // Verifichiamo che il metodo IsUtenteInserito restituisca true
            bool result = userManager.IsUtenteInserito();

            Assert.IsTrue(result);
        }
        //

    }
}
