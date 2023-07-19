using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ParkingClient;

namespace ParkingClient.nUnitTests
{

        [TestFixture]
        public class AutoveicoloBuilderTests
        {
            private AutoveicoloBuilder _builder;

            [SetUp]
            public void Setup()
            {
                // This method is called before each test to set up the environment
                _builder = new AutoveicoloBuilder();
            }

            [Test]
            public void SetTarga_ValidInput_TargaSetSuccessfully()
            {
                // Arrange
                string input = "ABC123";

                // Act
                _builder.SetTarga();

                // Assert
                Assert.AreEqual(input, _builder.vehicle.Targa);
            }

            [Test]
            public void SetTarga_InvalidInput_TargaNotSet()
            {
                // Arrange
                string input = " ";

                // Act
                _builder.SetTarga();

                // Assert
                Assert.AreNotEqual(input, _builder.vehicle.Targa);
                Assert.IsNull(_builder.vehicle.Targa);
            }

            [Test]
            public void SetMarca_ValidInput_MarcaSetSuccessfully()
            {
                // Arrange
                string input = "Fiat";

                // Act
                _builder.SetMarca();

                // Assert
                Assert.AreEqual(input, _builder.vehicle.Marca);
            }
        [Test]
        public void SetModello_ValidInput_ModelloSetSuccessfully()
        {
            // Arrange
            string input = "Punto";

            // Act
            _builder.SetModello();

            // Assert
            Assert.AreEqual(input, _builder.vehicle.Modello);
        }

        [Test]
        public void SetModello_InvalidInput_ModelloNotSet()
        {
            // Arrange
            string input = " ";

            // Act
            _builder.SetModello();

            // Assert
            Assert.AreNotEqual(input, _builder.vehicle.Modello);
            Assert.IsNull(_builder.vehicle.Modello);
        }

        [Test]
        public void SetAnno_ValidInput_AnnoSetSuccessfully()
        {
            // Arrange
            string input = "2022";

            // Act
            _builder.SetAnno();

            // Assert
            Assert.AreEqual(input, _builder.vehicle.Anno);
        }

        [Test]
        public void SetAnno_InvalidInput_AnnoNotSet()
        {
            // Arrange
            string input = "abc";

            // Act
            _builder.SetAnno();

            // Assert
            Assert.AreNotEqual(input, _builder.vehicle.Anno);
            Assert.IsNull(_builder.vehicle.Anno);
        }

        [Test]
        public void SetTipo_ValidInput_TipoSetSuccessfully()
        {
            // Arrange
            string expectedTipo = "Autoveicolo";

            // Act
            _builder.SetTipo();

            // Assert
            Assert.AreEqual(expectedTipo, _builder.vehicle.Tipo);
        }

    }
    }

