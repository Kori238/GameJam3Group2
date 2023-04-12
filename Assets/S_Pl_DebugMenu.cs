using System.Collections;
using TMPro;
using UnityEngine;

public class S_Pl_DebugMenu : MonoBehaviour
{
    public GameObject PlayerScript;
    public Transform PlayerTransform;
    
    public TMP_Text HealthINT;
    public GameObject AdminMenuControls;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private SpriteRenderer playerVisual;

    private bool isInvis;

    //private bool menuOpen = false;
    private bool waitingForInput;


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //menuOpen = true;
            Debug.Log("Admin Menu Open.");
            AdminMenuControls.SetActive(true);
            StartCoroutine(WaitForInput());
        }

        if (waitingForInput)
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
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                PlayerTransform.position = new Vector3(85, 45, 0);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                PlayerTransform.position = new Vector3(10, 90, 0);
            }
        }
    }

    private IEnumerator WaitForInput()
    {
        waitingForInput = true;
        yield return new WaitForSeconds(2f);
        //menuOpen = false;
        waitingForInput = false;
        Debug.Log("Admin Menu Closed.");
        AdminMenuControls.SetActive(false);
    }
}
