# VoiceScript

Desktopová aplikace s uživatelským rozhraním, která na základě hlasově zadávaných příkazů sestaví UML model třídy a odpovídající signaturu v C# kódu. Dále umožňuje uživateli vytvářet nové diagramy nebo modifikovat již hotové s využitím myši a klávesnice, přičemž změny se promítnou samozřejmě také v odpovídajícím kódu. Pro účely zápočtového programu se nebudeme zabývat implementací metod a zaměříme se pouze na signatury.

Pro převod hlasového vstupu na textový bude využívat existující API. Protokol zadávání příkazů bude striktně definovaný co se týče pořadí zadávání, klíčových slov a podobně (při odchýlení od této osnovy se zobrazí hlášení o chybném vstupu). Tento přístup bude sice vyžadovat, aby se uživatel podrobněji seznámil s dokumentací a dodržoval předepsaná pravidla hlasového zadávání, nicméně rozpoznávání příkazů lze postupně rozšiřovat. Pro účely zápočtového programu ale nechceme zacházet do příliš vysoké komplexity.

Aplikace by měla uživateli graficky zobrazit nadefinované UML diagramy a vytvořit `cs` soubor se signaturami tříd / interfaců (dle nastavení). Sestavený C# kód se rovnou validuje a pokud by nebylo možné jej přeložit, oznámí se chyba. Poskytneme uživateli volbu mezi klasickou třídou, abstraktní třídou a interface. Abstraktní třídy s abstraktními metodami stejně jako interface nevyžadují těla metod, lze tedy zobrazit pouze signaturu a kód se přeloží. Metody běžných tříd již tělo vyžadují, vložíme do něj tedy `NotImplementedException`. 

## Motivace

### Pohodlná tvorba objektového návrhu

Aplikace by měla urychlit vizualizaci objektového návrhu a jeho přenesení do podoby samotného kódu. Obecně by bylo užitečné mít nástroj, který umožňuje kód tvořit kombinovaně s využitím hlasového zadávání a klasického vstupu prostřednictvím klávesnice a myši. Přenos myšlenek pomocí hlasu je rychlejší, zároveň ale ne vždy budeme mít pracovní podmínky umožňující hlasové zadávání, takže je vhodné poskytnout hybridní zadávání vstupu.

### Rozšiřitelnost napříč programovacími jazyky

Program má za úkol nejprve sestavit UML diagramy a teprve poté k nim vytvořit odpovídající C# kód. Tento přístup je výhodný z hlediska rozšiřitelnosti - hotové UML diagramy bychom mohli využít nejen pro tvorbu C# kódu, ale obecně kódu v jakémkoli objektově orientovaném jazyce. Náš UML model třídy bude totiž obsahovat všechny potřebné informace pro silnou typovanost kódu, to znamená kromě názvů tříd, položek, metod a vlastností také jejich viditelnost a typy (parametrů metod, návratových hodnot...)

## Práce s aplikací

### Seznámení se s dokumentací

Manuál k aplikaci obsahuje důležité informace k zadávání vstupu. Je potřeba nastudovat:

- **klíčová slova**
  - vytvoření nového diagramu a ukončení jeho tvorby
  - editace existujícího diagramu
- **pořadí zadávání**
  - viditelnost, typ, název
  - povinné a volitelné parametry metod
  - návratové hodnoty metod
- **vzorové vstupy**

### Hlasové zadávání

Uživatel spustí okenní aplikaci a stiskne tlačítko pro spuštění hlasového zadávání. Program začne přijímat slovní příkazy uživatele a průběžně vyhodnocuje výsledky. Dynamicky se aktualizuje obrazovka a zobrazují se nové UML diagramy, jejich části a odpovídající kód. Ten je možné exportovat do samostatného  `cs` souboru nebo jej zobrazit na obrazovce v rámci aplikace. Při chybně zformulovaném vstupu se uživateli zobrazí odpovídající chyba. Hlasové zadávání lze pomocí tlačítka pozastavit.

### Manuální zadávání

Pro vytvoření nového UML diagramu lze využít speciální tlačítko. Při kliknutí myší do existujícího diagramu se spustí modifikace tohoto diagramu a je možné například změnit názvy třídy, položek, metod, přidat nové části diagramu či odstranit existující části.

