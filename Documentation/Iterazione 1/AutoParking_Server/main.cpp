#include "Posto.h"
#include <iostream>
#include "Piano.h"
#include "Parcheggio.h"
//Include dei drivers del Connector C++ usato per la connessione al db
#include "mysql_connection.h"
#include "mysql_driver.h" 
#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/prepared_statement.h>
#include "..\..\External Libraries\Crow\crow_all.h"

using namespace std;
const string server = "tcp://127.0.0.1:3306@autoparkdb";
const string username = "root";
const string password = "";

int main()
{
    sql::Driver* driver;
    static sql::Connection* con;
    sql::Statement* stmt;
    static sql::PreparedStatement* pstmt;

    crow::SimpleApp app;
    con = nullptr;
    try
    {
        driver = get_driver_instance();
        con = driver->connect(server, username, password);
        con->setSchema("autoparkdb");
        stmt = con->createStatement();
        stmt->execute("DROP TABLE IF EXISTS utente_parcheggiato");
        stmt->execute("DROP TABLE IF EXISTS veicolo");
        stmt->execute("DROP TABLE IF EXISTS utente");
        std::cout << "Finished dropping table (if existed)" << endl;
        stmt->execute("CREATE TABLE utente (id serial PRIMARY KEY, nome VARCHAR(50), cognome VARCHAR(50), email VARCHAR(230) UNIQUE NOT NULL, telefono varchar(30) UNIQUE);");
        stmt->execute("CREATE TABLE veicolo (targa varchar(40) PRIMARY KEY, marca VARCHAR(50), modello VARCHAR(50), anno INTEGER, utente_id serial, FOREIGN KEY (utente_id) REFERENCES utente(id) ON DELETE CASCADE);");
        stmt->execute("CREATE TABLE utente_parcheggiato(n_posto INT PRIMARY KEY, utente serial, targa VARCHAR(40), data_inizio TIMESTAMP DEFAULT CURRENT_TIMESTAMP, FOREIGN KEY (utente) REFERENCES utente(id) ON DELETE CASCADE, FOREIGN KEY (targa) REFERENCES veicolo(targa) ON DELETE CASCADE ON UPDATE CASCADE);");
        std::cout << "Finished creating table" << endl;
        std::cout << "Finished creating table" << endl;
        delete stmt;
        /*+pstmt = con->prepareStatement("INSERT INTO utente(name, quantity) VALUES(?,?)");
        pstmt->setString(1, "banana");
        pstmt->setInt(2, 150);
        pstmt->execute();
        cout << "One row inserted." << endl;

        pstmt->setString(1, "orange");
        pstmt->setInt(2, 154);
        pstmt->execute();
        cout << "One row inserted." << endl;

        pstmt->setString(1, "apple");
        pstmt->setInt(2, 100);
        pstmt->execute();
        cout << "One row inserted." << endl;
        delete pstmt;
        delete con;*/
    }
    catch (sql::SQLException e)
    {
        cout << "Could not connect to server. Error message: " << e.what() << endl;
        system("pause");
        exit(1);
    }
    CROW_ROUTE(app, "/register").methods("POST"_method)
        ([](const crow::request& req) {
        auto x = crow::json::load(req.body);
        if (!x) {
            return crow::response(400);
        }
        std::ostringstream os, nome, cognome, email, telefono;
        os << x;
        nome << x["nome"];
        cognome << x["cognome"];
        email << x["email"];
        telefono << x["telefono"];
        pstmt = con->prepareStatement("INSERT INTO utente(nome, cognome, email, telefono) VALUES(?,?,?,?)");
        pstmt->setString(1, nome.str());
        pstmt->setString(2, cognome.str());
        pstmt->setString(3, email.str());
        pstmt->setString(4, telefono.str());
        pstmt->execute();
        std::cout << "One row inserted." << endl;
        delete pstmt;
        return crow::response{os.str()};
        });
    app.bindaddr("127.0.0.1").port(18080)
        .multithreaded()
        .run();

    //return 0;
}