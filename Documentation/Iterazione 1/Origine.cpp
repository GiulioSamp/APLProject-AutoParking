#include <iostream>
#include <winsock2.h>
#include <thread>

#pragma comment(lib, "ws2_32.lib")
/// note
///  è una direttiva specifica del compilatore di Microsoft Visual Studio.
///  Viene utilizzata per specificare al compilatore di collegare automaticamente la libreria "ws2_32.lib"

#define BUFFER_SIZE 1024



void handleClientConnection(SOCKET clientSocket) {
    char buffer[BUFFER_SIZE]; //array di caratteri utilizzato come buffer per memorizzare i dati ricevuti dal client
    memset(buffer, 0, sizeof(buffer));
    /*viene utilizzata per impostare tutti i byte all'interno di un blocco di memoria a un valore specifico.
   
   //memset viene utilizzata per impostare tutti i byte all'interno di un blocco di memoria a un valore specific
    //La funzione memset accetta tre argomenti:

    Il primo argomento è un puntatore al blocco di memoria da inizializzare.
        Il secondo argomento è il valore da assegnare a ciascun byte nel blocco di memoria(in questo caso, 0 per impostare i byte a zero).
        Il terzo argomento è la dimensione in byte del blocco di memoria.
        In pratica, memset(buffer, 0, sizeof(buffer)) azzera tutti i byte nell'array buffer, garantendo che sia completamente vuoto e pronto*/
  
    while (true) {
        // Ricezione dei dati dal client
        int bytesRead = recv(clientSocket, buffer, sizeof(buffer) - 1, 0);
        if (bytesRead <= 0) {
            std::cerr << "Errore nella ricezione dei dati dal client" << std::endl;
            break;
        }

        // Stampa dei dati ricevuti
        std::cout << "Dati ricevuti dal client: " << buffer << std::endl;

        // Esempi di logica aggiuntiva
        // ...

        // Invio di una risposta al client (opzionale)
        const char* response = "Risposta dal server";
        send(clientSocket, response, strlen(response), 0);

        // Pulizia del buffer per la prossima ricezione
        memset(buffer, 0, sizeof(buffer));
    }

    // Chiusura del socket del client
    closesocket(clientSocket);
}

/// <summary>
        /// la struttura WSADATA viene utilizzata per memorizzare le informazioni di inizializzazione di Winsock.
        /// Questa struttura viene passata come argomento alla funzione WSAStartup().
        /// 

/// </summary>
void startServer() {
    // Inizializza Winsock
    /*modo piu long
    WORD ver = MAKEWORD(2, 2);
    è una macro definita nella libreria Winsock
    che viene utilizzata per creare un valore WORD (word) a 16 bit utilizzando due byte specificat
    viene utilizzato per specificare che si desidera utilizzare la versione 2.2 di Winsock
    he viene poi passato a WSAStartup() per inizializzare correttamente Winsock con quella versione specifica,
    altre versioni non stabili
    int wsOk = WSAStartup(ver, &wsData);
    if (wsOk != 0)*/
    WSADATA wsaData;
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
        std::cerr << "Errore durante l'inizializzazione di Winsock" << std::endl;
        return;
    }

    // Crea la socket del server
    SOCKET serverSocket = socket(AF_INET, SOCK_STREAM, 0);
    if (serverSocket == INVALID_SOCKET) {
        std::cerr << "Impossibile creare la socket del server" << std::endl;
        WSACleanup();
        return;
    }

    // Imposta l'indirizzo e la porta del server
    sockaddr_in serverAddress;
    serverAddress.sin_family = AF_INET;
    //serverAddress.sin_addr.s_addr = InetPton("127.0.0.1");
    serverAddress.sin_addr.s_addr = INADDR_ANY;
    serverAddress.sin_port = htons(12345); // Esempio di porta 12345

    // Effettua il binding della socket del server
    if (bind(serverSocket, (struct sockaddr*)&serverAddress, sizeof(serverAddress)) == SOCKET_ERROR) {
        std::cerr << "Errore durante il binding della socket del server" << std::endl;
        closesocket(serverSocket);
        WSACleanup();
        return;
    }

    // Mette il server in ascolto
    if (listen(serverSocket, SOMAXCONN) == SOCKET_ERROR) {
        std::cerr << "Errore durante l'ascolto del server" << std::endl;
        closesocket(serverSocket);
        WSACleanup();
        return;
    }

    std::cout << "Server in ascolto sulla porta 12345..." << std::endl;

    while (true) {
        // Accetta una connessione in arrivo
        SOCKET clientSocket = accept(serverSocket, NULL, NULL);
        if (clientSocket == INVALID_SOCKET) {
            std::cerr << "Errore durante l'accettazione della connessione del client" << std::endl;
            closesocket(serverSocket);
            WSACleanup();
            return;
        }

        // Gestisci la connessione client in un thread separato
        std::thread(handleClientConnection, clientSocket).detach();
    }

    // Chiudi la socket del server
    closesocket(serverSocket);


    // Termina Winsock
    WSACleanup();
    // viene utilizzata per terminare l'utilizzo della libreria Winsock una volta 
    // che il server ha finito di utilizzarla. È una pratica buona e necessaria per liberare 
    // le risorse allocate da Winsock e ripulire l'ambiente Quando si inizializza Winsock
    //utilizzando WSAStartup(), viene effettuata una chiamata di inizializzazione che imposta
    //lo stato interno di Winsock. Allo stesso modo, WSACleanup() esegue la chiusura e la pulizia di Winsock,
}

int main() {
    startServer();
    return 0;
}

/// <summary>
/// la funzione WSAStartup, viene creata la socket del server con la funzione socket, 
/// viene effettuato il binding della socket al proprio indirizzo con la funzione bind,
/// viene avviato l'ascolto sulla socket con la funzione listen
/// </summary>