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

##### 6)	Instalacja Docker’a
###
```sh
$ sudo apt-get update
$ sudo apt-get install \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg-agent \
    software-properties-common
```	

```sh
$ curl -fsSL https://download.docker.com/linux/debian/gpg | sudo apt-key add -
```	

```sh
$ sudo apt-key fingerprint 0EBFCD88
pub   4096R/0EBFCD88 2017-02-22
      Key fingerprint = 9DC8 5822 9FC7 DD38 854A  E2D8 8D81 803C 0EBF CD88
uid                  Docker Release (CE deb) <docker@docker.com>
sub   4096R/F273FCD8 2017-02-22
$ sudo add-apt-repository \
   "deb [arch=amd64] https://download.docker.com/linux/debian \
   $(lsb_release -cs) \
   stable"
```	

Komendy do instalacji:

```sh
$ sudo apt-get update
 $ sudo apt-get install docker-ce docker-ce-cli containerd.io
$ apt-cache madison docker-ce

  docker-ce | 5:18.09.1~3-0~debian-stretch | https://download.docker.com/linux/debian stretch/stable amd64 Packages
  docker-ce | 5:18.09.0~3-0~debian-stretch | https://download.docker.com/linux/debian stretch/stable amd64 Packages
  docker-ce | 18.06.1~ce~3-0~debian        | https://download.docker.com/linux/debian stretch/stable amd64 Packages
  docker-ce | 18.06.0~ce~3-0~debian        | https://download.docker.com/linux/debian stretch/stable amd64 Packages
  ...
```	

```sh
$ sudo apt-get install docker-ce=<VERSION_STRING> docker-ce-cli=<VERSION_STRING> containerd.io
```	

##### 7)	Instalacja nginx
###
```sh
$ sudo apt-get install nginx
```	

##### 8)	Instalacja bazy MSSQL
###
Skopiowanie skryptu + jego uruchomienie 
Wejsćie do folderu z pobranym projektem a następnie wejście do folderu dokumenty i podfolderu docker. W folderze tym odpalamy skrypt komendą 
```sh
$ sudo ./docker_create_local_MSSQL.sh
```	
Następnie sprawdzamy czy baza wstała: 
```sh
$ sudo docker ps –a
```	
##### 9)	Konfiguracja serwera http nginx
###
Zmianiemy ścieżkę na
```sh
/etc/nginx/sites-available
```	
Tworzymy plik default
```sh
$ sudo nano default 
```	
```sh
server {
    listen        80;
    server_name   example.com *.example.com;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}
```	

Zmianiamy ścieżkę na
```sh
project/CloudDrive/src/ 
```	
gdzie instalujemy obsługę FrontEndu
```sh
$ sudo apt-get install tmux 
dotnet run
curl –sl https://deb.nodesource.com/setup_14.x | bash - sudo apt –get install –y node/js
```	

##### 10)	Końcowym etapem napotkano niewielki problem z adresacją, do jego rozwiązania powołany został cały zespół odpowiedzialny za projekt. Wspólnymi siłamy szybko udało nam się pożegnać z problemem
###
##### 11)	Testy aplikacji na serwerze
###
Po udanym wdrożeniu aplikacji zaczeliśmy przeprowadzać testy aplikacji. Zostały wrzucone przykładowe pliki I udostępnione. Zostały założone konta użytkowników. Wysłany został również link z testowym plikiem do zaprzyjaźnionych osób aby sprawdzić działanie aplikacji.