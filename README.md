# Sistem de Procesare a Documentelor (Laborator 6)

Acest proiect demonstrează utilizarea **Structural Design Patterns** în C# (.NET 8).

## 🛠️ Pattern-uri aplicate:

### 1. Adapter
* **Unde este folosit:** `XmlParserAdapter` (adaptează `LegacyXmlParser` la interfața `IDocumentParser`).
* **Ce problemă rezolvă:** Permite claselor cu interfețe incompatibile să lucreze împreună. Sistemul nou așteaptă ca toate parserele să implementeze metoda `Parse(string content)` care returnează tipul modern `Document`. Sistemul legacy (`LegacyXmlParser`) nu poate fi modificat, folosește `XmlDocument` și returnează `LegacyDocument`. Adapterul traduce cererile din formatul nou în formatul vechi.

### 2. Decorator
* **Unde este folosit:** `LoggingDocumentParser`, `ValidationDocumentParser`, `CachingDocumentParser`.
* **Ce problemă rezolvă:** Oferă o alternativă flexibilă la moștenire pentru extinderea funcționalităților. În loc să creăm clase precum `XmlLoggingAndValidatingParser`, adăugăm comportamente "ambalând" obiectul de bază ca într-o foaie de ceapă. Fiecare decorator își rulează codul (logare, validare, cache) înainte sau după apelarea parserului intern (`_innerParser.Parse()`).

### 3. Facade
* **Unde este folosit:** `DocumentProcessingFacade`.
* **Ce problemă rezolvă:** Oferă o interfață simplificată către un ansamblu complex de clase. Clientul nu trebuie să știe cum să asambleze decoratorii, cum să detecteze formatul sau cum să instanțieze Adapterul. Clientul doar apelează `Process(string)` și obține rezultatul, în timp ce Facade orchestrează detecția, parsarea, validarea, logarea și salvarea fișierului pe disc, tratând și excepțiile intern.
