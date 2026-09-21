# Tutorial Template Sample

This is an editable sample of the LEAP Tutorial Module showing a complete working example with three tutorial lessons (C1, C2, C3).

## What's Included

- **TutorialTemplate.unity** - Ready-to-open scene with a working tutorial viewer
- **Tutorial Group assets** - Three lesson groups with 10 pages each
- **Tutorial Objects** - 30 individual lesson pages with titles and descriptions
- **ServiceLocator** - Already configured for dependency injection

## Getting Started

1. This sample is **automatically imported** when you import the package
2. Open **TutorialTemplate.unity** in the editor
3. Press Play to test the tutorial viewer
4. Use the buttons to navigate between pages and lessons

## Editing the Sample

The imported sample is **fully editable** and under your project's version control:
- Edit lesson content by modifying the Tutorial Group assets
- Add or remove pages by editing Tutorial Objects
- Customize the scene layout as needed
- Import new content via Google Sheets using "Update Tutorials"

## Key Components

- **TutorialManager** - Controls navigation and UI updates
- **Tutorial Groups** (C1, C2, C3) - Organize lessons into chapters
- **Tutorial Objects** - Individual lesson pages
- **ProgressBar** - Shows current progress through lessons
- **FullscreenToggle** - Toggle fullscreen mode

## Next Steps

1. **Customize Content** - Edit the Tutorial Group assets or use "Update Tutorials" to import from Google Sheets
2. **Integrate with Your App** - Use `ServiceLocator.WhenReady<TutorialManager>()` to drive tutorials from your code
3. **Add More Lessons** - Create new Tutorial Group assets following the same pattern

See the main README.md for API documentation and ServiceLocator usage patterns.
