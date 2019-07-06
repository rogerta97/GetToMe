using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun; 

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI yourTurnText;
    public TextMeshProUGUI timerText;

    public static UIManager Instance;

    private void Awake()
    {
        if (UIManager.Instance == null)
        {
            UIManager.Instance = this;
        }
        else
        {
            if (UIManager.Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void UpdateUIFromTurn(bool isTurn)
    {        
        AddaptTimer(); 
    }

    public void AddaptTurnText(bool isTurn)
    {
        if (isTurn)
        {
            yourTurnText.color = new Color(1, 0, 0);
        }
        else
        {
            yourTurnText.color = new Color(0.1f, 0.1f, 0.1f);
        }
    }

    public void AddaptTimer()
    {
      
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
