using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro firstFighterTimerText;
    [SerializeField] private TextMeshPro secondFighterTimerText;
    [SerializeField] private TextMeshPro preFightTimerText;

    public bool firstFighterTimerDone;
    public bool secondFighterTimerDone;
    public bool preFightTimerDone;

    [SerializeField] private Transform preFightTimerTransform;
    [SerializeField] private Vector3 preFightTimerInitialPos;
    [SerializeField] private Vector3 preFightTimerSecondPos;

    private float preFightTimer;
    private float firstFightTimer;
    private float secondFightTimer;

    [SerializeField] private GameStateManager stateManager;

    private void Awake()
    {
        firstFighterTimerText.enabled = false;
        secondFighterTimerText.enabled = false;
        preFightTimerText.enabled = false;
        
    }

    private void Update()
    {
        RunPreFightTimer();
        RunFirstFightTimer();
        RunSecondFightTimer();
    }

    private void RunPreFightTimer()
    {
        if (preFightTimer > 0)
        {
            preFightTimer -= Time.deltaTime;
            float displayTime = (preFightTimer / stateManager.preFightTime) * 3;
            preFightTimerText.text = "" + Mathf.Ceil(displayTime);

            if (preFightTimer < 0)
            {
                preFightTimerText.enabled = false;
                preFightTimerDone = true;
            }
        }
    }

    private void RunFirstFightTimer()
    {
        if (firstFightTimer > 0)
        {
            firstFightTimer -= Time.deltaTime;
            firstFighterTimerText.text = "" + Mathf.Round( firstFightTimer * 10.0f) * 0.1f;

            if (firstFightTimer < 0)
            {
                firstFighterTimerText.enabled = false;
                firstFighterTimerDone = true;
            }
        }

    }

    private void RunSecondFightTimer()
    {
        //extra buffer time for moves to come out.
        if (secondFightTimer > 0)
        {

            secondFightTimer -= Time.deltaTime;

            if (secondFightTimer >= stateManager.secondRoundBufferTime)
                secondFighterTimerText.text = "" + Mathf.Round((secondFightTimer - stateManager.secondRoundBufferTime) * 10.0f) * 0.1f;


            if (secondFightTimer <= 0)
            {
                secondFighterTimerText.enabled = false;
                secondFighterTimerDone = true;
            }
        }
    }

    public void StartPreFightTimer()
    {
        preFightTimerText.enabled = true;
        preFightTimer = stateManager.preFightTime;

    }

    public void StartFirstFighterTimer()
    {
        firstFighterTimerText.enabled = true;
        firstFightTimer = stateManager.roundTime;

    }

    public void StartSecondFighterTimer()
    {
        secondFighterTimerText.enabled = true;
        secondFightTimer = stateManager.roundTime + stateManager.secondRoundBufferTime;

    }

    public void MoveTimer(int side)
    {
        if (side == 1)
            preFightTimerTransform.position = preFightTimerInitialPos;
        else if (side == 2)
            preFightTimerTransform.position = preFightTimerSecondPos;
        else return;

    }
}
