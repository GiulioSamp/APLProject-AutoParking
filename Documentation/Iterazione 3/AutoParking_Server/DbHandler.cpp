#include "DbHandler.h"
#include "PaymentResolver.h"
#include <exception>
#include <locale>
#include <iostream>
#include <tuple>
#include "DataRes.h"
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
        stmt->execute("DROP TABLE IF EXISTS guadagno");
        stmt->execute("DROP TABLE IF EXISTS utente_parcheggiato");  //Cancellazione tabelle per facilitare fase di testing
        stmt->execute("DROP TABLE IF EXISTS veicolo");
        stmt->execute("DROP TABLE IF EXISTS utente");
        stmt->execute("DROP TABLE IF EXISTS tariffa");
        std::cout << "Finished dropping table (if existed)" << endl;
        stmt->execute("CREATE TABLE utente (id serial PRIMARY KEY, nome VARCHAR(50), cognome VARCHAR(50), email VARCHAR(230) UNIQUE NOT NULL, telefono varchar(30), pass varchar(256) NOT NULL);");
        stmt->execute("CREATE TABLE veicolo (targa varchar(40) PRIMARY KEY, marca VARCHAR(50), modello VARCHAR(50), anno VARCHAR(4), tipo VARCHAR(30), utente_id BIGINT UNSIGNED, FOREIGN KEY (utente_id) REFERENCES utente(id) ON DELETE CASCADE);");
        stmt->execute("CREATE TABLE tariffa (id serial PRIMARY KEY, val_fisso DOUBLE NOT NULL, aggiunta_ore DOUBLE NOT NULL, giorni VARCHAR(200) UNIQUE NOT NULL)");
        stmt->execute("CREATE TABLE utente_parcheggiato("
            " id_parcheggio serial PRIMARY KEY,"
            " n_piano VARCHAR(3),n_posto VARCHAR(3),"
            " utente BIGINT UNSIGNED, targa VARCHAR(40),"
            " data_inizio TIMESTAMP DEFAULT CURRENT_TIMESTAMP,"
            " tariffa BIGINT UNSIGNED,"
            " FOREIGN KEY (utente) REFERENCES utente(id) ON DELETE CASCADE,"
            " FOREIGN KEY (targa) REFERENCES veicolo(targa) ON DELETE CASCADE ON UPDATE CASCADE,"
            " FOREIGN KEY (tariffa) REFERENCES tariffa(id) ON DELETE CASCADE);");
        stmt->execute("CREATE TABLE guadagno("
            " id_transazione serial PRIMARY KEY,"
            " importo DOUBLE NOT NULL,"
            " utente BIGINT UNSIGNED, targa VARCHAR(40),"
            " data TIMESTAMP DEFAULT CURRENT_TIMESTAMP,"
            " tariffa BIGINT UNSIGNED,"
            " FOREIGN KEY (utente) REFERENCES utente(id) ON DELETE CASCADE,"
            " FOREIGN KEY (targa) REFERENCES veicolo(targa) ON DELETE CASCADE ON UPDATE CASCADE,"
            " FOREIGN KEY (tariffa) REFERENCES tariffa(id) ON UPDATE CASCADE);");
        pstmt = con->prepareStatement("INSERT INTO tariffa(val_fisso, aggiunta_ore, giorni) VALUES(?,?,?)");
        pstmt->setDouble(1, 1.50);
        pstmt->setDouble(2, 2.0);
        pstmt->setString(3, "tutti");
        pstmt->execute();
        pstmt = con->prepareStatement("INSERT INTO tariffa(val_fisso, aggiunta_ore, giorni) VALUES(?,?,?)");
        pstmt->setDouble(1, 2.50);
        pstmt->setDouble(2, 3.0);
        pstmt->setString(3, "sabato, domenica");
        pstmt->execute();
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
    res = stmt->executeQuery("SELECT targa FROM veicolo WHERE targa = '" + (std::string)x["Targa"].s() + "'");
    if (!res->next()) {
        res = stmt->executeQuery("SELECT id, email, pass FROM utente WHERE email = '" + (std::string)x["Email"].s() + "'");
        if (res->next()) {
            pstmt = con->prepareStatement("INSERT INTO veicolo(targa, marca, modello, anno, utente_id, tipo) VALUES(?,?,?,?,?,?)");
            pstmt->setString(1, (std::string)x["Targa"].s());
            pstmt->setString(2, (std::string)x["Marca"].s());
            pstmt->setString(3, (std::string)x["Modello"].s());
            pstmt->setString(4, (std::string)x["Anno"].s());
            pstmt->setString(6, (std::string)x["Tipo"].s());
            pstmt->setInt(5, res->getInt(1));
            pstmt->execute();
            std::cout << "One row inserted." << endl;
            return "Success, added car: " + (std::string)x["Targa"].s() + " to user: " + (std::string)x["Email"].s();
        }
        else
        {
            throw std::bad_exception();
        }
    }
    else {
        throw std::exception();
    }
}

int DbHandler::get_tariffa() {
    DataRes wday;
    res = stmt->executeQuery("SELECT id FROM tariffa WHERE giorni LIKE '%" + wday.getWeekday() + "%'");
    if (res->next()) {
        return res->getInt(1);
    }
    else {
        res = stmt->executeQuery("SELECT id FROM tariffa WHERE giorni LIKE '%tutti%'");
        while (res->next()) {
            return res->getInt(1);
        }
    }
}

int DbHandler::get_park_id(string targa) {
    res = stmt->executeQuery("SELECT * FROM utente_parcheggiato WHERE targa = '" + targa + "'");
    while (res->next()) {
        return res->getInt("id_parcheggio");
    }
}

std::string DbHandler::register_park(crow::json::rvalue x, Parcheggio& p) {
    res = stmt->executeQuery("SELECT * FROM utente_parcheggiato WHERE targa = '" + (std::string)x["Targa"].s() + "'");
    if (!res->next()) {
        res = stmt->executeQuery("SELECT id, email, pass FROM utente JOIN veicolo WHERE email = '" + (std::string)x["Email"].s()
            + "' AND targa = '" + (std::string)x["Targa"].s() + "'");
        if (res->next()) {
            try {
                std::tuple<int, int> places = p.occupaPosto();
                pstmt = con->prepareStatement("INSERT INTO utente_parcheggiato(n_posto, n_piano, utente, targa, tariffa) VALUES(?,?,?,?,?)");
                pstmt->setString(1, std::to_string(std::get<1>(places)));
                pstmt->setString(2, std::to_string(std::get<0>(places)));
                pstmt->setInt(3, res->getInt(1));
                pstmt->setString(4, (std::string)x["Targa"].s());
                pstmt->setInt(5, get_tariffa());
                pstmt->execute();
                std::cout << "One row inserted." << endl;
                return "Successo, Utente: " + (std::string)x["Email"].s() +
                    +"\nveicolo parcheggiato: " + (std::string)x["Targa"].s()
                    + "\n Piano: " + std::to_string(std::get<0>(places))
                    + "\n Posto: " + std::to_string(std::get<1>(places))
                    + "\nid parcheggio per il ritiro: " + std::to_string(get_park_id((std::string)x["Targa"].s()));
            }
            catch (std::bad_exception e) {
                return "Parcheggio Pieno!";
            }
        }
    else {
        throw std::exception(); //eccezione se nessun veicolo con la targa data è stato trovato
    }
    }
    else
    {
        throw std::bad_exception();  //Eccezione se il veicolo risulta già parcheggiato
    }
}

crow::json::wvalue DbHandler::retrieve_vehicle(crow::json::rvalue x, Parcheggio& p) {
    crow::json::wvalue result;
    res = stmt->executeQuery("SELECT n_piano, n_posto, id_parcheggio FROM utente_parcheggiato WHERE id_parcheggio = '" + (std::string)x["Id"].s() + "'");
    if (res->next()) {
        pstmt->execute();
        result["piano"] = res->getString(1);
        result["posto"] = res->getString(2);
        result["id"] = res->getInt(3);
        p.liberaPosto(stoi(res->getString(1)), stoi(res->getString(2)));
        pstmt = con->prepareStatement("DELETE FROM utente_parcheggiato WHERE id_parcheggio = '" + (std::string)x["Id"].s() + "'");
        pstmt->execute();
        return result;
    }
    else {
        throw std::exception();
    }
}


crow::json::wvalue DbHandler::resolve_payment(crow::json::rvalue x) {
    crow::json::wvalue result;
    res = stmt->executeQuery("SELECT data_inizio, tariffa, targa, utente FROM utente_parcheggiato WHERE id_parcheggio = '" + (std::string)x["Id"].s() + "'");
    if (res->next()) {
        string date = res->getString(1);
        DataRes data(date);
        int user = res->getInt("utente");
        string car = res->getString("targa");
        int tariffa = res->getInt(2);
        res = stmt->executeQuery("SELECT val_fisso, aggiunta_ore, id FROM tariffa WHERE id = '" + std::to_string(res->getInt(2)) + "'");
        while (res->next()) {
            double n = PaymentResolver::getInstance().pay(res->getDouble(1), res->getDouble(2), data);
            pstmt = con->prepareStatement("INSERT INTO guadagno(importo, utente, targa, tariffa) VALUES(?,?,?,?)");
            pstmt->setDouble(1, n);
            pstmt->setInt(2, user);
            pstmt->setString(3, car);
            pstmt->setInt(4, tariffa);
            result["costo"] = n;
            return result;
        }
    }
    else {
        throw std::exception();
    }
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
        throw std::exception();  //L'utente non esiste nel db
    }
    return utente;
}

crow::json::wvalue DbHandler::retrieveVehicleList(crow::json::rvalue x) {
    crow::json::wvalue  veicoli;
    res = stmt->executeQuery("SELECT targa, marca, modello, anno FROM veicolo join utente WHERE email = '" + (std::string)x["Email"].s() + "' AND id = utente_id");
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

crow::json::wvalue DbHandler::retrieveProfit() {
    crow::json::wvalue  profit;
    res = stmt->executeQuery("SELECT * FROM guadagno");
    if (!res->next()) {   //guard clause per lanciare un eccezione se l'utente non ha veicoli
        throw std::exception();
    }
    int i = 0;
    res->first();  //riporto il result set al primo valore
    do {
        profit[i]["IdTransazione"] = res->getInt(1);
        profit[i]["Importo"] = std::to_string(res->getDouble(2));
        profit[i]["Utente"] = to_string(res->getInt(3));
        profit[i]["Targa"] = res->getString(4);
        profit[i]["Data"] = res->getString(5);
        profit[i]["Tariffa"] = to_string(res->getInt(6));
        i++;
    } while (res->next());
    return profit;
}


