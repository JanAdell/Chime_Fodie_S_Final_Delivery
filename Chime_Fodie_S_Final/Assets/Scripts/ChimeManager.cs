using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ChimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int chimesCollected;
    public GameObject chime1;
    public GameObject chime2;
    public GameObject chime3;
    public GameObject chime4;
    public GameObject chime5;
    public GameObject chime6;
    public GameObject chime7;
    public GameObject chime8;
    public GameObject chime9;
    public GameObject chime10;
    
    //public ChimeScript fireScript;

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(chimesCollected);
    }
}
