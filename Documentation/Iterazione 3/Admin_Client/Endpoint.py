import requests
import json

#forse meglio nome ServerCommunication?
class Endpoint:
    #costruttore
    def __init__(self, admin_id):
        self.admin_id = admin_id

    def data_to_server(self, endpoint, data):
        url = "http://localhost:18080" + endpoint
        payload = {"admin_id": self.admin_id, "data": data}

        try:
            response = requests.post(url, data=payload)
            response.raise_for_status()  # Genera un'eccezione se la richiesta non ha successo
            print("Comunicazione al server con successo!")
            return response.text  #  la risposta come stringa 
        except requests.exceptions.RequestException as e:
            print("Errore nell'invio dei dati al server:", e)

    import json

class Endpoint:
    def __init__(self, admin_id):
        self.admin_id = admin_id

    def data_to_server(self, endpoint, data):
        url = "http://localhost:18080" + endpoint
        payload = {"admin_id": self.admin_id, "data": data}

        try:
            response = requests.post(url, data=payload)
            response.raise_for_status()  # Genera un'eccezione se la richiesta non ha successo
            print("Richiesta inviata al server con successo!")
            return response.text  # Restituisci la risposta come stringa di testo
        except requests.exceptions.RequestException as e:
            print("Errore nell'invio della comunicazione al server:", e)

    def get_occupied_spots_all_floors(self):
        endpoint = "/spots"
        response = self.data_to_server(endpoint, None)

        if response is not None:
            try:
                data = json.loads(response)  # la risposta JSON
                result = data["result"]  # estraggo il campo "result" dalla risposta
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







# Utilizzo dell'oggetto Endpoint
endpoint_obj = Endpoint(admin_id="Admin12345")
#endpoint_obj.data_to_server("/api/endpoint1", {"key": "value"})
    


