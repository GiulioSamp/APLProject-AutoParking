#include "Piano.h"
#include "Posto.h"
#include <iostream>
#include <string>
#include <vector>
#include <memory>

std::string Piano::stampa() {

	std::cout << "Piano: " << num_piano << ", con posti: " << posti_liberi << " su "<<std::to_string(posti.size())<< std::endl;
	return "Piano: " + std::to_string(num_piano+1) + ", con posti: " + std::to_string(posti_liberi) + " su " +std::to_string(posti.size()) +"\n";
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
}

int Piano::occupaPosto() {
	if (posti_liberi != 0) {
		for (int i = 0; i < posti.size(); i++) {
			try {
				if (posti[i].Posto::getStato() == 1) {
					posti[i].Posto::occupaPosto();
					posti_liberi--;
					std::cout << "Posto n." << i + 1 << " occupato!" << std::endl;
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
		std::cout << "Posto n." << num_posto + 1 << " liberato!" << std::endl;
		posti_liberi++;
	}
	catch (std::logic_error e) {
		std::cout << e.what() << std::endl;
	}
}

int Piano::sizeofPosti(){
	return int(posti.size());
}