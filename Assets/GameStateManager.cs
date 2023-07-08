using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    private int nextState;

    private bool menuStateRun;
    private bool preFirstFighterStateRun;
    private bool firstFighterStateRun;
    private bool preSecondFighterStateRun;
    private bool secondFighterStateRun;

    [SerializeField] private Button startButton;
    private bool exitingStartMenu;

    [SerializeField] private TimerManager timerManager;
    [SerializeField] private CameraManager camManager;
    [SerializeField] private InputTracker inputTracker;
    public float roundTime;
    public float preFightTime;
    [SerializeField] private float timeDecreasePerRoundPercent;
    public float secondRoundBufferTime;
    [SerializeField] private HitReceiverManager recieverManager;
    

    private void Awake()
    {
        nextState = 1;
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnMenuStartPressed);
    }

    private void Update()
    {
        switch (nextState)
        {
            case 1:

                nextState = RunMenuState();
                break;

            case 2:

                nextState = RunPreFirstFighterState();
                break;

            case 3:

                nextState = RunFirstFighterState();
                break;

            case 4:

                nextState = RunPreSecondFighterState();
                break;

            case 5:

                nextState = RunSecondFighterState();
                break;

            default:
                break;
        }
    }


    private int RunMenuState()
    {
        if (exitingStartMenu)
        {
            exitingStartMenu = false;
            return 2; //go to pre first fighter state
        }
        else return 1;
        
    }

    private int RunPreFirstFighterState()
    {
        if (!preFirstFighterStateRun)
        {
            timerManager.StartPreFightTimer();
            camManager.MoveCam(1);
            timerManager.MoveTimer(1);
            preFirstFighterStateRun = true;
        }

        if (timerManager.preFightTimerDone)
        {
            preFirstFighterStateRun = false;
            timerManager.preFightTimerDone = false;
            return 3;
        }

        return 2;
    }

    private int RunFirstFighterState()
    {
        if (!firstFighterStateRun)
        {
            timerManager.StartFirstFighterTimer();
            inputTracker.StartTracking();
            firstFighterStateRun = true;
        }

        if (timerManager.firstFighterTimerDone)
        {
            inputTracker.EndTracking();
            firstFighterStateRun = false;
            timerManager.firstFighterTimerDone = false;
            return 4;
        }

        return 3;
    }

    private int RunPreSecondFighterState()
    {
        if (!preSecondFighterStateRun)
        {
            timerManager.StartPreFightTimer();
            camManager.MoveCam(2);
            timerManager.MoveTimer(2);
            preSecondFighterStateRun = true;
        }

        if (timerManager.preFightTimerDone)
        {
            preSecondFighterStateRun = false;
            timerManager.preFightTimerDone = false;
            return 5;
        }

        return 4;
    }

    private int RunSecondFighterState()
    {
        if (!secondFighterStateRun)
        {
            recieverManager.StartTracking();
            inputTracker.BeginHitPlayback();
            secondFighterStateRun = true;
        }

        if (inputTracker.allAttacksPlayed)
        {
            timerManager.StartSecondFighterTimer();
            inputTracker.allAttacksPlayed = false;
        }

        if (timerManager.secondFighterTimerDone)
        {
            roundTime -= (roundTime * timeDecreasePerRoundPercent);
            recieverManager.EndTracking();
            secondFighterStateRun = false;
            timerManager.secondFighterTimerDone = false;
            return 2;
        }

        return 5;
    }

    private void OnMenuStartPressed()
    {
        exitingStartMenu = true;
    }
}
