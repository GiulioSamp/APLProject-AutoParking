using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient
{
    public class MotocicloBuilder : VehicleBuilder
    {
        //2ruote superiore 50cc
        public override void SetTarga()
        {
            vehicle.Targa = Validation.CheckInput("\n\nInserisci la targa del motociclo: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });
        }

        public override void SetMarca()
        {
            vehicle.Marca = Validation.CheckInput("\nInserisci la marca del motociclo: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });
        }

        public override void SetModello()
        {
            vehicle.Modello = Validation.CheckInput("\nInserisci il modello del motociclo: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });
        }

        public override void SetAnno()
        {
            vehicle.Anno = Validation.CheckInput("\nInserisci anno del motociclo (aaaa): ", input =>
            {
                return input.All(char.IsDigit) && input.Length == 4;
            });
        }
        public override void SetTipo()
        {
            vehicle.Tipo = "Motociclo";
        }
    }
}
