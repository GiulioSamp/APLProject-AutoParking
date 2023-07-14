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
    static DbHandler Db;
    Db.start();
    static Parcheggio Park(NUMERO_PIANI, NUMERO_POSTI);

    CROW_ROUTE(app, "/register").methods("POST"_method)
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

    CROW_ROUTE(app, "/vehicle").methods("POST"_method)
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
            });

    CROW_ROUTE(app, "/login").methods("POST"_method)
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

    CROW_ROUTE(app, "/park").methods("POST"_method)
        ([](const crow::request& req) {
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        try {
            string result = Db.register_park(x, Park);
            return crow::response(200, result);
        }
        catch (logic_error e) {
            cout << e.what() << endl;
            return crow::response(400, e.what());
        }
        catch (bad_exception e) {
            return crow::response(400, "Fallimento, veicolo già parcheggiato");
        }
        catch (exception e) {
            return crow::response(400, "Targa non trovata o Utente non riconosciuto");
        }
            });

    CROW_ROUTE(app, "/rvehicle").methods("POST"_method)
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

    CROW_ROUTE(app, "/ruser").methods("POST"_method)
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

    CROW_ROUTE(app, "/pay").methods("POST"_method)
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

    CROW_ROUTE(app, "/endpark").methods("POST"_method)
        ([](const crow::request& req) {
        ostringstream os;
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        //try {
            auto res = Db.retrieve_vehicle(x);
            return crow::response{200, res};
        //}
        //catch (exception e) {
        //    return crow::response(400, "l'id del parcheggio non è stato trovato");
        //}
            });

    app.bindaddr("127.0.0.1").port(18080)
        .multithreaded()
        .run();
    while (true) {

    }

}