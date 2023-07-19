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
            case 3:
                self.endpoint.update_rate()
            case 4:
                self.endpoint.add_rate()
            case 5:
                self.endpoint.get_gain()
            case 6:
                self.endpoint.show_rate()
            case _:
                print("Selezione non valida")

def main():
    menu = Menu(admin_id="Admin12345")

    while True:
        print("Seleziona un'opzione:")
        print("1. Ottieni posti occupati in tutti i piani") #uc3
        print("2. Visualizza posti occupati per piano")#uc4
        print("3. Aggiorna tariffa:")  # uc8
        print("4. Aggiungi nuova tariffa:")  # uc9
        print("5. Visualizza incasso giornaliero")#uc7
        print("6. Visualizza tariffe")
        print("0. Esci")

        try:
            selection = int(input("Inserisci il numero corrispondente all'opzione: "))
            if selection == 0:
                break
            elif selection in range(1, 7):
                menu.process_menu_selection(selection)
            else:
                print("Opzione non valida. Riprova.")
        except ValueError:
            print("Inserisci un numero valido.")

# Esegui il programma principale
if __name__ == "__main__":
    main()
