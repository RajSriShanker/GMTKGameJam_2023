using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class ReadHitInputs : MonoBehaviour
{
    [SerializeField] private Image redImage;
    [SerializeField] private Image greenImage;
    [SerializeField] private Image blueImage;
    [SerializeField] private Image purpleImage;

    [SerializeField] private InputActionReference redAction;
    [SerializeField] private InputActionReference greenAction;
    [SerializeField] private InputActionReference blueAction;
    [SerializeField] private InputActionReference purpleAction;

    private void Awake()
    {
        redImage.color = Color.red;
        greenImage.color = Color.green;
        blueImage.color = Color.blue;

    }
    private void OnEnable()
    {
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
        Debug.Log("Reading Red Input");
        redImage.color = Color.grey;
    }

    private void LetGoRed(InputAction.CallbackContext context)
    {
        redImage.color = Color.red;
    }

    private void HitGreen(InputAction.CallbackContext context)
    {
        Debug.Log("Reading Blue Input");
        greenImage.color = Color.grey;
    }

    private void LetGoGreen(InputAction.CallbackContext context)
    {
        greenImage.color = Color.green;
    }

    private void HitBlue(InputAction.CallbackContext context)
    {
        Debug.Log("Reading Blue Input");
        blueImage.color = Color.grey;
    }

    private void LetGoBlue(InputAction.CallbackContext context)
    {
        blueImage.color = Color.blue;
    }


}
