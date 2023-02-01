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

    void Update()
    {
        // Calculate the direction to move towards the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Move the sprite towards the target
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)  //Determinds the distance between the player and the enemy.
        {
            // Delete the enemy
            Destroy(gameObject);

            int hp = PlayerScript.GetComponent<S_Pl_Movement>().Health -= 20;
            
            HealthINT.text = hp.ToString();

        }

        }
}




