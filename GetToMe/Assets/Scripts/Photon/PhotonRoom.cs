using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom Instance;
    private PhotonView PV; 

    // Delay Start 
    public float timeToStart;
    public TextMeshProUGUI infoText; 
  
    private int ammountPlayersInRoom = 0;

    private Player[] photonPlayers; 

    private float timeToStartTimer = -1;

    private void Awake()
    {
        if (PhotonRoom.Instance == null)
        {
            PhotonRoom.Instance = this;
        }
        else
        {
            if (PhotonRoom.Instance != this)
            {
                Destroy(PhotonRoom.Instance.gameObject);
                PhotonRoom.Instance = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        photonPlayers = PhotonNetwork.PlayerList;
        ammountPlayersInRoom = photonPlayers.Length;
        infoText.text = "Joined to a room. Waiting for players (" + ammountPlayersInRoom + " of " + MultiplayerSetting.Instance.m_MaxPlayers + ")";

        if (MultiplayerSetting.Instance.delayStart)
        {
             
            if(ammountPlayersInRoom == MultiplayerSetting.Instance.m_MaxPlayers)
            {
                timeToStartTimer = 0;
                infoText.text = "Room completed.";

                if (!PhotonNetwork.IsMasterClient)
                    return;

                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }
    }

    [PunRPC]
    private void RPC_MatchBegin()
    {
        SceneManager.LoadScene("GameScene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        ammountPlayersInRoom++;
        photonPlayers = PhotonNetwork.PlayerList;
        infoText.text = "Joined to a room. Waiting for players (" + (MultiplayerSetting.Instance.m_MaxPlayers - ammountPlayersInRoom) + " of " + MultiplayerSetting.Instance.m_MaxPlayers + ")";
             
        if (MultiplayerSetting.Instance.delayStart)
        {
            if (ammountPlayersInRoom == MultiplayerSetting.Instance.m_MaxPlayers)
            {
                timeToStartTimer = 0;
                infoText.text = "Room completed.";

                if (!PhotonNetwork.IsMasterClient)
                    return;

                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    private void StartGame()
    {
        PV.RPC("RPC_MatchBegin", RpcTarget.All);        
    }

    private void Start()
    {
        PV = gameObject.GetComponent<PhotonView>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToStartTimer != -1)
        {
            timeToStartTimer += Time.deltaTime; 
            if(timeToStartTimer >= timeToStart)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("Im the master");
                    StartGame();
                }                   
                
                timeToStartTimer = -1;
            } else
            {
                int timeLeft = (int)timeToStart - (int)timeToStartTimer;
                infoText.text = "Match Beggins in: " + timeLeft;
                
            }
        }
    }
}
