using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public GameObject EnemyBulletPrefab;
    public Transform EnemyFirepoint;

    public void Fire()
    {
        GameObject EnemyBullet = Instantiate(EnemyBulletPrefab, EnemyFirepoint.position, EnemyFirepoint.rotation);
    }

    
}
