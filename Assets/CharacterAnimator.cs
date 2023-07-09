using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private GameObject MidAttack;
    [SerializeField] private GameObject LowAttack;
    [SerializeField] private GameObject HighAttack;
    [SerializeField] private GameObject IdleAttack;
    [SerializeField] private GameObject MidDodge;
    [SerializeField] private GameObject HighDodge;
    [SerializeField] private GameObject LowDodge;
    [SerializeField] private GameObject IdleDodge;

    [SerializeField] private float attackAnimationTime;
    private float attackAnimationTimer; 
    [SerializeField] private float dodgeAnimationTime;
    private float dodgeAnimationTimer;
    [SerializeField] private bool testAttack;

    private void Awake()
    {
        ActivateIdleAttack();
        ActivateIdleDodge();
    }

    private void Update()
    {
        if (attackAnimationTimer > 0)
        {
            attackAnimationTimer -= Time.deltaTime;
            if (attackAnimationTimer <= 0)
            {
                ActivateIdleAttack();
            }
        }


        if (dodgeAnimationTimer > 0)
        {
            dodgeAnimationTimer -= Time.deltaTime;
            if (dodgeAnimationTimer <= 0)
            {
                ActivateIdleDodge();
            }
        }

        if (testAttack)
        {
            testAttack = false;
            StartMidAttack();
        }





    }

    public void StartHighAttack()
    {
        ActivateHighAttack();
        attackAnimationTimer = attackAnimationTime;
    }

    public void StartMidAttack()
    {
        ActivateMidAttack();
        attackAnimationTimer = attackAnimationTime;
    }

    public void StartLowAttack()
    {
        ActivateLowAttack();
        attackAnimationTimer = attackAnimationTime;
    }

    public void StartHighDodge()
    {
        ActivateHighDodge();
        dodgeAnimationTimer = dodgeAnimationTime;
    }

    public void StartMidDodge()
    {
        ActivateMidDodge();
        dodgeAnimationTimer = dodgeAnimationTime;
    }

    public void StartLowDodge()
    {
        ActivateLowDodge();
        dodgeAnimationTimer = dodgeAnimationTime;
    }






    private void ActivateMidAttack()
    {
        MidAttack.SetActive(true); 
        LowAttack.SetActive(false);
        HighAttack.SetActive(false);
        IdleAttack.SetActive(false);
    }

    private void ActivateLowAttack()
    {
        MidAttack.SetActive(false);
        LowAttack.SetActive(true);
        HighAttack.SetActive(false);
        IdleAttack.SetActive(false);
    }

    private void ActivateHighAttack()
    {
        MidAttack.SetActive(false);
        LowAttack.SetActive(false);
        HighAttack.SetActive(true);
        IdleAttack.SetActive(false);
    }
    private void ActivateIdleAttack()
    {
        MidAttack.SetActive(false);
        LowAttack.SetActive(false);
        HighAttack.SetActive(false);
        IdleAttack.SetActive(true);
    }

    private void ActivateMidDodge()
    {
        MidDodge.SetActive(true);
        HighDodge.SetActive(false);
        LowDodge.SetActive(false);
        IdleDodge.SetActive(false);
    }
    private void ActivateHighDodge()
    {
        MidDodge.SetActive(false);
        HighDodge.SetActive(true);
        LowDodge.SetActive(false);
        IdleDodge.SetActive(false);
    }
    private void ActivateLowDodge()
    {
        MidDodge.SetActive(false);
        HighDodge.SetActive(false);
        LowDodge.SetActive(true);
        IdleDodge.SetActive(false);
    }

    private void ActivateIdleDodge()
    {
        IdleDodge.SetActive(true);
        MidDodge.SetActive(false);
        HighDodge.SetActive(false);
        LowDodge.SetActive(false);
    }





}
