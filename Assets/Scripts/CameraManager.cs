using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private Vector3 initialCamPos;
    [SerializeField] private Vector3 secondCamPos;
    [SerializeField] private float moveTime;
    private float moveTimer;
    private bool moveTo1;
    private bool moveTo2;

    private void Awake()
    {
        camTransform.position = initialCamPos;
    }

    public void MoveCam(int side)
    {
        if (side == 1)
        {
            moveTimer = 0;
            moveTo1 = true;
        }
        else if (side == 2)
        {
            moveTimer = 0;
            moveTo2 = true;
        }
        else return;
    }

    private void Update()
    {
        if (moveTo1)
        {
            moveTimer += Time.deltaTime;
            camTransform.position = Vector3.Lerp(secondCamPos, initialCamPos, moveTimer / moveTime);
            if (moveTimer >= moveTime)
            {
                moveTo1 = false;
            }
        }
        else if (moveTo2)
        {
            moveTimer += Time.deltaTime;
            camTransform.position = Vector3.Lerp(initialCamPos, secondCamPos, moveTimer / moveTime);
            if (moveTimer >= moveTime)
            {
                moveTo2 = false;
            }
        }


    }

}
