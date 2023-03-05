using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform test;
    [SerializeField] int viewRange;
    [SerializeField] float speed;
    [SerializeField] bool moving;
    [SerializeField] bool attacking;
    Path currentPath;
    Path newPath;
    [SerializeField] int currentPathIndex = 1;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float attackDamage = 5f;
    [SerializeField] GameObject target;
    [SerializeField] Collider2D destination;
    [SerializeField] Collider2D newDestination;

    public virtual void Start()
    {
        InvokeRepeating(nameof(Pathfind), 0f, 1f);
        InvokeRepeating(nameof(Attack), 0f, attackDelay);
    }

    public virtual void FixedUpdate()
    {
        TraversePath();
        CheckAttack();
        if (!moving && !attacking)
        {
            MoveTowardsCenter();
        }
    }

    public virtual void MoveTowardsCenter()
    {
        return;
    }

    public virtual void Attack()
    {
        Debug.Log("Attack");
        if (!attacking || target == null) return;
        Debug.Log(this.name + " Dealt " + attackDamage + " damage to " + target.name);
        target.GetComponent<Structure>().Damaged(attackDamage);
    }
    public virtual void CheckAttack()
    {
        if (!moving && destination != null && Vector3.Distance(destination.transform.position, transform.position) < 2f)
        {
            attacking = true;
        } else
        {
            attacking = false;
        }
    }

    public virtual void TraversePath()
    { 
        

        if (newPath != null && newPath != currentPath)
        {
            newDestination = newPath.attackPoint;
            currentPath = newPath;
            currentPathIndex = 0;
        }

        if (destination != newDestination)
        {
            destination = newDestination;
            moving = true;
        }
        if (!moving || currentPath == null) return;
        Node targetNode = currentPath.nodes[currentPathIndex];
        Vector3 targetPosition = new Vector3((targetNode.x + 0.5f) * 10/3, (targetNode.y + 0.5f) * 10/3, transform.position.z);
        if (Vector3.Distance(transform.position, targetPosition) > 1f)
        {
            Vector3 moveDir = (targetPosition - transform.position).normalized;
            transform.position = transform.position + speed * Time.deltaTime * moveDir;
        } else
        {
            currentPathIndex++;
            if (currentPathIndex >= currentPath.nodes.Count)
            {
                moving = false;
            }
        }



            
        
    }

    public virtual void Pathfind()
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, viewRange, LayerMask.GetMask("AttackPoints"));

        List<Path> paths = new();
        foreach (Collider2D result in results)
        {
            if (result == null) continue;
            Node node = result.GetComponent<AttackPoint>().parentNode;
            Vector2 enemyPos = Init.Instance.pathfinding.GetGrid().GetWorldCellPosition(transform.position.x, transform.position.y);
            Path path = Init.Instance.pathfinding.FindPath((int)enemyPos.x, (int)enemyPos.y, node.x, node.y);
            Structure structure = result.GetComponentInParent<Structure>();
            path.tCost = (int)(path.fCost / ((float)structure.priority * ((structure.health / structure.maxHealth + 1) / 2)));
            path.structure = result.transform.parent.gameObject;
            path.attackPoint = result;
            paths.Add(path);
        }
        Path lowestTCostPath = new() { tCost = int.MaxValue };
        foreach (Path path in paths)
        {
            if (path.tCost < lowestTCostPath.tCost && path.tCost != int.MinValue)
            {
                lowestTCostPath = path;
            }
            if (Init.Instance.debug) DrawPath(path, Color.red);
        }
        if (lowestTCostPath != null)
        {
            target = lowestTCostPath.structure;
            if (Init.Instance.debug) DrawPath(lowestTCostPath, Color.green);
            newPath = lowestTCostPath;
        }
    }
    
    private void DrawPath(Path path, Color color)
    {
        float nodeSpacing = Init.Instance.cellSize / Init.Instance.nodeCount;
        for (int i = 0; i < path.nodes.Count - 1; i++)
        {   
            Debug.DrawLine(new Vector3(path.nodes[i].x, path.nodes[i].y) * nodeSpacing + Vector3.one * nodeSpacing / 2, new Vector3(path.nodes[i + 1].x, path.nodes[i + 1].y) * nodeSpacing + Vector3.one * nodeSpacing / 2, color, 1f);
        }
    }

}

