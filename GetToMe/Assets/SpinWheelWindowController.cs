using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening; 
using UnityEngine.SceneManagement;

public enum PositionOrder
{
    MenuSlot, 
    SenteceSlot, 
    RollActionSlot,
}

public class SpinWheelWindowController : MonoBehaviour
{
    [SerializeField] private EndGameActionsData sentencesScriptable;

    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private TextMeshProUGUI rematchText;

    public GameObject rematchButton; 

    [SerializeField] private Button homeButton;
    [SerializeField] private Button rollActionButton;

    public GameObject rematchButtonHolder; 

    [Header("Tweening")]
    [SerializeField] private GameObject sentenceHolder;
    [SerializeField] private GameObject rollActionHolder;
    [SerializeField] private GameObject MenuHolder;

    [SerializeField] private GameObject[] initPositions;
    [SerializeField] private GameObject[] finalPositions;

    public PlayerLogic _playerLogic; 

    [HideInInspector] public bool _rematchRequestReceived;
    private bool isActionRolled = false;

    public void Start()
    {
        PlayerRPCCalls.Instance._spinWheelController = this;
    }

    public void AddaptWindow(bool isWin)
    {
        AddaptWindowFromWin(isWin);
    }

    void AnimateStartUI()
    {
        sentenceHolder.transform.DOMove(initPositions[(int)PositionOrder.SenteceSlot].transform.position, 1.5f).SetEase(Ease.OutElastic);
        rollActionHolder.transform.DOMove(initPositions[(int)PositionOrder.RollActionSlot].transform.position, 1.5f).SetEase(Ease.OutElastic);
        MenuHolder.transform.DOMove(initPositions[(int)PositionOrder.MenuSlot].transform.position, 1.5f).SetEase(Ease.OutElastic);
    }

    public void AnimateEndUI()
    {
        sentenceHolder.transform.DOMove(finalPositions[(int)PositionOrder.SenteceSlot].transform.position, 1.5f).SetEase(Ease.InElastic);
        rollActionHolder.transform.DOMove(finalPositions[(int)PositionOrder.RollActionSlot].transform.position, 1.5f).SetEase(Ease.InElastic);
        MenuHolder.transform.DOMove(finalPositions[(int)PositionOrder.MenuSlot].transform.position, 1.5f).SetEase(Ease.InElastic);
    }

    //IEnumerator testCR()
    //{
    //    AnimateStartUI();
    //    yield return new WaitForSeconds(1);
    //    AnimateEndUI(); 
    //}

    public void OnEnable()
    {
        rollActionHolder.SetActive(true);
        rematchButtonHolder.SetActive(false);

        actionText.text = "";

        AnimateStartUI();
    }

    public void OnRollAction()
    {
        if (isActionRolled == false)
        {
            isActionRolled = true;
            actionText.text = sentencesScriptable.GetRandomSentence();

            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.SendAction, sentencesScriptable.GetLastSentenceID());
            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.ReadyToRematch);
        }
    }

    public void SwapRollActionToRematch()
    {
        UpdateRollButtonData(); 
    }

    public void OnRematchRequestReceived()
    {
        _rematchRequestReceived = true;
        rematchText.gameObject.SetActive(true);
    }

    public void OnRematchClicked()
    {
        if (_rematchRequestReceived)
            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.Rematch);
        else
            PlayerRPCCalls.Instance.SendRPCCall(PlayerRPCCall.RematchRequest);

        rematchButton.GetComponent<Image>().color = UIManager.Instance.greenColor;
        rematchButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = UIManager.Instance.greenTextColor;
    }

    public void UpdateActionText(int index)
    {
        actionText.text = sentencesScriptable.ActionSentencesList[index];
    }

    public void UpdateRollButtonData()
    {
        rollActionHolder.SetActive(false);
        rematchButtonHolder.SetActive(true);
    }

    public void AddaptWindowFromWin(bool isWinner)
    {
        homeButton.gameObject.SetActive(false);

        if (isWinner)
        {
            rollActionButton.enabled = true;
            sentenceHolder.GetComponent<Image>().color = UIManager.Instance.blueColor;
            rollActionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Punish"; 
            rollActionButton.GetComponentInChildren<TextMeshProUGUI>().color = UIManager.Instance.blueColor;
        }
        else
        {
            rollActionButton.enabled = false;
            sentenceHolder.GetComponent<Image>().color = UIManager.Instance.redColor;
            rollActionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Wait";
            rollActionButton.GetComponentInChildren<TextMeshProUGUI>().color = UIManager.Instance.redColor;         
        }
    }

    public void OnHome()
    {
        SceneManager.LoadScene(0);
    }
}
