using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    public GameObject SoundControllerScript;

    public GameObject PlayerScript;

    public GameObject swordSwing;

    public int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == swordSwing.layer) //If the Collider is the Players attack range then...
        {
            Damaged(5);
            SoundControllerScript.GetComponent<S_SoundController>().HurtMonster();
            Debug.Log("HURT");
        }
    }


    public void Damaged(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
