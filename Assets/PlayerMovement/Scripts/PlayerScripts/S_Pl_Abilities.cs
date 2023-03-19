using System.Collections;
using System.Linq;
using UnityEngine;

public class S_Pl_Abilities : MonoBehaviour
{
    private bool flag, digging = false;
    private GameObject enemy1;
    private int CameraZoomValue = 40;
    Animator animator;
    public GameObject MovementScript;
    public GameObject SoundControllerScript;
    public Collider2D AttackCollider;
    [SerializeField] private Collider2D playerCollider, digTrigger;
    [SerializeField] private SpriteRenderer playerVisual, digIcon;
    [SerializeField] private float digSpeedMultiplier = 0.5f;
    private bool canAttack = true, canDig = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Dig()
    {

        if (!digging && canDig)
        {
            StartCoroutine(beginDig());
        }
        else
        {
            bool canEmerge = true;
            Collider2D[] results = new Collider2D[10];
            string[] obstructions = new string[0];
            ContactFilter2D filter = new ContactFilter2D().NoFilter();
            _ = digTrigger.OverlapCollider(filter, results);
            foreach (Collider2D result in results)
            {
                if (result != null && (result.CompareTag("Wall") || (result.gameObject.layer == LayerMask.NameToLayer("Structures"))))
                {
                    canEmerge = false;
                    obstructions.Append(result.name.ToString());
                }
            }
            if (canEmerge) StartCoroutine(finishDig());
            else Debug.Log("Player attempted to emerge however the location was obstructed by " + obstructions.ToString());
        }
    }

    private IEnumerator beginDig()
    {
        digging = true;
        canDig = false;
        playerCollider.enabled = false;
        playerVisual.enabled = false;
        digIcon.enabled = true;
        MovementScript.GetComponent<S_Pl_Movement>().Pl_Speed *= digSpeedMultiplier;
        yield return null;
    }

    private IEnumerator finishDig()
    {
        playerCollider.enabled = true;
        playerVisual.enabled = true;
        digging = false;
        canDig = true;
        digIcon.enabled = false;
        MovementScript.GetComponent<S_Pl_Movement>().Pl_Speed /= digSpeedMultiplier;
        yield return null;
    }



    void Ability1()
    {
        if (canAttack == true)
        {
            Debug.Log("MainAttack");
            SoundControllerScript.GetComponent<S_SoundController>().AttackSound();
            StartCoroutine(AttackDelay());
        }
        else
        {
            Debug.Log("Can't Attack");
        }
    }

    private IEnumerator AttackDelay()
    {
        canAttack = false;
        AttackCollider.enabled = true;
        animator.Play("Attack");
        yield return new WaitForSeconds(1f);
        animator.Play("Idle");
        AttackCollider.enabled = false;
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ability1();

        }
        else if (Input.GetButtonDown("Ability2"))
        {
            animator.Play("Idle");
            Debug.Log("SideAttack");
        }
        else if (Input.GetButtonDown("Camera"))
        {
            if (!flag)
            {
                CameraZoomValue = 40;
                flag = true;
                MovementScript.GetComponent<S_Pl_Movement>().Pl_Speed = 5;
            }
            else
            {
                CameraZoomValue = 18;
                flag = false;
                MovementScript.GetComponent<S_Pl_Movement>().Pl_Speed = 16;
            }
            Camera.main.orthographicSize = CameraZoomValue;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Dig();
        }

    }

    public void DashZoom()
    {
        CameraZoomValue = 16;
        Camera.main.orthographicSize = CameraZoomValue;
    }
    public void DashunZoom()
    {
        CameraZoomValue = 18;
        Camera.main.orthographicSize = CameraZoomValue;
    }

}
