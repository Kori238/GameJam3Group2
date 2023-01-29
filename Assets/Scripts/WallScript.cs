using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Structure values;
    public float health = 0;
    public float maxHealth = 0;

    void Awake()
    {
        health = values.health;
        maxHealth = values.health;
        
    }
}
