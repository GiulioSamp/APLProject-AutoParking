#pragma once
#include "Posto.h"
#include "Piano.h"
#include <iostream>
#include <vector>

class Parcheggio {
	std::vector<Piano> piani;
	bool stato;
public:
	Parcheggio(int,int);
	~Parcheggio();
	void occupaPosto();
	void liberaPosto(int, int);
	void stampaPosti();
	bool verificaPosto();
};
