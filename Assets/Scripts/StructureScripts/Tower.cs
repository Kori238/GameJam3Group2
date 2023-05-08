using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Tower : Interactable
{
    public int attackDamage = 5;
    [SerializeField] float attackRate = 1f;
    [SerializeField] int viewRange = 40;
    private const int TARGETCLOSEST = 0;
    private const int TARGETFURTHEST = 1;
    private const int TARGETHEALTHIEST = 2;
    private const int TARGETWEAKEST = 3;
    [SerializeField] private int targetPrioritization = TARGETCLOSEST;
    [SerializeField] private EnemyPathfinding target = null;
    [SerializeField] private Transform projectile;
    [SerializeField] private int flightSpeed;
    private GameObject home;
    [SerializeField] private Transform occupant = null;
    [SerializeField] private Transform firingPoint = null;
    private bool facingRight = true;
    [SerializeField] private Animator animator = null;
    [SerializeField] private AnimationClip shootClip = null;
    [SerializeField] private AnimationClip idleClip = null;
    [SerializeField] private bool previousDestroyedState = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite destroyedSprite;

    private GameObject InstanceMenu;
    [SerializeField] private GameObject PopMenu;
    public bool toOpen = true;
    private int woodUpgradeCost=1;
    private int stoneUpgradeCost=1;
    private string upgradeDescription;
    private int woodRepairCost=25;
    private int stoneRepairCost=50;

    private int BuildingLevel =1;

    public override void Start()
    {
        upgradeDescription = "Upgrade to level 2";
        home = Init.Instance.grid.GetStructureAtCell((int)(Init.Instance.gridDimensions.x - 1) / 2,
            (int)(Init.Instance.gridDimensions.y - 1) / 2);
        InvokeRepeating(nameof(AttackTarget), 0f, attackRate);
        base.Start();
    }


    public override void UpdateStructure()
    {
        if (destroyed == previousDestroyedState)
        {
            base.UpdateStructure();
            return;
        }
        previousDestroyedState = destroyed;
        Debug.Log(destroyed);
        if (!destroyed)
        {
            spriteRenderer.sprite = defaultSprite;
            if (occupant != null) occupant.gameObject.SetActive(true);
        }
        else {
            spriteRenderer.sprite = destroyedSprite;
            if (occupant != null) occupant.gameObject.SetActive(false);
        }
        base.UpdateStructure(); 
    }

    public override void Interact()
    {
        if (toOpen)
        {
            InstanceMenu = Instantiate(PopMenu);
            toOpen = false;
            InstanceMenu.GetComponent<towerMenuScript>().SetParentStructure(this);

        }
    



    }
    public void setTargetPriority(int newpriority)
    {
        switch(newpriority)
        {
            case 0: targetPrioritization=0; break;
                case 1:targetPrioritization=1; break;
                case 2:targetPrioritization= 2; break;
                case 3:targetPrioritization=3; break;
        }
    }
    public int getWoodUpgradecost() { return woodUpgradeCost; }
    public int getStoneUpgradecost() { return stoneUpgradeCost; }
    public int getWoodRepairCost() { return woodRepairCost; }
    public int getStoneRepairCost() { return stoneRepairCost; }
    public string getUpgradeDescription() { return upgradeDescription; }
    public int getBuildingLevel() { return BuildingLevel; }
    public bool upgradeTower()
    {
        if (woodUpgradeCost <= Init.Instance.resourceManager.GetWood() && stoneUpgradeCost <= Init.Instance.resourceManager.GetStone())
        {
            Init.Instance.resourceManager.AddWood(-woodUpgradeCost);
            Init.Instance.resourceManager.AddStone(-stoneUpgradeCost);
            switch (BuildingLevel)
            {
                case 1: // upgades to level 2
                    {
                        
                        BuildingLevel = 2;
                        upgradeDescription = "Upgrade to level 3 ";
                        woodUpgradeCost = 1;
                        stoneUpgradeCost = 1;
                        attackDamage = attackDamage * 2;
                        attackRate= attackRate * 1.2f;
                       

                        break;
                    }
                case 2: // upgades to level 3
                    {
                       
                        BuildingLevel = 3;
                        upgradeDescription = "Upgrade to level 4  ";
                        woodUpgradeCost = 1;
                        stoneUpgradeCost = 1;
                        attackDamage = attackDamage * 2;
                        attackRate = attackRate * 1.2f;

                        break;
                    }
                case 3: // upgades to level 4
                    {
                        
                        BuildingLevel = 4;
                        upgradeDescription = "Max Level";
                        woodUpgradeCost = 1;
                        stoneUpgradeCost = 1;
                        attackDamage = attackDamage * 2;
                        attackRate = attackRate * 1.2f;

                        break;
                    }
            }

            return true;
        }
        return false;
    }
    public bool repair()
    {
        if (Init.Instance.resourceManager.GetWood() >= woodRepairCost && Init.Instance.resourceManager.GetStone() >= stoneRepairCost)
        {
            Debug.Log("repairing");
            SetHealth(0f, true);
            UpdateStructure();
            return true;
        }
        else { Debug.Log("Not enough resources to repair");return false;} 
    }

    public int getTargetPriority() { return targetPrioritization; }
    public virtual void AttackTarget()
    {
        if (destroyed) return;
        if (target == null) FindTarget();
        if (target == null) return;

        Transform firePoint = transform;
        if (firingPoint != null) firePoint = firingPoint;
        target.Damaged(attackDamage);
        if (Vector3.Distance(firePoint.position, target.transform.position) < viewRange)
        {
            
            StartCoroutine(FireProjectile(firePoint.position, target.transform.position));
        }
        if (occupant != null)
        {
            if (firingPoint.position.x > target.transform.position.x && facingRight)
            {
                facingRight = false;
                occupant.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (firingPoint.position.x < target.transform.position.x && !facingRight)
            {
                facingRight = true;
                occupant.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public virtual IEnumerator FireProjectile(Vector2 start, Vector2 end)
    {
        //if (animator != null) animator.Play(idleClip.name);
        if (animator != null) animator.Play(shootClip.name);
        Vector3 directionVector = end - start;
        end = new Vector2(end.x, end.y);
        if (Vector2.Distance(transform.position, end) > viewRange) yield break;
        Transform proj = Instantiate(projectile, start, Quaternion.LookRotation(directionVector) * Quaternion.FromToRotation(Vector3.right, Vector3.forward), transform);
        while (Vector2.Distance(new Vector2(proj.position.x, proj.position.y), end) > 0.5f)
        {
            proj.position += directionVector.normalized * (flightSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
            if (Vector2.Distance(proj.position, transform.position) > viewRange) break;
            if (Vector2.Distance(start, end) < Vector2.Distance(start, proj.position)) break;
        }
        StartCoroutine(proj.GetComponent<Projectile>().Destroyed());
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
    private void OnDestroy()
    {

        Destroy(InstanceMenu);

    }
}
