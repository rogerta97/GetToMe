using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public enum PlayerRPCCall
{
    CircleSpawned,
    CircleCatched,
    NextTurn,
    SendTime, 
    GameOver, 
    OponentSpinWheelRequest, 
    SpinWheel,
    SendAction, 
    ReadyToRematch, 
    RematchRequest, 
    Rematch, 
}

public class PlayerRPCCalls : MonoBehaviour
{
    public static PlayerRPCCalls Instance;
    [HideInInspector] public PlayerLogic _playerLogic;
    [HideInInspector] public MatchManager _matchManager;
    [HideInInspector] public SpinWheelWindowController _spinWheelController;
    PhotonView PV;

    private void Awake()
    {

        if (PlayerRPCCalls.Instance == null)
        {
            PlayerRPCCalls.Instance = this;
        }
        else
        {
            if (PlayerRPCCalls.Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void InitPV(PlayerLogic playerLogic)
    {
        PV = PhotonRoom.Instance.photonView;
        _playerLogic = playerLogic; 
    }

    public void SendRPCCall(PlayerRPCCall callType, params object[] args)
    {
        switch (callType)
        {
            case PlayerRPCCall.CircleSpawned:
                PV.RPC("RPC_OnOponentSpawnedCircle", RpcTarget.Others, args); 
                break;

            case PlayerRPCCall.SendTime:
                PV.RPC("RPC_SendTimeToOponent", RpcTarget.Others, args);
                break;

            case PlayerRPCCall.OponentSpinWheelRequest:
                PV.RPC("RPC_OnOponentSpinWheelRequest", RpcTarget.Others);
                break;

            case PlayerRPCCall.SendAction:
                PV.RPC("RPC_OnSendActionToOponent", RpcTarget.Others, args);
                break;

            case PlayerRPCCall.RematchRequest:
                PV.RPC("RPC_OnRematchRequest", RpcTarget.Others);
                break;

            case PlayerRPCCall.CircleCatched:
                PV.RPC("RPC_OnCircleCatched", RpcTarget.All, args);
                break;

            case PlayerRPCCall.NextTurn:
                PV.RPC("RPC_AdvanceRound", RpcTarget.All);
                break;

            case PlayerRPCCall.GameOver:
                PV.RPC("RPC_OnGameOver", RpcTarget.All);
                break;

            case PlayerRPCCall.SpinWheel:
                PV.RPC("RPC_OnSpinWheel", RpcTarget.All);
                break;

            case PlayerRPCCall.Rematch:
                PV.RPC("RPC_OnRematch", RpcTarget.All);
                break;

            case PlayerRPCCall.ReadyToRematch:
                PV.RPC("RPC_OnReadyToRematch", RpcTarget.All);
                break;

        }
    }

    [PunRPC]
    private void RPC_OnOponentSpawnedCircle(Vector3 pressedPos)
    {
        _playerLogic.OnOponentSpawnCircle(pressedPos); 
    }

    [PunRPC]
    private void RPC_OnReadyToRematch()
    {
        _spinWheelController.SwapRollActionToRematch();
    }

    [PunRPC]
    private void RPC_OnOponentSpinWheelRequest()
    {
        _matchManager.OponentRequestedSpinWheel(); 
    }

    [PunRPC]
    private void RPC_SendTimeToOponent(float finalOponentTime)
    {
        _playerLogic.OnOponentRecieveTime(finalOponentTime);
    }

    [PunRPC]
    private void RPC_OnSendActionToOponent(int actionIndex)
    {
        UIManager.Instance.UpdateActionText(actionIndex); 
    }

    [PunRPC]
    private void RPC_OnGameOver()
    {
        _playerLogic.OnGameOver();
    }

    [PunRPC]
    private void RPC_OnSpinWheel()
    {
        _matchManager.TransitToSpinWheel(); 
    }

    [PunRPC]
    private void RPC_AdvanceRound()
    {
        _playerLogic._matchManager.AdvanceRound();
    }

    [PunRPC]
    private void RPC_OnRematchRequest()
    {
        UIManager.Instance.spinWheelController.OnRematchRequestReceived();
    }

    [PunRPC]
    private void RPC_OnRematch()
    {
        StartCoroutine(_playerLogic.Rematch()); 
    }

    [PunRPC]
    private void RPC_OnCircleCatched(float oponentSeekTime)
    {
        _playerLogic.OnCircleCatched(oponentSeekTime);
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
