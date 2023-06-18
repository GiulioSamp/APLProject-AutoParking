using System;
namespace ParkingUser
{
    internal class Program
    {
        
        
        //Con sopra; all'inizio del metodo main creo una variabile persona
        //accessibile all'interno del metodo main e uso per memorizzre l'ogg Persona corrente
        static void Main(string[] args)
        {

            Persona p=new Persona();
            //crea unistanza della classe GestionePersona chiamata gP.
            //gp tipo GestionePersona, per accedere ai metodi NON STATICdella classe tramite questa istanza
            bool continua = true;
            while (continua)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Aggiungi persona");
                Console.WriteLine("2. Modifica dati inseriti");
                Console.WriteLine("3. Visualizza persone"); // da fare amministrator no utent vede todo
                Console.WriteLine("4. Esci");

                Console.Write("Scelta: ");
                string scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                       p.InserisciPersona();
                        break;
                    case "2":
                        p.ModificaPersona();

                        break;
                    case "3":
                        p.VisualizzaPersona();
                        break;
                    case "4":
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