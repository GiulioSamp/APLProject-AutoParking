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
	for (int i = 1; i < m; i++) {
		posti.push_back(*(new Posto(i)));	
	}
}

Piano::~Piano() {
	posti.clear();
	num_piano = NULL;
	posti_liberi = NULL;
}

int Piano::occupaPosto() {
	if (posti_liberi != 0) {
		for (int i = 1; i < posti.size(); i++) {
			try {
				if (posti[i].Posto::getStato() == 1) {
					posti[i].Posto::occupaPosto();
					posti_liberi--;
					std::cout << "Posto n." << i << " occupato!" << std::endl;
					return i;
				}
			}
			catch (std::logic_error e) {
				std::cout << e.what() << std::endl;
			}
		}
	}
	else
	{
		return NULL;
	}
}

void Piano::liberaPosto(int num_posto) {
	try {
		posti[num_posto].liberaPosto();
		std::cout << "Posto n." << num_posto << " liberato!" << std::endl;
		posti_liberi++;
	}
	catch (std::logic_error e) {
		std::cout << e.what() << std::endl;
	}
}

int Piano::sizeofPosti(){
	return int(posti.size());
}