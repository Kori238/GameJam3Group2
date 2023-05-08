using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0EProjectile : Projectile
{
    [SerializeField] private float AOERadius;

    
    public virtual void DamageAOE()
    {
        var results = Physics2D.OverlapCircleAll(transform.position, AOERadius, LayerMask.GetMask("Enemy"));

        foreach (var target in results)
        {
            if (towerScript == null)
            {
                Debug.Log("AOE Projectile could not access parent tower script");
                return;
            }
            target.GetComponent<EnemyPathfinding>().Damaged(towerScript.attackDamage);
        }
    }

    public override IEnumerator Destroyed(GameObject target, bool damageTarget = false)
    {
        DamageAOE();
        return base.Destroyed(target, damageTarget);
    }
}
