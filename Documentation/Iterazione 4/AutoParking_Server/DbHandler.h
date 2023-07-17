#pragma once
//Include dei drivers del Connector C++ usato per la connessione al db
#include "Parcheggio.h"
#include "mysql_connection.h"
#include "mysql_driver.h" 
#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/prepared_statement.h>
#include "..\..\External Libraries\Crow\crow_all.h"

using namespace std;
class DbHandler {
private:
	const string server = "tcp://127.0.0.1:3306@autoparkdb";
	const string username = "root";
	const string password = "";
	sql::Driver* driver;
	sql::Connection* con;
	sql::Statement* stmt;
	sql::PreparedStatement* pstmt;
	sql::ResultSet* res;
	crow::SimpleApp app;
public:							//Classe Facade che si occupa di interfacciare C++ COnnector al resto del sistema
	DbHandler();
	~DbHandler();
	string start();
	//UTENTE
	string add_user(crow::json::rvalue);
	crow::json::wvalue retrieveUser(crow::json::rvalue);
	string check_user(crow::json::rvalue);
	//VEICOLO
	string add_vehicle(crow::json::rvalue);
	crow::json::wvalue retrieveVehicleList(crow::json::rvalue);
	//PARCHEGGIO
	int get_park_id(string);
	string register_park(crow::json::rvalue, Parcheggio&);
	crow::json::wvalue end_park(crow::json::rvalue, Parcheggio&);
	//PAGAMENTO
	crow::json::wvalue retrieveProfit();
	crow::json::wvalue resolve_payment(crow::json::rvalue);
	//TARIFFA
	int getRate();
	crow::json::wvalue addRate(crow::json::rvalue);
	crow::json::wvalue showRate();
	crow::json::wvalue updateRate(crow::json::rvalue);
};
