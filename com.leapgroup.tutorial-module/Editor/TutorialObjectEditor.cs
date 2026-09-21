using UnityEditor;
using UnityEngine;
using LEAPGroup.Tutorials;

namespace LEAPGroup.Tutorials
{
    [CustomEditor(typeof(TutorialObject))]
    public class TutorialObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TutorialObject myTarget = (TutorialObject)target;
            if (GUILayout.Button("Test Enter", GUILayout.Height(40)))
            {
                myTarget.TestEnter();
            }
            if (GUILayout.Button("Test Exit", GUILayout.Height(40)))
            {
                myTarget.ExitTutorial();
            }
        }
    }
}
