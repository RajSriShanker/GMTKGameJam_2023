using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[System.Serializable]
public struct Combo
{
    public int moveNum;
    public HitType move1;
    public HitType move2;
    public HitType move3;
    public HitType move4;
    public HitType move5;
    public int length;
}


[System.Serializable]
public struct ComboInt
{
    public int moveNum;
    public int[] moves;
    public int length;
}

[System.Serializable]
public struct ComboAttacks
{
    public float totalMoves;
    public HitType[] hits;
    public float[] hitSpeeds;
    public float[] hitTimes;

}

[System.Serializable]
public struct ComboAttacksInt
{
    public float totalMoves;
    public int[] hits;
    public float[] hitSpeeds;
    public float[] hitTimes;

}

public enum HitType
{
    none, //0
    red, //1
    blue, //2
    green, //3
    up, //4
    down, //5
    right//6
}

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

    //private int lastInput;
    //private float lastInputTime;
    //private int secondToLastInput;
    //private float secondToLastInputTime;

    [SerializeField] private float timeBetweenHitMultiplier;
    [SerializeField] private float noteSwitchMultiplier;
    [SerializeField] private float baseNoteScore;

    [SerializeField] private Slider crowdSlider;
    [SerializeField] private float crowdMaxValue;


    [SerializeField] private float comboTime;
    private float comboTimer;
    [SerializeField] private float timeBetweenCombos;
    private float betweenComboTimer;
    private int comboIterator;

    private int[] combosToPlay;
    private int maxComboToPlay;
    private int combosPlayed;
    public bool allAttacksPlayed;
    private bool comboInProgress;
    private int currentComboMove;
    [SerializeField] private Combo[] validComboList;
    private ComboInt[] validComboIntList;
    [SerializeField] private ComboAttacks[] comboAttacks;
    [SerializeField] private ComboAttacksInt[] comboAttacksInt;
    [SerializeField] private IndicatorManager indicatorManager;
    private int currentDisplayedCombo;



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
        comboIterator = 0;
        validComboIntList = new ComboInt[30];
        comboAttacksInt = new ComboAttacksInt[30];
        CreateComboTable();
        combosToPlay = new int[50];

    }

    private void CreateComboTable()
    {
        int z = 0;
        foreach (Combo combo in validComboList)
        {
            validComboIntList[z] = TranslateComboList(combo);
            z++;
        }

        z = 0;
        foreach (ComboAttacks attack in comboAttacks)
        {
            comboAttacksInt[z] = TranslateAttackList(attack);
            z++;
        }
    }


    private ComboInt TranslateComboList(Combo combo)
    {
        ComboInt comboToReturn = new ComboInt();
        comboToReturn.moves = new int[6];
        //move num
        comboToReturn.moveNum = combo.moveNum;

        //move 1
        if (combo.move1 == HitType.red)
            comboToReturn.moves[0] = 1;
        else if (combo.move1 == HitType.blue)
            comboToReturn.moves[0] = 2;
        else if (combo.move1 == HitType.green)
            comboToReturn.moves[0] = 3;
        else if (combo.move1 == HitType.up)
            comboToReturn.moves[0] = 4;
        else if (combo.move1 == HitType.down)
            comboToReturn.moves[0] = 5;
        else if (combo.move1 == HitType.right)
            comboToReturn.moves[0] = 6;
        else comboToReturn.moves[0] = 0;


        //move 2
        if (combo.move2 == HitType.red)
            comboToReturn.moves[1] = 1;
        else if (combo.move2 == HitType.blue)
            comboToReturn.moves[1] = 2;
        else if (combo.move2 == HitType.green)
            comboToReturn.moves[1] = 3;
        else if (combo.move2 == HitType.up)
            comboToReturn.moves[1] = 4;
        else if (combo.move2 == HitType.down)
            comboToReturn.moves[1] = 5;
        else if (combo.move2 == HitType.right)
            comboToReturn.moves[1] = 6;
        else comboToReturn.moves[1] = 0;


        //move 3
        if (combo.move3 == HitType.red)
            comboToReturn.moves[2] = 1;
        else if (combo.move3 == HitType.blue)
            comboToReturn.moves[2] = 2;
        else if (combo.move3 == HitType.green)
            comboToReturn.moves[2] = 3;
        else if (combo.move3 == HitType.up)
            comboToReturn.moves[2] = 4;
        else if (combo.move3 == HitType.down)
            comboToReturn.moves[2] = 5;
        else if (combo.move3 == HitType.right)
            comboToReturn.moves[2] = 6;
        else comboToReturn.moves[2] = 0;


        //move 4
        if (combo.move4 == HitType.red)
            comboToReturn.moves[3] = 1;
        else if (combo.move4 == HitType.blue)
            comboToReturn.moves[3] = 2;
        else if (combo.move4 == HitType.green)
            comboToReturn.moves[3] = 3;
        else if (combo.move4 == HitType.up)
            comboToReturn.moves[3] = 4;
        else if (combo.move4 == HitType.down)
            comboToReturn.moves[3] = 5;
        else if (combo.move4 == HitType.right)
            comboToReturn.moves[3] = 6;
        else comboToReturn.moves[3] = 0;


        //move 5
        if (combo.move5 == HitType.red)
            comboToReturn.moves[4] = 1;
        else if (combo.move5 == HitType.blue)
            comboToReturn.moves[4] = 2;
        else if (combo.move5 == HitType.green)
            comboToReturn.moves[4] = 3;
        else if (combo.move5 == HitType.up)
            comboToReturn.moves[4] = 4;
        else if (combo.move5 == HitType.down)
            comboToReturn.moves[4] = 5;
        else if (combo.move5 == HitType.right)
            comboToReturn.moves[4] = 6;
        else comboToReturn.moves[4] = 0;

        //length
        comboToReturn.length = combo.length;

        return comboToReturn;

    }

    private ComboAttacksInt TranslateAttackList(ComboAttacks attacks)
    {
        ComboAttacksInt attacksToReturn = new ComboAttacksInt();

        attacksToReturn.hitSpeeds = attacks.hitSpeeds;
        attacksToReturn.hitTimes = attacks.hitTimes;
        attacksToReturn.totalMoves = attacks.totalMoves;
        attacksToReturn.hits = new int[attacks.hits.Length];

        for (int u = 0; u < attacks.hits.Length; u++)
        {
            if (attacks.hits[u] == HitType.red)
                attacksToReturn.hits[u] = 1;
            else if (attacks.hits[u] == HitType.blue)
                attacksToReturn.hits[u] = 2;
            else if (attacks.hits[u] == HitType.green)
                attacksToReturn.hits[u] = 3;
            else if (attacks.hits[u] == HitType.up)
                attacksToReturn.hits[u] = 4;
            else if (attacks.hits[u] == HitType.down)
                attacksToReturn.hits[u] = 5;
            else if (attacks.hits[u] == HitType.right)
                attacksToReturn.hits[u] = 6;
            else
                attacksToReturn.hits[u] = 0;
        }

        return attacksToReturn;
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

    private void AddScore(int scoreToAdd)
    {

        Debug.Log("adding score");
        crowdSlider.value = crowdSlider.value +  scoreToAdd;;
    
    }


    public void StartTracking()
    {
        inputIterator = 0;

        //re init arrays
        Array.Clear(hitInputs, 0, 2000);

        //turn on tracking
        trackInputs = true;

        DisplayNewCombo();
    }

    public void EndTracking()
    {
        trackInputs = false;
        //hitInputs[inputIterator] = -1;
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
            CheckCombo();

        }
        else if (blueHitDetected)
        {
            blueHitDetected = false;
            hitInputs[inputIterator] = 2;
            CheckCombo();
        }
        else if (greenHitDetected)
        {
            greenHitDetected = false;
            hitInputs[inputIterator] = 3;
            CheckCombo();
        }

    }

    private void CheckCombo()
    {
        Debug.Log("Checking combo input, input iterator = " + inputIterator);
        //if the input in the sequence does not match the expected one
        if (hitInputs[inputIterator] != validComboIntList[currentDisplayedCombo].moves[inputIterator])
        {
            Debug.Log("Combo invalid, spawning new");
            RemoveCombo();
            DisplayNewCombo();
            
        }
        //if not invalid and is the final iterator, register combo
        else if (inputIterator == validComboIntList[currentDisplayedCombo].length-1)
        {
            Debug.Log("combo over, register combo");
            RegisterCombo(currentDisplayedCombo);
        }
        //correct input but not done, update combo display
        else
        {
            Debug.Log("Correct input, not done so update display");
            UpdateComboDisplay(inputIterator);
            inputIterator += 1;
        }
        


    }

    private void UpdateComboDisplay(int orbToUpdate)
    {

    }

    private void RegisterCombo(int comboNum)
    {
        Debug.Log("Combo " + comboNum + "  has been performed");
        combosToPlay[maxComboToPlay] = comboNum;
        maxComboToPlay += 1;
        AddScore(25); //TEMP
        RemoveCombo();
        DisplayNewCombo();
    }
    private void PrintHitInputArray()
    {
        for(int a = 0; a < 2000 ; a++)
        {
            if (hitInputs[a] != 0)
                //Debug.Log("Hit input " + a + " = " + hitInputs[a] + " Hit Found at time " + hitTimes[a]);

            if (hitInputs[a] == -1)
                break;
        }
    }

    public void BeginHitPlayback()
    {
        Debug.Log("beggining hit playback");
        playbackHits = true;
        combosPlayed = 0;
        comboInProgress = true; ;
        comboTimer = 0;
        currentComboMove = 0;
    }

    private void PlayHitsBack()
    {
        if (combosPlayed >= maxComboToPlay) //done playing combos
        {
            Debug.Log("combos played  = " + combosPlayed + ", max combo to play = " + maxComboToPlay + ", done playing combos");
            playbackHits = false;
            allAttacksPlayed = true;
            Array.Clear(combosToPlay, 0, 20);
            maxComboToPlay = 0;

        }
        else if (!comboInProgress) //start playing new combo
        {
            betweenComboTimer -= Time.fixedDeltaTime;

            if (betweenComboTimer <= 0)
            {
                Debug.Log("start another combo after time delay");
                //reset individual combo vars (combo being the set of orbs that come out per combo)
                comboInProgress = true;
                comboTimer = 0;
                currentComboMove = 0;
            }
        }
        else //run the combo
        {
            if (currentComboMove >= comboAttacksInt[combosToPlay[combosPlayed]].totalMoves)
            {
                Debug.Log("current moves total moves are done, increase combos played");
                comboInProgress = false;
                betweenComboTimer = timeBetweenCombos;
                combosPlayed += 1;

            }
            else if (comboAttacksInt[combosToPlay[combosPlayed]].hitTimes[currentComboMove] < comboTimer)
            {
                Debug.Log("running attack " + currentComboMove + " in for combo " + combosToPlay[combosPlayed] + " which is combo " + combosPlayed + " in the list");
                PlayHit(comboAttacksInt[combosToPlay[combosPlayed]].hits[currentComboMove], comboAttacksInt[combosToPlay[combosPlayed]].hitSpeeds[currentComboMove]);
                currentComboMove += 1;
            }

            comboTimer += Time.fixedDeltaTime;
        }
    }

    private void PlayHit(int hitToPlay, float speed)
    {
        if (hitToPlay == 1)  //red
        {
            Debug.Log("RED HIT");
            GameObject redShotInst = Instantiate(redShot, redIndicator.transform);
            redShotInst.GetComponent<Rigidbody>().velocity = new Vector3(speed, 0f, 0f);
            Destroy(redShotInst, 3f);

        }
        if (hitToPlay == 2) //blue
        {
            Debug.Log("BLUE HIT");
            GameObject blueShotInst = Instantiate(blueShot, blueIndicator.transform);
            blueShotInst.GetComponent<Rigidbody>().velocity = new Vector3(speed, 0f, 0f);
            Destroy(blueShotInst, 3f);
        }
        if (hitToPlay == 3) //green
        {
            Debug.Log("GREEN HIT");
            GameObject greenShotInst = Instantiate(greenShot, greenIndicator.transform);
            greenShotInst.GetComponent<Rigidbody>().velocity = new Vector3(speed, 0f, 0f);
            Destroy(greenShotInst, 3f);
        }
    }

    private void DisplayNewCombo()
    {
        currentDisplayedCombo = Random.Range(0, 5);
        Debug.Log("Current combo = " + currentDisplayedCombo);

        inputIterator = 0;

        //instatiate new images



    }

    private void RemoveCombo()
    {

    }
}
