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

    public virtual void Start()
    {
        view.radius = viewRange;
        StartCoroutine(PathfindingUpdate(1f));
    }

    public virtual void FindTarget()
    {
        return;
    }

    public virtual IEnumerator PathfindingUpdate(float frequency)
    {
        Pathfind();
        yield return new WaitForSeconds(frequency);
        yield return StartCoroutine(PathfindingUpdate(frequency));
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
            if (Init.Instance.debug) DrawPath(lowestTCostPath, Color.green);
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

