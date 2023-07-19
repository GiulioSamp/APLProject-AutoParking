using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient
{
    public class AutoveicoloBuilder : VehicleBuilder
    {
        //bus o camion fino a 4,5t
        public override void SetTarga()
        {
            vehicle.Targa = Validation.CheckInput("\n\nInserisci la targa dell'autoveicolo: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });
        }

        public override void SetMarca()
        {
            vehicle.Marca = Validation.CheckInput("\nInserisci la marca dell'autoveicolo: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });
        }

        public override void SetModello()
        {
            vehicle.Modello = Validation.CheckInput("\nInserisci il modello dell'autoveicolo: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });
        }

        public override void SetAnno()
        {
            vehicle.Anno = Validation.CheckInput("\nInserisci anno dell'autoveicolo (aaaa): ", input =>
            {
                return input.All(char.IsDigit) && input.Length == 4;
            });
        }

        public override void SetTipo()
        {
            vehicle.Tipo = "Autoveicolo";
        }
    }

}
