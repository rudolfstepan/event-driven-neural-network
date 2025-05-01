# Event-Driven Neural Network

Ein leichtgewichtiges, ereignisbasiertes neuronales Netzwerk-Framework in C#.

Dieses Projekt implementiert ein asynchrones, modular aufgebautes neuronales Netzwerk, das auf eintreffende Events reagiert. Die Neuronen feuern bei Erreichen eines Schwellwertes und kÃ¶nnen Ã¼ber Events miteinander kommunizieren. Es ist universell einsetzbar fÃ¼r Mustererkennung, Ereignisverarbeitung oder simulationsbasierte Logik.

---

## Eigenschaften

- **Eventbasierte Architektur:** Neuronen feuern, wenn ihre Eingabe einen Schwellenwert Ã¼berschreitet.
- **Trainierbar & modular:** Neuronen lassen sich manuell oder datengetrieben kalibrieren.
- **Anwendungsagnostisch:** Egal ob Bildverarbeitung, Sensordatenauswertung oder Entscheidungsfindung.
- **Einfache Erweiterbarkeit:** ZusÃ¤tzliche Neurontypen oder Lernstrategien sind leicht integrierbar.

---

## Projektstruktur

- `EventNeuron.cs` â€“ Zentrales Neuron mit Event-Trigger.
- `VisualNet.cs` â€“ 2D-Netzwerk aus Neuronen, ideal fÃ¼r Pixel- oder Sensormuster.
- `CentralSignal.cs` â€“ Globale Koordinationsinstanz fÃ¼r kollektives Verhalten.
- `Program.cs` â€“ Beispiel zur Demonstration und einfachem Training mit Bilddaten.
- `testImages/` â€“ Beispielbilder zur Mustererkennung.

---

## MÃ¶gliche AnwendungsfÃ¤lle

- **Mustererkennung** (Bilddaten, QR-Codes, Handschrift)
- **EchtzeitÃ¼berwachung** (Sensorwerte, Bewegungsdaten)
- **Neuronale Simulationen** (Agentenverhalten, KI-Entscheidungen)
- **Regelbasierte Reaktionssysteme** (z.â€¯B. Home Automation)

---

## Schnellstart

1. Projekt mit Visual Studio oder `dotnet` Ã¶ffnen:
   ```bash
   dotnet build
   dotnet run --project EventNetwork
   ```

2. Beispielbild (`test.jpg`) wird geladen und verarbeitet. Ausgabe erscheint in der Konsole.

---

## Weiterentwicklung

Dieses Framework ist bewusst einfach gehalten, um als Ausgangspunkt fÃ¼r eigene Architekturen zu dienen. ErweiterungsmÃ¶glichkeiten:

- Backpropagation oder Hebb'sches Lernen
- GPU/Parallelisierung
- Komplexere Netzarchitekturen (Rekurrente, Schichten)

---

## Lizenz

Dieses Projekt steht unter der MIT-Lizenz.

(c) Rudolf Stepan
