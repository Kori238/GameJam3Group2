using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking.Types;
using static UnityEngine.UI.CanvasScaler;

public class Structure : MonoBehaviour
{
    private Transform attackPointPrefab;
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
        attackPointPrefab = Init.Instance.attackPointPrefab;
        FindOccupiedSpace();
        OccupySpace();
        CreateAttackPoints();
    }

    public virtual void CreateAttackPoints()
    {
        Vector2 nodePos = gridPos * 3;
        NodeGrid nodeGrid = Init.Instance.pathfinding.GetGrid();
        List<Vector2> attackPositions = new List<Vector2> {
            new Vector2(nodePos.x-1, nodePos.y+1),
            new Vector2(nodePos.x+3, nodePos.y+1),
            new Vector2(nodePos.x+1, nodePos.y-1),
            new Vector2(nodePos.x+1, nodePos.y+3)};

        foreach (Vector2 point in attackPositions)
        {
            Node node = nodeGrid.gridArray[(int)point.x, (int)point.y];
            if (node != null && node.isWalkable)
            {
                Transform attackPoint = GameObject.Instantiate(attackPointPrefab, ((point) * (3.3333f) + new Vector2(10/6, 10/6) * 1.66f), Quaternion.identity, transform);
                // Do I understand why the vector has to be timesed by 1.66? No... Does it work? Unfortunatly... :(
                attackPoint.GetComponent<AttackPoint>().parentNode = node;
                node.SetAttackPoint(attackPoint.gameObject);
                attackPoints.Add(node);
            }
            
        }
    }

    public virtual void RemoveAttackPoints()
    {
        foreach (Node node in attackPoints)
        {
            node.RemoveAttackPoint();
        }
        attackPoints = new();
    }

    public virtual void UpdateAllAttackPoints() //Updates the attack points of adjacent cells which contain structures
    {
        RemoveAttackPoints();
        CreateAttackPoints();
        foreach (Structure structure in FindAdjacentStructures())
        {
            structure.RemoveAttackPoints();
            structure.CreateAttackPoints();
        }
    }

    public List<Structure> FindAdjacentStructures()
    {
        List<Structure> adjacent = new();
        if (Init.Instance.grid.GetStructureAtCell((int)gridPos.x, (int)gridPos.y + 1) != null)
            adjacent.Add(Init.Instance.grid.GetStructureAtCell((int)gridPos.x, (int)gridPos.y + 1).GetComponent<Structure>());
        if (Init.Instance.grid.GetStructureAtCell((int)gridPos.x + 1, (int)gridPos.y) != null)
            adjacent.Add(Init.Instance.grid.GetStructureAtCell((int)gridPos.x + 1, (int)gridPos.y).GetComponent<Structure>());
        if (Init.Instance.grid.GetStructureAtCell((int)gridPos.x, (int)gridPos.y - 1) != null)
            adjacent.Add(Init.Instance.grid.GetStructureAtCell((int)gridPos.x, (int)gridPos.y - 1).GetComponent<Structure>());
        if (Init.Instance.grid.GetStructureAtCell((int)gridPos.x - 1, (int)gridPos.y) != null)
            adjacent.Add(Init.Instance.grid.GetStructureAtCell((int)gridPos.x - 1, (int)gridPos.y).GetComponent<Structure>());
        return adjacent;
    }

    public virtual void OccupySpace()
    {
        Debug.Log(gameObject.name + " space has been occupied");
        if (occupiedSpace.Count > 0)
        {
            foreach (Node node in occupiedSpace)
            {
                if (node.GetAttackPoint() != null)
                {
                    attackPoints.Remove(node);
                    node.RemoveAttackPoint();
                }
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
        UpdateAllAttackPoints();
    }


    public virtual void UpdateOccupiedSpace()
    {
        Debug.Log(gameObject.name + "space was updated" + destroyed);
        if (destroyed && isSpaceOccupied)
        {
            DeoccupySpace();
        } else if (!destroyed && !isSpaceOccupied)
        {
            OccupySpace();
        }
    }
}
