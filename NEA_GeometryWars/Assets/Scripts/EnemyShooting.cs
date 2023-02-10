using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : EnemyMovement
{
    private float AngleSpeed = 400f;

    [SerializeField]
    private Transform EnemyFirepoint;
    [SerializeField]
    private GameObject EnemBulletPrefab;

    //IEnumarator and enum used to let the enemy shoot every time interval (2s)
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


    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector2 Diff = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
            distance = Diff.magnitude;
        }

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
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            Vector2 AimAt = player.transform.position - transform.position;
            Quaternion RotateToPlayer = Quaternion.LookRotation(transform.forward, AimAt);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, RotateToPlayer, Time.deltaTime * AngleSpeed);
            if (player.GetComponent<CircleCollider2D>().radius + radius > distance)
            {
                NeedToGetStats.PlayDeathSFX();
                NeedToGetStats.Life--;
                NeedToGetStats.PlayerSpawnState = RandomSpawner.PlayerJustSpawned.SpawnPlayerAgain;
                Destroy(player);
            }
        }
    }
}
