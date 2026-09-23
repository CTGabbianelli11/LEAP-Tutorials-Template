using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControllerForScriptableObjects : MonoBehaviour
{
    public UIPageCollection pageCollection;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    private int currentIndex = 0;

    void Start()
    {
        DisplayPage(0);
    }
    void DisplayPage(int index)
        {
            var page = pageCollection.pages[index];

            titleText.text = page.title;
            descriptionText.text = page.description;

        }
    public void NextPage()
    {
        if (currentIndex < (pageCollection.pages.Length - 1))
        {
            currentIndex++;
            DisplayPage(currentIndex);
        }
    }

    public void PreviousPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            DisplayPage(currentIndex);
        }
    }

    
}