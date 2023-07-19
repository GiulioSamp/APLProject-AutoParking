using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient
{
    internal class Menu
    {
        Handler handler = Handler.Instance;

        public void EntryMenu()
        {
            while (true)
            {
                try
                {
                    PrintEMenu();

                    int sceltaMenu = GetMenuChoice();

                    switch (sceltaMenu)
                    {
                        case 1:
                            handler.Register();
                            InternalMenu();
                            break;

                        case 2:
                            if (handler.DoLogin())
                            {
                                InternalMenu();
                            }
                            break;

                        case 3:
                            handler.Takevehicle();
                            
                            break;

                        default:
                            Console.WriteLine("Scelta non valida. Riprova.");
                            break;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Errore durante l'invio dei dati al server: " + ex.Message);
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine(ex.Message + " Login negato, Riprova.");
                }
            }
        }
        
        #region private
        private void InternalMenu()
        {
            while (true)
            {
                try
                {
                    PrintIMenu();

                    string scelta = Console.ReadLine();

                    switch (scelta)
                    {
                        case "1":
                            handler.ChoiceVehicleListForPark();
                            break;

                        case "2":
                            handler.RegisterVehicle();
                            break;

                        case "3":
                            handler.ModdVehicle();
                            break;

                        case "4":
                            handler.ModUser();
                            break;

                        case "5":
                            handler.RetrieveUser();
                            break;

                        case "6":
                            handler.RetrieveVehicleList();
                            break;

                        case "7":
                            Console.WriteLine("Menù terminato.");
                            return;

                        default:
                            Console.WriteLine("Scelta non valida. Riprova.");
                            break;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Errore durante l'invio dei dati al server: " + ex.Message);
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine(ex.Message + " Login negato, Riprova.");
                }
            }
        }

        private void PrintEMenu()
        {
            Console.WriteLine("1. Registrazione");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Ritiro veicolo");
            Console.Write("\nDigita il numero dell'operazione desiderata tra quelle elencate sopra: ");
        }

        private int GetMenuChoice()
        {
            int.TryParse(Console.ReadLine(), out int sceltaMenu);
            return sceltaMenu;
        }

        private void PrintIMenu()
        {
            Console.WriteLine($"\nBenvenuto/a! Seleziona un'opzione:");
            Console.WriteLine("1. Inizia parcheggio");
            Console.WriteLine("2. Aggiungi nuovo veicolo");
            Console.WriteLine("3. Modifica dati veicolo");
            Console.WriteLine("4. Modifica dati utente");
            Console.WriteLine("5. Visualizza dati utente");
            Console.WriteLine("6. Visualizza dati veicoli");
            Console.WriteLine("7. Logout");
            Console.Write("Scelta: ");
        }
        #endregion
    }

}
