#include "Piano.h"
#include "Posto.h"
#include <iostream>
#include <string>
#include <vector>
#include <memory>


void Piano::stampa() {

	std::cout << "Piano: " << num_piano << ", con posti: " << posti_liberi << std::endl;
}

Piano::Piano(int n, int m) {
	num_piano = n;
	posti_liberi = m;
	for (int i = 0; i < m; i++) {
	posti.push_back(*(new Posto(i)));	
	}
}

Piano::~Piano() {
	posti.clear();
	num_piano = NULL;
	posti_liberi = NULL;
	std::cout << "Piano DISTRUTTO" << std::endl;
}

void Piano::occupaPosto() {
	for (int i = 0; i < posti.size(); i++) {
		if (posti[i].Posto::getStato() == 1) {
			posti[i].Posto::occupaPosto();
			posti_liberi--;
			break;
		}
		else {
			i++;
		}
	}
}

void Piano::liberaPosto() {
	for (int i = 0; i < posti.size(); i++) {
		if (posti[i].Posto::getStato() == 0) {
			posti[i].Posto::liberaPosto();
			posti_liberi++;
			break;
		}
		else {
			i++;
		}
	}
}

std::string Piano::sizeofPosti(){
	return std::to_string(posti.size());
}