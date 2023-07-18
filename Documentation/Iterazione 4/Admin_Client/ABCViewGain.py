from abc import ABC, abstractmethod


class AbstactGetGain(ABC):
    #classe base per visualizzazioni incassi
    
    def get_incasso(self, data):
        scelta = input("Scegli l'opzione di visualizzazione dell'incasso:\n 1-totale / 2-giornaliero / 3-mensile / 4-annuale: ")

        try:
            scelta = int(scelta)
        except ValueError:
            raise ValueError("Opzione non valida.")

        if scelta == 1:
            self.get_incasso_totale(data)
        elif scelta == 2:
            self.get_incasso_giornaliero(data)
        elif scelta == 3:
            self.get_incasso_mensile(data)
        elif scelta == 4:
            self.get_incasso_annuale(data)
        else:
            raise ValueError("Opzione non valida.")

    @abstractmethod
    def get_incasso_totale(self, data):
        pass

    @abstractmethod
    def get_incasso_giornaliero(self, data):
        pass

    @abstractmethod
    def get_incasso_mensile(self, data):
        pass

    @abstractmethod
    def get_incasso_annuale(self, data):
        pass
