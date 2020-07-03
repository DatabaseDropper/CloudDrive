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