using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public bool damageable;
    public bool destroyed;
    public int priority;
    public Vector2 gridPos;
    public bool isSpaceOccupied;
    public bool hasCentralAttackPoint = false;
    public int occupiedSpaceCount;
    private Transform attackPointPrefab;
    public List<Node> attackPoints = new List<Node>();
    public Node centralAttackPoint;
    public List<Node> occupiedSpace = new List<Node>();
    public virtual void Start()
    {
        attackPointPrefab = Init.Instance.attackPointPrefab;
        FindOccupiedSpace();
        OccupySpace();
        CreateAttackPoints();
    }

    public virtual void CreateAttackPoints()
    {
        if (destroyed) return;
        var nodePos = gridPos * 3;
        var nodeGrid = Init.Instance.pathfinding.GetGrid();
        var attackPositions = new List<Vector2>
        {
            new Vector2(nodePos.x - 1, nodePos.y + 1),
            new Vector2(nodePos.x + 3, nodePos.y + 1),
            new Vector2(nodePos.x + 1, nodePos.y - 1),
            new Vector2(nodePos.x + 1, nodePos.y + 3)
        };

        nodePos = nodePos + new Vector2(1, 1);
        var centerNode = nodeGrid.gridArray[(int)nodePos.x, (int)nodePos.y];
        var centerAttackPoint = Instantiate(attackPointPrefab, nodePos * 3.3333f + new Vector2(10 / 6, 10 / 6) * 1.66f,
                Quaternion.identity, transform);
        centerNode.central = true;
        centerAttackPoint.GetComponent<AttackPoint>().parentNode = centerNode;
        centerNode.SetAttackPoint(centerAttackPoint.gameObject);
        attackPoints.Add(centerNode);
        centralAttackPoint = centerNode;

        foreach (var point in attackPositions)
        {
            var node = nodeGrid.gridArray[(int)point.x, (int)point.y];
            if (node != null && node.isWalkable)
            {
                var attackPoint = Instantiate(attackPointPrefab, point * 3.3333f + new Vector2(10 / 6, 10 / 6) * 1.66f,
                    Quaternion.identity, transform);
                // Do I understand why the vector has to be timesed by 1.66? No... Does it work? Unfortunatly... :(
                attackPoint.GetComponent<AttackPoint>().parentNode = node;
                node.SetAttackPoint(attackPoint.gameObject);
                attackPoints.Add(node);
            }
        }

        //if (!hasCentralAttackPoint)
        //{
            
            //hasCentralAttackPoint = true;
        //}
    }

    public virtual void RemoveAttackPoints()
    {
        foreach (var node in attackPoints)
        {
            node.RemoveAttackPoint();
        }
        attackPoints = new List<Node>();
    }

    public virtual void UpdateAllAttackPoints() //Updates the attack points of adjacent cells which contain structures
    {
        RemoveAttackPoints();
        CreateAttackPoints();
        foreach (var structure in FindAdjacentStructures())
        {
            structure.RemoveAttackPoints();
            structure.CreateAttackPoints();
        }
    }

    public List<Structure> FindAdjacentStructures()
    {
        var adjacent = new List<Structure>();
        if (Init.Instance.grid.GetStructureAtCell((int)gridPos.x, (int)gridPos.y + 1) != null)
        {
            adjacent.Add(
                Init.Instance.grid.GetStructureAtCell((int)gridPos.x, (int)gridPos.y + 1).GetComponent<Structure>());
        }
        if (Init.Instance.grid.GetStructureAtCell((int)gridPos.x + 1, (int)gridPos.y) != null)
        {
            adjacent.Add(
                Init.Instance.grid.GetStructureAtCell((int)gridPos.x + 1, (int)gridPos.y).GetComponent<Structure>());
        }
        if (Init.Instance.grid.GetStructureAtCell((int)gridPos.x, (int)gridPos.y - 1) != null)
        {
            adjacent.Add(
                Init.Instance.grid.GetStructureAtCell((int)gridPos.x, (int)gridPos.y - 1).GetComponent<Structure>());
        }
        if (Init.Instance.grid.GetStructureAtCell((int)gridPos.x - 1, (int)gridPos.y) != null)
        {
            adjacent.Add(
                Init.Instance.grid.GetStructureAtCell((int)gridPos.x - 1, (int)gridPos.y).GetComponent<Structure>());
        }
        return adjacent;
    }

    public virtual void OccupySpace()
    {
        Debug.Log(gameObject.name + " space has been occupied");
        if (occupiedSpace.Count > 0)
        {
            foreach (var node in occupiedSpace)
            {
                if (node.GetAttackPoint() != null && !node.central)
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
        var nodePos = gridPos * 3;
        var nodeGrid = Init.Instance.pathfinding.GetGrid();
        for (var y = (int)nodePos.y; y < (int)nodePos.y + 3; y++)
        {
            for (var x = (int)nodePos.x; x < (int)nodePos.x + 3; x++)
                occupiedSpace.Add(nodeGrid.gridArray[x, y]);
        }
    }

    public virtual void DeoccupySpace()
    {
        Debug.Log(gameObject.name + " Space was deoccupied");
        if (occupiedSpace.Count > 0)
        {
            foreach (var node in occupiedSpace)
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
            
        }
        else
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
        if (amount > maxHealth || fullyHeal)
        {
            health = maxHealth;
        }
        else if (amount < 0)
        {
            health = amount;
        }
        else
        {
            health = 0;
        }
        if (destroyed && health > 0)
        {
            destroyed = false;
        }
        else if (!destroyed && health <= 0)
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
        }
        else
        {
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
        }
        else if (!destroyed && !isSpaceOccupied)
        {
            OccupySpace();
        }
    }
}
