using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private GameObject Enemy;
    private GameObject[] AllEnemies;
    protected float distance;
    protected float Speed = 20f;
    protected RandomSpawner ToGetStats;
    private GameObject[] AllEnemyBullets;
    private GameObject AnEnemyBullet;

    public Vector2 tempVector;

    [SerializeField]
    private GameObject ExplodeEffect;

    protected Vector2 ScreenBounds;

    private void CreateExplosionFX()
    {
        int SpawnHere = Random.Range(0, 360);
        for(int i = 0; i < 4; i++)
        {
            Instantiate(ExplodeEffect, transform.position, Random.rotation);
        }
    }

    private void Start()
    {
        ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        ToGetStats = GameObject.FindObjectOfType<RandomSpawner>();
    }
    private void Update()
    {
        AllEnemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (OutOfScreen())
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < AllEnemies.Length; i++)
        {
            Enemy = AllEnemies[i];
            if (Enemy != null)
            {
                Vector2 Diff = Enemy.transform.position - transform.position;
                distance = Diff.magnitude;
                if (Enemy.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius > distance)
                {
                    CreateExplosionFX();
                    PlayerMovement.KillsForLevel++;
                    ToGetStats.PlayExplodeSFX();
                    Destroy(Enemy);
                    ToGetStats.CurrentScore += 10;
                    if (PlayerMovement.KillsForLevel == ToGetStats.level)
                    {
                        ToGetStats.LevelCleared = true;
                        PlayerMovement.KillsForLevel = 0;
                    }
                    Destroy(gameObject);
                }
            }
           
        }

        for(int i = 0; i < AllEnemyBullets.Length; i++)
        {
            AnEnemyBullet = AllEnemyBullets[i];
            Vector2 Diff = AnEnemyBullet.transform.position - transform.position;
            distance = Diff.magnitude;
            if(AnEnemyBullet.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius > distance)
            {
                ToGetStats.CurrentScore += 5;
                ToGetStats.PlayExplodeSFX();
                CreateExplosionFX();
                Destroy(AnEnemyBullet);
                Destroy(gameObject);
            }
        } 
    }

    //this can be shared with the child class of an EnemyBullet as moving both bullets works exactly the same way
    protected void FixedUpdate()
    {
        tempVector = Vector2.up;
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    //to destroy the gameobject once it is out of screen to
    //prevent too many objects existing and causing potential lag
    protected bool OutOfScreen()
    {
        if(transform.position.x > ScreenBounds.x)
        {
            return true;
        }
        if(transform.position.x < -ScreenBounds.x)
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
}
