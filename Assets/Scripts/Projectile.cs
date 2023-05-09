using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Animator animator = null;
    public AnimationClip flyClip = null;
    public AnimationClip destroyedClip = null;
    public Tower towerScript;
    public float destroyDelay = 0f;

    public void Start()
    {
        towerScript = GetComponentInParent<Tower>();
    }


    public virtual IEnumerator Destroyed(GameObject target, bool damageTarget = false)
    {
        if (animator != null) animator.Play(destroyedClip.name);
        if (damageTarget)
        {
            EnemyPathfinding enemyScript = target.GetComponent<EnemyPathfinding>();
            if (enemyScript != null)
            {
                enemyScript.Damaged(towerScript.attackDamage);
            }
        }
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
