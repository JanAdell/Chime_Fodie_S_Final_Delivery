using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ChimeScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool triggerActive = false;
    
    public InputAction pickup;
    public PlayerInput input;
    //public GameObject text;

    /*private void Awake()
    {
        input = new PlayerInput;
    }

    private void OnEnable()
    {
        pickup = input.Player.Move;
        pickup.Enable();
        
    }

    private void OnDisable()
    {
        pickup.Disable();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerActive = true;
            //text.SetActive(true);
            //print("Collision detected");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerActive = false;
            //text.SetActive(false);
        }
    }

    public void Update()
    {
        if (triggerActive == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //SceneManager.LoadScene(0);
                Debug.Log("Pressing select button");
            }
        }
    }*/
}
