using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun;
using UnityEngine.UI;

public enum GameWindow
{
    Lobby, 
    GamePlay, 
    GameOver, 
    SpinWheel, 
}

public class UIManager : MonoBehaviour
{    
    public TextMeshProUGUI youText;
    public TextMeshProUGUI oponentText;
    public Color turnColor;
    public Color waitColor; 

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI oponentTimerText;

    public GameObject roundSticksHolder;
    private Image[] roundStickImages; 

    public TextMeshProUGUI winOrLoseText;

    public GameObject _gamePlayWindow;
    public GameObject _gameOverWindow;
    public GameObject _spinwheelWindow;

    public MatchManager matchManager;

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

        roundStickImages = new Image[roundSticksHolder.transform.childCount]; 
        for (int i = 0; i < roundSticksHolder.transform.childCount; i++)       
            roundStickImages[i] = roundSticksHolder.transform.GetChild(i).GetComponent<Image>();               
    }

    public void OnGameOver(bool isWinner)
    {
        ShowWindow(GameWindow.GameOver); 
        SetWinOrLoseText(isWinner);
    }

    public void ShowWindow(GameWindow gameWindow)
    {
        switch (gameWindow)
        {
            case GameWindow.Lobby:
                _spinwheelWindow.SetActive(false);
                _gamePlayWindow.SetActive(false);
                _gameOverWindow.SetActive(false); 
                break;
            case GameWindow.GamePlay:
                _spinwheelWindow.SetActive(false);
                _gamePlayWindow.SetActive(true);
                _gameOverWindow.SetActive(false);
                break;
            case GameWindow.GameOver:
                _spinwheelWindow.SetActive(false);
                _gamePlayWindow.SetActive(false);
                _gameOverWindow.SetActive(true);
                break;
            case GameWindow.SpinWheel:
                _spinwheelWindow.SetActive(true);
                _gamePlayWindow.SetActive(false);
                _gameOverWindow.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void SetWinOrLoseText(bool winner)
    {
        if (winner)
        {
            winOrLoseText.text = "You are the winner";
        }
        else
        {
            winOrLoseText.text = "You are the loser";
        }
    }

    public void UpdateRounds()
    {
        for (int i = 0; i < roundStickImages.Length; i++)
        {
            if (i < matchManager.currentRound)
                roundStickImages[i].color = turnColor;
            else
                roundStickImages[i].color = waitColor;
        }        
    }

    public void AddaptTurnText(bool isTurn)
    {
        if (isTurn)
        {
            youText.color = turnColor;
            oponentText.color = waitColor; 
        }
        else
        {
            youText.color = waitColor;
            oponentText.color = turnColor;
        }
    }

    public void AddaptTimer(float secondsInSeek, float oponentSecondsInSeek)
    {
        timerText.text = secondsInSeek.ToString("n2");
        oponentTimerText.text = oponentSecondsInSeek.ToString("n2");
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
