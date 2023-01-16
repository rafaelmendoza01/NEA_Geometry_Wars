using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private GameObject player;
    private float distance;
    private float EnemyBulletSpeed = 3f;
    private RandomSpawner ToGetPlayerStat;
    private Vector2 ScreenBounds;


    //to destroy the gameobject once it is out of screen to
    //prevent too many objects existing and causing potential lag
    private bool OutOfScreen()
    {
        if (transform.position.x > ScreenBounds.x)
        {
            return true;
        }
        if (transform.position.x < -ScreenBounds.x)
        {
            return true;
        }
        if (transform.position.y > ScreenBounds.y)
        {
            return true;
        }
        if (transform.position.y < -ScreenBounds.y)
        {
            return true;
        }

        return false;
    }
    private void Start()
    {
        ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        ToGetPlayerStat = GameObject.FindObjectOfType<RandomSpawner>();
    }

    void Update()
    {
        if (OutOfScreen())
        {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 Difference = player.transform.position - transform.position;
        distance = Difference.magnitude;

        if(player.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius > distance)
        {
            ToGetPlayerStat.Life--;
            ToGetPlayerStat.PlayerSpawnState = RandomSpawner.PlayerJustSpawned.SpawnPlayerAgain;
            ToGetPlayerStat.PlayDeathSFX();
            Destroy(player);
            Destroy(gameObject);         
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * EnemyBulletSpeed * Time.deltaTime);
    }
}
