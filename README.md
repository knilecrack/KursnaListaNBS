# KursnaListaNBS
Veoma jednostavna aplikacija za parsovanje sajta i generisanje json fajla sa dnevnom kursnom listom.
Posto ne postoji API za fizicka lica od strane NBS(Narodne banke Srbije) parsujemo sajt.

Tabela sa ovog linka:
[Kursna lista](https://nbs.rs/kursnaListaModul/srednjiKurs.faces?lang=lat)

# nemojte da spamujete sajt!!!

## How to build

za Visual Studio 2019 v16.3.10  samo otvorite solution.

iz terminala

```bash, PowerShell,cmd

git clone https://github.com/knilecrack/KursnaListaNBS.git
cd KursnaListaNBS
dotnet restore
dotnet build
dotnet run

```

ako treba moze da se pokrene sa pathom npr: c:\temp i JSON ce biti generisan c:\temp\kursnalista.json

TODO:
- [ ] Treba napraviti argument parser za app, da mogu normalno da se proslede arugmenti gde ce da se generise JSON trenutno je nubovski odradjeno samo da radi