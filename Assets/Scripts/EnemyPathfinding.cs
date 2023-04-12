using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform test;
    [SerializeField] public int viewRange;
    [SerializeField] private float speed;
    [SerializeField] private bool moving;
    [SerializeField] public bool attacking;
    [SerializeField] private int currentPathIndex = 1;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] public float attackDamage = 5f;
    [SerializeField] public GameObject target;
    [SerializeField] private Vector2 destination;
    [SerializeField] private Vector2 newDestination;
    [SerializeField] private bool _initiated = false;
    [SerializeField] public bool flying = false;
    private Path _currentPath = null;
    public Path _newPath = null;
    private GameObject home;

    public virtual void Start()
    {
        home = Init.Instance.grid.GetStructureAtCell((int)(Init.Instance.gridDimensions.x - 1) / 2,
            (int)(Init.Instance.gridDimensions.y - 1) / 2);
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
        var position = transform.position;
        var targetPosition = new Vector3(home.transform.position.x, home.transform.position.y, position.z);
        var moveDir = (targetPosition - position).normalized;
        position += speed * Time.deltaTime * moveDir;
        transform.position = position;
    }

    public virtual void Attack()
    {
        if (!attacking || target == null) return;
        Debug.Log(name + " Dealt " + attackDamage + " damage to " + target.name);
        target.GetComponent<Structure>().Damaged(attackDamage);
    }
    public virtual void CheckAttack()
    {
        if (!moving && destination != null && Vector3.Distance(destination, transform.position) < 2f)
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
        //if (attacking && target != null) return;
        if (_newPath != null && _newPath != _currentPath && _newPath.attackPoint != null)
        {
            newDestination = new Vector2(_newPath.nodes.Last().x, _newPath.nodes.Last().y) * (Init.Instance.cellSize / Init.Instance.nodeCount) + (Vector2.one * (Init.Instance.cellSize / Init.Instance.nodeCount) / 2);
            //newDestination = new Vector2(_newPath.attackPoint.transform.position.x, _newPath.attackPoint.transform.position.y);
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
            Pathfind();
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
            var position = transform.position;
            var moveDir = (targetPosition - position).normalized;
            position += speed * Time.deltaTime * moveDir;
            transform.position = position;
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

    public IEnumerator PathfindingLoop()
    {
        Pathfind();
        yield return new WaitForSeconds(1f);
        yield return PathfindingLoop();
    }

    public virtual void Pathfind()
    {
        var results = Physics2D.OverlapCircleAll(transform.position, viewRange, LayerMask.GetMask("AttackPoints"));

        var paths = new List<Path>();
        foreach (var result in results)
        {
            if (!result) continue;
            var node = result.GetComponent<AttackPoint>().parentNode;
            var enemyPos = Init.Instance.pathfinding.GetGrid()
                .GetWorldCellPosition(transform.position.x, transform.position.y);
            var path = Init.Instance.pathfinding.FindPath((int)enemyPos.x, (int)enemyPos.y, node.x, node.y, viewRange, flying);
            if (path == null) continue;
            var structure = result.GetComponentInParent<Structure>();
            path.tCost = (int)((path.fCost + 10) * 10 /
                               (structure.priority * ((structure.health / structure.maxHealth + 1) / 2)));
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

    public void DrawPath(Path path, Color color)
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