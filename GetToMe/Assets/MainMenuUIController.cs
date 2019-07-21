using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro; 

public class MainMenuUIController : MonoBehaviour
{
    public float timeToReachDestination;

    public GameObject gameTitle; 
    public GameObject playButton;

    public Color interpolationColor1;
    public Color interpolationColor2; 

    public GameObject gameTitlePosition;
    public GameObject playButtonPosition;

    private TextMeshProUGUI gameTitleText; 

    // Start is called before the first frame update
    void Start()
    {
        gameTitleText = gameTitle.GetComponentInChildren<TextMeshProUGUI>(); 
        Sequence titleSequence = DOTween.Sequence();

        titleSequence
            .Append(DOTween.To(() => gameTitleText.color, x => gameTitleText.color = x, interpolationColor2, 0.6f))
            .Append(DOTween.To(() => gameTitleText.color, x => gameTitleText.color = x, interpolationColor1, 0.6f))
            .SetLoops(-1)
            .Play(); 
           
        gameTitle.transform.DOMove(gameTitlePosition.transform.position, timeToReachDestination).SetEase(Ease.OutExpo);
        playButton.transform.DOMove(playButtonPosition.transform.position, timeToReachDestination).SetEase(Ease.InOutElastic);
    }

}
