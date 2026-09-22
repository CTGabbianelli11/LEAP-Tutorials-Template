using System.IO;
using UnityEditor;
using UnityEngine;

namespace LEAPGroup.Tutorials
{
    [CustomEditor(typeof(TutorialGroup))]
    public class TutorialGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TutorialGroup myTarget = (TutorialGroup)target;

            if (GUILayout.Button("Progress", GUILayout.Height(40)))
            {
                myTarget.GoToNextTutorial();
            }

            EditorGUILayout.Space();

            bool urlIsUsable = TutorialSheetImporter.TryParseSheetUrl(
                myTarget.sheetUrl, out string spreadsheetId, out string gid);

            if (!urlIsUsable)
            {
                EditorGUILayout.HelpBox(
                    "Sheet URL is empty or not a sheet tab address. Paste it from the browser, e.g.\n"
                    + "https://docs.google.com/spreadsheets/d/<id>/edit#gid=<gid>",
                    MessageType.Warning);
            }

            using (new EditorGUI.DisabledScope(!urlIsUsable))
            {
                if (GUILayout.Button("Update Tutorials", GUILayout.Height(40)))
                {
                    string folder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(myTarget));
                    TutorialSheetImporter.CreateTutorials(spreadsheetId, gid, folder);
                }
            }
        }
    }
}
