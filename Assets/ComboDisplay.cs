using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboDisplay : MonoBehaviour
{

    [SerializeField] private InputActionReference redAction;
    [SerializeField] private InputActionReference greenAction;
    [SerializeField] private InputActionReference blueAction;

    private int[] last6Presses;
    [SerializeField] private int[] move1List;
    [SerializeField] private int[] move2List;
    [SerializeField] private int[] move3List;
    [SerializeField] private int[] move4List;
    [SerializeField] private int[] move5List;
    [SerializeField] private int[] move6List;

    [SerializeField] private GameObject[] move1Counters;
    [SerializeField] private GameObject[] move2Counters;
    [SerializeField] private GameObject[] move3Counters;
    [SerializeField] private GameObject[] move4Counters;
    [SerializeField] private GameObject[] move5Counters;
    [SerializeField] private GameObject[] move6Counters;

    

    [SerializeField] private bool trackingInputs;
    [SerializeField] private float smallSphereScale;
    [SerializeField] private float largerSphereScale;

    private void Awake()
    {
        last6Presses = new int[6];
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
        if (trackingInputs)
        {
            moveButtonPresses(1);

        }

    }

    private void HitBlue(InputAction.CallbackContext context)
    {
        if (trackingInputs)
        {
            moveButtonPresses(2);

        }

    }

    private void HitGreen(InputAction.CallbackContext context)
    {
        if (trackingInputs)
        {
            moveButtonPresses(3);

        }

    }

    private bool[] CheckMoveSequence(int[] moveList)
    {
        //check starting at 6, if input is first valid input, then set 6 as true,
        //check 5 against second input, if true continue. If all true, then return array
        //if however, input 4 is not true, start over and check 3 against input 1 until you
        //get to 0. 

        int inputIterator = 0; //first input
        bool[] returnArray = new bool[6];
        bool[] flipArray = new bool[6];

        for (int z = 0; z < 6; z++)
        {
            if (moveList[z] == last6Presses[inputIterator])
            {
                returnArray[z] = true;
                inputIterator += 1;
            }
            else
            {
                resetList(returnArray);
                inputIterator = 0;
            }
        }

        //now invert the list since its backwards
        flipArray = returnArray;
        returnArray[0] = flipArray[5];
        returnArray[1] = flipArray[4];
        returnArray[2] = flipArray[3];
        returnArray[3] = flipArray[2];
        returnArray[4] = flipArray[1];
        returnArray[5] = flipArray[0];

        //so returned list has first bubble as 0
        return returnArray;

    }

    private void WriteLights(bool[] lightList, GameObject[] moveCounterList)
    {
        for (int y = 0; y < 6; y++)
        {//LEFT OFF




        }
    }

    private void IncreaseRadius(GameObject objToExpand)
    {

    }

    private void DecreaseRadius(GameObject objToShrink)
    {

    }

    private bool[] resetList(bool[] moveList)
    {
        for (int z = 0; z < 6; z++)
        {
            moveList[z] = false;
        }

        return moveList;
    }


    public void StartTracking()
    {
        trackingInputs = true;
    }

    public void StopTracking()
    {
        trackingInputs = false;
    }

    private void moveButtonPresses(int newPress)
    {
        last6Presses[5] = last6Presses[4];
        last6Presses[4] = last6Presses[3];
        last6Presses[3] = last6Presses[2];
        last6Presses[2] = last6Presses[1];
        last6Presses[1] = last6Presses[0];
        last6Presses[0] = newPress;

        //PrintButtonList();
    }

    private void PrintButtonList()
    {
        for (int i = 0; i < 6; i++)
            Debug.Log("button press " + (i + 1) + " = " + last6Presses[i]); 
    }



}
