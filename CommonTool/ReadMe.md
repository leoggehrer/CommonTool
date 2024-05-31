# Benutzeranleitung für die C# Klassenbibliothek

## Überblick

Diese Bibliothek bietet Basisklassen zur schnellen Erstellung von Konsolenanwendungen mit integrierten Auswahlmenüs. Sie ermöglicht es Entwicklern, effizient robuste CLI-Tools zu entwickeln, die benutzerfreundliche Menüs für die Navigation und die Ausführung verschiedener Aufgaben bieten.

## Installation

Anleitung zur Einrichtung und Installation der Bibliothek in einer Entwicklungs- oder Produktionsumgebung.

### Voraussetzungen

- .NET Core Version (net8.0)
- Visual Studio Code oder ein anderer C#-fähiger Editor

### Installationsprozess

#### Variante A - Verwendung der Quelldateien

1. Klonen/Download des [Repository](https://github.com/leoggehrer/CommonTool)
2. Öffnen der Solution-Datei (`CommonTool.sln`) in Visual Studio Code
3. Build der Lösung zur Überprüfung auf Abhängigkeiten
4. Einbinden der Klassenbibliothek in die Konsolen-Anwendung

#### Variante B - Verwendung als nuget-Package

1. Erstellen der Konsolen-Anwendung
2. Hinzufügen des nuget-Package [CommonTool.Console](https://www.nuget.org/packages/CommonTool.Console/)

## Hauptklassen und Methoden

### Application

Eine zentrale Klasse, die die Basisfunktionalitäten zur Verwaltung des Lebenszyklus einer Konsolenanwendung bereitstellt.

![Application (CD)](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/leoggehrer/CommonTool/master/CommonTool/diagrams/cd_Application.puml)

- **Initialisierung**
  - `Initialisierung im statischen Konstruktor`: Initialisiert wichtige Eigenschaften der Anwendung wie den 'UserPath', 'SourcePath' und den 'SolutionPath'.

### ConsoleApplication

Diese Klasse stellt die Basis-Implementierung für die Konsolenanwendung mit einem Auswahlmenü zur Verfügung.

![ConsoleApplication (CD)](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/leoggehrer/CommonTool/master/CommonTool/diagrams/cd_ConsoleApplication.puml)

### TemplatePath

Enthält Hilfsmethoden zur Pfadverwaltung, die in der Konsolenanwendung verwendet werden können.

![TemplatePath (CD)](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/leoggehrer/CommonTool/master/CommonTool/diagrams/cd_TemplatePath.puml)

## Beispielanwendungen

Eine Schritt-für-Schritt-Anleitung einer Konsolenanwendung mit Auswahlmenü findet sich als Repository.

![SampleCommonTool.ConApp](https://github.com/leoggehrer/CommonTool/tree/main/SampleCommonTool.ConApp)

## FAQ und Problembehandlung

Häufig gestellte Fragen und Lösungen zu typischen Problemen, die Benutzer bei der Verwendung der Bibliothek erleben könnten.

- **Frage 1**: Wie integriere ich weitere Befehle in das Auswahlmenü?
- **Antwort**: In der Methode ***CreateMenuItems()*** kann der Menüpunkt hinzugefügt werden.

```csharp
...
var menuItems = new List<MenuItem>
{
    CreateMenuSeparator(),
    new()
    {
        Key = $"{++mnuIdx}",
        Text = ToLabelText("Force", "Change force flag"),
        Action = (self) => ChangeForce(),
    },
    new()
    {
        Key = $"{++mnuIdx}",
        Text = ToLabelText("Depth", "Change max sub path depth"),
        Action = (self) => ChangeMaxSubPathDepth(),
    },
    new()
    {
        Key = $"{++mnuIdx}",
        Text = ToLabelText("Projects path", "Change projects path"),
        Action = (self) =>
        {
            var savePath = SourcePath;

            SourcePath = SelectOrChangeToSubPath(SourcePath, MaxSubPathDepth, [ SourcePath ]);
        },
    },
    new()
    {
        Key = $"{++mnuIdx}",
        Text = ToLabelText("New short text", "New menu long text."),
        Action = (self) => NewMenuAction(),
    },
    CreateMenuSeparator(),
};
...
```
