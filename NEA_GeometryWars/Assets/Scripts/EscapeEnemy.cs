using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EscapeEnemy : EnemyMovement
{
    GameObject[] PlayerBullets; 

    private void Update()
    {
        PlayerBullets = GameObject.FindGameObjectsWithTag("Bullet");
        if(PlayerBullets.Length == 0)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector2 Diff = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
                distance = Diff.magnitude;
            }
        }
    }

    private void FixedUpdate()
    {
        if(PlayerBullets.Length == 0)
        {
            if (player != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
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
}
