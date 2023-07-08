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

    }

    private void OnDisable()
    {
        redAction.action.started -= HitRed;
        blueAction.action.started -= HitBlue;
        greenAction.action.started -= HitGreen;

    }

    private void HitRed(InputAction.CallbackContext context)
    {
        if (trackInputs)
        {
            redHitDetected = true;
            redTargetMesh.material.color = Color.grey;
            redColorTimer = colorChangeTime;
        }

    }

    private void HitBlue(InputAction.CallbackContext context)
    {
        if (trackInputs)
        {
            blueHitDetected = true;
            blueTargetMesh.material.color = Color.grey;
            blueColorTimer = colorChangeTime;
        }

    }

    private void HitGreen(InputAction.CallbackContext context)
    {
        if (trackInputs)
        {
            greenHitDetected = true;
            greenTargetMesh.material.color = Color.grey;
            greenColorTimer = colorChangeTime;
        }
    }

    private void CheckColorTimers()
    {
        if (redColorTimer > 0)
        {
            redColorTimer -= Time.deltaTime;
            if (redColorTimer <= 0)
            {
                redTargetMesh.material.color = Color.red;
            }
        }

        if (greenColorTimer > 0)
        {
            greenColorTimer -= Time.deltaTime;
            if (greenColorTimer <= 0)
            {
                greenTargetMesh.material.color = Color.green;
            }
        }

        if (blueColorTimer > 0)
        {
            blueColorTimer -= Time.deltaTime;
            if (blueColorTimer <= 0)
            {
                blueTargetMesh.material.color = Color.blue;
            }
        }
    }

    private void Update()
    {
        CheckColorTimers();
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
            if (blueTarget.currentlySelectedHit != null)
            {
                Destroy(blueTarget.currentlySelectedHit);
            }
            if (greenTarget.currentlySelectedHit != null)
            {
                Destroy(greenTarget.currentlySelectedHit);
            }
            redHitDetected = false;
        }


        if (blueHitDetected)
        {
            if (redTarget.currentlySelectedHit != null)
            {
                Destroy(redTarget.currentlySelectedHit);
            }
            if (greenTarget.currentlySelectedHit != null)
            {
                Destroy(greenTarget.currentlySelectedHit);
            }
            blueHitDetected = false;
        }


        if (greenHitDetected)
        {
            if (blueTarget.currentlySelectedHit != null)
            {
                Destroy(blueTarget.currentlySelectedHit);
            }
            if (redTarget.currentlySelectedHit != null)
            {
                Destroy(redTarget.currentlySelectedHit);
            }
            greenHitDetected = false;
        }

        if (playerTarget.currentlySelectedHit != null)
        {
            Destroy(playerTarget.currentlySelectedHit);
            playerHealth -= healthLostOnHit;
            playerHealthSlider.value = playerHealth;
        }
    }




}
