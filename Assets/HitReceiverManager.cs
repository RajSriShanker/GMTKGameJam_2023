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
            redTargetMesh.material.color = Color.grey;
        }

    }

    private void StartBlue()
    {
        if (trackInputs)
        {
            blueHitDetected = true;
            blueTargetMesh.material.color = Color.grey;
        }

    }

    private void StartGreen()
    {
        if (trackInputs)
        {
            greenHitDetected = true;
            greenTargetMesh.material.color = Color.grey;
        }

    }

    private void EndRed()
    {
        if (trackInputs)
        {
            redHitDetected = false;
            redTargetMesh.material.color = Color.red;
        }

    }

    private void EndBlue()
    {
        if (trackInputs)
        {
            blueHitDetected = false;
            blueTargetMesh.material.color = Color.blue;
        }

    }
    private void EndGreen()
    {
        if (trackInputs)
        {
            greenHitDetected = false;
            greenTargetMesh.material.color = Color.green;
        }

    }

    private void Update()
    {
        CheckReceivers();
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
                indicatorManager.ShowDodge();
            }
        }


        if (blueHitDetected)
        {
            if (blueTarget.currentlySelectedHit != null)
            {
                Destroy(blueTarget.currentlySelectedHit);
                indicatorManager.ShowDodge();
            }
        }


        if (greenHitDetected)
        {
            if (greenTarget.currentlySelectedHit != null)
            {
                Destroy(greenTarget.currentlySelectedHit);
                indicatorManager.ShowDodge();
            }
        }

        if (playerTarget.currentlySelectedHit != null)
        {
            Destroy(playerTarget.currentlySelectedHit);
            playerHealth -= healthLostOnHit;
            playerHealthSlider.value = playerHealth;
            indicatorManager.ShowHit();
        }
    }




}
