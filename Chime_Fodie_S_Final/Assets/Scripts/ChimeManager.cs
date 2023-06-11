using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

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

    //UI
    //[SerializeField] private CanvasGroup forestChimes;
    //[SerializeField] private CanvasGroup fieldChimes;
    //[SerializeField] private CanvasGroup totalChimes;
    [SerializeField] int forestCount = 0;
    [SerializeField] int fieldCount = 0;
    [SerializeField] int totalCount = 0;

    private int uiChime1 = 0;
    private int uiChime2 = 0;
    private int uiChime3 = 0;
    private int uiChime4 = 0;
    private int uiChime5 = 0;
    private int uiChime6 = 0;
    private int uiChime7 = 0;
    private int uiChime8 = 0;
    private int uiChime9 = 0;
    private int uiChime10 = 0;

    public Text forestText;
    
    public Text fieldText;
    
    public Text totalText;
    
    private bool fadeIn = false;
    private bool fadeOut = false;

    //final bell
    public bool finalBell;
    public GameObject finalBellObj;

    //public ChimeScript fireScript;

    void Start()
    {
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        
    }


    // Update is called once per frame
    void Update()
    {
        UpdateHUD();
        
        //Keyboard.current.anyKey.isPressed == true;
        if (Keyboard.current.escapeKey.isPressed == true)
            Application.Quit();

        //Debug.Log(chimesCollected);

        if (chime1 == true)
        {
            chime1Obj.SetActive(false);
            uiChime1 = 1;
            chime1 = false;
        }

        if (chime2 == true)
        {
            chime2Obj.SetActive(false);
            uiChime2 = 1;
        }
        if (chime3 == true)
        {
            chime3Obj.SetActive(false);
            uiChime3 = 1;
        }
        if (chime4 == true)
        {
            chime4Obj.SetActive(false);
            uiChime4 = 1;
        }
        if (chime5 == true)
        {
            chime5Obj.SetActive(false);
            uiChime5 = 1;
        }
        if (chime6 == true)
        {
            chime6Obj.SetActive(false);
            uiChime6 = 1;
        }
        if (chime7 == true)
        {
            chime7Obj.SetActive(false);
            uiChime7 = 1;
        }
        if (chime8 == true)
        {
            chime8Obj.SetActive(false);
            uiChime8 = 1;
        }
        if (chime9 == true)
        {
            chime9Obj.SetActive(false);
            uiChime9 = 1;
        }
        if (chime10 == true)
        {
            chime10Obj.SetActive(false);
            uiChime10 = 1;
        }

        if (chime1 == true && chime2 == true && chime3 == true && chime4 == true && chime5 == true && chime6 == true && chime7 == true && chime8 == true && chime9 == true && chime10 == true)
        {
            finalBellObj.SetActive(true);
            //SceneManager.LoadScene(1);
            Debug.Log("FINAL BELL SPAWNED");
        }

        if(finalBell == true)
            SceneManager.LoadScene(2);

    }
    
    private void UpdateHUD()
    {
        forestCount = uiChime1 + uiChime7 + uiChime9;
        fieldCount = uiChime3 + uiChime4 + uiChime5 + uiChime6 + uiChime8;
        totalCount = uiChime1 + uiChime2 + uiChime3 + uiChime4 + uiChime5 + uiChime6 + uiChime7 + uiChime8 + uiChime9 + uiChime10;
        forestText.text = forestCount.ToString();
        fieldText.text = fieldCount.ToString();
        totalText.text = totalCount.ToString();
    }

}
