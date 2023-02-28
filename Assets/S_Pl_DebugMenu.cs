using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_Pl_DebugMenu : MonoBehaviour
{
    public GameObject PlayerScript;
    public Transform PlayerTransform;
    public TMP_Text HealthINT;
    private bool menuOpen = false;
    private bool waitingForInput = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            menuOpen = true;
            Debug.Log("Admin Menu Open.");
            StartCoroutine(WaitForInput() );
        }

        if (waitingForInput == true)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                PlayerScript.GetComponent<S_Pl_Movement>().Health = 10000;
                HealthINT.text = PlayerScript.GetComponent<S_Pl_Movement>().Health.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerScript.GetComponent<S_Pl_Movement>().Pl_Speed = 50f;
            }
        }
    }

    IEnumerator WaitForInput()
    {
        waitingForInput = true;
        yield return new WaitForSeconds(2f);
        menuOpen = false;
        waitingForInput = false;
        Debug.Log("Admin Menu Closed.");
    }
}
