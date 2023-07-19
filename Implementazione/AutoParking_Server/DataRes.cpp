#include "DataRes.h"
#include <chrono>
#include <iomanip>
#include <string>

DataRes::DataRes() {
	auto now = std::chrono::system_clock::now();
    std::time_t time = std::chrono::system_clock::to_time_t(now);
    tm local;
    localtime_s(&local, &time);
	y = local.tm_year;
	m = months[local.tm_mon];
	d = std::to_string(local.tm_mday);
	weekday = weekdays[local.tm_wday-1];
	h = local.tm_hour;
}

DataRes::~DataRes() {

}

DataRes::DataRes(string dbdate) {
	y = dbdate.substr(0,4);
	m = months[stoi(dbdate.substr(5, 2))-1];
	d = dbdate.substr(8, 2);
	h = stoi(dbdate.substr(11, 2));
	weekday = weekalgorithm(stoi(d), stoi(dbdate.substr(5, 2)), stoi(y));
}

string DataRes::weekalgorithm(int day, int month, int year){  //algoritmo per calcolare il giorno della settimana a partire dalla data d/m/y
	int mon;
	if (month > 2)
		mon = month;
	else {
		mon = (12 + month);
		year--;
	}
	int y = year % 100;
	int c = year / 100;
	int w = (day + floor((13 * (mon + 1)) / 5) + y + floor(y / 4) + floor(c / 4) + (5 * c));
	w = w % 7;
	return weekdays[w];
};

int DataRes::operator-(const DataRes& parkdate) {
	int d1 = stoi(parkdate.d);
	int d2 = stoi(d);
	int d_diff = d1 - d2;
	if (d_diff == 0) {
		return h - parkdate.h;
	}
	else {
		return ((24 * d_diff) + h) - parkdate.h;
	}
	return 0;
}

string DataRes::getDay() {
	return d;
}

int DataRes::getHour() {
	return h;
}

string DataRes::getMonth() {
	return m;
}

string DataRes::getWeekday() {
	return weekday;
}