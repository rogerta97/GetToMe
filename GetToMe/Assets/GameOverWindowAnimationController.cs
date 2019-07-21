using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameOverWindowAnimationController : MonoBehaviour
{
    public GameObject winOrLoseTextHolder;
    public GameObject scoreTextHolder;
    public GameObject buttonHolder;
    public Button toSpinWheelButton; 

    public Transform buttonPositionFinal;
    public Transform scorePositionFinal;
    public Transform winOrLosePositionFinal;

    public Transform initButtonPosition;
    public Transform initScorePosition;
    public Transform initWinOrLosePosition;

    // Start is called before the first frame update
    void OnEnable()
    {
        toSpinWheelButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Go"; 
        StartCoroutine(AnimateUI()); 
    }

    IEnumerator AnimateUI()
    {
        scoreTextHolder.transform.DOMove(scorePositionFinal.position, 1).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(0.3f);

        winOrLoseTextHolder.transform.DOMove(winOrLosePositionFinal.position, 1).SetEase(Ease.OutElastic);
        buttonHolder.transform.DOMove(buttonPositionFinal.position, 1).SetEase(Ease.OutElastic);   
    }

    public IEnumerator ReverseUIAnimation()
    {  
        winOrLoseTextHolder.transform.DOMove(initWinOrLosePosition.position, 1).SetEase(Ease.OutElastic);
        Tween buttonHolderTween = buttonHolder.transform.DOMove(initButtonPosition.position, 1).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(0.2f);

        Tween outScoreTween = scoreTextHolder.transform.DOMove(initScorePosition.position, 1).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(0.2f);

        UIManager.Instance.ShowWindow(GameWindow.SpinWheel); 

    }
}
