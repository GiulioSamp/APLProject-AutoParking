#include "Posto.h"
#include <iostream>

int main(){
	Posto p(0);
	p.occupaPosto();
	p.stampa();
	p.liberaPosto();
	p.stampa();
	return 0;
}