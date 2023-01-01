using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject[] AllEnemies;
    private float distance;
    private float FireForce = 20f;
    public PlayerMovement player;
    public EnemyMovement ToChangeStatusOfEnemy;
    private RandomSpawner ToGetlevel;
    private GameObject[] AllEnemyBullets;
    private GameObject AnEnemyBullet;

    [SerializeField]
    private GameObject ExplodeEffect;
    

    private void CreateExposionFX()
    {
        int SpawnHere = Random.Range(0, 360);
        for(int i = 0; i < 4; i++)
        {
            Instantiate(ExplodeEffect, transform.position, Random.rotation);
        }
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
        ToChangeStatusOfEnemy = GameObject.FindObjectOfType<EnemyMovement>();
        ToGetlevel = GameObject.FindObjectOfType<RandomSpawner>();
    }
    private void Update()
    {


        AllEnemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < AllEnemies.Length; i++)
        {
            Enemy = AllEnemies[i];
            if (Enemy != null)
            {
                Vector2 Diff = Enemy.transform.position - transform.position;
                distance = Diff.magnitude;
                if (Enemy.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius > distance)
                {
                    ToChangeStatusOfEnemy.status = false;
                    CreateExposionFX();
                    player.KillHistory++;
                    ToGetlevel.PlayExplodeSFX();
                    Destroy(Enemy);
                    Destroy(gameObject);
                }
            }
            if(player.KillHistory == ToGetlevel.level)
            {
                ToGetlevel.LevelCleared = true;
                player.KillHistory = 0;
            } 
        }

        for(int i = 0; i < AllEnemyBullets.Length; i++)
        {
            AnEnemyBullet = AllEnemyBullets[i];
            Vector2 Diff = AnEnemyBullet.transform.position - transform.position;
            distance = Diff.magnitude;
            if(AnEnemyBullet.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius > distance)
            {
                ToGetlevel.PlayExplodeSFX();
                CreateExposionFX();
                Destroy(AnEnemyBullet);
                Destroy(gameObject);
            }
        }

        
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * FireForce * Time.deltaTime);

    }
}
