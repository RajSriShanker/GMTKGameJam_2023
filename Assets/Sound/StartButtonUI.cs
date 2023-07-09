using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/StartUI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
