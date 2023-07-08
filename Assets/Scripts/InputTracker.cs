using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class InputTracker : MonoBehaviour
{
    [SerializeField] private InputActionReference redAction;
    [SerializeField] private InputActionReference greenAction;
    [SerializeField] private InputActionReference blueAction;
    [SerializeField] private InputActionReference purpleAction;

    private bool trackInputs;
    private float inputTrackingTime;
    private float inputTrackingTimer;
    // 0 = no hit, 1 =red, 2 = blue, 3 = green 
    private int[] hitInputs = new int[2000];
    private float[] hitTimes = new float[2000];
    private int inputIterator;
    private bool redHitDetected;
    private bool blueHitDetected;
    private bool greenHitDetected;
    [SerializeField] private bool testStart;
    [SerializeField] private bool testPlayback;

    private float playbackTimer;
    private int hitToPlay;
    private bool playbackHits;

    [SerializeField] private GameObject redShot;
    [SerializeField] private GameObject greenShot;
    [SerializeField] private GameObject blueShot;

    [SerializeField] private GameObject redIndicator;
    [SerializeField] private GameObject greenIndicator;
    [SerializeField] private GameObject blueIndicator;

    private MeshRenderer redIndicatorMesh;
    private MeshRenderer greenIndicatorMesh;
    private MeshRenderer blueIndicatorMesh;
    private float redColorTimer;
    private float greenColorTimer;
    private float blueColorTimer;
    [SerializeField] private float colorChangeTime;
    [SerializeField] private GameStateManager stateManager;

    private int lastInput;
    private float lastInputTime;
    private int secondToLastInput;
    private float secondToLastInputTime;

    [SerializeField] private float timeBetweenHitMultiplier;
    [SerializeField] private float noteSwitchMultiplier;
    [SerializeField] private float baseNoteScore;

    [SerializeField] private Slider crowdSlider;
    [SerializeField] private float crowdMaxValue;

    private void Awake()
    {
        redIndicatorMesh = redIndicator.GetComponent<MeshRenderer>();
        greenIndicatorMesh = greenIndicator.GetComponent<MeshRenderer>();
        blueIndicatorMesh = blueIndicator.GetComponent<MeshRenderer>();
        redIndicatorMesh.material.color = Color.red;
        greenIndicatorMesh.material.color = Color.green;
        blueIndicatorMesh.material.color = Color.blue;
        inputTrackingTime = stateManager.roundTime;
        crowdSlider.maxValue = crowdMaxValue;
        crowdSlider.minValue = 0;

    }


    private void OnEnable()
    {
        //leave in on disable in case we want hold functionality
        redAction.action.started += HitRed;
        redAction.action.canceled += LetGoRed;
        blueAction.action.started += HitBlue;
        blueAction.action.canceled += LetGoBlue;
        greenAction.action.started += HitGreen;
        greenAction.action.canceled += LetGoGreen;

    }

    private void OnDisable()
    {
        redAction.action.started -= HitRed;
        redAction.action.canceled -= LetGoRed;
        blueAction.action.started -= HitBlue;
        blueAction.action.canceled -= LetGoBlue;
        greenAction.action.started -= HitGreen;
        greenAction.action.canceled -= LetGoGreen;

    }

    private void HitRed(InputAction.CallbackContext context)
    {
        if (trackInputs)
        {
            redHitDetected = true;
            redIndicatorMesh.material.color = Color.grey;
            redColorTimer = colorChangeTime;
        }

    }

    private void LetGoRed(InputAction.CallbackContext context)
    {

    }

    private void HitBlue(InputAction.CallbackContext context)
    {
        if (trackInputs)
        {
            blueHitDetected = true;
            blueIndicatorMesh.material.color = Color.grey;
            blueColorTimer = colorChangeTime;
        }

    }

    private void LetGoBlue(InputAction.CallbackContext context)
    {

    }

    private void HitGreen(InputAction.CallbackContext context)
    {
        if (trackInputs)
        {
            greenHitDetected = true;
            greenIndicatorMesh.material.color = Color.grey;
            greenColorTimer = colorChangeTime;
        }
    }

    private void LetGoGreen(InputAction.CallbackContext context)
    {

    }

    private void AddScore(int inputColor, float timeInputted)
    {
        float timeBonus = 1;
        float noteChangeBonus = 1;

        if (inputColor != lastInput)
        {
            noteChangeBonus = noteSwitchMultiplier;
        }

        timeBonus = timeBetweenHitMultiplier / (timeInputted - lastInputTime);

        float scoreToAdd = baseNoteScore + (baseNoteScore * timeBonus) + (noteChangeBonus * baseNoteScore);

        Debug.Log("adding score");
        crowdSlider.value = crowdSlider.value +  scoreToAdd;
        
        secondToLastInput = lastInput;
        secondToLastInputTime = lastInputTime;
        lastInput = inputColor;
        lastInputTime = timeInputted;
    
    }


    public void StartTracking()
    {
        //reset timer, iterator
        inputTrackingTimer = 0;
        inputIterator = 0;

        //reset score tracking vars
        lastInput = 0;
        lastInputTime = 0;
        secondToLastInput = 0;
        secondToLastInputTime = 0;

        //re init arrays
        Array.Clear(hitInputs, 0, 2000);
        Array.Clear(hitTimes, 0, 2000);

        //turn on tracking
        trackInputs = true;
    }

    public void EndTracking()
    {
        trackInputs = false;
        hitInputs[inputIterator] = -1;
        crowdSlider.value = 0;
        PrintHitInputArray();
    }

    private void FixedUpdate()
    {
        if (testStart)
        {
            StartTracking();
            testStart = false;
        }

        if (testPlayback)
        {
            BeginHitPlayback();
            testPlayback = false;
        }

        if (trackInputs)
        {
            //update array with current inputs
            UpdateTrackingArray();
        }

        if (playbackHits)
        {
            PlayHitsBack();
        }
    }

    private void Update()
    {
        CheckColorTimers();
    }

    private void CheckColorTimers()
    {
        if (redColorTimer > 0)
        {
            redColorTimer -= Time.deltaTime;
            if (redColorTimer <= 0)
            {
                redIndicatorMesh.material.color = Color.red;
            }
        }

        if (greenColorTimer > 0)
        {
            greenColorTimer -= Time.deltaTime;
            if (greenColorTimer <= 0)
            {
                greenIndicatorMesh.material.color = Color.green;
            }
        }

        if (blueColorTimer > 0)
        {
            blueColorTimer -= Time.deltaTime;
            if (blueColorTimer <= 0)
            {
                blueIndicatorMesh.material.color = Color.blue;
            }
        }
    }

    private void UpdateTrackingArray()
    {
        if (redHitDetected)
        {
            redHitDetected = false;
            hitInputs[inputIterator] = 1;
            hitTimes[inputIterator] = inputTrackingTimer;
            inputIterator += 1;
            AddScore(1, inputTrackingTimer);

        }
        else if (blueHitDetected)
        {
            blueHitDetected = false;
            hitInputs[inputIterator] = 2;
            hitTimes[inputIterator] = inputTrackingTimer;
            inputIterator += 1;
            AddScore(2, inputTrackingTimer);
        }
        else if (greenHitDetected)
        {
            greenHitDetected = false;
            hitInputs[inputIterator] = 3;
            hitTimes[inputIterator] = inputTrackingTimer;
            inputIterator += 1;
            AddScore(3, inputTrackingTimer);
        }


        //increase time
        inputTrackingTimer += Time.deltaTime;
    }

    private void PrintHitInputArray()
    {
        for(int a = 0; a < 2000 ; a++)
        {
            if (hitInputs[a] != 0)
                Debug.Log("Hit input " + a + " = " + hitInputs[a] + " Hit Found at time " + hitTimes[a]);

            if (hitInputs[a] == -1)
                break;
        }
    }

    public void BeginHitPlayback()
    {
        playbackTimer = 0;
        hitToPlay= 0;
        playbackHits = true;
    }

    private void PlayHitsBack()
    {
        if (hitTimes[hitToPlay] <= playbackTimer)
        {
            PlayHit(hitInputs[hitToPlay]);
            hitToPlay += 1;
        }

        playbackTimer += Time.deltaTime;

        if (playbackTimer > (inputTrackingTime+1))
        {
            playbackHits = false;
            Debug.Log("Playback Over");
        }
    }

    private void PlayHit(int hitToPlay)
    {
        if (hitToPlay == 1) //red
        {
            Debug.Log("RED HIT");
            GameObject redShotInst = Instantiate(redShot, redIndicator.transform);
            redShotInst.GetComponent<Rigidbody>().velocity = new Vector3(10f, 0f, 0f);
            Destroy(redShotInst, 3f);

        }
        if (hitToPlay == 2) //blue
        {
            Debug.Log("BLUE HIT");
            GameObject blueShotInst = Instantiate(blueShot, blueIndicator.transform);
            blueShotInst.GetComponent<Rigidbody>().velocity = new Vector3(10f, 0f, 0f);
            Destroy(blueShotInst, 3f);
        }
        if (hitToPlay == 3) //green
        {
            Debug.Log("GREEN HIT");
            GameObject greenShotInst = Instantiate(greenShot, greenIndicator.transform);
            greenShotInst.GetComponent<Rigidbody>().velocity = new Vector3(10f, 0f, 0f);
            Destroy(greenShotInst, 3f);
        }
    }
}
