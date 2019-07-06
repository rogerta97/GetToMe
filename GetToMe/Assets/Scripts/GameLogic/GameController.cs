using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public PlayerInfo _playerInfo;
    public PlayerLogic _playerLogic;
    public bool simulateTurn; 

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.Instance == null)
        {
            GameController.Instance = this;
        }
        else
        {
            if (GameController.Instance != this)
            {
                Destroy(GameController.Instance.gameObject);
                GameController.Instance = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);

        _playerInfo = gameObject.GetComponent<PlayerInfo>();
        _playerLogic = gameObject.GetComponent<PlayerLogic>();
        UIManager.Instance.AddaptTurnText(_playerInfo.isMyTurn);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInfo.isMyTurn || simulateTurn)
        {
            PlayTurn();
        }
    }

    private void PlayTurn()
    {
        _playerLogic.PlayTurnLogic(); 
    }

    private void SendScreenPosition()
    {

    }

    private bool WaitForInput()
    {
        return false;
    }
}
