using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class RandomSpawner : MonoBehaviour
{
    //source: https://youtu.be/Vrld13ypX_I

    [SerializeField]
    private Transform[] SpawnPoints;
    //to set up spawn points for enemies as an array
    private PlayerMovement player;

    [SerializeField]
    private  GameObject playerPrefab;
    public int Life;
    public bool LevelCleared = true;

    private int SpawnRemaining;
    private int NewSet;

    public int BombsUsed = 3;
    public int CurrentScore;

    private OptionsMenu TheirChosenSettings;

    [SerializeField]
    private AudioSource Triangulation;
    [SerializeField]
    private AudioSource GW;
    [SerializeField]
    private AudioSource Tempest;
    [SerializeField]
    private AudioSource CollisionSoundFX;
    [SerializeField]
    private AudioSource DeathSFX;
    [SerializeField]
    private AudioSource ShootingSFX;
    [SerializeField]
    private AudioSource BombSFX;

    private GameObject DoesBombStillExist;

    public void PlayBombSFX()
    {
        BombSFX.Play();
    }

    public void PlayDeathSFX()
    {
        DeathSFX.Play();
    }

    public void PlayShootingSound()
    {
        ShootingSFX.Play();
    }
    public void PlayExplodeSFX()
    {
        CollisionSoundFX.Play();
    }

    public enum PlayerJustSpawned
    {
        SpawnPlayerAgain,
        WaitingToDie,
    }

    public enum SpawnState
    {
        TimeToSpawn, 
        Spawning,
        BombStillAround,
        BombRecentlyDestroyed,
    }
    //to determine state of enemies, whether they need to be spawned etc.

    public PlayerJustSpawned PlayerSpawnState = PlayerJustSpawned.WaitingToDie;

    private GameObject[] LivingBullets;
    private GameObject[] EnemyBullets;

    public int level = 0;
    //Level corresponds to the number of enemies that need to be spawned in that time
    private GameObject[] LivingEnemies;
    //to store the number of any existing enemies.

    public GameObject[] enemyPrefabs;
    public SpawnState State = SpawnState.TimeToSpawn;
    //to store the types of enemies to be spawned as an array.

    private float TimeBetweenWaves = 0.6f;
    private float TimeBetweenEnemies = 0.1f;
    //to wait correct amount of time between each enemy and wave of enemies

    private void Start()
    {
        CurrentScore = 0;
        Life = 3;
        if (OptionsMenu.Music3Wanted == true)
        {
            Triangulation.Play();
        }
        else if(OptionsMenu.Music2Wanted == true)
        {
            Tempest.Play();
        }
        else
        {
            GW.Play();
        } 
    }

    void Update()
    {
        DoesBombStillExist = GameObject.FindGameObjectWithTag("Bomb");
        player = GameObject.FindObjectOfType<PlayerMovement>();
        LivingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        LivingBullets = GameObject.FindGameObjectsWithTag("Bullet");
        EnemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

        if (DoesBombStillExist != null)
        {
            State = SpawnState.BombStillAround;
            StopAllCoroutines();
        }

        if (player != null)
        {
            if(player.KillHistory == level)
            {
                LevelCleared = true;
            }

            if (LevelCleared == true) 
            {
                State = SpawnState.TimeToSpawn;
            }

            if (DoesBombStillExist == null)
            {
                if (State == SpawnState.TimeToSpawn && LevelCleared)
                {
                    level++;
                    StartCoroutine(SpawnWave(level));
                    LevelCleared = false;
                }
                if (State == SpawnState.BombRecentlyDestroyed && !LevelCleared)
                {
                    StartCoroutine(SpawnWave(SpawnRemaining));
                    LevelCleared = false;
                }
                if (State == SpawnState.BombRecentlyDestroyed && LevelCleared)
                {
                    StartCoroutine(SpawnWave(level++));
                    LevelCleared = false;
                }
            }
        }
        else
        {
            for(int i = 0; i < LivingEnemies.Length; i++)
            {
                Destroy(LivingEnemies[i]);
            }
            for(int i = 0; i < LivingBullets.Length; i++)
            {
                Destroy(LivingBullets[i]);
            }
            for(int i = 0; i < EnemyBullets.Length; i++)
            {
                Destroy(EnemyBullets[i]);
            }

            if(Life > 0)
            {
                StopAllCoroutines();
                Instantiate(playerPrefab, transform.position, Quaternion.identity);
                StartCoroutine(SpawnWave(level));
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }

    //purpose is to know when to spawn enemies
    IEnumerator SpawnWave(int SpawnSet)
    {
        NewSet = 0;
        int waves = SpawnSet / 6;
        int NumEnemyToSpawnLast = SpawnSet % 6;
        while (NewSet < SpawnSet) //|| (State == SpawnState.TimeToSpawn))
        {
            if (level < 5)
            {
                for (int i = 0; i < SpawnSet; i++)
                {
                    SpawnEnemy(0, i);
                    State = SpawnState.Spawning;
                    NewSet++;
                    SpawnRemaining = level - NewSet;
                    yield return new WaitForSeconds(TimeBetweenEnemies);            
                }
            }
            else
            {
                for (int i = 0; i < waves; i++)
                {
                    for (int j = 0; j < SpawnPoints.Length; j++)
                    {
                        int Type = Random.Range(0, enemyPrefabs.Length);
                        SpawnEnemy(Type, j);
                        NewSet++;
                        SpawnRemaining = level - NewSet;
                        State = SpawnState.Spawning;
                        yield return new WaitForSeconds(TimeBetweenEnemies);
                    }
                    yield return new WaitForSeconds(TimeBetweenWaves);
                }

                for (int i = 0; i < NumEnemyToSpawnLast; i++)
                {
                    int Type = Random.Range(0, enemyPrefabs.Length);
                    SpawnEnemy(Type, i);
                    State = SpawnState.Spawning;
                    NewSet++;
                    SpawnRemaining = level - NewSet;
                    yield return new WaitForSeconds(TimeBetweenEnemies);
                }
            }
        }
        yield break;
    }  

    void SpawnEnemy(int TypeOfEnemy, int WhereToSpawn)
    {
        Instantiate(enemyPrefabs[TypeOfEnemy], SpawnPoints[WhereToSpawn].position, Quaternion.identity);
    }  
}