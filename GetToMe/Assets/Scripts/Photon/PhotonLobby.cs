using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public GameObject battleButton;
    public GameObject cancelButton;
    public GameObject offlineButton;

    public TextMeshProUGUI infoText;

    public PhotonLobby Instance;

    private void Awake()
    {
        Instance = this;
        Screen.SetResolution(414, 736, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        infoText.text = "Connected to " + PhotonNetwork.CloudRegion + " servers";
        PhotonNetwork.AutomaticallySyncScene = true;

        battleButton.SetActive(true);
        offlineButton.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        infoText.text = "Looking for a room to join...";
        battleButton.SetActive(false);
        cancelButton.SetActive(true);

        AudioManager.Instance.PlayAudioClip(AudioClipTrack.ButtonClick); 
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed trying to join a random room.");
        CreateRandomRoom();
    }

    private void CreateRandomRoom(bool visible = true, bool isOpen = true)
    {
        Debug.Log("CreatingRoom"); 
        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.Instance.m_MaxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed creating a random room.");
        CreateRandomRoom();
    }

    public void OnCancelButtonClicked()
    {
        cancelButton.SetActive(false);
        battleButton.SetActive(true);

        AudioManager.Instance.PlayAudioClip(AudioClipTrack.ButtonClick); 
        PhotonNetwork.LeaveRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
