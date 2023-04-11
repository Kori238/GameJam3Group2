using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoRangedPathfinding : EnemyPathfinding
{
    private Structure rangedTarget = null;

    public override void Attack()
    {
        if (!attacking || target == null) return;
        GetRangedTarget();
        Debug.Log(name + " Dealt " + attackDamage + " damage to " + rangedTarget.name);
        rangedTarget.Damaged(attackDamage);
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
        //List<float> priorities = new(); 
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
