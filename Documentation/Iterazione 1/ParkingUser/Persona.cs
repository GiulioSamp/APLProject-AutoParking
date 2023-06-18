using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingUser
{
    //internal
    public class Persona
    {
        public string Nome { get;set; }
        public string Cognome { get; set; }
        public string NumeroTelefono { get; set; }
        public string Email { get; set; }

        public Persona(string nome, string cognome, string numeroTelefono, string email)
        {
            Nome = nome;
            Cognome = cognome;
            NumeroTelefono = numeroTelefono;
            Email = email;
        }
        public Persona() { } //serve   

        public void InserisciPersona()
        {
            Console.WriteLine("Inserisci i dati utente:");
            try
            {
                Console.Write("Nome: ");
                Nome= Console.ReadLine();
                
                Console.Write("Cognome: ");
                // string cognome = Console.ReadLine();
                Cognome = Console.ReadLine();
                Console.Write("Numero di telefono: ");
                //string numeroTelefono = Console.ReadLine();
                NumeroTelefono = Console.ReadLine();
                Console.Write("Email: ");
                Email = Console.ReadLine();
                Console.WriteLine("Dati inseriti correttamente\n");
                if (!Email.Contains("@"))
                {
                    throw new Exception("L'email deve contenere il simbolo '@'.");
                }
                
               // return new Persona(nome, cognome, numeroTelefono, email);
            }
            catch (Exception ex)
            {
                while (!Email.Contains("@"))
                {
                    Console.WriteLine($"Errore: {ex.Message}");
                    //return null;
                    Console.Write("Email: ");
                    Email = Console.ReadLine();
                }
                
            }
            
        }
        /*INSERIMEN VOID per case uguali
          public static void InserisciPersona()
          {
        try
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Cognome: ");
            string cognome = Console.ReadLine();
            Console.Write("Numero di telefono: ");
            string numeroTelefono = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();

            if (!email.Contains("@"))
            {
                throw new Exception("L'email deve contenere il simbolo '@'.");
            }

            Persona persona = new Persona(nome, cognome, numeroTelefono, email);
            Console.WriteLine("Persona inserita correttamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
        }
    }*/

        public void ModificaPersona()
        {
            try
            {
                if (Nome == null)
                {
                    Console.WriteLine("Nessuna persona presente. Inserisci una persona prima di effettuare una modifica.");
                    return;
                }
                Console.WriteLine("Modifica i dati inseriti precedentemente:");
                //Console.WriteLine($"Modifica dei dati di {persona.Nome} {persona.Cognome}:");
                Console.Write("Nome attuale: {0} inserisci nuovo nome: ", Nome);
                string nuovoNome = Console.ReadLine();
                Console.Write("Cognome attuale: {0} inserisci nuovo cognome: ", Cognome);
                string nuovoCognome = Console.ReadLine();
                Console.Write("Numero di telefono attuale {0} inserisci nuovo numero di telefono: ", NumeroTelefono);
                string nuovoNumeroTelefono = Console.ReadLine();
                Console.Write("E-mai attuale {0}, inserisci nuova email: ", Email);
                string nuovaEmail = Console.ReadLine();

                if (!nuovaEmail.Contains("@"))
                {
                    throw new Exception("L'email deve contenere il simbolo '@'.");
                }

                // Aggiornamento dei dati della persona
                Nome = nuovoNome;
                Cognome = nuovoCognome;
                NumeroTelefono = nuovoNumeroTelefono;
                Email = nuovaEmail;

                Console.WriteLine("Dati della persona modificati con successo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }

        public void VisualizzaPersona()
        {
            try
            {
                if (Nome == null)
                {
                    Console.WriteLine("Nessuna persona presente.Inserisci una persona prima di visualizzare i dati.");
                    return;
                }
                Console.WriteLine("Visualizza dati utente inseriti:");
                Console.WriteLine($"Nome: {Nome}");
                Console.WriteLine($"Cognome: {Cognome}");
                Console.WriteLine($"Numero di telefono: {NumeroTelefono}");
                Console.WriteLine($"E-mail: {Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }

        }

    }
}






    







