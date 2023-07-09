using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HitReceiverManager : MonoBehaviour
{
    private HitReciever redTarget;
    private HitReciever blueTarget;
    private HitReciever greenTarget;
    private HitReciever playerTarget;
    [SerializeField] private InputActionReference redAction;
    [SerializeField] private InputActionReference greenAction;
    [SerializeField] private InputActionReference blueAction;
    [SerializeField] private InputActionReference purpleAction;
    [SerializeField] private bool trackInputs;

    [SerializeField] private GameObject redIndicator;
    [SerializeField] private GameObject greenIndicator;
    [SerializeField] private GameObject blueIndicator;
    [SerializeField] private GameObject playerTargetObj;

    private MeshRenderer redTargetMesh;
    private MeshRenderer greenTargetMesh;
    private MeshRenderer blueTargetMesh;
    private float redColorTimer;
    private float greenColorTimer;
    private float blueColorTimer;
    [SerializeField] private float colorChangeTime;
    private bool redHitDetected;
    private bool blueHitDetected;
    private bool greenHitDetected;
    private float playerHealth;
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private float healthLostOnHit;
    [SerializeField] private IndicatorManager indicatorManager;
    [SerializeField] private SpriteRenderer redMesh;
    [SerializeField] private SpriteRenderer greenMesh;
    [SerializeField] private SpriteRenderer blueMesh;
    [SerializeField] private Color redColor;
    [SerializeField] private Color greenColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color greyColor;
    [SerializeField] private CameraShakeController shakeController;
    [SerializeField] private CharacterAnimator animator;
    [SerializeField] private FMODPlayer soundPlayer;
    private int blocksInARow;
    private int failsInARow;
    [SerializeField] private int failsInARowAllowed;
    [SerializeField] private int blocksInARowForCheer;
    [SerializeField] private float booCoolDown;
    private float bootimer;
    private bool booAllowed;


    private void Awake()
    {
        redTargetMesh = redIndicator.GetComponent<MeshRenderer>();
        greenTargetMesh = greenIndicator.GetComponent<MeshRenderer>();
        blueTargetMesh = blueIndicator.GetComponent<MeshRenderer>();
        redTarget = redIndicator.GetComponent<HitReciever>();
        blueTarget = blueIndicator.GetComponent<HitReciever>();
        greenTarget = greenIndicator.GetComponent<HitReciever>();
        playerTarget = playerTargetObj.GetComponent<HitReciever>();

        playerHealthSlider.maxValue = playerMaxHealth;
        playerHealth = playerMaxHealth;

        redMesh.color = redColor;
        greenMesh.color = greenColor;
        blueMesh.color = blueColor;
        booAllowed = true;
    }

    private void OnEnable()
    {
        //leave in on disable in case we want hold functionality
        redAction.action.started += HitRed;
        blueAction.action.started += HitBlue;
        greenAction.action.started += HitGreen;
        redAction.action.canceled += LetGoRed;
        blueAction.action.canceled += LetGoBlue;
        greenAction.action.canceled += LetGoGreen;

    }

    private void OnDisable()
    {
        redAction.action.started -= HitRed;
        blueAction.action.started -= HitBlue;
        greenAction.action.started -= HitGreen;

    }

    private void HitRed(InputAction.CallbackContext context)
    {
        StartRed();
        EndBlue();
        EndGreen();
    }

    private void HitBlue(InputAction.CallbackContext context)
    {
        StartBlue();
        EndRed();
        EndGreen();

    }

    private void HitGreen(InputAction.CallbackContext context)
    {
        StartGreen();
        EndBlue();
        EndRed();
    }

    private void LetGoGreen(InputAction.CallbackContext context)
    {
        EndGreen();
    }

    private void LetGoRed(InputAction.CallbackContext context)
    {
        EndRed();
    }

    private void LetGoBlue(InputAction.CallbackContext context)
    {
        EndBlue();
    }


    private void StartRed()
    {
        if (trackInputs)
        {
            redHitDetected = true;
            redMesh.color = greyColor;
        }

    }

    private void StartBlue()
    {
        if (trackInputs)
        {
            blueHitDetected = true;
            blueMesh.color = greyColor;
        }

    }

    private void StartGreen()
    {
        if (trackInputs)
        {
            greenHitDetected = true;
            greenMesh.color = greyColor;
        }

    }

    private void EndRed()
    {
        if (trackInputs)
        {
            redHitDetected = false;
            redMesh.color = redColor;
        }

    }

    private void EndBlue()
    {
        if (trackInputs)
        {
            blueHitDetected = false;
            blueMesh.color = blueColor;
        }

    }
    private void EndGreen()
    {
        if (trackInputs)
        {
            greenHitDetected = false;
            greenMesh.color = greenColor;
        }

    }

    private void Update()
    {
        CheckReceivers();
        CheckBooTimer();
    }

    public void StartTracking()
    {
        trackInputs = true;
    }

    public void EndTracking()
    {
        trackInputs = false;
    }

    private void CheckReceivers()
    {
        if (redHitDetected)
        {
            if (redTarget.currentlySelectedHit != null)
            {
                Destroy(redTarget.currentlySelectedHit);
                soundPlayer.PlayDodge();
                soundPlayer.PlayRedShot();
                indicatorManager.ShowDodge();
                animator.StartMidAttack();
                animator.StartMidDodge();
                RegisterBlock();
            }
        }


        if (blueHitDetected)
        {
            if (blueTarget.currentlySelectedHit != null)
            {
                Destroy(blueTarget.currentlySelectedHit);
                soundPlayer.PlayDodge();
                soundPlayer.PlayBlueShot();
                indicatorManager.ShowDodge();
                animator.StartLowAttack();
                animator.StartLowDodge();
                RegisterBlock();
            }
        }


        if (greenHitDetected)
        {
            if (greenTarget.currentlySelectedHit != null)
            {
                Destroy(greenTarget.currentlySelectedHit);
                soundPlayer.PlayDodge();
                soundPlayer.PlayGreenShot();
                indicatorManager.ShowDodge();
                animator.StartHighAttack();
                animator.StartHighDodge();
                RegisterBlock();
            }
        }

        if (playerTarget.currentlySelectedHit != null)
        {
            Destroy(playerTarget.currentlySelectedHit);
            playerHealth -= healthLostOnHit;
            playerHealthSlider.value = playerHealth;
            soundPlayer.PlayHurt();
            blocksInARow = 0;
            indicatorManager.ShowHit();
            RegisterBoo();
            shakeController.OnShake(0.1f, 0.2f);
        }
    }

    private void RegisterBlock()
    {
        failsInARow = 0;
        blocksInARow += 1;
        if (blocksInARow >= blocksInARowForCheer)
        {
            blocksInARow = 0;
            soundPlayer.PlayCheer();
        }
    }

    private void RegisterBoo()
    {
        failsInARow += 1;
        if ((failsInARow >= failsInARowAllowed) && (booAllowed))
        {
            soundPlayer.PlayBoo();
            failsInARow = 0;
            bootimer = booCoolDown;
            booAllowed = false;
        }
    }

    private void CheckBooTimer()
    {
        if (bootimer > 0)
        {
            bootimer -= Time.deltaTime;
            if (bootimer <= 0)
            {
                booAllowed = true;
            }
        }
    }




}
