# KursnaListaNBS

Posto ne postoji API za fizicka lica od strane NBS(Narodne banke Srbije) parsujemo sajt.

Tabela sa ovog linka:
[Kursna lista](https://nbs.rs/kursnaListaModul/srednjiKurs.faces?lang=lat)
se pretvara u JSON fajl

nemojte da spamujete sajt!!!

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
