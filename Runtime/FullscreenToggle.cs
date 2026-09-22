using UnityEngine;

namespace LEAPGroup.Tutorials
{
    public class FullscreenToggle : MonoBehaviour
    {
        public void ToggleFullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
