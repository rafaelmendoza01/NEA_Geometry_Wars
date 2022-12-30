using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{

    private RandomSpawner NeedToGetPlayerStats;
    private float moveSpeed = 2f;
    Vector2 GoHereIfNoPlayer;
    public GameObject player;
    public float radius;
    public float distance;
    public bool status = true;


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
        
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            if (player.GetComponent<CircleCollider2D>().radius + radius > distance)
            {
                NeedToGetPlayerStats.PlayExplodeSFX();
                NeedToGetPlayerStats.Life--;
                NeedToGetPlayerStats.PlayerSpawnState = RandomSpawner.PlayerJustSpawned.SpawnPlayerAgain;
                Destroy(player);
            }
        }
    }
}