using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Structure
{
    [SerializeField] private int attackDamage = 5;
    [SerializeField] int attackRate = 1;
    [SerializeField] int viewRange = 40;
    private const int TARGETCLOSEST = 0;
    private const int TARGETFURTHEST = 1;
    private const int TARGETHEALTHIEST = 2;
    private const int TARGETWEAKEST = 3;
    [SerializeField] private int targetPrioritization = TARGETCLOSEST;
    [SerializeField] private EnemyPathfinding target = null;
    private GameObject home;
    

    public override void Start()
    {
        home = Init.Instance.grid.GetStructureAtCell((int)(Init.Instance.gridDimensions.x - 1) / 2,
            (int)(Init.Instance.gridDimensions.y - 1) / 2);
        InvokeRepeating(nameof(AttackTarget), 0f, attackRate);
        base.Start();
    }

    public virtual void AttackTarget()
    {
        Debug.Log(target);
        if (target == null) FindTarget();
        Debug.Log(target);
        if (target != null)
        {
            target.Damaged(attackDamage);
        }
    }


    public virtual void FindTarget()
    {
        var results = Physics2D.OverlapCircleAll(transform.position, viewRange, LayerMask.GetMask("Enemy"));

        if (results.Length <= 0) return;
        if (targetPrioritization == TARGETCLOSEST) target = FilterClosest(results).GetComponent<EnemyPathfinding>();
        else if (targetPrioritization == TARGETFURTHEST) target = FilterFurthest(results).GetComponent<EnemyPathfinding>();
        else if (targetPrioritization == TARGETHEALTHIEST) target = FilterHealthiest(results).GetComponent<EnemyPathfinding>();
        else if (targetPrioritization == TARGETWEAKEST) target = FilterWeakest(results).GetComponent<EnemyPathfinding>();
        else target = null;
    }

    public Collider2D FilterClosest(Collider2D[] results)
    {
        Collider2D closestTarget = null;
        float closestDistance = Int32.MaxValue;
        foreach (Collider2D result in results)
        {
            float distance = Vector2.Distance(result.transform.position, home.transform.position);
            if (distance < closestDistance)
            {
                closestTarget = result;
                closestDistance = distance;
            }
        }
        return closestTarget;
    }

    public Collider2D FilterFurthest(Collider2D[] results)
    {
        Collider2D furthestTarget = null;
        float furthestDistance = Int32.MinValue;
        foreach (Collider2D result in results)
        {
            float distance = Vector2.Distance(result.transform.position, home.transform.position);
            if (distance > furthestDistance)
            {
                furthestTarget = result;
                furthestDistance = distance;
            }
        }
        return furthestTarget;
    }
    public Collider2D FilterHealthiest(Collider2D[] results)
    {
        Collider2D healthiestTarget = null;
        float healthiestHealth = Int32.MinValue;
        foreach (Collider2D result in results)
        {
            int health = result.GetComponent<EnemyPathfinding>().health;
            if (health > healthiestHealth)
            {
                healthiestTarget = result;
                healthiestHealth = health;
            }
        }
        return healthiestTarget;
    }
    public Collider2D FilterWeakest(Collider2D[] results)
    {
        Collider2D weakestTarget = null;
        float weakestHealth = Int32.MaxValue;
        foreach (Collider2D result in results)
        {
            int health = result.GetComponent<EnemyPathfinding>().health;
            if (health < weakestHealth)
            {  
                weakestTarget = result;
                weakestHealth = health;
            }
        }
        return weakestTarget;
    }

}
