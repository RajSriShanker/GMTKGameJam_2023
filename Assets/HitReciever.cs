using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReciever : MonoBehaviour
{
    public GameObject currentlySelectedHit;
    public GameObject secondSelectedHit;


    private void OnTriggerEnter(Collider other)
    {
        //register which note is currently selected (some notes are so close such that two enters will precede two exits so keep that in mind)

        //TODO: MAKE WORK FOR TWO IN RApid succession)_
        currentlySelectedHit = other.transform.gameObject;


    }

    private void OnTriggerExit(Collider other)
    {
        currentlySelectedHit = null;
        
    }
}
