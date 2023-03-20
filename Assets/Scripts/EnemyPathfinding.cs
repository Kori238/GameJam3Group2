using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform test;
    [SerializeField] private int viewRange;
    [SerializeField] private float speed;
    [SerializeField] private bool moving;
    [SerializeField] private bool attacking;
    [SerializeField] private int currentPathIndex = 1;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private GameObject target;
    [SerializeField] private Collider2D destination;
    [SerializeField] private Collider2D newDestination;
    private bool _initiated = false;
    private Path _currentPath;
    private Path _newPath;

    public virtual void Start()
    {
        
        StartCoroutine(PathfindingLoop());
        InvokeRepeating(nameof(Attack), 0f, attackDelay);
    }

    public virtual void FixedUpdate()
    {
        TraversePath();
        CheckAttack();
        if (!moving && !attacking)
        {
            MoveTowardsBase();
        }
    }

    public virtual void MoveTowardsBase()
    {
        var home = Init.Instance.grid.GetStructureAtCell((int)(Init.Instance.gridDimensions.x - 1) / 2,
            (int)(Init.Instance.gridDimensions.y - 1) / 2);
        var targetPosition = new Vector3(home.transform.position.x, home.transform.position.y, transform.position.z);
        var moveDir = (targetPosition - transform.position).normalized;
        transform.position += speed * Time.deltaTime * moveDir;
    }

    public virtual void Attack()
    {
        Debug.Log("Attack");
        if (!attacking || target == null) return;
        Debug.Log(name + " Dealt " + attackDamage + " damage to " + target.name);
        target.GetComponent<Structure>().Damaged(attackDamage);
    }
    public virtual void CheckAttack()
    {
        if (!moving && destination != null && Vector3.Distance(destination.transform.position, transform.position) < 2f)
        {
            attacking = true;
        }
        else
        {
            attacking = false;
        }
    }

    public virtual void TraversePath()
    {
        if (_newPath != null && _newPath != _currentPath)
        {
            newDestination = _newPath.attackPoint;
            _currentPath = _newPath;
            currentPathIndex = 0;
        }

        if (destination != newDestination)
        {
            destination = newDestination;
            moving = true;
        }
        if (target == null)
        {
            StartCoroutine(OneTimePathfind());
            if (target == null)
            {
                MoveTowardsBase();
                return;
            }
        }
        if (!moving || _currentPath == null || _currentPath.nodes.Count <= 0) return;
        var targetNode = _currentPath.nodes[currentPathIndex];
        var targetPosition =
            new Vector3((targetNode.x + 0.5f) * 10 / 3, (targetNode.y + 0.5f) * 10 / 3, transform.position.z);
        if (Vector3.Distance(transform.position, targetPosition) > 1f)
        {
            var moveDir = (targetPosition - transform.position).normalized;
            transform.position = transform.position + speed * Time.deltaTime * moveDir;
        }
        else
        {
            currentPathIndex++;
            if (currentPathIndex >= _currentPath.nodes.Count)
            {
                moving = false;
            }
        }
    }

    public IEnumerator OneTimePathfind()
    {
        if (Init.Instance.highUsage)
        {
            yield return new WaitForFixedUpdate();
            yield return OneTimePathfind();
        }
        Pathfind();
    }

    public IEnumerator PathfindingLoop()
    {
        if (!_initiated)
        {
            
        }
        if (Init.Instance.highUsage)
        {
            yield return new WaitForFixedUpdate();
            yield return PathfindingLoop();
        }
        Pathfind();
        yield return new WaitForSeconds(1f);
        yield return PathfindingLoop();



    }

    public void Pathfind()
    {
        var results = Physics2D.OverlapCircleAll(transform.position, viewRange, LayerMask.GetMask("AttackPoints"));

        var paths = new List<Path>();
        foreach (var result in results)
        {
            if (result == null) continue;
            var node = result.GetComponent<AttackPoint>().parentNode;
            var enemyPos = Init.Instance.pathfinding.GetGrid()
                .GetWorldCellPosition(transform.position.x, transform.position.y);
            var path = Init.Instance.pathfinding.FindPath((int)enemyPos.x, (int)enemyPos.y, node.x, node.y);
            var structure = result.GetComponentInParent<Structure>();
            if (path == null) continue;
            path.tCost = (int)((path.fCost + 10) /
                               (structure.priority * 10 * ((structure.health / structure.maxHealth + 1) / 2)));
            path.structure = result.transform.parent.gameObject;
            path.attackPoint = result;
            paths.Add(path);
        }
        var lowestTCostPath = new Path { tCost = int.MaxValue };
        foreach (var path in paths)
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
            _newPath = lowestTCostPath;
        }
        else
        {
            _newPath = null;
        }
    }

    private void DrawPath(Path path, Color color)
    {
        var nodeSpacing = Init.Instance.cellSize / Init.Instance.nodeCount;
        for (var i = 0; i < path.nodes.Count - 1; i++)
        {
            Debug.DrawLine(new Vector3(path.nodes[i].x, path.nodes[i].y) * nodeSpacing + Vector3.one * nodeSpacing / 2,
                new Vector3(path.nodes[i + 1].x, path.nodes[i + 1].y) * nodeSpacing + Vector3.one * nodeSpacing / 2, color,
                1f);
        }
    }
}
