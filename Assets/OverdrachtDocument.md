### GameManager Klasse Overzicht

De `GameManager` klasse in Unity beheert de spelcyclus voor een tegelbaserend spel, inclusief het starten, herstarten en beëindigen van het spel.

- **Starten van het spel:** Bij het opstarten roept de `Start()` methode `NewGame()` aan. Hierbij worden de UI-elementen voor 'game over' en 'winnaar' gereset (onzichtbaar en niet-interactief gemaakt) en het spelbord klaargemaakt met twee initiële tegels.

- **Einde van het spel:** De methoden `GameOver()` en `Winner()` handelen het spel einde af, afhankelijk van of de speler heeft verloren of gewonnen. Beide methoden deactiveren het spelbord en maken de relevante eindschermen interactief en zichtbaar met een geleidelijke fade-in effect, geregeld door de `Fade()` coroutine.

- **Fade Effect:** De `Fade()` coroutine past de zichtbaarheid van de UI-elementen aan door de alpha-waarde geleidelijk te interpoleren over de opgegeven duur, wat resulteert in een fade-in van de eindschermen.

### Tile Klasse Overzicht

De `Tile` klasse in Unity beheert de individuele tegels in een tegelbaserend spel, inclusief hun weergave en animatie.

- **Initialisatie:** In de `Awake()` methode worden de componenten `Image` voor de achtergrond en `TextMeshProUGUI` voor tekst geïnitialiseerd.

- **Staat en Weergave:** `SetState(TileState state, string emoji)` configureert de tegel met een specifieke `TileState` en een emoji teken, waarbij de achtergrondkleur en tekst worden aangepast aan de toestand van de tegel.

- **Spawning en Bewegen:**
    - `Spawn(TileCell cell)` plaatst de tegel op een specifieke `TileCell`, waarbij de vorige cel, indien aanwezig, wordt gereset.
    - `MoveTo(TileCell cell)` verplaatst de tegel naar een nieuwe `TileCell` met een animatie die de positie geleidelijk interpoleert.

- **Samenvoegen:** `Merge(TileCell cell)` combineert de tegel met een andere tegel in de opgegeven `TileCell`, waarbij de tegel verwijderd wordt na een animatie als het samenvoegen voltooid is.

- **Animatie:** `Animate(Vector3 to, bool merging)` voert een lineaire interpolatie uit van de huidige positie naar de doelpositie gedurende een korte duur, en verwijdert de tegel indien nodig na het samenvoegen.

Deze functionaliteiten maken de `Tile` klasse een flexibele component voor het beheren van tegelgedrag en animaties in het spel.

### TileBoard Klasse Overzicht

De `TileBoard` klasse in Unity beheert het spelbord voor een tegelbaserend spel. Het regelt tegelbewegingen, samenvoegingen, en spelstatus (winst en verlies).

- **Initialisatie en Tegelbeheer:**
  - `Awake()` initialiseert het spelbord en bereidt een lijst voor met tegels.
  - `ClearBoard()` verwijdert alle tegels van het bord.
  - `CreateTile()` genereert een nieuwe tegel op een willekeurige lege cel met een standaard emoji.

- **Tegelbewegingen:**
  - Gebruikersinput (W, A, S, D of pijltjestoetsen) in `Update()` triggert de beweging van tegels in de gespecificeerde richting.
  - `MoveTiles()` berekent de nieuwe posities voor tegels gebaseerd op de bewegingsrichting en voert bewegingen uit.

- **Samenvoegen van Tegels:**
  - `CanMerge(Tile a, Tile b)` controleert of twee tegels samengevoegd kunnen worden, op basis van emoji gelijkenis en of de doeltegel niet geblokkeerd is.
  - `MergeTiles(Tile a, Tile b)` voegt twee samenvoegbare tegels samen en actualiseert hun staat en emoji.

- **Emoji en Spelstatus Beheer:**
  - `DecideEmoji(string emoji)` bepaalt de nieuwe emoji van een tegel na samenvoegen.
  - `CheckForWin()` en `CheckForGameOver()` evalueren of de speler gewonnen of verloren heeft, respectievelijk.

- **Animaties en Updates:**
  - `WaitForChanges()` coördineert animaties en updates na elke beweging, controleert spelstatus en voegt nieuwe tegels toe indien nodig.

Deze klasse maakt gebruik van dynamische gebruikersinteracties en geavanceerde spellogica om een responsieve en uitdagende spelervaring te bieden.

### TileCell Klasse Overzicht

De `TileCell` klasse in Unity fungeert als een container voor een enkele cel binnen een tegelgrid. Deze klasse is cruciaal voor het beheer van de positie en status van tegels op het bord.

- **Eigenschappen:**
  - `position`: Bewaart de positie van de cel binnen het grid als een `Vector2Int`.
  - `tile`: Houdt de tegel bij die momenteel in deze cel is geplaatst, of is `null` als de cel leeg is.

- **Status Checks:**
  - `empty`: Geeft terug of de cel leeg is (`true` als er geen tegel is).
  - `hasTile`: Geeft aan of er een tegel in de cel aanwezig is (`true` als er een tegel is).

Deze klasse biedt een eenvoudige maar effectieve manier om de toewijzing van tegels aan cellen in het spelgrid te beheren, wat essentieel is voor de spellogica van tegelbewegingen en -interacties.

### TileGrid Klasse Overzicht

De `TileGrid` klasse in Unity beheert de structuur en organisatie van het tegelgrid in een tegelbaserend spel. Deze klasse zorgt voor het positioneren en toegankelijk maken van cellen binnen het grid.

- **Initialisatie en Configuratie:**
  - `Awake()`: Verzamelt alle `TileRow` en `TileCell` componenten binnen het grid.
  - `Start()`: Stelt de positie in van elke cel in het grid, waardoor elke cel zijn locatie binnen het grid kent.

- **Eigenschappen:**
  - `size`: Het totaal aantal cellen in het grid.
  - `height`: Het aantal rijen in het grid.
  - `width`: Het aantal cellen per rij, berekend als totaal aantal cellen gedeeld door het aantal rijen.

- **Celtoegang en Manipulatie:**
  - `Getcell(int x, int y)`: Geeft een specifieke cel terug gebaseerd op x- en y-coördinaten, of `null` als de coördinaten buiten het grid vallen.
  - `Getcell(Vector2Int position)`: Overbelaste methode die dezelfde functionaliteit biedt met een `Vector2Int` positie.
  - `GetAdjacentCell(TileCell cell, Vector2Int direction)`: Berekent de aangrenzende cel in een gegeven richting van een specifieke cel.
  - `GetRandomEmptyCell()`: Zoekt een willekeurige lege cel in het grid, handig voor het plaatsen van nieuwe tegels.

Deze klasse speelt een cruciale rol in de dynamiek van het spel door het beheren van het grid waarop alle spelacties plaatsvinden.

### TileRow Klasse Overzicht

De `TileRow` klasse in Unity is ontworpen om de cellen binnen een enkele rij van een tegelgrid te beheren. Deze klasse speelt een fundamentele rol in de organisatie van de cellen die bij elk spelonderdeel betrokken zijn.

- **Initialisatie:**
  - `Awake()`: Verzamelt alle `TileCell` componenten die kinderen zijn van dit GameObject. Dit zorgt ervoor dat elke rij een directe referentie heeft naar de cellen die het bevat.

- **Eigenschappen:**
  - `cells`: Een array van `TileCell` die alle cellen in deze specifieke rij vertegenwoordigt. Dit stelt andere klassen in staat om gemakkelijk toegang te krijgen tot elke cel binnen de rij.

Deze klasse is essentieel voor het structureren van het tegelgrid, waardoor elke rij afzonderlijk kan worden beheerd en gemanipuleerd binnen de grotere gridstructuur.


### TileState Klasse Overzicht

De `TileState` klasse in Unity is een `ScriptableObject` die bedoeld is om de visuele staat van een tegel in een tegelbaserend spel te definiëren. Dit maakt het eenvoudig om verschillende visuele eigenschappen van tegels te beheren en te variëren.

- **Functionaliteiten:**
  - **`backgroundColor`**: Dit veld slaat de achtergrondkleur op die gebruikt wordt voor de tegel wanneer deze zich in een specifieke staat bevindt. Hierdoor kan de uiterlijke presentatie van de tegel aangepast worden op basis van de spellogica.

- **ScriptableObject:**
  - Als een `ScriptableObject`, kan `TileState` in de Unity-editor gecreëerd en geconfigureerd worden zonder dat het aan een specifiek GameObject vastzit. Dit zorgt voor een modulaire en herbruikbare aanpak voor het definiëren van tegelstaten, die gemakkelijk tussen verschillende projecten of componenten gedeeld kan worden.

Deze klasse biedt een krachtige en flexibele manier om de visuele aspecten van tegels te manipuleren, wat cruciaal is voor het duidelijk weergeven van verschillende speltoestanden.
