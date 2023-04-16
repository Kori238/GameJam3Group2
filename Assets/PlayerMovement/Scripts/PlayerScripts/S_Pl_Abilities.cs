using System.Collections;
using System.Linq;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class S_Pl_Abilities : MonoBehaviour
{
    public GameObject MovementScript;
    public GameObject SoundControllerScript;
    public Collider2D AttackCollider;
    [SerializeField] private Collider2D playerCollider, digTrigger;
    [SerializeField] private SpriteRenderer playerVisual, digIcon;
    [SerializeField] private float digSpeedMultiplier = 0.5f;
    private Animator animator;
    private bool canAttack = true, canDig = true;
    private GameObject enemy1;
    private bool flag, digging;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
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
        else if (Input.GetKeyDown(KeyCode.G))
        {
            animator.Play("Axe Swing");
            animator.Play("Idle");
        }
        //else if (Input.GetButtonDown("Camera"))           
        //{
        //    if (!flag)
        //    {
        //        CameraZoomValue = 40;
        //        flag = true;
        //        MovementScript.GetComponent<S_Pl_Movement>().Pl_Speed = 5;
        //    }
        //    else
        //    {
        //        CameraZoomValue = 18;
        //        flag = false;
        //        MovementScript.GetComponent<S_Pl_Movement>().Pl_Speed = 16;
        //    }
        //    Camera.main.orthographicSize = CameraZoomValue;
        //}
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Dig();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if (Camera.main.orthographicSize >= 15)
            {
                Camera.main.orthographicSize -= 5;
            }
            
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (Camera.main.orthographicSize <= 200)
            {
                Camera.main.orthographicSize += 5;
            }
           
        }
    }

    public void Dig()
    {
        if (!digging && canDig)
        {
            StartCoroutine(beginDig());
        }
        else
        {
            var canEmerge = true;
            var results = new Collider2D[10];
            var obstructions = new string[0];
            var filter = new ContactFilter2D().NoFilter();
            _ = digTrigger.OverlapCollider(filter, results);
            foreach (var result in results)
            {
                if (result != null && (result.CompareTag("Wall") ||
                                       result.gameObject.layer == LayerMask.NameToLayer("Structures")))
                {
                    canEmerge = false;
                    obstructions.Append(result.name);
                }
            }
            if (canEmerge) StartCoroutine(finishDig());
            else Debug.Log("Player attempted to emerge however the location was obstructed by " + obstructions);
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



    private void Ability1()
    {
        if (canAttack)
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

    public void DashZoom()
    {
        Camera.main.orthographicSize = Camera.main.orthographicSize - 10;
    }
    public void DashunZoom()
    {
        Camera.main.orthographicSize = Camera.main.orthographicSize + 10;
    }
}
