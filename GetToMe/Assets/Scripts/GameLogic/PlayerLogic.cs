using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

enum playerState
{
    WaitingToSeek,
    Seeking, 
    SwapTurns,
    WaitingToSpawnPoint,
    PointSpawned,
    GameOver,
}

public class PlayerLogic : MonoBehaviour
{
    public GameObject playerCirclePrefab;

    private GameObject circleInScreen;

    private PlayerInfo _playerInfo;
    private PlayerRPCCalls playerRPCCalls;
    private PhotonView photonView;

    private playerState _playerState;

    private float seekTimer = 0;
    private bool isSeeking = false;

    private bool isPlayTime = true;

    private bool changeTurn = false;
    private float swapTurnTimer = 0; 
    private float waitForTurn = 1.0f;
    private int turnsPassed = 0;

    //Oponent
    private float oponentTime = 0; 

    public MatchManager matchManager;

    private void Awake()
    {
        photonView = PhotonRoom.Instance.photonView;
        PlayerRPCCalls.Instance.InitPV(this);
        _playerInfo = GetComponent<PlayerInfo>();
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        matchManager._playerInfo = _playerInfo; 

        if (_playerInfo.isMyTurn)
            _playerState = playerState.WaitingToSpawnPoint;
        else
            _playerState = playerState.WaitingToSeek;
    }

    private void Update()
    {
        if (_playerState != playerState.GameOver)
        {

            ManagePlayTime();

            if (!isPlayTime)
                return;

            if (_playerInfo.isMyTurn)
                PlayTurnLogic();
            else
                PlayWaitLogic();

        }
    }

    private void ManagePlayTime()
    {
        if (changeTurn)
        {
            swapTurnTimer += Time.deltaTime;

            if (swapTurnTimer >= waitForTurn)
            {
                isPlayTime = true;
                changeTurn = false;
                swapTurnTimer = 0; 
            }
        }
    }

    private void PlayWaitLogic()
    {
        if (isSeeking)
        {
            seekTimer += Time.deltaTime;
            UIManager.Instance.AddaptTimer(seekTimer);
        }
    }

    public void PlayTurnLogic()
    {
        if (Input.GetMouseButtonDown(0) && circleInScreen == null)
        {
            circleInScreen = Instantiate(playerCirclePrefab);
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            circleInScreen.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.CircleSpawned, spawnPosition);
            _playerState = playerState.PointSpawned;
        }
    }

    public void OnCirclePressed()
    {
        if(_playerState == playerState.Seeking)
            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.CircleCatched);
    }

    public void OnGameOver()
    {
        _playerState = playerState.GameOver;
        PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.SendTime, seekTimer);       
    }

    public void OnOponentRecieveTime(float oponentTime)
    {
        this.oponentTime = oponentTime;
        _playerInfo.isWinner = false;

        if (oponentTime > seekTimer)
            _playerInfo.isWinner = true;

        UIManager.Instance.OnGameOver(_playerInfo.isWinner);
    }

    public void OnCircleCatched()
    {
        if (isSeeking)
            isSeeking = false;

        if (_playerInfo.isHost) {
            if(turnsPassed != 0 && turnsPassed % 2 != 0)
                PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.NextTurn);
        }

        _playerInfo.isMyTurn = !_playerInfo.isMyTurn;
        changeTurn = true;
        isPlayTime = false;
        UIManager.Instance.AddaptTurnText(_playerInfo.isMyTurn);

        CleanScreen();
    }

    private void CleanScreen()
    {
        turnsPassed++;
        Destroy(circleInScreen.gameObject);
        circleInScreen = null; 
    }

    public void OnOponentSpawnCircle(Vector3 pressedPoint)
    {
        _playerState = playerState.Seeking; 
        circleInScreen = Instantiate(playerCirclePrefab);
        circleInScreen.transform.position = new Vector3(pressedPoint.x, pressedPoint.y, 0);

        isSeeking = true; 
    }
}
