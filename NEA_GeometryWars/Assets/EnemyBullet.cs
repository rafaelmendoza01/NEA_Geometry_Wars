using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject player;
    private float distance;
    private float EnemyBulletSpeed = 3f;
    private RandomSpawner ToGetPlayerStat;

    private void Start()
    {
        ToGetPlayerStat = GameObject.FindObjectOfType<RandomSpawner>();
    }

    void Update()
    {
        

        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 Difference = player.transform.position - transform.position;
        distance = Difference.magnitude;

        if(player.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius > distance)
        {
            ToGetPlayerStat.Life--;
            ToGetPlayerStat.PlayerSpawnState = RandomSpawner.PlayerJustSpawned.SpawnPlayerAgain;
            ToGetPlayerStat.PlayExplodeSFX();
            Destroy(player);
            Destroy(gameObject);
            
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * EnemyBulletSpeed * Time.deltaTime);
    }
}
