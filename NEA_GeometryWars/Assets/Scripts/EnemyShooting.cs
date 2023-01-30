using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private GameObject player;
    private float AngleSpeed = 400f;

    [SerializeField]
    private Transform EnemyFirepoint;
    [SerializeField]
    private GameObject EnemBulletPrefab;

    private enum ShootingTime
    {
        ShootNow,
        WaitFirst,
    }

    ShootingTime ShootStatus = ShootingTime.ShootNow;

    IEnumerator ShootEvery()
    {
        Instantiate(EnemBulletPrefab, EnemyFirepoint.position, EnemyFirepoint.rotation);
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
        if (player != null)
        {
            Vector2 AimAt = player.transform.position - transform.position;
            Quaternion RotateToPlayer = Quaternion.LookRotation(transform.forward, AimAt);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, RotateToPlayer, Time.deltaTime * AngleSpeed);
        }
    }
}
