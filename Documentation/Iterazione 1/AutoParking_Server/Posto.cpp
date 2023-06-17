#include "Posto.h"
#include <iostream>

Posto::Posto(int n) {
	num_posto = n;
	stato = true;
	std::cout << "Posto num." << n << " creato con stato: " << stato << std::endl;
}

void Posto::stampa() {
	if (stato == 0) {
		std::cout << "Posto: " << num_posto << ", Stato: Occupato" << std::endl;
	}else{
		std::cout << "Posto: " << num_posto << ", Stato: Libero" << std::endl;
	}
}

void Posto::occupaPosto() {
	if (stato == 1) {
		stato = false;
	}
	else {
		std::cout << "Errore: posto gi� occupato" << std::endl;
	}
}

void Posto::liberaPosto() {
	if (stato == 0) {
		stato = true;
	}
	else {
		std::cout << "Posto gi� libero!" << std::endl;
	}
}