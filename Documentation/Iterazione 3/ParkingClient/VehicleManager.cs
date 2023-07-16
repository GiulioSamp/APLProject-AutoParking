using System;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using ParkingClient;

namespace ParkingClient
{
    public class VehicleManager
    {
        public string Targa { get; set; }
        public string Marca { get; set; }
        public string Modello { get; set; }
        public string Anno { get; set; }
        public string Tipo { get; set; }

        public VehicleManager(string targa, string marca, string modello, string anno, string tipo)
        {
            Targa = targa;
            Marca = marca;
            Modello = modello;
            Anno = anno;
            Tipo = tipo;
        }
        public VehicleManager(){}
       /* public override string ToString()
        {
            return "Targa: " + Targa + "Marca:" + Marca+ "Modello:" + Modello+ "Anno:" + Anno;
        } */
        public VehicleManager EnterVehicle(UserManager utente)
         {
            //passo parametro utente che rappresenta il riferimento alla classe UserManager per poter usare check metodo is..

            VehicleBuilder vehicleBuilder = ChooseVehicleBuilder();

            vehicleBuilder.CreateNewVehicle();
            vehicleBuilder.SetTarga();
            vehicleBuilder.SetMarca();
            vehicleBuilder.SetModello();
            vehicleBuilder.SetAnno();
            vehicleBuilder.SetTipo();
            VehicleManager vehicle = vehicleBuilder.GetVehicle();
           return vehicle;


        }

        public void ModifyVehicle(UserManager utente)
        {
            try
            {
                if (Targa == null)
                {
                    Console.WriteLine("Nessun veicolo presente.\nInserisci prima un veicolo\n");
                    return;
                }
                Console.WriteLine("Targa attuale {0}. Vuoi modificare la targa del veicolo? (s/n):", Targa);
                string risposta = Console.ReadLine();

                if (risposta.ToLower() == "s")
                {
                    Targa = Validation.CheckInput("\nInserisci la nuova targa: ", input =>
                    {
                        return !string.IsNullOrWhiteSpace(input);
                    });
                }//
                Console.WriteLine("Marca attuale {0}. Vuoi modificare la marca di veicolo? (s/n): ", Marca);
                risposta = Console.ReadLine();
                if (risposta.ToLower() == "s")
                {
                    Marca = Validation.CheckInput("\nInserisci la nuova marca del veicolo: ", input =>
                    {
                        return !string.IsNullOrWhiteSpace(input);
                    });

                }
                Console.WriteLine("Modello attuale {0}. Vuoi modificare il modello del veicolo? (s/n): ",Modello);
                risposta = Console.ReadLine();
                if (risposta.ToLower() == "s")
                {
                    Modello = Validation.CheckInput("\nInserisci il nuovo modello del veicolo: ", input =>
                    {
                        return !string.IsNullOrWhiteSpace(input);
                    });

                }
                Console.WriteLine("Anno inserito {0}. Vuoi modificare l'anno del veicolo? (s/n): ", Anno);
                risposta = Console.ReadLine();
                if (risposta.ToLower() == "s")
                {
                    Anno = Validation.CheckInput("\nInserisci anno del veicolo ", input =>
                    {
                        return input.All(char.IsDigit) && input.Length == 4;
                    });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Si è verificato un errore durante la modifica dei dati del veicolo:");
                Console.WriteLine(ex.Message);
            }
        }

        private VehicleBuilder ChooseVehicleBuilder()
        {
            Console.WriteLine("Seleziona il tipo di veicolo:");
            Console.WriteLine("1. Auto");
            Console.WriteLine("2. Motociclo");
            Console.WriteLine("3. Autoveicolo/Autobus");

            int scelta;
            while (!int.TryParse(Console.ReadLine(), out scelta) || scelta < 1 || scelta > 3)
            {
                Console.WriteLine("Scelta non valida. Riprova.");
            }

            VehicleBuilder vehicleBuilder = null;

            switch (scelta)
            {
                case 1:
                    vehicleBuilder = new AutovettureBuilder();
                    break;
                case 2:
                    vehicleBuilder = new MotocicloBuilder();
                    break;
                case 3:                   
                    vehicleBuilder = new AutoveicoloBuilder();                    
                    break;
            }
            
            return vehicleBuilder;
        }




    }
}
