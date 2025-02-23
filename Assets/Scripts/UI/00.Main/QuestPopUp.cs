using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestPopUp : MonoBehaviour
{
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questContentText;

    public void ActivatePopUP(string title, string content)
    {
        questTitleText.text = title;
        
        if (content.Contains("  "))
            content = content.Replace("  ", "\n");

        questContentText.text = content;
        gameObject.SetActive(true);
    }

    public void DeactivatePopUp()
    {
        gameObject.SetActive(false);
    }
}
