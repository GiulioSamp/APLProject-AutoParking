using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//per rest api HTTP POST
using System.Net.Http;
using System.Text.Json;

namespace ParkingUser
{
    //internal
    public class Utente
    {
        public string Nome { get;set; }
        public string Cognome { get; set; }
        public string NumeroTelefono { get; set; }
        public string Email { get; set; }

        public Utente(string nome, string cognome, string numeroTelefono, string email)
        {
            Nome = nome;
            Cognome = cognome;
            NumeroTelefono = numeroTelefono;
            Email = email;
        }
        public Utente() { } //serve   

        public void InserisciUtente()
        {
            Console.WriteLine("\nInserisci i dati utente:");
            try
            {
                Console.Write("Nome: ");
                Nome= Console.ReadLine();
                
                Console.Write("Cognome: ");
                // string cognome = Console.ReadLine();
                Cognome = Console.ReadLine();
                bool numeroValido = false;
                while (!numeroValido)
                {
                    Console.Write("Inserisci il numero di telefono: ");
                    NumeroTelefono = Console.ReadLine();

                    if (ContieneNumeri(NumeroTelefono))
                    {
                        numeroValido = true;
                    }
                    else
                    {
                        Console.WriteLine("Il numero di telefono deve contenere almeno numeri. Riprova.");
                    }
                }
                Console.Write("Email: ");
                Email = Console.ReadLine();
                Console.WriteLine("Dati inseriti correttamente\n");
                if (!Email.Contains("@"))
                {
                    throw new Exception("L'email deve contenere il simbolo '@'.");
                }
                
               // return new Utente(nome, cognome, numeroTelefono, email);
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
          public static void InserisciUtente()
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

        private bool ContieneNumeri(string input)
        {
            foreach (char carattere in input)
            {
                if (char.IsDigit(carattere))
                {
                    return true;
                }
            }
            return false;
        }

        public void ModificaUtente()
        {
            try
            {
                if (Nome == null)
                {
                    Console.WriteLine("Nessun utente presente. Inserisci una persona prima di effettuare una modifica.");
                    return;
                }
                Console.WriteLine("\nModifica i dati inseriti precedentemente:");
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

                Console.WriteLine("\nDati del utente modificati con successo.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }

        public void VisualizzaUtente()
        {
            try
            {
                if (Nome == null)
                {
                    Console.WriteLine("Nessun utente presente.Inserisci un utente prima di visualizzare i dati.");
                    return;
                }
                Console.WriteLine("\nVisualizza dati utente inseriti:");
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

        public void InviaDatiAlServer(string url)
        { //da approfondire doc
            using (HttpClient client = new HttpClient())
            {
                // Creazione dell'oggetto Utente con i dati dell'utente da inviare
                //Utente p = new Utente { Nome = nome, Cognome = cognome, Telefono = telefono };

                // Serializza l'oggetto UtenteDto in formato JSON
                string jsonData = JsonSerializer.Serialize(this);

                // Crea il contenuto della richiesta HTTP
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Invia la richiesta POST al server
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // Verifica lo stato della risposta
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Dati utente inviati con successo al server.");
                }
                else
                {
                    Console.WriteLine("Errore durante l'invio dei dati utente al server. Codice di stato: " + response.StatusCode);
                }
            }
        }

    }
}






    







