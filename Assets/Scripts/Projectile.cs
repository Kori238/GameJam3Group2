using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private AnimationClip flyClip = null;
    [SerializeField] private AnimationClip destroyedClip = null;
    [SerializeField] private float destroyDelay = 0f;


    public virtual IEnumerator Destroyed()
    {
        if (animator != null) animator.Play(destroyedClip.name);
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
