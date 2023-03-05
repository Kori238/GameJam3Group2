using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] CircleCollider2D view;
    public Transform test;
    [SerializeField] int viewRange;
    [SerializeField] float speed;
    [SerializeField] bool moving;
    Path currentPath;
    Path newPath;
    [SerializeField] int currentPathIndex = 1;
    [SerializeField] GameObject target;

    public virtual void Start()
    {
        view.radius = viewRange;
        Pathfind();
        StartCoroutine(PathfindingUpdate(1f));
    }

    public virtual void FixedUpdate()
    {
        TraversePath();
    }

    public virtual void FindTarget()
    {
        return;
    }
    


   

    public virtual IEnumerator PathfindingUpdate(float frequency)
    {
        if (moving)
        {
            newPath = Pathfind();
            //TraversePath();
            //if (currentPath.fCost < 10) moving = false;
            //else if (currentPath == null) MoveTowardsCenter();
        }
        
        yield return new WaitForSeconds(frequency);
        yield return StartCoroutine(PathfindingUpdate(frequency));
    }

    public virtual void MoveTowardsCenter()
    {
        return;
    }

    public virtual void TraversePath()
    {

        if (newPath != null && newPath != currentPath)
        {
            currentPath = newPath;
            currentPathIndex = 0;
        }

        if (currentPath == null) return;
        Debug.Log(currentPath.nodes.Count);
        Node targetNode = currentPath.nodes[currentPathIndex];
        Vector3 targetPosition = new Vector3(targetNode.x, targetNode.y, transform.position.z);
        Debug.Log(Vector3.Distance(transform.position, targetPosition));
        if (Vector3.Distance(transform.position, targetPosition) > 1f)
        {
            Vector3 moveDir = (targetPosition - transform.position).normalized;
            transform.position = transform.position + speed * Time.deltaTime * moveDir;
        } else
        {
            currentPathIndex++;
            if (currentPathIndex >= currentPath.nodes.Count)
            {
                currentPath = null;
            }
        }



            
        
    }

    public virtual Path Pathfind()
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
            path.structure = result.gameObject;
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
            return lowestTCostPath;
        } else return null;
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

