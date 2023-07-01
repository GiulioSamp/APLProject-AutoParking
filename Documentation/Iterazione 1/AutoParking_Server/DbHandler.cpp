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
        stmt->execute("CREATE TABLE veicolo (targa varchar(40) PRIMARY KEY, marca VARCHAR(50), modello VARCHAR(50), anno INTEGER, utente_id BIGINT UNSIGNED, FOREIGN KEY (utente_id) REFERENCES utente(id) ON DELETE CASCADE);");
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
    pstmt->setString(1, ((std::string)x["nome"].s()));
    pstmt->setString(2, (std::string)x["cognome"].s());
    pstmt->setString(3, (std::string)x["email"].s());
    pstmt->setString(4, (std::string)x["telefono"].s());
    pstmt->setString(5, (std::string)x["pass"].s());
    pstmt->execute();
    current_user = (std::string)x["email"].s();
    current_password = (std::string)x["pass"].s();
    std::cout << "One row inserted. current user: "<< current_user << endl;
    return "Success";
}

std::string DbHandler::add_vehicle(crow::json::rvalue x) {
    if (current_user.length() != 0 && current_password.length() != 0){
        string clear_pass;
            res = stmt->executeQuery("SELECT PASSWORD('" + current_password + "'");
            while (res->next()) {
                clear_pass = res->getString(1);
            }
        res = stmt->executeQuery("SELECT id, email, pass FROM utente WHERE email = '" + current_user + "'");
            cout << "PASSME" << endl;
            while (res->next()) {
                int id = res->getInt("id");
                    if (clear_pass == res->getString(3)) {
                        pstmt = con->prepareStatement("INSERT INTO veicolo(targa, marca, modello, anno, utente_id) VALUES(?,?,?,?,?)");
                        pstmt->setString(1, (std::string)x["targa"].s());
                        pstmt->setString(2, (std::string)x["marca"].s());
                        pstmt->setString(3, (std::string)x["modello"].s());
                        pstmt->setInt(4, x["anno"].i());
                        pstmt->setInt(5, id);
                        pstmt->execute();
                        std::cout << "One row inserted." << endl;
                        return "Success";
                    }
                    else {
                        return "password not valid";
                    }
            }
    }
    else {
        return "User not logged";
    }
}

