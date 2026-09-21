using UnityEngine;
using TMPro;
using LEAPGroup.Core;

namespace LEAPGroup.Tutorials
{
    public class TutorialManager : MonoBehaviour
    {
        public TutorialGroup[] tutorialGroups;
        public TutorialGroup currentGroup;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public ProgressBar progressBar;

        void Awake()
        {
            ServiceLocator.Register(this);
        }

        void Start()
        {
            if (tutorialGroups == null || tutorialGroups.Length == 0)
            {
                Debug.LogError("TutorialManager: No tutorial groups assigned!", this);
                return;
            }

            if (currentGroup == null)
            {
                currentGroup = tutorialGroups[0];
            }

            currentGroup.Initialize();
            UpdateContent();
        }

        public void LastTutorial()
        {
            if (currentGroup == null) return;
            currentGroup.GoBack();
            UpdateContent();
        }

        public void NextTutorial()
        {
            if (currentGroup == null) return;
            currentGroup.GoToNextTutorial();
            UpdateContent();
        }

        void UpdateContent()
        {
            TutorialObject current = currentGroup.CurrentTutorial;

            if (current != null)
            {
                titleText.text = current.title;
                descriptionText.text = current.description;
            }

            int totalSteps = currentGroup.tutorialObjects.Count;
            progressBar.UpdateProgressBar(currentGroup.tutorialIndex, totalSteps);
        }

        public void SwitchGroup(int index)
        {
            currentGroup = tutorialGroups[index];
            currentGroup.Initialize();

            UpdateContent();
        }
    }
}
