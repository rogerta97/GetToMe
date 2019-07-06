using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class PlayerLogic : MonoBehaviour
{
    public GameObject playerCircle; 
    private float seekTimer = 0;

    private PhotonView photonView;

    private bool isOnTurn = false;

    private void Awake()
    {
        photonView = PhotonRoom.Instance.photonView;
        PlayerRPCCalls.Instance.InitPV(this); 
    }

    public void PlayTurnLogic()
    {
        isOnTurn = true; 
        if (Input.GetMouseButtonDown(0))
        {
            GameObject circleInstance = Instantiate(playerCircle);
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            circleInstance.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.CircleSpawned, spawnPosition);
            seekTimer = 0;
        }

        if (seekTimer <= 0)
        {
            seekTimer += Time.deltaTime;
        }
    }

    public void OnOponentPressed(Vector3 pressedPoint){
        GameObject circleInstance = Instantiate(playerCircle);
        circleInstance.transform.position = pressedPoint;
    }
}
