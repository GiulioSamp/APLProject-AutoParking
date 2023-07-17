#include "Posto.h"
#include <iostream>
#include <exception>
#include <chrono>
#include "Piano.h"
#include "Parcheggio.h"
#include "DataRes.h"
#include "DbHandler.h"
//Include dei drivers del Connector C++ usato per la connessione al db
#include "..\..\External Libraries\Crow\crow_all.h"

#define NUMERO_POSTI 50
#define NUMERO_PIANI 10

using namespace std;
int main()
{
    crow::SimpleApp app;
    static DbHandler Db;   //static in quanto la funzione lambda Crow Route non accetta parametri fuori dal suo scope
    Db.start();
    
    static Parcheggio Park(NUMERO_PIANI, NUMERO_POSTI); //static in quanto la funzione lambda Crow Route non accetta parametri fuori dal suo scope
    CROW_ROUTE(app, "/register").methods("POST"_method)  //Endpoint di registrazione utente
        ([](const crow::request& req) {
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        string result = Db.add_user(x);
        std::ostringstream os;
        os << x;
        return crow::response{os.str() + " " + result};
            });

    CROW_ROUTE(app, "/vehicle").methods("POST"_method) //Endpoint di registrazione veicolo
        ([](const crow::request& req) {
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        try {
            string result = Db.add_vehicle(x);
            return crow::response(200, result);
        }
        catch (bad_exception e) {
            return crow::response{400, "Credenziali non valide"};
        }
        catch (exception e) {
            return crow::response(400, "Veicolo già inserito");
        }
            });

    CROW_ROUTE(app, "/login").methods("POST"_method)  //endpoint di login
        ([](const crow::request& req) {
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        try {
            string result = Db.check_user(x);
            return crow::response(200, result);
        }
        catch (bad_exception e) {
            cout << "Incorrect password" << endl;
            return crow::response(400, "Password non valida");
        }
        catch (exception e) {
            cout << "Incorrent credentials" << endl;
            return crow::response(400, "Credenziali non corrette");
        }
            });

    CROW_ROUTE(app, "/park").methods("POST"_method) //Endpoint per registrare un avvenuto parcheggio
        ([](const crow::request& req) {
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        try {
            string result = Db.register_park(x, Park);
            return crow::response(200, result);
        }
        catch (bad_exception e) {
            return crow::response(400, "Fallimento, veicolo già parcheggiato");
        }
        catch (exception e) {
            return crow::response(400, "Targa non trovata o Utente non riconosciuto");
        }
            });

    CROW_ROUTE(app, "/rvehicle").methods("POST"_method)  //Endpoint per ricavare una lista di veicoli di un singolo utente
        ([](const crow::request& req) {
        ostringstream os;
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        try {
            auto veicoli = Db.retrieveVehicleList(x);           //Lista di mappa che contiene le informazioni dei veicoli dell'utente
            return crow::response{200, veicoli};
        }
        catch (exception e) {
            return crow::response(400, "L'utente non ha veicoli registrati!");
        }
        catch (bad_exception e) {
            return crow::response(400, "Utente non riconosciuto");
        }
        });

    CROW_ROUTE(app, "/ruser").methods("POST"_method)  //Endpoint per ricavare le informazioni di un utente
        ([](const crow::request& req) {
        ostringstream os;
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        try {
            auto utente = Db.retrieveUser(x);           //Mappa che contiene le informazioni dell'utente
            return crow::response{200, utente};
        }
        catch (exception e) {
            return crow::response(400, "L'utente non è stato trovato");
        }
            });

    CROW_ROUTE(app, "/pay").methods("POST"_method)  //Endpoint per ricavare l'importo dovuto
        ([](const crow::request& req) {
        ostringstream os;
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        try {
            auto res = Db.resolve_payment(x);           
            return crow::response{200, res};
        }
        catch (exception e) {
            return crow::response(400, "l'id del parcheggio non è stato trovato");
        }
        });

    CROW_ROUTE(app, "/endpark").methods("POST"_method)  //endpoint per terminare una sosta
        ([](const crow::request& req) {
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        try {
            auto res = Db.retrieve_vehicle(x, Park);
            return crow::response{200, res};
        }
        catch (exception e) {
            return crow::response(400, "l'id del parcheggio non è stato trovato");
        }
            });

    CROW_ROUTE(app, "/spots").methods("POST"_method)  //Endpoint per ricavare il numero di posti occupati su posti liberi per piano
        ([](const crow::request& req) {
        string res = Park.stampaPosti();
        crow::json::wvalue posti;
        posti["result"] = res;
        return crow::response(200, posti);
            });

    CROW_ROUTE(app, "/gain").methods("POST"_method)
        ([](const crow::request& req) {
        auto res = Db.retrieveProfit();
        return crow::response(200, res);
            });

    CROW_ROUTE(app, "/addrate").methods("POST"_method)
        ([](const crow::request& req) {
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        try {
            auto res = Db.addRate(x);
            return crow::response(200, res);
        }
        catch (bad_exception e) {
            return crow::response(400, "Questo/i giorno/i ha/hanno già una tariffa applicata!");
        }
            });

    CROW_ROUTE(app, "/showrate").methods("POST"_method)
        ([](const crow::request& req) {
        auto res = Db.showRate();
        return crow::response(200, res);
            });

    CROW_ROUTE(app, "/updaterate").methods("POST"_method)
        ([](const crow::request& req) {
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        try {
            auto res = Db.updateRate(x);
            return crow::response(200, res);
        }
        catch (bad_exception e) {
            return crow::response(400, "Questo/i giorno/i ha/hanno già una tariffa applicata!");
        }
            });

    app.bindaddr("127.0.0.1").port(18080)
        .multithreaded()
        .run();
    while (true) {

    }

}