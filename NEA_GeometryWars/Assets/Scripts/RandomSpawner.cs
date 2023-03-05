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

    public int PlayerKillsAcrossAllLevel;
    public int BombsUsed = 3;
    public int CurrentScore;

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

    [SerializeField]
    private GameObject scoreMenuUI;

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

    private bool StartedRandomSpawning = false;
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
            if (!OptionsMenu.SpecialGameMode)
            {
                if (PlayerMovement.KillsForLevel == level)
                {
                    LevelCleared = true;
                    PlayerMovement.KillsForLevel = 0;
                    StopAllCoroutines();
                }

                if (LevelCleared)
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
                        level++;
                        StartCoroutine(SpawnWave(level));
                        LevelCleared = false;
                    }
                }
            }
            else
            {
                if(DoesBombStillExist == null)
                {
                    if(State == SpawnState.BombRecentlyDestroyed)
                    {
                        StartCoroutine(SpecialGameSpawn());
                        State = SpawnState.TimeToSpawn;
                        StartedRandomSpawning = true;
                    }
                    if (!StartedRandomSpawning)
                    {
                        StartCoroutine(SpecialGameSpawn());
                        StartedRandomSpawning = true;
                    }
                }
            }
        }
        else
        {
            StopAllCoroutines();
            for (int i = 0; i < LivingEnemies.Length; i++)
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

            if(Life > 0 && !OptionsMenu.SpecialGameMode)
            {
                Instantiate(playerPrefab, transform.position, Quaternion.identity);
                StartCoroutine(SpawnWave(level - PlayerMovement.KillsForLevel));
            }
            else if(Life > 0 && OptionsMenu.SpecialGameMode)
            {
                Instantiate(playerPrefab, transform.position, Quaternion.identity);
                StartedRandomSpawning = true;
                StartCoroutine(SpecialGameSpawn());
            }
            else
            {
                scoreMenuUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    //To ensure the distance between the player and a randomly spawned enemy isnt too close, thus making it more fair.
    private bool IsWithingRange(GameObject ThePlayer, Vector2 Pos)
    {
        Vector2 PlayerPositionAsVector2 = new Vector2(ThePlayer.transform.position.x, ThePlayer.transform.position.y);
        Vector2 diff = PlayerPositionAsVector2 - Pos; 
        float dist = diff.magnitude;

        if (dist < 3.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator SpecialGameSpawn()
    {
        GameObject _Enemy;
        GameObject _Player = GameObject.FindGameObjectWithTag("Player");
        Vector2 SpawnHere;
        int temp = 0;
        while (temp < 10)
        {
            //the code keeps on finding a new vector position to spawn the enemy until the spawn point isnt too close.
            do
            {
                int EnemyType = Random.Range(0, enemyPrefabs.Length);
                _Enemy = enemyPrefabs[EnemyType];
                Vector2 screenAsVector = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
                float x = Random.Range(-screenAsVector.x + _Enemy.GetComponent<CircleCollider2D>().radius, screenAsVector.x - _Enemy.GetComponent<CircleCollider2D>().radius);
                float y = Random.Range(-screenAsVector.y + _Enemy.GetComponent<CircleCollider2D>().radius, screenAsVector.y - _Enemy.GetComponent<CircleCollider2D>().radius);
                SpawnHere = new Vector2(x, y);

            } while (IsWithingRange(_Player, SpawnHere));
            Instantiate(_Enemy, SpawnHere, Quaternion.identity);
            temp++;
            float waitingTime = Random.Range(0.4f, 0.9f);
            yield return new WaitForSeconds(waitingTime);
        }

        if (temp == 10)
        {
            StartedRandomSpawning = false;
        }
        yield break;
    }

    //purpose is to spawn the enemies in a sytematic way in the "normal" game mode.
    IEnumerator SpawnWave(int SpawnSet)
    {
        NewSet = 0;
        int waves = SpawnSet / 6;
        //6 spawn points so waves are the SpawnSet/6
        int NumEnemyToSpawnLast = SpawnSet % 6;
        //as the number of enemies wont be in multiple of 6s all the time, there has to be a way to calculate the last set of enemies (<6)
        while (NewSet < SpawnSet)
        {
            if (level < 5)
            {
                for (int i = 0; i < SpawnSet; i++)
                {
                    int Type = Random.Range(0, 2);
                    SpawnEnemy(Type, i);
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

    //this helps to write the instatiation of enemies in a neater way for the normal game mode.
    private void SpawnEnemy(int TypeOfEnemy, int WhereToSpawn)
    {
        Instantiate(enemyPrefabs[TypeOfEnemy], SpawnPoints[WhereToSpawn].position, Quaternion.identity);
    }  
}