#include "Posto.h"
#include <iostream>
#include <exception>
#include "Piano.h"
#include "Parcheggio.h"
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
        return crow::response{os.str()+" "+result};
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
            return crow::response{400, "User not logged in, invalid credentials"};
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
            return crow::response(400, "Invalid password");
        }
        catch (exception e) {
            cout << "Incorrent credentials" << endl;
            return crow::response(400, "Login details not present in db");
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
            return crow::response(400, "Failure! Vehicle already parked!");
        }
        catch (exception e) {
            return crow::response(400, "Vehicle registration id is not valid!");
        }
            });

    app.bindaddr("127.0.0.1").port(18080)
        .multithreaded()
        .run();
    while (true) {

    }

    //return 0;
}