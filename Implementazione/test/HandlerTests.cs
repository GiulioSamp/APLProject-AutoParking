using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using System.Net;
using Newtonsoft.Json;

namespace ParkingClient.nUnitTests
{

    [TestFixture]
    public class HandlerTests
    {
        [Test]
        public void TestDoLogin_SuccessfulLogin_ReturnsTrue()
        {
            //'istanza di Handler
            var handler = Handler.Instance;

            // l'input utente durante il login
            string email = "a@";
            string password = "a";
            SimulateUserInput(email, password);

            // Es.login
            bool result = handler.DoLogin();

            // Verifichiamo
            Assert.IsTrue(result);
        }

        // ssimulare l'input utente
        private void SimulateUserInput(string email, string password)
        {
            var inputStream = new System.IO.StringReader($"{email}\n{password}\n");
            Console.SetIn(inputStream);
        }

      
    }

}




    

