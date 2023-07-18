using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient
{
    public class AutovettureBuilder : VehicleBuilder
    {
        public override void SetTarga()
        {
            vehicle.Targa = Validation.CheckInput("\n\nInserisci la targa dell'auto: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });
        }

        public override void SetMarca()
        {
            vehicle.Marca = Validation.CheckInput("\nInserisci la marca dell'auto: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });
        }

        public override void SetModello()
        {
            vehicle.Modello = Validation.CheckInput("\nInserisci il modello dell'auto: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });
        }

        public override void SetAnno()
        {
            vehicle.Anno = Validation.CheckInput("\nInserisci anno dell'auto (aaaa): ", input =>
            {
                return input.All(char.IsDigit) && input.Length == 4;
            });
        }

        public override void SetTipo()
        {
            vehicle.Tipo = "Autovetture";
        }
    }
}
