using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sentences", menuName = "Model/Sentences")]
public class EndGameActionsData : ScriptableObject
{
    [SerializeField]
    public List<string> ActionSentencesList; 

    public string GetRandomSentence()
    {
        int sentenceAmount = ActionSentencesList.Count;
        int sentenceSelected = Random.Range(0, sentenceAmount - 1);
        return ActionSentencesList[sentenceSelected];        
    }
}
