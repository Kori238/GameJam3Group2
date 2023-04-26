using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyStats : MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] private float moveSpeed;
    private float healthPoint;
    [SerializeField] private float maxHealthPoint;

    private void start()
    {
        healthPoint = maxHealthPoint;
    }


}
