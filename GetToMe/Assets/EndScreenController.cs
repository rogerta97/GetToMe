using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreenController : MonoBehaviour
{
    [SerializeField] private EndGameActionsData sentencesScriptable;
    [SerializeField] private TextMeshProUGUI actionText;

    [SerializeField] private Button homeButton; 
    [SerializeField] private Button rollActionButton;

    public void AddaptWindowFromWin(bool isWinner)
    {
        if (!isWinner)
        {
            rollActionButton.enabled = false;
            rollActionButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Wainting for oponent roll";
        }
    }

    public void OnRollAction()
    {
        actionText.text = sentencesScriptable.GetRandomSentence(); 
        rollActionButton.interactable = false; 
    }

    public void OnHome()
    {
        SceneManager.LoadScene(0); 
    }
}
