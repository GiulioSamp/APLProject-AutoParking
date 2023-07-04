#include "DbHandler.h"
#include <locale>
#include <iostream>
//Include dei drivers del Connector C++ usato per la connessione al db
#include "mysql_connection.h"
#include "mysql_driver.h" 
#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/prepared_statement.h>
#include "..\..\External Libraries\Crow\crow_all.h"

DbHandler::DbHandler() {
	con = nullptr;
}
DbHandler::~DbHandler() {
	delete con;
	delete stmt;
	delete pstmt;
	delete res;
}

std::string DbHandler::start() {
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
        stmt->execute("CREATE TABLE utente (id serial PRIMARY KEY, nome VARCHAR(50), cognome VARCHAR(50), email VARCHAR(230) UNIQUE NOT NULL, telefono varchar(30), pass varchar(256) NOT NULL);");
        stmt->execute("CREATE TABLE veicolo (targa varchar(40) PRIMARY KEY, marca VARCHAR(50), modello VARCHAR(50), anno VARCHAR(4), utente_id BIGINT UNSIGNED, FOREIGN KEY (utente_id) REFERENCES utente(id) ON DELETE CASCADE);");
        stmt->execute("CREATE TABLE utente_parcheggiato(n_posto INT PRIMARY KEY, utente serial, targa VARCHAR(40), data_inizio TIMESTAMP DEFAULT CURRENT_TIMESTAMP, FOREIGN KEY (utente) REFERENCES utente(id) ON DELETE CASCADE, FOREIGN KEY (targa) REFERENCES veicolo(targa) ON DELETE CASCADE ON UPDATE CASCADE);");
        std::cout << "Finished creating table" << endl;
        return (std::string)"Finished creating table";
    }
    catch (sql::SQLException e)
    {
        cout << "Could not connect to server. Error message: " << e.what() << endl;
        system("pause");
        exit(1);
    }
}

std::string DbHandler::add_user(crow::json::rvalue x) {
    pstmt = con->prepareStatement("INSERT INTO utente(nome, cognome, email, telefono, pass) VALUES(?,?,?,?,PASSWORD(?))");
    pstmt->setString(1, ((std::string)x["Nome"].s()));
    pstmt->setString(2, (std::string)x["Cognome"].s());
    pstmt->setString(3, (std::string)x["Email"].s());
    pstmt->setString(4, (std::string)x["Telefono"].s());
    pstmt->setString(5, (std::string)x["Pass"].s());
    pstmt->execute();
    current_user = (std::string)x["Email"].s();
    current_password = (std::string)x["Pass"].s();
    std::cout << "One row inserted. current user: "<< current_user << endl;
    return "Success";
}

std::string DbHandler::check_user(crow::json::rvalue x) {
    string hashed_password;
    if (current_user.length() == 0 && current_password.length() == 0) {
        res = stmt->executeQuery("SELECT PASSWORD('"+(std::string)x["Pass"].s()+"')");
        while (res->next()) {
            hashed_password = res->getString(1);
        }
        res = stmt->executeQuery("SELECT id, email, pass FROM utente WHERE email = '"+(std::string)x["Email"].s()+"'");
        while (res->next()) {
            if (res->getString(2) == (std::string)x["Email"].s() && res->getString(3) == hashed_password) {
                current_user = (std::string)x["Email"].s();
                current_password = (std::string)x["Pass"].s();
                return "User verified, id: " + res->getInt(1);
            }
            else
                return "User not verified";
        }
    }
    else {
        res = stmt->executeQuery("SELECT PASSWORD('" + current_password + "')");
        while (res->next()) {
            hashed_password = res->getString(1);
        }
        res = stmt->executeQuery("SELECT id, email, pass FROM utente WHERE email = '" + current_user + "'");
        while (res->next()) {
            if (res->getString(2) == current_user && res->getString(3) == hashed_password) {
                return "User verified, id: " + res->getInt(1);
            }
            else
                return "User not verified";
        }
    }
}

std::string DbHandler::add_vehicle(crow::json::rvalue x) {
    if (current_user.length() != 0) {
        res = stmt->executeQuery("SELECT id, email, pass FROM utente WHERE email = '" + current_user + "'");
        while (res->next()) {
            pstmt = con->prepareStatement("INSERT INTO veicolo(targa, marca, modello, anno, utente_id) VALUES(?,?,?,?,?)");
            pstmt->setString(1, (std::string)x["Targa"].s());
            pstmt->setString(2, (std::string)x["Marca"].s());
            pstmt->setString(3, (std::string)x["Modello"].s());
            pstmt->setString(4, (std::string)x["Anno"].s());
            pstmt->setInt(5, res->getInt(1));
            pstmt->execute();
            std::cout << "One row inserted." << endl;
            return "Success";
        }
    }
    else
    {
        return "User not logged in, can't proceed!";
    }
}

