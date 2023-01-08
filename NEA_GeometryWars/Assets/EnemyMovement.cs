using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{

    private RandomSpawner NeedToGetStats;
    private float moveSpeed = 2f;
    private float increaseSpeedBy = 0.5f;
    private GameObject player;
    private float radius;
    private float distance;

    private enum ToIncreaseSpeed
    {
        JustIncrease,
        Waiting,
        LevelJustChanged,
    }
    ToIncreaseSpeed CurrentState = ToIncreaseSpeed.Waiting;

    private void Update()
    {
        
        NeedToGetStats = GameObject.FindObjectOfType<RandomSpawner>();
        player = GameObject.FindGameObjectWithTag("Player");
        radius = GetComponent<CircleCollider2D>().radius;

        if (player != null)
        {
            Vector2 Diff = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
            distance = Diff.magnitude;
        }
        
        if (NeedToGetStats.level % 5 != 0)
        {
            CurrentState = ToIncreaseSpeed.Waiting;
        }

        if (NeedToGetStats.level % 5 == 0 && CurrentState == ToIncreaseSpeed.Waiting)
        {
            CurrentState = ToIncreaseSpeed.LevelJustChanged;
        }

        if (CurrentState == ToIncreaseSpeed.LevelJustChanged)
        {
            moveSpeed += increaseSpeedBy;
            CurrentState = ToIncreaseSpeed.JustIncrease;
        }
    }

    private void FixedUpdate()
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