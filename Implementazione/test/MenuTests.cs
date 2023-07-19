using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient.nUnitTests
{
    [TestFixture]
    internal class MenuTests
    {
        private Menu menu;
        private Mock<Handler> handlerMock;

        [SetUp]
        public void SetUp()
        {
            handlerMock = new Mock<Handler>();
            menu = new Menu();
        }

        [Test]
        public void TestEntryMenu_RegisterChoice()
        {
            // Simula l'input dell'utente: "1"
            SimulateUserInput("1");

            // Configura il mock per il metodo Register del Handler
            handlerMock.Setup(h => h.Register());

            // Esegui il menu
            menu.EntryMenu();

            // Verifica che il metodo Register() del Handler sia stato chiamato
            handlerMock.Verify(h => h.Register(), Times.Once);
        }

        [Test]
        public void TestEntryMenu_LoginChoice_Success()
        {
            // Simula l'input dell'utente: "2"
            SimulateUserInput("2");

            // Configura il mock per il metodo DoLogin del Handler
            handlerMock.Setup(h => h.DoLogin()).Returns(true);

            // Esegui il menu
            menu.EntryMenu();

            // Verifica che il metodo DoLogin() del Handler sia stato chiamato
            handlerMock.Verify(h => h.DoLogin(), Times.Once);

        }

        [Test]
        public void TestEntryMenu_InvalidChoice()
        {
            // Prova l'input dell'utente: "invalid"
            SimulateUserInput("invalid");

            // Crea una nuova istanza di Menu (senza utilizzare il mock per Handler)
            Menu menu = new Menu();

            // Cattura l'output del menu durante l'esecuzione
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Esegui il metodo EntryMenu del menu
            menu.EntryMenu();

            // Verifica se il messaggio di errore è stato stampato correttamente
            string expectedErrorMessage = "Scelta non valida. Riprova.";
            Assert.IsTrue(consoleOutput.ToString().Contains(expectedErrorMessage));

        }
        


        // Metodo per simulare l'input utente durante i test
        private void SimulateUserInput(string input)
        {
            var inputStream = new StringReader(input);
            Console.SetIn(inputStream);
        }
    }

}


