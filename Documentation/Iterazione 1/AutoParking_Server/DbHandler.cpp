#include "DbHandler.h"
#include <exception>
#include <locale>
#include <iostream>
#include <tuple>
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
        stmt->execute("CREATE TABLE utente_parcheggiato(id_posto serial PRIMARY KEY,n_piano VARCHAR(3),n_posto VARCHAR(3), utente BIGINT UNSIGNED, targa VARCHAR(40), data_inizio TIMESTAMP DEFAULT CURRENT_TIMESTAMP, FOREIGN KEY (utente) REFERENCES utente(id) ON DELETE CASCADE, FOREIGN KEY (targa) REFERENCES veicolo(targa) ON DELETE CASCADE ON UPDATE CASCADE);");
        std::cout << "Finished creating table" << endl;
        return (std::string)"Finished creating table";
    }
    catch (sql::SQLException e)
    {
        cout << "Error message: " << e.what() << endl;
        system("pause");
        exit(1);
    }
}

std::string DbHandler::add_user(crow::json::rvalue x) {
    try {
        pstmt = con->prepareStatement("INSERT INTO utente(nome, cognome, email, telefono, pass) VALUES(?,?,?,?,PASSWORD(?))");
        pstmt->setString(1, ((std::string)x["Nome"].s()));
        pstmt->setString(2, (std::string)x["Cognome"].s());
        pstmt->setString(3, (std::string)x["Email"].s());
        pstmt->setString(4, (std::string)x["Telefono"].s());
        pstmt->setString(5, (std::string)x["Pass"].s());
        pstmt->execute();
        std::cout << "One row inserted. current user: " << (std::string)x["Email"].s() << endl;
        return "Success";
    }
    catch (sql::SQLException e)
    {
        cout << "Error message: " << e.what() << endl;
        system("pause");
        exit(1);
    }
}

std::string DbHandler::check_user(crow::json::rvalue x) {
    string hashed_password;
        res = stmt->executeQuery("SELECT PASSWORD('"+(std::string)x["Pass"].s()+"')");
        while (res->next()) {
            hashed_password = res->getString(1);
        }
        res = stmt->executeQuery("SELECT id, email, pass FROM utente WHERE email = '"+(std::string)x["Email"].s()+"'");
        if (res->next()) {
            if (res->getString(2) == (std::string)x["Email"].s() && res->getString(3) == hashed_password) {
                return "Success: User verified, id: " + res->getInt(1);
            }
            else
                throw std::bad_exception();
        }
        else {
            throw std::exception();
        }
}

std::string DbHandler::add_vehicle(crow::json::rvalue x) {
        res = stmt->executeQuery("SELECT id, email, pass FROM utente WHERE email = '" + (std::string)x["Email"].s() + "'");
        if (res->next()) {
            pstmt = con->prepareStatement("INSERT INTO veicolo(targa, marca, modello, anno, utente_id) VALUES(?,?,?,?,?)");
            pstmt->setString(1, (std::string)x["Targa"].s());
            pstmt->setString(2, (std::string)x["Marca"].s());
            pstmt->setString(3, (std::string)x["Modello"].s());
            pstmt->setString(4, (std::string)x["Anno"].s());
            pstmt->setInt(5, res->getInt(1));
            pstmt->execute();
            std::cout << "One row inserted." << endl;
            return "Success, added car: "+ (std::string)x["Targa"].s()+" to user: "+ (std::string)x["Email"].s();
        }
    else
    {
        throw std::bad_exception();
        return "Invalid User, can't proceed!";
    }
}

std::string DbHandler::register_park(crow::json::rvalue x,Parcheggio& p) {
    res = stmt->executeQuery("SELECT * FROM utente_parcheggiato WHERE targa = '" + (std::string)x["Targa"].s() + "'");
    if (!res->next()) {
        res = stmt->executeQuery("SELECT id, email, pass FROM utente JOIN veicolo WHERE email = '" + (std::string)x["Email"].s()
            + "' AND targa = '" + (std::string)x["Targa"].s() + "'");
        if (res->next()) {
            std::tuple<int, int> places = p.occupaPosto();
            if (std::get<0>(places) != NULL) {
                pstmt = con->prepareStatement("INSERT INTO utente_parcheggiato(n_posto, n_piano, utente, targa) VALUES(?,?,?,?)");
                pstmt->setString(1, std::to_string(std::get<1>(places)));
                pstmt->setString(2, std::to_string(std::get<0>(places)));
                pstmt->setInt(3, res->getInt(1));
                pstmt->setString(4, (std::string)x["Targa"].s());
                pstmt->execute();
                std::cout << "One row inserted." << endl;
                return "Success, User: " + res->getString(2)
                    + " parked his car: " + (std::string)x["Targa"].s()
                    + " in floor: " + std::to_string(std::get<0>(places))
                    + " and spot: " + std::to_string(std::get<1>(places));
            }
            else {
                throw std::logic_error("Park is full!");  //Eccezione se il parcheggio non ha più posti
                return "Failure: Park is full!";
            }
        }
        else {
            throw std::exception();
            return "Failure: Vehicle registration id not valid!";  //eccezione se nessun veicolo con la targa data è stato trovato
        }
    }
    else
    {
        throw std::bad_exception();
        return "Failure: Vehicle is already parked!";  //Eccezione se il veicolo risulta già parcheggiato
    }
}

crow::json::wvalue DbHandler::retrieveVehicleList(crow::json::rvalue x) {
    res = stmt->executeQuery("SELECT targa, marca, modello, anno FROM utente JOIN veicolo WHERE email = '" + (std::string)x["Email"].s() + "'");
    crow::json::wvalue  veicoli;
    if (!res->next()) {   //guard clause per lanciare un eccezione se l'utente non ha veicoli
        throw std::exception();
    }
    int i = 0;
    res->first();  //riporto il result set al primo valore
    do {
       veicoli[i]["Targa"] = res->getString(1);
       veicoli[i]["Marca"] = res->getString(2);
       veicoli[i]["Modello"] = res->getString(3);
       veicoli[i]["Anno"] = res->getString(4);
       i++;
    } while (res->next());
    return veicoli;
 }

crow::json::wvalue DbHandler::retrieveUser(crow::json::rvalue x) {
    res = stmt->executeQuery("SELECT id, nome, cognome, email, telefono FROM utente WHERE email = '" + (std::string)x["Email"].s() + "'");
    crow::json::wvalue utente;
    if (res->next()) {
        utente["Id"] = res->getInt(1);
        utente["Nome"] = res->getString(2);
        utente["Cognome"] = res->getString(3);
        utente["Email"] = res->getString(4);
        utente["Telefono"] = res->getString(5);
    }
    else {
        throw std::exception();
    }
    return utente;
}

