#include "Posto.h"
#include <iostream>
#include "Piano.h"
int main(){
	Piano &p = *(new Piano(0, 20));
	p.stampa();
	p.occupaPosto();
	p.stampa();
	p.liberaPosto();
	p.stampa();
	delete &p;
	return 0;
}
