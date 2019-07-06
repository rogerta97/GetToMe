using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public enum PlayerRPCCall
{
    CircleSpawned
}

public class PlayerRPCCalls : MonoBehaviour
{
    public static PlayerRPCCalls Instance;
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
    }

    public void SendRPCCall(PlayerRPCCall callType, params object[] args)
    {
        switch (callType)
        {
            case PlayerRPCCall.CircleSpawned:
                PV.RPC("RPC_OnOponentSpawnedCircle", RpcTarget.All, args); 
                break; 
        }
    }

    [PunRPC]
    private void RPC_OnOponentSpawnedCircle(Vector3 pressedPos)
    {
        GameController.Instance._playerLogic.OnOponentPressed(pressedPos); 
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
