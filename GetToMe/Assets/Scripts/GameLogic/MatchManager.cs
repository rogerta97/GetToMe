using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    [SerializeField] private int maxRounds;
    [HideInInspector] public int currentRound = 0;

    [SerializeField] private Button spinwheelButton;
    private bool oponentRequestedSpinWheel; 

    public PlayerInfo _playerInfo; 

    public void TransitToSpinWheel()
    {
        UIManager.Instance.ShowWindow(GameWindow.SpinWheel); 
    }

    public void OponentRequestedSpinWheel()
    {
        oponentRequestedSpinWheel = true; 
    }

    public void OnSpinwheelClicked()
    {
        if (oponentRequestedSpinWheel)
        {
            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.SpinWheel);
        } else
        {
            TextMeshProUGUI buttonText = spinwheelButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "Waiting oponent request...";
            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.OponentSpinWheelRequest); 
        } 
    }

    public void AdvanceRound()
    { 
        currentRound++;
        UIManager.Instance.UpdateRounds();

        if (currentRound >= maxRounds)
        {
            OnGameFinished();
        }
    }

    private void OnGameFinished()
    {
        if (_playerInfo.isHost)
            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.GameOver);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerRPCCalls.Instance._matchManager = this; 
        UIManager.Instance.UpdateRounds();
        UIManager.Instance.AddaptTurnText(_playerInfo.isMyTurn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
