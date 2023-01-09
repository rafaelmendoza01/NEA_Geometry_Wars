using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    new Vector2 startsize = new Vector2(1, 1);
    new Vector2 endsize = new Vector2(10, 10);
    bool increase;
    private GameObject[] AllEnemies;
    private GameObject[] AllEnemyBullet;

    private enum KnowWhenToIncrease
    {
        Increase,
        Leave,
    }
    KnowWhenToIncrease IncreaseSizeState = KnowWhenToIncrease.Increase;
    void Start()
    {
        increase = true;

    }

    
    void Update()
    {  
        if (IncreaseSizeState == KnowWhenToIncrease.Increase)
        {
            IncreaseSizeState = KnowWhenToIncrease.Leave;
            StartCoroutine(IncreaseBombSize());
        }
    }

    IEnumerator IncreaseBombSize()
    {
        float i = 1;
        while (i < 20)
        {
            transform.localScale = new Vector2(i += 0.1f, i += 0.1f);
            GetComponent<CircleCollider2D>().radius += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }
}
