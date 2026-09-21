# LEAP Tutorial Module

A ScriptableObject-driven tutorial and lesson viewer for Unity, with Google Sheets content import and the LEAP ServiceLocator foundation for dependency injection.

## Features

- **Data-driven lesson system** — `TutorialGroup` and `TutorialObject` ScriptableObjects define ordered lessons with title and description.
- **Google Sheets integration** — Author lessons in a spreadsheet, import them into the project.
- **TextMeshPro UI** — Lessons display in a TextMeshPro panel with next/back navigation and a progress bar.
- **ServiceLocator foundation** — Every project built from this template uses `LEAPGroup.Core.ServiceLocator` from day one, eliminating `public static Instance` singletons.
- **Editor authoring tools** — Custom inspectors with "Update Tutorials" buttons to refresh from the sheet.

## Prerequisites

- **Unity 6.0+** (tested on 6000.3.10f1)
- **TextMesh Pro Essential Resources** — import these from Window > TextMeshPro > Import TMP Essential Resources
- **uGUI** 2.0.0 (automatically included in the package dependencies)

## Getting Started

1. Install the package via Git URL:  
   `https://github.com/<owner>/com.leapgroup.tutorial-module.git#v0.1.0`

2. Import TextMesh Pro Essential Resources if not already done:  
   Window > TextMeshPro > Import TMP Essential Resources

3. In the Package Manager, click **Import** next to the Tutorial Template sample to get an editable copy in your project.

4. Open the imported `Assets/Samples/LEAP Tutorial Module/Tutorial Template/TutorialTemplate.unity` scene to see it in action.

## Authoring Tutorials

### Setting up a new lesson group

1. Create a new `TutorialGroup` asset (Right-click > Create > LEAP Tutorial Module > Tutorial Group).
2. Paste the Google Sheets URL in the `Sheet URL` field (e.g., `https://docs.google.com/spreadsheets/d/<spreadsheetId>/edit#gid=<gid>`).
3. Click "Update Tutorials" to import the sheet's content into `TutorialObject` assets.

### Service Locator usage

Every sim built from this template should use the ServiceLocator pattern, not singletons:

```csharp
// In your game code:
public class MyGameManager : MonoBehaviour
{
    void Awake()
    {
        ServiceLocator.Register(this);  // Register yourself
    }
    
    void Start()
    {
        // Resolve the tutorial manager when the scene is ready
        ServiceLocator.WhenReady<TutorialManager>(this, tm => {
            Debug.Log("Tutorial manager is ready: " + tm.name);
        });
    }
}
```

The `ServiceLocator` opens its readiness gate at the end of frame 1, after all `Awake()` and `Start()` calls. Services that need to be available earlier should resolve synchronously with `Get<T>()` or `TryGet<T>()`.

## License

This package is part of the LEAP Group educational initiative at MIT.

## Contributing

Changes to this package should be submitted via pull request to the main repository.
