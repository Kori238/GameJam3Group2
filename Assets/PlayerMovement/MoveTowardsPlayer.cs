using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveTowardsPlayer : MonoBehaviour
{
    public Transform target; // The target sprite
    public float speed = 5f; // The speed of movement

    public TMP_Text HealthINT;

    public GameObject PlayerScript;

    public int health = 20;

    private bool canAttack = true;

    void Update()
    {
        // Calculate the direction to move towards the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Move the sprite towards the target
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) <= 4f)  //Determinds the distance between the player and the enemy.
        {
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(DamagePlayer());
            }
        }

    }

    private IEnumerator DamagePlayer()
    {
        int hp = PlayerScript.GetComponent<S_Pl_Movement>().Health -= 5;
        HealthINT.text = hp.ToString();

        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) //If the Collider is the Players attack range then...
        {
            Damaged(5);
        }
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




