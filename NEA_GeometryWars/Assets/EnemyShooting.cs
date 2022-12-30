using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject player;
    private float AngleSpeed = 400f;
    public EnemyWeapon WeaponOfEnemy;

    public enum ShootingTime
    {
        ShootNow,
        WaitFirst,
    }

    ShootingTime ShootStatus = ShootingTime.ShootNow;

    IEnumerator ShootEvery()
    {
        WeaponOfEnemy.Fire();
        yield return new WaitForSeconds(2f);
        ShootStatus = ShootingTime.ShootNow;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (ShootStatus == ShootingTime.ShootNow)
        {
            ShootStatus = ShootingTime.WaitFirst;
            StartCoroutine(ShootEvery());
        }
    }

    private void FixedUpdate()
    {
        Vector2 AimAt = player.transform.position - transform.position;
        Quaternion RotateToPlayer = Quaternion.LookRotation(transform.forward, AimAt);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, RotateToPlayer, Time.deltaTime * AngleSpeed);
    }
}
