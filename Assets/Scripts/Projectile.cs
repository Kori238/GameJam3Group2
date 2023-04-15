using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public virtual IEnumerator Destroyed()
    {
        Destroy(gameObject);
        yield return null;
    }
}
