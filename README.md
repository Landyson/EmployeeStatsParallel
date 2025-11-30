# EmployeeStatsParallel – stručný popis

## Kde najdu aplikaci

Hotová aplikace je ve složce:

`EmployeeStatsParallel/App`

V ní je hlavní spustitelný soubor:

`App/EmployeeStatsParallel.exe`

---

## Jak aplikaci spustit

1. Otevři složku `App`.
2. Spusť `EmployeeStatsParallel.exe`.
3. Program načte data, spočítá statistiky a výsledek vypíše do konzole
   a uloží do výstupního souboru (dle nastavení v configu).

---

## Jak pracovat s daty

Data o zaměstnancích jsou v souboru:

`App/data/employees.json`

Postup:
- v tomto souboru můžeš přidávat / mazat / upravovat záznamy zaměstnanců,
- po úpravě souboru stačí znovu spustit `EmployeeStatsParallel.exe`.

---

## Jak pracovat s konfigurací

Konfigurační soubor je:

`App/config/config.json`

V configu můžeš změnit například:
- `inputFile` – odkud se berou data (JSON soubor),
- `outputFile` – kam se ukládají výsledné statistiky,
- `workerCount` – kolik paralelních workerů se použije.

Po změně configu stačí znovu spustit aplikaci.

---

## Překopírování aplikace jinam

Pokud chceš aplikaci přenést na jiné místo (jiná složka / disk / PC),
**vždy zkopíruj celou složku `App`**, včetně podadresářů `config` a `data`.

Např.:
- zkopíruj celou složku `App` na flashku,
- na cílovém PC otevři tuhle složku a spusť `EmployeeStatsParallel.exe`.

Bez kompletní složky `App` (hlavně `config` a `data`) nebude aplikace fungovat správně.
