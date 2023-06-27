#include "Posto.h"
#include <iostream>
#include <exception>
Posto::Posto(int n) {
	num_posto = n;
	stato = true;
#ifdef _DEBUG
	std::cout << "Posto num." << n << " creato con stato: " << stato << std::endl;
#endif
}

Posto::~Posto() {
	num_posto = NULL;
	stato = NULL;
}

void Posto::stampa() {
	if (stato == 0) {
		std::cout << "Posto: " << num_posto << ", Stato: Occupato" << std::endl;
	}else{
		std::cout << "Posto: " << num_posto << ", Stato: Libero" << std::endl;
	}
}

void Posto::occupaPosto() {
	if (stato == true) {
		stato = false;
	}
	else {
		throw std::logic_error("ERRORE: Posto gia' occupato!");
	}
}

void Posto::liberaPosto() {
		if (stato == false) {
			stato = true;
		}
		else {
			throw std::logic_error("ERRORE: Posto gia' libero!");
		}
}

bool Posto::getStato() {
	return stato;
}