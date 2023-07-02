#include "Posto.h"
#include <iostream>
#include "Piano.h"
#include "Parcheggio.h"
#include "DbHandler.h"
//Include dei drivers del Connector C++ usato per la connessione al db
#include "..\..\External Libraries\Crow\crow_all.h"

using namespace std;
int main()
{
    crow::SimpleApp app;
    static DbHandler Db;
    Db.start();
    CROW_ROUTE(app,"/register").methods("POST"_method)
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
    CROW_ROUTE(app,"/vehicle").methods("POST"_method)
        ([](const crow::request& req) {
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        string result = Db.add_vehicle(x);
        if (result == "Success") {
            std::ostringstream os;
            os << x;
            return crow::response{os.str() + " " + result};
        }
        else {
            return crow::response{"User not registered or wrong password"};
        }
        });
    app.bindaddr("127.0.0.1").port(18080)
        .multithreaded()
        .run();

    //return 0;
}