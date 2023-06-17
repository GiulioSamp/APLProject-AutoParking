#include "Posto.h"
#include <iostream>
#include <vector>

class Piano {
	int num_piano;
	bool stato;
	std::vector<Posto> posti;
public:
	void stampa();
	void occupaPosto();
	void liberaPosto();
};