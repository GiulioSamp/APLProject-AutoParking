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
    public class ValidationTests
    {
        [Test]
        public void CheckInput_ValidInput_ReturnsInput()
        {
            string input = "ValidInput";
            Func<string, bool> validation = (s) => !string.IsNullOrEmpty(s); // Esempio di validazione semplice

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);
                Console.SetIn(new System.IO.StringReader(input));

                string result = Validation.CheckInput("Enter input: ", validation);

                Assert.AreEqual(input, result);
                Assert.AreEqual("Enter input: ", sw.ToString());
            }
        }

        [Test]
        public void CheckInput_InvalidInputThenValidInput_ReturnsValidInput()
        {
            string invalidInput = "";
            string validInput = "ValidInput";
            Func<string, bool> validation = (s) => !string.IsNullOrEmpty(s); // Esempio di validazione semplice

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);
                Console.SetIn(new System.IO.StringReader(invalidInput + Environment.NewLine + validInput));

                string result = Validation.CheckInput("Enter input: ", validation);

                Assert.AreEqual(validInput, result);
                StringAssert.Contains("Input non valido. Riprova.", sw.ToString());
            }
        }
    }
}

