using System.Collections;
using TMPro;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public Transform target; // The target sprite
    public float speed = 5f; // The speed of movement

    public TMP_Text HealthINT;

    public GameObject SoundControllerScript;

    public GameObject PlayerScript;

    public int health = 20;

    private bool canAttack = true;

    private void Update()
    {
        // Calculate the direction to move towards the target
        var direction = (target.position - transform.position).normalized;

        // Move the sprite towards the target
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) <=
            4f) //Determinds the distance between the player and the enemy.
        {
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(DamagePlayer());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) //If the Collider is the Players attack range then...
        {
            Damaged(5);
            SoundControllerScript.GetComponent<S_SoundController>().HurtMonster();
        }
    }

    private IEnumerator DamagePlayer()
    {
        var playerhp = PlayerScript.GetComponent<S_Pl_Movement>().Health -= 5;
        HealthINT.text = playerhp.ToString();
        SoundControllerScript.GetComponent<S_SoundController>().AttackHit();
        yield return new WaitForSeconds(2f);
        canAttack = true;
    }

    public void Damaged(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
            StopCoroutine(DamagePlayer());
        }
    }
}
