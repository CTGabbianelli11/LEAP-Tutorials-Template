using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UIcontroller : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descriptionText;

    private int currentPage = 0;

    void Start()
    {
        headerText.text = infomation[currentPage].title;
        descriptionText.text = infomation[currentPage].descriptions;
    }
    
    public List<tutorialImformation> infomation;
   
    private string[] headers =
    {
        "HeaderText1",
        "HeaderText2",
        "HeaderText3"
    };

    private string[] descriptions =
    {
        "DiscriptionText1",
        "DiscriptionText2",
        "DiscriptionText3"
    };

    public void NextPage()
    {
        if (currentPage < headers.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    void UpdatePage()
    {
        headerText.text = infomation[currentPage].title;
        descriptionText.text = infomation[currentPage].descriptions;

    }
}
[Serializable]
public class tutorialImformation
{
    public string title;
    public string descriptions;
}