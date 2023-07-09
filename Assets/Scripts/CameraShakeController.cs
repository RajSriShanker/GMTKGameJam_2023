using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShakeController : MonoBehaviour
{
    public static CameraShakeController Instance;

    private void Awake() => Instance = this;

    public void OnShake(float duration, float strength)
    {
        transform.DOShakePosition(duration, strength);
        transform.DOShakeRotation(duration, strength);
    }

    public static void Shake(float duration, float strength) => Instance.OnShake(duration, strength);

    //Call this method by 
    //CameraShakeController.Shake(0.5f, 0.5f);


}
