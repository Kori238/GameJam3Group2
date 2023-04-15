using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoRangedPathfinding : EnemyPathfinding
{
    private Structure rangedTarget = null;
    [SerializeField] private Transform projectile;
    [SerializeField] private float flightTime = 100f;

    public override void Attack()
    {
        if (!attacking || target == null) return;
        GetRangedTarget();
        Debug.Log(name + " Dealt " + attackDamage + " damage to " + rangedTarget.name);
        rangedTarget.Damaged(attackDamage);
        StartCoroutine(FireProjectile(transform.position, rangedTarget.transform.position, flightTime));
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
        StartCoroutine(proj.GetComponent<Projectile>().Destroyed());
    }


    public virtual void GetRangedTarget()
    {
        var results = Physics2D.OverlapCircleAll(transform.position, viewRange, LayerMask.GetMask("AttackPoints"));
        List<Structure> structures = new();
        foreach (var result in results) {
            if (!result) continue;
            var node = result.GetComponent<AttackPoint>().parentNode;
            if (!node.central) continue;
            var structure = result.GetComponentInParent<Structure>();
            structures.Add(structure);
        }
        float bestValue = 0f;
        Structure bestStructure = null;
        foreach (var structure in structures)
        {
            float value = 10f + structure.priority * (structure.maxHealth / structure.health);
            if (value > bestValue)
            {
                bestValue = value;
                bestStructure = structure;
            }
        }
        rangedTarget = bestStructure;
    }
}
