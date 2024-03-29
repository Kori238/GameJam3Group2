using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class S_Pl_DebugMenu : MonoBehaviour
{
    public GameObject PlayerScript;
    public Transform PlayerTransform;
    public GameObject ComputerUsage;
    //public TMP_Text HealthINT;
    public GameObject AdminMenuControls;
    public GameObject TimeController;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private SpriteRenderer playerVisual;

    private bool isInvis;

    //private bool menuOpen = false;
    private bool waitingForInput;
    private bool statsOpen;

    //public bool adminMode = false;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (GameObject.Find("DebugMode") != null)
            {
                bool adminModeFetch = checkPassword.Instance.adminMode;
                Debug.Log("Fetch: " + adminModeFetch);

                if (adminModeFetch == true)
                {
                    DebugMode.Instance.enabled = true;
                    Debug.Log("Admin Menu Open.");
                    AdminMenuControls.SetActive(true);
                    StartCoroutine(WaitForInput());
                }
                else
                {
                    Debug.Log("Access Denied: please enter admin password in options.");
                }
            }
        }

        if (waitingForInput)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                PlayerScript.GetComponent<S_Pl_Movement>().Health = 10000;
                //HealthINT.text = PlayerScript.GetComponent<S_Pl_Movement>().Health.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerScript.GetComponent<S_Pl_Movement>().Pl_Speed = 50f;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                if (isInvis == false) //85 45
                {
                    playerCollider.enabled = false;
                    playerVisual.color = Color.black;
                    isInvis = true;
                }
                else
                {
                    playerCollider.enabled = true;
                    playerVisual.color = Color.white;
                    isInvis = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                Camera.main.orthographicSize = 200;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (statsOpen == true)
                {
                    ComputerUsage.SetActive(false);
                    statsOpen = false;
                }
                else
                {
                    ComputerUsage.SetActive(true);
                    statsOpen = true;
                }

            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                TimeController.GetComponent<TimeController>().ForceSpawnEnemy();
            }
            else if (Input.GetKeyDown("1"))
            {
                PlayerTransform.position = new Vector3(51, 440, 0);
            }
            else if (Input.GetKeyDown("2"))
            {
                PlayerTransform.position = new Vector3(451, 44, 0);
            }
            else if (Input.GetKeyDown("3"))
            {
                PlayerTransform.position = new Vector3(45, 45, 0);
            }
        }
    }

    private IEnumerator WaitForInput()
    {
        waitingForInput = true;
        yield return new WaitForSeconds(5f);
        //menuOpen = false;
        waitingForInput = false;
        Debug.Log("Admin Menu Closed.");
        AdminMenuControls.SetActive(false);
    }
}
