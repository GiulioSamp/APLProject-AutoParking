#include "PaymentResolver.h"
#include "DataRes.h"

PaymentResolver::PaymentResolver() {

}

PaymentResolver& PaymentResolver::getInstance() {
	static PaymentResolver instance;
	return instance;
}

double PaymentResolver::pay(double cost_fisso, double aggiunta, DataRes& data) {
	DataRes current_date;
	if (data.getMonth() != (current_date.getMonth()))
		PaymentResolver::guadagni_mensili = 0;
	if (data.getDay() != current_date.getDay())
		guadagni_giornalieri = 0;
	int ore = current_date - data;
	double x = cost_fisso + (aggiunta * ore);
	return x;
}

void PaymentResolver::aggiorna_guadagni(double x) {
	guadagni_totali = x;
	guadagni_mensili += x;
	guadagni_giornalieri += x;
}

std::vector<double> PaymentResolver::get_guadagni() {
	std::vector<double> guadagni;
	guadagni.push_back(guadagni_totali);
	guadagni.push_back(guadagni_giornalieri);
	guadagni.push_back(guadagni_mensili);
	return guadagni;
}