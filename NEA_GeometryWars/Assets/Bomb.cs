using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private GameObject[] AllEnemies;
    private GameObject[] AllEnemyBullet;
    private float distance;
    private PlayerMovement player;
    private RandomSpawner ToSetLevelCleared;
    private enum KnowWhenToIncrease
    {
        Increase,
        Leave,
    }
    KnowWhenToIncrease IncreaseSizeState = KnowWhenToIncrease.Increase;
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        ToSetLevelCleared = FindObjectOfType<RandomSpawner>();
    }

    
    void Update()
    {
        AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        AllEnemyBullet = GameObject.FindGameObjectsWithTag("EnemyBullet");

        for (int i = 0; i < AllEnemyBullet.Length; i++)
        {
            if (AllEnemyBullet[i] != null)
            {
                Vector2 Diff = AllEnemyBullet[i].transform.position - transform.position;
                distance = Diff.magnitude;

                if (AllEnemyBullet[i].GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius >= distance)
                {
                    Destroy(AllEnemyBullet[i]);
                }
            }
        }

        for (int i = 0; i < AllEnemies.Length; i++)
        {
            if (AllEnemies[i] != null)
            {
                Vector2 Diff = AllEnemies[i].transform.position - transform.position;
                distance = Diff.magnitude;

                if (AllEnemies[i].GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius >= distance)
                {
                    Destroy(AllEnemies[i]);
                    player.KillHistory++;
                    if (player.KillHistory == ToSetLevelCleared.level)
                    {
                        ToSetLevelCleared.LevelCleared = true;
                        player.KillHistory = 0;
                    }
                }
            }
        }

        if (IncreaseSizeState == KnowWhenToIncrease.Increase)
        {
            IncreaseSizeState = KnowWhenToIncrease.Leave;
            StartCoroutine(IncreaseBombSize());
        }
    }


    IEnumerator IncreaseBombSize()
    {
        float i = 1;
        while (i < 25)
        {
            i += 0.2f;
            transform.localScale = new Vector2(i, i);
            GetComponent<CircleCollider2D>().radius += 0.2f;
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(gameObject);
        yield break;
    }
}
