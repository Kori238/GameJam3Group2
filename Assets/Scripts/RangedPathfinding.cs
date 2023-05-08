using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedPathfinding : EnemyPathfinding
{
    public float attackRange = 25f;
    public Transform projectile;
    [SerializeField] private float flightTime; 

    public override void Attack()
    {
        if (!attacking || target == null) return;
        Debug.Log(name + " Dealt " + attackDamage + " damage to " + target.name);
        target.GetComponent<Structure>().Damaged(attackDamage);
        StartCoroutine(FireProjectile(transform.position, target.transform.position, flightTime));
    }

    public virtual IEnumerator FireProjectile(Vector2 start, Vector2 end, float flightTime)
    {
        Vector3 directionVector = end - start;
        Transform proj = Instantiate(projectile, transform.position, Quaternion.LookRotation(directionVector) * Quaternion.FromToRotation(Vector3.right, Vector3.forward), transform);
        while (Vector2.Distance(new Vector2(proj.position.x, proj.position.y), end) > 0.5f)
        {
            proj.position += directionVector.normalized * (flightTime * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
            if (Vector2.Distance(proj.position, transform.position) > viewRange) break;
            if (Vector2.Distance(start, end) < Vector2.Distance(start, proj.position)) break;
        }
        StartCoroutine(proj.GetComponent<Projectile>().Destroyed(target.gameObject));
    }


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
