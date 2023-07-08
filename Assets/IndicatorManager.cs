using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicatorManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro comboIndicator;
    [SerializeField] private TextMeshPro dodgeIndicator;
    private bool startComboTimer;
    private bool startDodgeTimer;
    [SerializeField] private float dodgeMessageTime;
    [SerializeField] private float comboMessageTime;
    private float dodgeTimer;
    private float comboTimer;

    [SerializeField] private bool testHit;
    [SerializeField] private bool testDodge;
    [SerializeField] private bool testCombo;
    [SerializeField] private bool testFail;

    private void Awake()
    {
        dodgeIndicator.text = "";
        comboIndicator.text = "";
    }

    public void ShowHit() //just text for now, instatiate the image later
    {
        dodgeIndicator.text = "HIT";
        dodgeTimer = dodgeMessageTime;
        startDodgeTimer = true;
    }

    public void ShowDodge()
    {
        dodgeIndicator.text = "DODGE";
        dodgeTimer = dodgeMessageTime;
        startDodgeTimer = true;
    }

    public void ShowCombo()
    {
        comboIndicator.text = "COMBO!!";
        comboTimer = comboMessageTime;
        startComboTimer = true;

    }

    public void ShowComboFail()
    {
        comboIndicator.text = "FAIL";
        comboTimer = comboMessageTime;
        startComboTimer = true;

    }


    void Update()
    {
        if (testCombo)
        {
            testCombo = false;
            ShowCombo();
        }

        if (testFail)
        {
            testFail = false;
            ShowComboFail();
        }


        if (testDodge)
        {
            testDodge = false;
            ShowDodge();
        }

        if (testHit)
        {
            testHit = false;
            ShowHit();
        }


        if (startDodgeTimer)
        {
            dodgeTimer -= Time.deltaTime;
            if (dodgeTimer <= 0)
            {
                startDodgeTimer = false;
                dodgeIndicator.text = "";
            }
        }


        if (startComboTimer)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                startComboTimer = false;
                comboIndicator.text = "";
            }
        }

    }
}
