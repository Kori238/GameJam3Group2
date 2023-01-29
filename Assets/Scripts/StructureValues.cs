using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureValues : MonoBehaviour
{
    public Structure values;
    public float health;
    public float maxHealth;
    public bool damageable = false;
    void Awake()
    {
        health = values.health;
        maxHealth = values.health;
        if (values is Wall)
        {
            damageable = ((Wall)values).damageable;
        } else
        {
            damageable = false;
        }
        
    }

}
