import requests
import json
import datetime
import locale
import ast
from ABCViewGain import AbstactGetGain
from ConcreteViewGain import ConcreteGetGain
#forse meglio nome ServerCommunication?
class Endpoint:
    def __init__(self, admin_id):
        self.admin_id = admin_id
        self.visualizza_incasso_strategy = ConcreteGetGain()

    def data_to_server(self, endpoint, data):
        url = "http://localhost:18080" + endpoint
        try:
            headers = {'Content-Type': 'application/json'}  # Imposto l'intestazione della richiesta HTTP per inviare JSON
            response = requests.post(url, headers=headers, json=data)
            response.raise_for_status()  # Genera un'eccezione se la richiesta non ha successo
            return response.text  # Restituisce la risposta come stringa di testo
        except requests.exceptions.RequestException as e:
             if isinstance(e, requests.exceptions.HTTPError) and e.response.status_code == 400:
                print("Errore del server:", e.response.text)
             else:
                print("Errore durante l'invio della richiesta al server:", e)

    def get_non_empty_input(self,prompt):
        while True:
            user_input = input(prompt)
            if user_input.strip():
                return user_input
            print("Input non valido. Riprova.")
  #uc3
    def get_occupied_spots_all_floors(self):
        endpoint = "/spots"
        response = self.data_to_server(endpoint, None)

        if response is not None:
            try:
                data = json.loads(response)  # la risposta JSON
                result = data["result"]  # estraggo il campo {"result":" dalla risposta
                result = result.strip()  #  eventuali spazi iniziali e finali lstrip()solo iniz
                result = result.replace('\\n', '\n')  # Sostituisci "\\n" con "\n" per andare a capo nella stampa
                print("Visualizzazione numero di posti occupati su numero di posti liberi")
                print(result)
                # return result se mi serve val ritorn
            except json.JSONDecodeError as e:
                print("Errore nella decodifica della risposta JSON:", e)
            except KeyError as e:
                print("La risposta non contiene il campo 'result':", e)
            except Exception as e:
                print("Errore nella gestione della risposta del server:", e)
                # return None
        else:
            print("Errore nella richiesta dei posti occupati.")
            # return None
#uc4
    def get_occupied_spots_forf(self):
        endpoint = "/spots"
        response = self.data_to_server(endpoint, None)

        if response is not None:
            try:
                data = json.loads(response)  # la risposta JSON
                result = data["result"]  # estraggo il campo "result" dalla risposta
                result = result.strip()  #  eventuali spazi iniziali e finali lstrip()solo iniz
                result = result.replace('\\n', '\n')  # Sostitu"\\n" con "\n" per andare a capo nella stampa 
                # Chiedi all'utente di inserire il numero del piano
                while True:
                    try:
                        piano_numero = int(input("Inserisci il numero del piano da 1 a 10: "))
                        if piano_numero < 1 or piano_numero > 10:
                            raise ValueError("Numero piano non valido.")
                        break
                    except ValueError:
                        print("Errore: Inserisci un numero di piano valido da 1 a 10.")

                piano_stringa = f"Piano: {piano_numero},"
                piano_inizio = result.index(piano_stringa)
                piano_fine = result.index('\n', piano_inizio)
                piano_risultato = result[piano_inizio:piano_fine]
                print(piano_risultato)
                #else:
                #print("Il piano specificato non è presente nella risposta.")

            except json.JSONDecodeError as e:
                print("Errore nella decodifica della risposta JSON:", e)
            except KeyError as e:
                print("La risposta non contiene il campo 'result':", e)
            except Exception as e:
                print("Errore nella gestione della risposta del server:", e)
                # return None 
#uc7
    def get_gain(self):
        endpoint = "/gain"
        response = self.data_to_server(endpoint, None)
        if response is not None:
            try:
                data = json.loads(response)  # Decodifica la risposta JSON
                if isinstance(data, list) and len(data) > 0:
                    #visualizza_incasso_strategy è un riferimento a istanza di una classe che implementa l'interfaccia AbstractVisualizzaIncasso
                    self.visualizza_incasso_strategy.get_incasso(data)  # assa 'data' come parametro
                else:
                    print("La risposta non contiene dati di guadagno.")
            except json.JSONDecodeError as e:
                print("Errore nella decodifica della risposta JSON:", e)
            except ValueError as e:
                print("Errore: Input non valido.", e)
            except Exception as e:
                print("Errore nella gestione della risposta del server:", e)
        else:
            print("Errore nella richiesta degli incassi.")
#uc8
    def get_giorni_scelti(self):
        giorni_disponibili = {
            '1': 'Lunedi',
            '2': 'Martedi',
            '3': 'Mercoledi',
            '4': 'Giovedi',
            '5': 'Venerdi',
            '6': 'Sabato',
            '7': 'Domenica'
        }
        print("1=Lunedi,2=Martedi,3=Mercoledi,4=Giovedi,5=Venerdi,6=Sabato,7=Domenica")
        giorni_input = self.get_non_empty_input("Inserisci i numeri corrispondenti ai giorni desiderati separati da spazi o virgole: ")

        giorni_input = giorni_input.replace(',', ' ')  # Sostituisci le virgole con spazi per gestire entrambi i separatori
        giorni_scelti = []
        for giorno in giorni_input.split():
            giorno = giorno.strip()
            if giorno in giorni_disponibili:
                giorni_scelti.append(giorni_disponibili[giorno])
            else:
                raise ValueError(f"Il numero {giorno} non corrisponde a un giorno disponibile.")

        return giorni_scelti

    def show_rate(self):
        # Prima comunicazione al server per ottenere le tariffe attuali
        data_to_send = None  
        response = self.data_to_server("/showrate", data_to_send)

        if response is not None:
            try:
                data = json.loads(response)  # la risposta JSON

                print("Ecco la lista delle tariffe presenti: ")
                for item in data:
                    for key, value in item.items():
                        if isinstance(value, str):
                            value = value.replace('.', ',')
                            if ',' in value:
                                parts = value.split(',')
                                if len(parts[1]) > 2:
                                    value = f"{parts[0]},{parts[1][:2]}"
                        elif isinstance(value, float):
                            value = f"{value:.2f}".replace('.', ',')
                        print(f"{key}: {value}")
            
            except json.JSONDecodeError as e:
                print("Errore nella decodifica della risposta JSON:", e)
            except KeyError as e:
                print("La risposta non contiene i campi desiderati", e)
            except Exception as e:
                print("Errore nella gestione della risposta del server:", e)
        else:
            print("Errore nella richiesta di visualizzare le tariffe presenti.")

    def update_rate(self):
        self.show_rate()
        tariffa_id = self.get_non_empty_input("Inserisci l'ID della tariffa da aggiornare: ")
        nuovo_valore_fisso = self.get_non_empty_input("Inserisci il nuovo valore fisso: ")
        nuovo_aggiornamento_ore = self.get_non_empty_input("Inserisci il nuovo valore di maggiorazione per ore trascorse: ")

        giorni_scelti = self.get_giorni_scelti()

        data_to_send = {
            "Id": tariffa_id,
            "Valore_fisso": float(nuovo_valore_fisso.replace(',', '.')),
            "Aggiunta_ore": float(nuovo_aggiornamento_ore.replace(',', '.')),
            "Giorno": giorni_scelti
        }
    
        # Seconda comunicazione al server con invio dei dati
        response = self.data_to_server("/updaterate", data_to_send)
        if response is not None:
            response_data = json.loads(response)
            result_value = response_data.get("result", "")
            print("Risposta del server:", result_value)   
        elif response and response.status_code == 400:
                print("Errore nella richiesta di visualizzare le tariffe presenti.")
                return 
#uc9

    def add_rate(self):
        self.show_rate()
        nuovo_valore_fisso = self.get_non_empty_input("Inserisci il nuovo valore fisso: ")
        nuovo_aggiornamento_ore = self.get_non_empty_input("Inserisci il nuovo valore di maggiorazione per ore trascorse: ")
        giorni_scelti = self.get_giorni_scelti()
        data_to_send = {
            "Valore_fisso": float(nuovo_valore_fisso.replace(',', '.')),
            "Aggiunta_ore": float(nuovo_aggiornamento_ore.replace(',', '.')),
            "Giorno": giorni_scelti
        }
        
        response= self.data_to_server("/addrate", data_to_send)  #{'Result': 'Successo'}
        if response is not None:
            try:
                response_data = json.loads(response)
                result_value = response_data["Result"]
                print("Risposta del server:", result_value)   
            except json.JSONDecodeError as e:
                print("Errore nella decodifica della risposta JSON:", e)
            except KeyError as e:
                print("La risposta non contiene il campo 'result':", e)
            except Exception as e:
                print("Errore nella gestione della risposta del server:", e)
                # return None
        else:
            print("Errore nell'inserimento di una nuova tariffa.")
            # return None
       
           
            
            
            
            
            
            
            #response_json = json.loads(response_text)
        #print("Risposta del server:", response_json)

    


