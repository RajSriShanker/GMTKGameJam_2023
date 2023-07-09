using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public int move1;
    public int move2;
    public int move3;
    public int move4;
    public int move5;
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

    private int lastInput;
    private float lastInputTime;
    private int secondToLastInput;
    private float secondToLastInputTime;

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
        FMODUnity.RuntimeManager.PlayOneShot("event:/Combo/Combo1");
        comboAttacksInt = new ComboAttacksInt[30];
        CreateComboTable();
        combosToPlay = new int[20];

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

    private ComboInt TranslateComboList(Combo combo)
    {
        ComboInt comboToReturn = new ComboInt();
        //move num
        comboToReturn.moveNum = combo.moveNum;

        //move 1
        if (combo.move1 == HitType.red)
            comboToReturn.move1 = 1;
        else if (combo.move1 == HitType.blue)
            comboToReturn.move1 = 2;
        else if (combo.move1 == HitType.green)
            comboToReturn.move1 = 3;
        else if (combo.move1 == HitType.up)
            comboToReturn.move1 = 4;
        else if (combo.move1 == HitType.down)
            comboToReturn.move1 = 5;
        else if (combo.move1 == HitType.right)
            comboToReturn.move1 = 6;
        else comboToReturn.move1 = 0;


        //move 2
        if (combo.move2 == HitType.red)
            comboToReturn.move2 = 1;
        else if (combo.move2 == HitType.blue)
            comboToReturn.move2 = 2;
        else if (combo.move2 == HitType.green)
            comboToReturn.move2 = 3;
        else if (combo.move2 == HitType.up)
            comboToReturn.move2 = 4;
        else if (combo.move2 == HitType.down)
            comboToReturn.move2 = 5;
        else if (combo.move2 == HitType.right)
            comboToReturn.move2 = 6;

        else comboToReturn.move2 = 0;


        //move 3
        if (combo.move3 == HitType.red)
            comboToReturn.move3 = 1;
        else if (combo.move3 == HitType.blue)
            comboToReturn.move3 = 2;
        else if (combo.move3 == HitType.green)
            comboToReturn.move3 = 3;
        else if (combo.move3 == HitType.up)
            comboToReturn.move3 = 4;
        else if (combo.move3 == HitType.down)
            comboToReturn.move3 = 5;
        else if (combo.move3 == HitType.right)
            comboToReturn.move3 = 6;
        else comboToReturn.move3 = 0;


        //move 4
        if (combo.move4 == HitType.red)
            comboToReturn.move4 = 1;
        else if (combo.move4 == HitType.blue)
            comboToReturn.move4 = 2;
        else if (combo.move4 == HitType.green)
            comboToReturn.move4 = 3;
        else if (combo.move4 == HitType.up)
            comboToReturn.move4 = 4;
        else if (combo.move4 == HitType.down)
            comboToReturn.move4 = 5;
        else if (combo.move4 == HitType.right)
            comboToReturn.move4 = 6;
        else comboToReturn.move4 = 0;


        //move 5
        if (combo.move5 == HitType.red)
            comboToReturn.move5 = 1;
        else if (combo.move5 == HitType.blue)
            comboToReturn.move5 = 2;
        else if (combo.move5 == HitType.green)
            comboToReturn.move5 = 3;
        else if (combo.move5 == HitType.up)
            comboToReturn.move5 = 4;
        else if (combo.move5 == HitType.down)
            comboToReturn.move5 = 5;
        else if (combo.move5 == HitType.right)
            comboToReturn.move5 = 6;
        else comboToReturn.move5 = 0;

        //length
        comboToReturn.length = combo.length;

        return comboToReturn;

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
            FMODUnity.RuntimeManager.PlayOneShot("event:/High Punch", GetComponent<Transform>().position);
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
            FMODUnity.RuntimeManager.PlayOneShot("event:/Punch2", GetComponent<Transform>().position);
        }
    }

    private void LetGoGreen(InputAction.CallbackContext context)
    {

    }

    private void AddScore(int scoreToAdd)
    {

        Debug.Log("adding score");
        crowdSlider.value = crowdSlider.value + scoreToAdd; ;

    }


    public void StartTracking()
    {
        //reset timer, iterator
        inputTrackingTimer = 0;
        inputIterator = 0;
        comboIterator = 0;

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
            comboIterator += 1;
            CheckForCombo();
            inputIterator += 1;

        }
        else if (blueHitDetected)
        {
            blueHitDetected = false;
            hitInputs[inputIterator] = 2;
            hitTimes[inputIterator] = inputTrackingTimer;
            comboIterator += 1;
            CheckForCombo();
            inputIterator += 1;
        }
        else if (greenHitDetected)
        {
            greenHitDetected = false;
            hitInputs[inputIterator] = 3;
            hitTimes[inputIterator] = inputTrackingTimer;
            comboIterator += 1;
            CheckForCombo();
            inputIterator += 1;
        }


        //increase time
        inputTrackingTimer += Time.deltaTime;
    }

    private void CheckForCombo()
    {
        if (comboIterator >= 5)
            CheckForFiveHit();
        if (comboIterator >= 4)
            CheckForFourHit();
        if (comboIterator >= 3)
            CheckForThreeHit();
    }

    private void CheckForThreeHit()
    {
        //Debug.Log("Checking for valid 3 hit combo");
        float timeBetweenThree = (hitTimes[inputIterator] - hitTimes[inputIterator - 2]);
        if (timeBetweenThree < comboTime) //move has gotten three hits within the proper time
        {
            //Debug.Log("Three hit time ok");
            for (int x = 0; x < validComboIntList.Length; x++)
            {
                if ((validComboIntList[x].length == 3)
                    && (validComboIntList[x].move1 == hitInputs[inputIterator - 2])
                    && (validComboIntList[x].move2 == hitInputs[inputIterator - 1])
                    && (validComboIntList[x].move3 == hitInputs[inputIterator])) //is valid three hit
                {
                    RegisterCombo(validComboIntList[x].moveNum);
                    comboIterator = 0;
                    break;
                }
            }
        }
    }
    private void CheckForFourHit()
    {
        //Debug.Log("Checking for valid 4 hit combo");
        float timeBetweenFour = (hitTimes[inputIterator] - hitTimes[inputIterator - 3]);
        if (timeBetweenFour < comboTime) //move has gotten three hits within the proper time
        {
            for (int x = 0; x < validComboIntList.Length; x++)
            {
                if ((validComboIntList[x].length == 4)
                    && (validComboIntList[x].move1 == hitInputs[inputIterator - 3])
                    && (validComboIntList[x].move2 == hitInputs[inputIterator - 2])
                    && (validComboIntList[x].move3 == hitInputs[inputIterator - 1])
                    && (validComboIntList[x].move4 == hitInputs[inputIterator])) //is valid three hit
                {
                    RegisterCombo(validComboIntList[x].moveNum);
                    comboIterator = 0;
                    break;
                }
            }
        }

    }
    private void CheckForFiveHit()
    {
        //Debug.Log("Checking for valid 5 hit combo");
        float timeBetweenFive = (hitTimes[inputIterator] - hitTimes[inputIterator - 4]);
        if (timeBetweenFive < comboTime) //move has gotten three hits within the proper time
        {
            for (int x = 0; x < validComboIntList.Length; x++)
            {
                if ((validComboIntList[x].length == 5)
                    && (validComboIntList[x].move1 == hitInputs[inputIterator - 4])
                    && (validComboIntList[x].move2 == hitInputs[inputIterator - 3])
                    && (validComboIntList[x].move3 == hitInputs[inputIterator - 2])
                    && (validComboIntList[x].move4 == hitInputs[inputIterator - 1])
                    && (validComboIntList[x].move5 == hitInputs[inputIterator])) //is valid three hit
                {
                    RegisterCombo(validComboIntList[x].moveNum);
                    comboIterator = 0;
                    break;
                }
            }
        }

    }

    private void RegisterCombo(int comboNum)
    {
        Debug.Log("Combo " + comboNum + "  has been performed");
        combosToPlay[maxComboToPlay] = comboNum;
        maxComboToPlay += 1;
        AddScore(25); //TEMP
    }
    private void PrintHitInputArray()
    {
        for (int a = 0; a < 2000; a++)
        {
            if (hitInputs[a] != 0)
                //Debug.Log("Hit input " + a + " = " + hitInputs[a] + " Hit Found at time " + hitTimes[a]);

                if (hitInputs[a] == -1)
                    break;
        }
    }

    public void BeginHitPlayback()
    {
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
                comboInProgress = false;
                combosPlayed += 1;
                betweenComboTimer = timeBetweenCombos;

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
}
