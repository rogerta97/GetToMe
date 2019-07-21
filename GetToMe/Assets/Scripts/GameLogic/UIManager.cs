using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun;
using DG.Tweening; 
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
    [Header("GamePlay Window")]
    public GameObject bottomBar;
    public GameObject bottomBarPosition;
    public TextMeshProUGUI youText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI oponentTimerText;
    public TextMeshProUGUI oponentText;
    public GameObject roundSticksHolder;
    private bool transitToGameOver;
    Tween botomBarTween;
    private Vector3 initBottomBarPosition;


    [Header("GameOver Window")]
    public TextMeshProUGUI finalYouText;
    public TextMeshProUGUI finalOponentText;
    public TextMeshProUGUI finalSeekTimer;
    public TextMeshProUGUI finalOponentSeekTimer;
    public TextMeshProUGUI GoText;
    public TextMeshProUGUI winOrLoseText;

    [Header("SpinWheel")]
    public Image actionBackground;
    public TextMeshProUGUI rematchText;
    public SpinWheelWindowController spinWheelController; 

    [Header("GameColors")]
    public Color blueColor;
    public Color greenColor;
    public Color greenTextColor;
    public Color whiteColor;
    public Color redColor;

    public GameObject _gamePlayWindow;
    public GameObject _gameOverWindow;
    public GameObject _spinwheelWindow;

    private Image[] roundStickImages; 
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

        initBottomBarPosition = bottomBar.transform.position;
        AnimateBottomBar(); 
    }

    public void AnimateBottomBar()
    {
        botomBarTween = bottomBar.transform.DOMove(bottomBarPosition.transform.position, 1).SetEase(Ease.OutBounce);
    }

    public void OnGameOver(bool isWinner)
    {
        StartCoroutine(GameOverCr(isWinner)); 
    }

    IEnumerator GameOverCr(bool isWinner)
    {      
        botomBarTween = bottomBar.transform.DOMove(initBottomBarPosition, 1).SetEase(Ease.OutBounce);
        yield return botomBarTween.WaitForCompletion(); 

        ShowWindow(GameWindow.GameOver);
        //AudioManager.Instance.PlayAudioClip(AudioClipTrack.WinSound); 

        SetGameOverWindowData(isWinner);
        SetSpinWheelData(isWinner);
    }

    public void SetGameOverWindowData(bool isWinner)
    {
        SetWinOrLoseText(isWinner);

        if (isWinner)
        {
            finalYouText.color = blueColor;
            finalOponentText.color = redColor;
            GoText.color = blueColor;

        } else
        {
            finalYouText.color = redColor;
            finalOponentText.color = blueColor;
            GoText.color = redColor; 
        }
    }

    public void UpdateGamePlayUI(bool isMyTurn, float seekTimer = 0, float oponentSeekTimer = 0)
    {
        UpdateSeekTimers(seekTimer, oponentSeekTimer, false);
        UpdateTurnTextColor(isMyTurn);
        UpdateRounds(); 
    }

    public void UpdateActionText(int actionIndex)
    {
        spinWheelController.UpdateActionText(actionIndex); 
    }

    public void SetSpinWheelData(bool isWinner)
    {
        spinWheelController.AddaptWindow(isWinner);       
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
            winOrLoseText.text = "WINNER";
            winOrLoseText.color = blueColor; 
        }
        else
        {
            winOrLoseText.text = "LOSER";
            winOrLoseText.color = redColor;
            
        }
    }

    public void UpdateRounds()
    {
        for (int i = 0; i < roundStickImages.Length; i++)
        {
            if (i < matchManager.currentRound)
                roundStickImages[i].color = blueColor;
            else
                roundStickImages[i].color = whiteColor;
        }        
    }

    public void UpdateTurnTextColor(bool isTurn)
    {
        if (isTurn)
        {
            youText.color = blueColor;
            oponentText.color = whiteColor; 
        }
        else
        {
            youText.color = whiteColor;
            oponentText.color = blueColor;
        }
    }

    public void UpdateSeekTimers(float secondsInSeek, float oponentSecondsInSeek, bool finalTimer)
    {
        if (finalTimer)
        {
            finalSeekTimer.text = secondsInSeek.ToString("n2");
            finalOponentSeekTimer.text = oponentSecondsInSeek.ToString("n2");

        }
        else
        {
            timerText.text = secondsInSeek.ToString("n2");
            oponentTimerText.text = oponentSecondsInSeek.ToString("n2");
        }

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
