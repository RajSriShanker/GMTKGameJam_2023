using FMOD.Studio;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FMODPlayer : MonoBehaviour
{


    public FMOD.Studio.EventInstance instance;

    public FMODPlayer(EventInstance instance)
    {
        this.instance = FMODUnity.RuntimeManager.CreateInstance("event:/CrowdAmbeince");
        instance.start();
    }

    //void Start()
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Red", transform.position); //red one shot
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Blue", transform.position); //blue one shot
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Green", transform.position); //green blue shot

    //    //player dodges
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Dodge");

    //    //player hurt
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerHurt");

    //    //placeholder combo sfx (working on a better one)
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Combo");

    //    //cheers for when combo is successful
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/CrowdCheer-oneshots");

    //    //boos for when combo fails
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/CrowdBoo-oneshots");

    public void PlayRedShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Red", transform.position);
    }

    public void PlayBlueShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Blue", transform.position);
    }

    public void PlayGreenShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Green", transform.position);
    }

    public void PlayDodge()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Dodge");
    }

    public void PlayHurt()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerHurt");
    }

    public void PlayCombo()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Combo");
    }

    public void PlayCheer()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/CrowdCheer-oneshots");
    }

    public void PlayBoo()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/CrowdBoo-oneshots");

    }

    public void StopBoo()
    {

    }


}
    
  





