using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{
    private RandomSpawner NeedToGetStats;
    private float moveSpeed = 2f;
    private float increaseSpeedBy = 0.2f;
    private GameObject player;
    private float radius;
    private float distance;


    //purpose of the enum is to ensure that the speed of the enemy isnt constantly increased every frame
    private enum ToIncreaseSpeed
    {
        JustIncrease,
        Waiting,
        StillSame,
    }
    ToIncreaseSpeed CurrentState = ToIncreaseSpeed.Waiting; 


    private void Start()
    {
        NeedToGetStats = GameObject.FindObjectOfType<RandomSpawner>();
        if (NeedToGetStats.level % 5 == 0)
        {
            CurrentState = ToIncreaseSpeed.Waiting;
        }
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        radius = GetComponent<CircleCollider2D>().radius;

        if (player != null)
        {
            Vector2 Diff = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
            distance = Diff.magnitude;
        }

        if (NeedToGetStats.level % 5 == 0 && CurrentState == ToIncreaseSpeed.Waiting)
        {
            CurrentState = ToIncreaseSpeed.StillSame;
        }

        if (CurrentState == ToIncreaseSpeed.StillSame)
        {
            moveSpeed += (NeedToGetStats.level/5 * increaseSpeedBy);
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