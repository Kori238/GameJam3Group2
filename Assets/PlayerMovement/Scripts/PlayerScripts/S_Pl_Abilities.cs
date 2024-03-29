using System.Collections;
using System.Linq;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class S_Pl_Abilities : MonoBehaviour
{
    public GameObject MovementScript;
    public GameObject SoundControllerScript;
    public Collider2D AttackCollider;
    public Slider swordSwingSlider;
    public GameObject swordSwingObject;
    [SerializeField] private Collider2D playerCollider, digTrigger;
    [SerializeField] private SpriteRenderer playerVisual, digIcon;
    [SerializeField] private float digSpeedMultiplier = 0.5f;
    public int maxScroll;

    private Animator animator;
    private bool canAttack = true, canDig = true;
    private GameObject enemy1;
    public bool flag, digging;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
            if (Camera.main.orthographicSize <= maxScroll)
            {
                Camera.main.orthographicSize += 5;
            }
           
        }
        else if (digging)
        {
            
        }
        else if (Input.GetButtonDown("Fire1"))
        {
           // Ability1();
        }
        else if (Input.GetButtonDown("Ability2"))
        {
            animator.Play("Idle");
            Debug.Log("SideAttack");
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            if (canAttack)
            {
                Debug.Log("AxeSwing");
                SoundControllerScript.GetComponent<S_SoundController>().AttackSound();
                StartCoroutine(SwingAxe());
            }
            else
            {
                Debug.Log("Can't Axe");
            }
        }
        
    }

    private IEnumerator SwingAxe()
    {
        animator.Play("Axe Swing");
        yield return new WaitForSeconds(0.5f);
        animator.Play("Idle");
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
        playerCollider.gameObject.layer = 13;
        playerVisual.enabled = false;
        digIcon.enabled = true;
        MovementScript.GetComponent<S_Pl_Movement>().Pl_Speed *= digSpeedMultiplier;
        yield return null;
    }

    private IEnumerator finishDig()
    {
        playerCollider.gameObject.layer = 6;
        playerVisual.enabled = true;
        digging = false;
        canDig = true;
        digIcon.enabled = false;
        MovementScript.GetComponent<S_Pl_Movement>().Pl_Speed /= digSpeedMultiplier;
        yield return null;
    }



    public void Ability1()
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
        swordSwingObject.SetActive(true);
        swordSwingSlider.value = 0;
        canAttack = false;
        animator.Play("Attack");
        float timer = 0.7f;
        float i;
        for (i = 0; i < timer; i += 0.1f) //Increases value of slider over time with the delay.
        {
            yield return new WaitForSeconds(0.1f);
            swordSwingSlider.value = i;
            if(i > 0.1 && i < 0.6)
            {
                AttackCollider.enabled = true;
            }
        }
        swordSwingSlider.value = timer;
        animator.Play("Idle");
        yield return new WaitForSeconds(0.2f);
        AttackCollider.enabled = false;
        canAttack = true;
        swordSwingObject.SetActive(false);
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
