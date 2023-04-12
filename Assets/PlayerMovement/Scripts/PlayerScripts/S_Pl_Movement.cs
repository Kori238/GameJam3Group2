using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S_Pl_Movement : MonoBehaviour
{
    public float Pl_Speed = 16f; //Player Speed
    public Rigidbody2D RigidBody; //Reference to RigidBody2D
    public int Health = 100;
    public AudioSource hitSound;
    public GameObject SoundControllerScript;
    public Transform PlayerTransform;
    private Vector2 Movement;
    public Slider staminaBar;
    private float staminaRegenDelay = 0.2f;

    private void Update() // Update is called once per frame
    {
        //Input:

        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");


        if (Input.GetButtonDown("LeftShift"))
        {
            if (staminaBar.value >= 50)
            {
                SoundControllerScript.GetComponent<S_SoundController>().Dash();
                StartCoroutine(DashDelay());
            }
        }

        if (Health <= 0)
        {
            PlayerDeath();
        }
    }

    private void FixedUpdate() //Executed on a fixed timer (Not on framerate)
    {
        //Movement:

        RigidBody.MovePosition(RigidBody.position + Movement * Pl_Speed * Time.fixedDeltaTime);

        if (Movement.x > 0)
        {
            gameObject.transform.localScale = new Vector3(6.401755f, 7.14f, 0.62f);
        }
        else if (Movement.x < 0)
        {
            gameObject.transform.localScale = new Vector3(-6.401755f, 7.14f, 0.62f);
        }
    }

    private void PlayerDeath()
    {
        PlayerTransform.position = new Vector3(2.154672f, 13.90528f, -0.4391842f);
        Health = 100;
        Debug.Log("YOU DIED"); //REPLACE HERE FOR PLAYER DEATH SCREEN, REMOVE ALL RESOURCES
    }

    private IEnumerator DashDelay()
    {
        var abilities = GetComponent<S_Pl_Abilities>();
        Pl_Speed = 45f;
        abilities.DashZoom();
        DecreaseStamina(50);
        yield return new WaitForSeconds(0.3f);
        Pl_Speed = 16f;
        abilities.DashunZoom();
    }

    private void DecreaseStamina(int staminaUsed)
    {
        staminaBar.value -= staminaUsed;
        StartCoroutine(RegenStamina());
    }

    private IEnumerator RegenStamina()
    {
        while (staminaBar.value != 100)
        {
            staminaBar.value += 1;
            yield return new WaitForSeconds(staminaRegenDelay);
        }
    }
}
