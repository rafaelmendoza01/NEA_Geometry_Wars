using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameObject[] allEnemies;
    private GameObject[] allEnemyBullets;
    private float distance;
    private RandomSpawner toSetLevelCleared;
    private enum eKnowWhenToIncrease
    {
        Increase,
        Leave,
    }
    eKnowWhenToIncrease IncreaseSizeState = eKnowWhenToIncrease.Increase;
    void Start()
    {
        toSetLevelCleared = FindObjectOfType<RandomSpawner>();
        toSetLevelCleared.PlayBombSFX();
    }

    
    void Update()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        allEnemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

        for (int i = 0; i < allEnemyBullets.Length; i++)
        {
            if (allEnemyBullets[i] != null)
            {
                Vector2 Diff = allEnemyBullets[i].transform.position - transform.position;
                distance = Diff.magnitude;

                if (allEnemyBullets[i].GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius >= distance)
                {
                    toSetLevelCleared.CurrentScore += 5;
                    Destroy(allEnemyBullets[i]);
                }
            }
        }

        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i] != null)
            {
                Vector2 Diff = allEnemies[i].transform.position - transform.position;
                distance = Diff.magnitude;

                if (allEnemies[i].GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius >= distance)
                {
                    Destroy(allEnemies[i]);
                    PlayerMovement.KillsForLevel++;
                    toSetLevelCleared.CurrentScore += 10;
                    if (PlayerMovement.KillsForLevel == toSetLevelCleared.level)
                    {
                        toSetLevelCleared.LevelCleared = true;
                        PlayerMovement.KillsForLevel = 0;
                    }
                }
            }
        }

        if (IncreaseSizeState == eKnowWhenToIncrease.Increase)
        {
            IncreaseSizeState = eKnowWhenToIncrease.Leave;
            StartCoroutine(IncreaseBombSize());
        }
    }


    IEnumerator IncreaseBombSize()
    {
        const float scale_increment = 0.2f;
        const float scale_interval  = 0.02f;

        float i = 1f;
        while (i < 25f)
        {
            i += scale_increment;
            transform.localScale = new Vector2(i, i);
            GetComponent<CircleCollider2D>().radius += scale_increment;
            yield return new WaitForSeconds(scale_interval);
        }
        toSetLevelCleared.State = RandomSpawner.SpawnState.BombRecentlyDestroyed;
        Destroy(gameObject);
        yield break;
    }
}
