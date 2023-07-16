#include "Parcheggio.h"
#include "Posto.h"
#include "Piano.h"
#include <iostream>
#include <vector>
#include <exception>
#include <tuple>

Parcheggio::Parcheggio(int num_piani, int num_posti) {
	stato = true;
	for (int i = 0; i < num_piani; i++) {
		piani.push_back(*(new Piano(i,num_posti)));
	}
}

Parcheggio::~Parcheggio() {
	stato = NULL;
	piani.clear();
	std::cout << "Parcheggio DISTRUTTO" << std::endl;
}

std::tuple<int, int>  Parcheggio::occupaPosto() {
	if (stato == true) {
		for (int i = 0; i < piani.size(); i++) {
			if (piani[i].Piano::sizeofPosti() != 0) {
				int x = piani[i].Piano::occupaPosto();
				return std::make_tuple(i, x);
				break;
			}
			std::cout << "Parcheggio pieno" << std::endl;
			throw std::bad_exception();
		}
	}std::cout << "Parcheggio chiuso" << std::endl;
	throw std::bad_exception();
}

void Parcheggio::liberaPosto(int num_piano,int num_posto) {
	try {
		if (num_piano > piani.size() || num_posto > piani[num_piano].sizeofPosti()) {
			throw std::out_of_range("Errore: il piano o posto selezionato non esistono.");
		}
		piani[num_piano].Piano::liberaPosto(num_posto);
	}
	catch (std::out_of_range e) {
		std::cout << e.what() << std::endl;
	}
}

std::string Parcheggio::stampaPosti() {
	std::string posti;
	for (int i = 0; i < piani.size(); i++) {
		posti += piani[i].Piano::stampa();
	}
	return posti;
}

bool Parcheggio::verificaPosto() {
	for (int i = 0; i < piani.size(); i++) {
		if (piani[i].Piano::sizeofPosti() != 0) {
			return true;
		}
		return false;
	}
}

void Parcheggio::chiudiParcheggio() {
	stato = false;
}

void Parcheggio::apriParcheggio() {
	stato = true;
}