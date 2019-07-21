using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sentences", menuName = "Model/Sentences")]
public class EndGameActionsData : ScriptableObject
{
    private int lastID = 0; 

    [SerializeField]
    public List<string> ActionSentencesList;

    public int GetLastSentenceID()
    {
        return lastID; 
    }

    public string GetRandomSentence()
    {
        int sentenceAmount = ActionSentencesList.Count;
        lastID = Random.Range(0, sentenceAmount - 1);
        return ActionSentencesList[lastID];        
    }
}
