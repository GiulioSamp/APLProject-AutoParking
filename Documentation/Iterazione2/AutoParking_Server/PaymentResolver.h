#pragma once
#include "DataRes.h"

class PaymentResolver {
private:
	double guadagni_totali;
	double guadagni_giornalieri;
	double guadagni_mensili;
	PaymentResolver();
public:
	PaymentResolver(PaymentResolver& other) = delete;
	void operator=(const PaymentResolver&) = delete;
	static PaymentResolver& getInstance();
	double pay(double, double, DataRes&);
	void aggiorna_guadagni(double);
};
