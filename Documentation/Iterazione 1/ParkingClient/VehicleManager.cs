using System;
using System.Net;
using System.Text;
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
        

        public VehicleManager(string targa, string marca, string modello, string anno)
        {
            Targa = targa;
            Marca = marca;
            Modello = modello;
            Anno = anno;

        }

        //public VehicleManager() { }
        public VehicleManager()
        {
           
        }
        public void EnterVehicle(UserManager utente)
         { //passo parametro utente che rappresenta il riferimento alla classe UserManager per poter usare check metodo is..
            
            try
            {
                if (utente.IsUtenteInserito())
                {
                
                Targa = Validation.CheckInput("\n\nInserisci la targa: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });
                
                
                Marca = Validation.CheckInput("\nInserisci la marca del veicolo: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });
                   
                Modello = Validation.CheckInput("\nInserisci il modello del veicolo: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });
                    
                Anno = Validation.CheckInput("\nInserisci anno del veicolo (aaaa) ", input =>
                {
                    return input.All(char.IsDigit) && input.Length == 4;
                });
                                
                }
                else
                {
                    Console.WriteLine("Devi inserire prima i dati dell'utente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Si è verificato un errore durante l'inserimento dei dati del veicolo: ");
                Console.WriteLine(ex.Message);
            }

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

        /*public void AddNewVehicleInList()
        {
            
                var veicolo = new VehicleManager();

                veicolo.Targa = Validation.CheckInput("\n\nInserisci la targa: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                veicolo.Marca = Validation.CheckInput("\nInserisci la marca del veicolo: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                veicolo.Modello = Validation.CheckInput("\nInserisci il modello del veicolo: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                veicolo.Anno = Validation.CheckInput("\nInserisci anno del veicolo (aaaa): ", input =>
                {
                    return input.All(char.IsDigit) && input.Length == 4;
                });

                Veicoli.Add(veicolo); // Add il nuovo veicolo alla lista           
            
        }*/

    }
}
