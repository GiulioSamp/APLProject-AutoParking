#pragma once
class Posto {
	int num_posto;
	bool stato;
public:
	Posto(int);
	~Posto();
	void stampa();
	void occupaPosto();
	void liberaPosto();
	bool getStato();
};
