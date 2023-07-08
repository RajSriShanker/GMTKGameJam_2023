using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIController : MonoBehaviour
{
    public float moveSpeed = 1;
    public Image menuBackgroundImage;
    public GameObject gameTitle;
    public Button startButton;
    public Button informationButton;
    public Image informationUI;
    private float infoButtonStartPos = -3000;
    private float infoButtonEndPos = 0;


    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        informationButton.onClick.AddListener(InformationDisplay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InformationDisplay()
    {
        if (informationUI.transform.localPosition.x == infoButtonStartPos)
        {
            informationUI.transform.DOLocalMoveX(infoButtonEndPos, moveSpeed);
            gameTitle.transform.DOLocalMoveX(infoButtonStartPos, moveSpeed);
        }
        else
        {
            informationUI.transform.DOLocalMoveX(infoButtonStartPos, moveSpeed);
            gameTitle.transform.DOLocalMoveX(infoButtonEndPos, moveSpeed);
        }
    }
    
    void StartGame()
    {
        informationUI.transform.DOLocalMoveX(infoButtonStartPos, moveSpeed);
        menuBackgroundImage.transform.DOLocalMoveX(infoButtonStartPos, moveSpeed);
        startButton.gameObject.transform.DOLocalMoveX(infoButtonStartPos, moveSpeed);
        informationButton.gameObject.transform.DOLocalMoveX(infoButtonStartPos, moveSpeed);
    }
}
