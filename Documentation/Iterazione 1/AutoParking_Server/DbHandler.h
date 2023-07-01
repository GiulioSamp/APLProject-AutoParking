#pragma once
#include "sha.h"
//Include dei drivers del Connector C++ usato per la connessione al db
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
	string current_user;
	string current_password;
	sql::Driver* driver;
	sql::Connection* con;
	sql::Statement* stmt;
	sql::PreparedStatement* pstmt;
	sql::ResultSet* res;
	crow::SimpleApp app;
public:
	DbHandler();
	~DbHandler();
	string start();
	string add_user(crow::json::rvalue);
	string add_vehicle(crow::json::rvalue);
};
