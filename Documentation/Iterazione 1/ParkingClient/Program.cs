using System;
namespace ParkingUser
{
    internal class Program
    {
        
        
        static void Main(string[] args)
        {

            Utente p=new Utente();
            Veicolo v=new Veicolo();
            //crea unistanza della classe Persona chiamata P.
            //p tipo Persona, per accedere ai metodi NON STATICdella classe tramite questa istanza
            bool continua = true;
            while (continua)
            {
                Console.WriteLine("-Menu:\n");
                Console.WriteLine(" 1. Aggiungi dati utente");
                Console.WriteLine(" 2. Modifica dati inseriti");
                Console.WriteLine(" 3. Visualizza dati inseriti"); // da fare amministrator no utent vede todo
                Console.WriteLine(" 4. Inserisci dati del veicolo");
                Console.WriteLine(" 5. Modifica dati del veicolo");
                Console.WriteLine(" 6. Visualizza dati del veicolo");
                Console.WriteLine(" 7. Conferma dati inseirit");
                Console.WriteLine(" 8. Esci");

                Console.Write("Scelta: ");
                string scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                       p.InserisciUtente();
                        break;
                    case "2":
                        p.ModificaUtente();

                        break;
                    case "3":
                        p.VisualizzaUtente();
                        break;
                    case "4":
                        v.InserisciVeicolo();
                        break;
                    case "5":
                        v.ModificaDati();
                        break;
                    case "6":
                        v.VisualizzaDati();
                        break;
                    case "7":
                        v.InviaDatiAlServer("http://indirizzo-server/api/veicoli");
                        p.InviaDatiAlServer("http://indirizzo-server/api/utenti");

                        break;
                    case "8":
                        continua = false;
                        break;
                    default:
                        Console.WriteLine("Scelta non valida. Riprova.");
                        break;
                }

                Console.WriteLine();
            }
        }

    }
}