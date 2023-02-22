using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Structure : MonoBehaviour
{

    public float health;
    public float maxHealth;
    public bool damageable;
    public bool destroyed;
    public Vector2 gridPos;

    public virtual void Damaged(float amount)
    {
        if (health - amount < 0)
        {
            health = 0;
        } else
        {
            health -= amount;
        }
        
        if (health <= 0)
        {
            Destroyed();
        }
    }

    public virtual void Destroyed()
    {
        destroyed = true;
        Debug.Log(gameObject.name + " has been Destroyed!");
        UpdatePathfinding();
    }

    public virtual void Demolished()
    {
        Destroy(gameObject);
    }

    public virtual void SetHealth(float amount, bool fullyHeal = false)
    {
        if (amount > maxHealth || fullyHeal) {
            health = maxHealth;
        } else if (amount < 0){
            health = amount;
        } else {
            health = 0;
        }
        if (destroyed && health > 0)
        {
            destroyed = false;
        } else if (!destroyed && health <= 0)
        {
            destroyed = true;
        }
        UpdatePathfinding();
    }

    public virtual void Healed(float amount)
    {
        if (health + amount > maxHealth)
        {
            health = maxHealth;
        } else {
            health += amount;
        }
        if (destroyed && health > 0)
        {
            destroyed = false;
        }
        UpdatePathfinding();
    }

    public virtual void UpdatePathfinding()
    {
        Init.Instance.grid.gridArray[(int)gridPos.x, (int)gridPos.y].Values["pathfindable"] = !destroyed;
    }
}
