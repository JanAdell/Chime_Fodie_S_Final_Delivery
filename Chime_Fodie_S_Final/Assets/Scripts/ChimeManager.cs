using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class ChimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    //chimes
    //public int chimesCollected;
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

    //final bell
    public bool finalBell;
    public GameObject finalBellObj;

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

        //Debug.Log(chimesCollected);

        if (chime1 == true)
            chime1Obj.SetActive(false);

        if (chime2 == true)
            chime2Obj.SetActive(false);

        if (chime3 == true)
            chime3Obj.SetActive(false);

        if (chime4 == true)
            chime4Obj.SetActive(false);

        if (chime5 == true)
            chime5Obj.SetActive(false);

        if (chime6 == true)
            chime6Obj.SetActive(false);
        
        if (chime7 == true)
            chime7Obj.SetActive(false);

        if (chime8 == true)
            chime8Obj.SetActive(false);
             
        if (chime9 == true)
            chime9Obj.SetActive(false);
        
        if (chime10 == true)
            chime10Obj.SetActive(false);


        if (chime1 == true && chime2 == true && chime3 == true && chime4 == true && chime5 == true && chime6 == true && chime7 == true && chime8 == true && chime9 == true && chime10 == true)
        {
            finalBellObj.SetActive(true);
            //SceneManager.LoadScene(1);
            Debug.Log("FINAL BELL SPAWNED");
        }

        if(finalBell == true)
            SceneManager.LoadScene(2);

    }
}
