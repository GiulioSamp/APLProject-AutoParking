import datetime
import locale
from ABCViewGain import AbstactGetGain

class ConcreteGetGain(AbstactGetGain):

    def get_incasso_totale(self, data):
        totale = 0.0
        for profit in data:
            if "Importo" in profit:
                importo = profit["Importo"]
                totale += float(importo)
        importo_formattato = locale.format_string("%.2f", totale, grouping=True)
        print("Incasso totale:", importo_formattato)

    def get_incasso_giornaliero(self, data):
        year = int(input("Inserisci l'anno: "))
        month = int(input("Inserisci il mese: "))
        day = int(input("Inserisci il giorno: "))
        user_date = datetime.date(year, month, day)
        incasso_giornaliero = 0.0
        for profit in data:
            if "Data" in profit:
                data_part = profit["Data"].split(" ")[0]
                profit_date = datetime.datetime.strptime(data_part, "%Y-%m-%d").date()
                if profit_date == user_date and "Importo" in profit:
                    importo = profit["Importo"]
                    incasso_giornaliero += float(importo)
        importo_formattato = locale.format_string("%.2f", incasso_giornaliero, grouping=True)
        print("Incasso giornaliero:", importo_formattato)

    def get_incasso_mensile(self, data):
        year = int(input("Inserisci l'anno: "))
        month = int(input("Inserisci il mese: "))
        incasso_mensile = 0.0
        for profit in data:
            if "Data" in profit:
                data_part = profit["Data"].split(" ")[0]
                profit_date = datetime.datetime.strptime(data_part, "%Y-%m-%d").date()
                if profit_date.year == year and profit_date.month == month and "Importo" in profit:
                    importo = profit["Importo"]
                    incasso_mensile += float(importo)
        importo_formattato = locale.format_string("%.2f", incasso_mensile, grouping=True)
        print("Incasso mensile:", importo_formattato)

    def get_incasso_annuale(self, data):
        year = int(input("Inserisci l'anno: "))
        incasso_annuale = 0.0
        for profit in data:
            if "Data" in profit:
                data_part = profit["Data"].split(" ")[0]
                profit_date = datetime.datetime.strptime(data_part, "%Y-%m-%d").date()
                if profit_date.year == year and "Importo" in profit:
                    importo = profit["Importo"]
                    incasso_annuale += float(importo)
        importo_formattato = locale.format_string("%.2f", incasso_annuale, grouping=True)
        print("Incasso annuale:", importo_formattato)