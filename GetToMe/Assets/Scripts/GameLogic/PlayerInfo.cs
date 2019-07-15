using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviour
{
    [HideInInspector] public bool isHost = false;
    [HideInInspector] public bool isMyTurn = false;
    [HideInInspector] public bool isWinner = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isHost = true;
            isMyTurn = true; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsHost()
    {
        return isHost;
    }

    public bool IsTurn()
    {
        return isMyTurn;
    }
}
