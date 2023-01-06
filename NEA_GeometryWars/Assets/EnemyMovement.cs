using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{

    private RandomSpawner NeedToGetPlayerStats;
    private float moveSpeed = 2f;
    private float increaseSpeedBy = 0.5f;
    Vector2 GoHereIfNoPlayer;
    public GameObject player;
    public float radius;
    public float distance;

    private enum ToIncreaseSpeed
    {
        JustIncrease,
        Waiting,
        LevelJustChanged,
    }
    ToIncreaseSpeed CurrentState = ToIncreaseSpeed.Waiting;

    private void Update()
    {
        
        NeedToGetPlayerStats = GameObject.FindObjectOfType<RandomSpawner>();
        player = GameObject.FindGameObjectWithTag("Player");
        radius = GetComponent<CircleCollider2D>().radius;

        if (player != null)
        {
            Vector2 Diff = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
            distance = Diff.magnitude;
        }
        
        if (NeedToGetPlayerStats.level % 5 != 0)
        {
            CurrentState = ToIncreaseSpeed.Waiting;
        }

        if (NeedToGetPlayerStats.level % 5 == 0 && CurrentState == ToIncreaseSpeed.Waiting)
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
                NeedToGetPlayerStats.PlayDeathSFX();
                NeedToGetPlayerStats.Life--;
                NeedToGetPlayerStats.PlayerSpawnState = RandomSpawner.PlayerJustSpawned.SpawnPlayerAgain;
                Destroy(player);
            }
        }
    }
}