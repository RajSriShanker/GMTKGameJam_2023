using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private Vector3 initialCamPos;
    [SerializeField] private Vector3 secondCamPos;

    private void Awake()
    {
        MoveCam(1);
    }

    public void MoveCam(int side)
    {
        if (side == 1)
            camTransform.position = initialCamPos;
        else if (side == 2)
            camTransform.position = secondCamPos;
        else return;
    }

}
