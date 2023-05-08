using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnProjectile : Projectile
{
    public override IEnumerator Destroyed(GameObject target, bool damageTarget = false)
    {
        if (animator != null) animator.Play(destroyedClip.name);
        if (damageTarget)
        {
            EnemyPathfinding enemyScript = target.GetComponent<EnemyPathfinding>();
            if (enemyScript != null)
            {
                enemyScript.Damaged(towerScript.attackDamage);
                yield return enemyScript.Burn((int)towerScript.burnDamage, towerScript.burnTick, towerScript.burnDuration);
            }
        }
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
