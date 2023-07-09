#include "Parcheggio.h"
#include "Posto.h"
#include "Piano.h"
#include <iostream>
#include <vector>
#include <exception>
#include <tuple>

Parcheggio::Parcheggio(int num_piani, int num_posti) {
	stato = true;
	for (int i = 1; i < num_piani; i++) {
		piani.push_back(*(new Piano(i,num_posti)));
	}
}

Parcheggio::~Parcheggio() {
	stato = NULL;
	piani.clear();
	std::cout << "Parcheggio DISTRUTTO" << std::endl;
}

std::tuple<int, int>  Parcheggio::occupaPosto() {
	for (int i = 1; i < piani.size(); i++) {
		if (piani[i].Piano::sizeofPosti() != 0) {
			int x = piani[i].Piano::occupaPosto();
			return std::make_tuple(i, x);
		}
		else {
			i++;
		}
		std::cout << "Parcheggio pieno" << std::endl;
		return std::make_tuple(NULL,NULL);
	}
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

void Parcheggio::stampaPosti() {
	for (int i = 1; i < piani.size(); i++) {
		piani[i].Piano::stampa();
	}
}

bool Parcheggio::verificaPosto() {
	for (int i = 1; i < piani.size(); i++) {
		if (piani[i].Piano::sizeofPosti() != 0) {
			return true;
		}
		return false;
	}
}