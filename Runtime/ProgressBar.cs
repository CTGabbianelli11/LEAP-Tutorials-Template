using UnityEngine;

namespace LEAPGroup.Tutorials
{
    public class ProgressBar : MonoBehaviour
    {
        public RectTransform progressBarFill;
        public RectTransform background;

        public void UpdateProgressBar(int currentIndex, int total)
        {
            float progress = (float)(currentIndex + 1) / total;
            float width = background.rect.width;

            progressBarFill.sizeDelta = new Vector2(width * progress, progressBarFill.sizeDelta.y);
        }
    }
}

