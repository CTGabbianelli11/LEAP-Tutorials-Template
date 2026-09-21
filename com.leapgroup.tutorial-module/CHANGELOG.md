# Changelog

All notable changes to this project will be documented in this file.

## [0.1.0] - 2026-09-18

### Added
- Initial release of LEAP Tutorial Module
- `LEAPGroup.Core.ServiceLocator` foundation with readiness gate
- `LEAPGroup.Tutorials` runtime module with:
  - `TutorialGroup` and `TutorialObject` ScriptableObjects
  - `TutorialManager` MonoBehaviour for UI control
  - `ProgressBar` for step progress visualization
  - `FullscreenToggle` utility
- `LEAPGroup.Tutorials.Editor` module with:
  - `TutorialSheetImporter` for Google Sheets integration
  - Custom inspectors for `TutorialGroup` and `TutorialObject`
- Tutorial Template sample scene
- Unit tests for sheet URL parsing and ServiceLocator

### Known Limitations
- The marker expansion table (Greek letters, math symbols) is hardcoded in `TutorialSheetImporter.ProcessSpecialCharacters()`; data-driven markers are a future enhancement
- No localization support for lesson content
- The lesson viewer does not support conditional branching or validation logic
