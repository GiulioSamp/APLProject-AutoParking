from Endpoint import Endpoint

import json


class Menu:
    def __init__(self, admin_id):
        self.endpoint = Endpoint(admin_id)
#match case solo da 3.10 in su
    def process_menu_selection(self, selection):
        match selection:
            case 1:
                self.endpoint.get_occupied_spots_all_floors()
            case 2:
                self.endpoint.get_occupied_spots_forf()
            case 5:
                self.endpoint.get_gain()
            case _:
                print("Selezione non valida")

def main():
    menu = Menu(admin_id="Admin12345")

    while True:
        print("Seleziona un'opzione:")
        print("1. Ottieni posti occupati in tutti i piani")
        print("2. Visualizza posti occupati per piano")
        print("3. Chiudi parcheggio")
        print("4. Apri parcheggio")
        print("5. Visualizza incasso giornaliero")
        print("6. Crea pacchetto ore")
        print("0. Esci")

        selection = int(input("Inserisci il numero corrispondente all'opzione: "))

        if selection == 0:
            break

        menu.process_menu_selection(selection)

# Esegui il programma principale
if __name__ == "__main__":
    main()
