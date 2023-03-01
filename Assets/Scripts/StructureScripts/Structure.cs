using Newtonsoft.Json.Bson;
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
    public int priority;
    public Vector2 gridPos;
    public List<Node> occupiedSpace = new();
    public List<Node> attackPoints = new();
    public bool isSpaceOccupied = false;
    public int occupiedSpaceCount = 0;

    public virtual void Start()
    {
        FindOccupiedSpace();
        OccupySpace();

    }

    public virtual void OccupySpace()
    {
        Debug.Log(gameObject.name + " space has been occupied");
        if (occupiedSpace.Count > 0)
        {
            foreach (Node node in occupiedSpace)
            {
                node.isWalkable = false;
            }
            isSpaceOccupied = true;
        }
        occupiedSpaceCount = occupiedSpace.Count;
    }

    public virtual void FindOccupiedSpace()
    {
        Vector2 nodePos = gridPos * 3;
        NodeGrid nodeGrid = Init.Instance.pathfinding.GetGrid();
        for (int y = (int)nodePos.y; y < (int)nodePos.y+3; y++)
        {
            for (int x = (int)nodePos.x; x < (int)nodePos.x+3; x++)
                occupiedSpace.Add(nodeGrid.gridArray[x, y]);
        }
    }

    public virtual void DeoccupySpace()
    {
        Debug.Log(gameObject.name + " Space was deoccupied");
        if (occupiedSpace.Count > 0)
        {
            foreach (Node node in occupiedSpace)
            {
                node.isWalkable = true;
            }
            isSpaceOccupied = false;
        }
        occupiedSpaceCount = occupiedSpace.Count;
    }


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
        UpdateStructure();
        Debug.Log(gameObject.name + " has been Destroyed!");
        
    }

    public virtual void Demolished()
    {
        destroyed = true;
        UpdateStructure();
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
            Destroyed();
        }
        UpdateStructure();
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
        UpdateStructure();
    }

    public virtual void UpdateStructure()
    {
        Debug.Log(gameObject.name + "was Updated");
        UpdateOccupiedSpace();
    }


    public virtual void UpdateOccupiedSpace()
    {
        Debug.Log(gameObject.name + "space was updated" + destroyed);
        if (destroyed)
        {
            DeoccupySpace();
        } else
        {
            OccupySpace();
        }
    }
}
