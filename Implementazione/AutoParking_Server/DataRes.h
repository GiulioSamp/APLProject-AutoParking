#pragma once
#include <iostream>

using namespace std;
//Classe per gestire la data del parcheggio avvenuto e confrontarla a quella attuale, classe prevalentemente di supporto per PaymentResolver e DbHandler
class DataRes {
private:
	string y;
	string m;
	string d;
	string weekday;
	const string weekdays[7] = { "lunedì","martedì","mercoledì","giovedì","venerdì","sabato","domenica" };
	const string months[12] = { "Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre" };
	int h;

public:
	DataRes();
	DataRes(string);
	~DataRes();
	time_t String2time_t(const string&);
	int operator-(const DataRes&);
	string weekalgorithm(int, int, int);
	string getDay();
	string getMonth();
	string getWeekday();
	int getHour();
};