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
    public bool chime1;
    public bool chime2;
    public bool chime3;
    public bool chime4;
    public bool chime5;
    public bool chime6;
    public bool chime7;
    public bool chime8;
    public bool chime9;
    public bool chime10;
    public GameObject chime1Obj;
    public GameObject chime2Obj;
    public GameObject chime3Obj;
    public GameObject chime4Obj;
    public GameObject chime5Obj;
    public GameObject chime6Obj;
    public GameObject chime7Obj;
    public GameObject chime8Obj;
    public GameObject chime9Obj;
    public GameObject chime10Obj;
    //private Keyboard keyboard;

    //public ChimeScript fireScript;

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        //Keyboard.current.anyKey.isPressed == true;
        if (Keyboard.current.escapeKey.isPressed == true)
            Application.Quit();

        Debug.Log(chimesCollected);

        if (chime1 == true)
        {
            chime1 = false;
            chime1Obj.SetActive(false);
            chimesCollected++;
            
        }

        if (chime2 == true)
        {
            chime2Obj.SetActive(false);
            chimesCollected++;
            chime2 = false;
        }

        if (chime3 == true)
        {
            chime3Obj.SetActive(false);
            chimesCollected++;
            chime3 = false;
        }

        if (chime4 == true)
        {
            chime4Obj.SetActive(false);
            chimesCollected++;
            chime4 = false;
        }

        if (chime5 == true)
        {
            chime5Obj.SetActive(false);
            chimesCollected++;
            chime5 = false;
        }

        if (chime6 == true)
        {
            chime6Obj.SetActive(false);
            chimesCollected++;
            chime6 = false;
        }

        if (chime7 == true)
        {
            chime7Obj.SetActive(false);
            chimesCollected++;
            chime7 = false;
        }

        if (chime8 == true)
        {
            chime8Obj.SetActive(false);
            chimesCollected++;
            chime8 = false;
        }

        if (chime9 == true)
        {
            chime9Obj.SetActive(false);
            chimesCollected++;
            chime9 = false;
        }

        if (chime10 == true)
        {
            chime10Obj.SetActive(false);
            chimesCollected++;
            chime10 = false;
        }

        if (chimesCollected == 10)
        {
            //SceneManager.LoadScene(1);
        }

    }
}
