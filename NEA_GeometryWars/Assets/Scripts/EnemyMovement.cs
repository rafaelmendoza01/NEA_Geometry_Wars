using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{
    protected RandomSpawner NeedToGetStats;
    protected float moveSpeed = 2f;
    protected float increaseSpeedBy = 0.04f;
    protected GameObject player;
    protected float radius;
    protected float distance;
    protected Vector2 BoundsOfPosition;
    protected void Start()
    {
        BoundsOfPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        radius = GetComponent<CircleCollider2D>().radius;
        NeedToGetStats = GameObject.FindObjectOfType<RandomSpawner>();

        if (NeedToGetStats.level % 5 == 0 || NeedToGetStats.level > 5)
        { 
            moveSpeed += (NeedToGetStats.level / 5 * increaseSpeedBy);
        }
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 Diff = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
            distance = Diff.magnitude;
        }
    }

    void FixedUpdate()
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