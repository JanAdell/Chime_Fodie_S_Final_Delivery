using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeScript : MonoBehaviour
{
    //this script will be used mainly for the chime's SFX control (delay) and showing the player the pickup related UI
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayDelayed(5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
