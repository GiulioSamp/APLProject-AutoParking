#pragma once
#include "Posto.h"
#include <iostream>
#include <vector>

class Piano {
	int num_piano;
	std::vector<Posto> posti;
	int posti_liberi;
public:
	Piano(int, int);
	~Piano();
	std::string stampa();
	int occupaPosto();
	void liberaPosto(int);
	int sizeofPosti();
};