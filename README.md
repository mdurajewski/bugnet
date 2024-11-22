# bugnet

**BUG.Net #24**

Prezentacja dostępna w folderze `presentations` w formacie .pdf i .pptx.

W folderze głównym aplikacji znajduje się plik `docker-compose.yml` należy w nim uzupełnić hasło inicjacyjne dla GrayLog'a a następnie możemy go uruchomić lokalnie lub na serwerze za pomocą komendy 
```
docker-compose up -d
```
lub
```
docker compose up -d
```
w zależności od zainstalowanej wersji docker i docker-compose.

Przed uruchomieniem w pliku `Program.cs` należy uzupełnić adres do graylog'a oraz skonfigurować Input oraz Stream

Dokumentacja GrayLog: https://go2docs.graylog.org/current/home.htm

*Stream*

![image](https://github.com/user-attachments/assets/ae87e522-e485-434a-b369-b627838a8edf)

*Input*

![image](https://github.com/user-attachments/assets/8ba92282-ee80-405b-963d-1a3f3072e835)

Po konfiguracji upewniamy się ,że *Stream* jak i *Input* mają status `running`.
