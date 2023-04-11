using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedPathfinding : EnemyPathfinding
{
    public float attackRange = 25f;

    public override void Pathfind()
    {
        var results = Physics2D.OverlapCircleAll(transform.position, viewRange, LayerMask.GetMask("AttackPoints"));

        var paths = new List<Path>();
        foreach (var result in results)
        {
            if (!result) continue;
            var node = result.GetComponent<AttackPoint>().parentNode;
            if (!node.central) continue;
            var enemyPos = Init.Instance.pathfinding.GetGrid()
                .GetWorldCellPosition(transform.position.x, transform.position.y);
            int attackSpace = Mathf.FloorToInt((attackRange / 10) * 3);
            int diagonalAttackSpace = Mathf.FloorToInt((attackRange / 14) * 3);
            List<Vector2> rangedAttackPoints = new List<Vector2>
            {
                new Vector2(node.x - attackSpace, node.y), //Cardinals
                new Vector2(node.x + attackSpace, node.y),
                new Vector2(node.x, node.y - attackSpace),
                new Vector2(node.x, node.y + attackSpace),
                new Vector2(node.x - diagonalAttackSpace, node.y - diagonalAttackSpace), //Diagonals
                new Vector2(node.x - diagonalAttackSpace, node.y + diagonalAttackSpace),
                new Vector2(node.x + diagonalAttackSpace, node.y - diagonalAttackSpace),
                new Vector2(node.x + diagonalAttackSpace, node.y + diagonalAttackSpace)
            };
            foreach (var rangedAttackPoint in rangedAttackPoints)
            {
                var path = Init.Instance.pathfinding.FindPath((int)enemyPos.x, (int)enemyPos.y, (int)rangedAttackPoint.x, (int)rangedAttackPoint.y, viewRange, flying);
                var structure = result.GetComponentInParent<Structure>();
                if (path == null) continue;
                path.tCost = (int)((path.fCost + 10) /
                                   (structure.priority * 10 * ((structure.health / structure.maxHealth + 1) / 2)));
                path.structure = result.transform.parent.gameObject;
                path.attackPoint = result;
                paths.Add(path);
            }
            
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
}
