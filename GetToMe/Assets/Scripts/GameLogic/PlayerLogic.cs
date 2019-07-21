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
    CircleCatched, 
    GameOver,
}

public class PlayerLogic : MonoBehaviour
{
    public GameObject playerCirclePrefab;
    public MatchManager _matchManager; 

    private GameObject circleInScreen;

    private PlayerInfo _playerInfo;
    public SpinWheelWindowController spinWheelController; 
    private PlayerRPCCalls playerRPCCalls;
    private PhotonView photonView;

    private playerState _playerState;

    private float seekTimer = 0;
    private float oponentSeekTimer = 0; 

    private bool isSeeking = false;

    private bool isPlayTime = true;

    private bool changeTurn = false;
    private float swapTurnTimer = 0; 
    private float waitForTurn = 0.3f;
    private int turnsPassed = 0;

    //Oponent
    private float finalOponentTime = 0; 

    private void Awake()
    {
        photonView = PhotonRoom.Instance.photonView;
        PlayerRPCCalls.Instance.InitPV(this);
        _playerInfo = GetComponent<PlayerInfo>();
        _matchManager._playerInfo = _playerInfo; 

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

    public IEnumerator Rematch()
    {
        //Interpolations
        spinWheelController.AnimateEndUI();
        yield return new WaitForSeconds(0.2f); 

        UIManager.Instance.ShowWindow(GameWindow.GamePlay);

        // Reset match manager
        _matchManager.currentRound = 0;

        // Reset Player Data
        seekTimer = oponentSeekTimer = 0;
        spinWheelController._rematchRequestReceived = false; 
        _playerInfo.ResetData();

        if (_playerInfo.isMyTurn)
            _playerState = playerState.WaitingToSpawnPoint;
        else
            _playerState = playerState.WaitingToSeek;

        //Addapt UI
        UIManager.Instance.UpdateGamePlayUI(_playerInfo.isMyTurn);
        UIManager.Instance.AnimateBottomBar(); 
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
            UIManager.Instance.UpdateSeekTimers(seekTimer, oponentSeekTimer, false);
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

        if(_playerState == playerState.PointSpawned)
        {
            oponentSeekTimer += Time.deltaTime;
            UIManager.Instance.UpdateSeekTimers(seekTimer, oponentSeekTimer, false);
        }
    }

    public void OnCirclePressed()
    {
        if(_playerState == playerState.Seeking)
        {
            _playerState = playerState.CircleCatched; 
            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.CircleCatched, seekTimer);
        }         
    }

    public void OnGameOver()
    {
        _playerState = playerState.GameOver;
        PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.SendTime, seekTimer);
    }

    public void OnOponentRecieveTime(float oponentTime)
    {
        this.finalOponentTime = oponentTime;
        _playerInfo.isWinner = false;

        if (seekTimer < oponentTime)
            _playerInfo.isWinner = true;

        UIManager.Instance.OnGameOver(_playerInfo.isWinner);
        UIManager.Instance.UpdateSeekTimers(seekTimer, oponentTime, true);
      
    }

    public void OnCircleCatched(float oponentSeekTime)
    {
        if (isSeeking)
            isSeeking = false;

        _playerInfo.isMyTurn = !_playerInfo.isMyTurn;
        changeTurn = true;
        isPlayTime = false;

        if(!_playerInfo.isMyTurn)
            oponentSeekTimer = oponentSeekTime;

        UIManager.Instance.UpdateSeekTimers(seekTimer, oponentSeekTimer, false);
        UIManager.Instance.UpdateTurnTextColor(_playerInfo.isMyTurn);

        if (_playerInfo.isHost) {
            if(turnsPassed != 0 && turnsPassed % 2 != 0)
                PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.NextTurn);
        }

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

        AudioManager.Instance.PlayAudioClip(AudioClipTrack.CircleSpawned); 

        isSeeking = true; 
    }
}
