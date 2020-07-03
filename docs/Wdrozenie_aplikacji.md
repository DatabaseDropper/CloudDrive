# Wdrożenie aplikacji

##### 1)	Zakupienie serwera VPS ze strony https://www.ovh.pl/. 
##### 2)	Zalogowanie się do serwera po SSH – wstępne testy serwera.
##### 3)	Instalacja Git’a
##### 4)	Pobranie kodu z repozytorium na serwer za pomocą narzędzia git clone.
##### 5)	Instalacja .net core z repozytorium Microsoft’u

Instalacja przebiegła przy pomocy poniższych komend:

```sh
wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
```
oraz
```sh
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
 sudo apt-get install -y dotnet-sdk-3.1
```

