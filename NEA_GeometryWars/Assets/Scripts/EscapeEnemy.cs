using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EscapeEnemy : EnemyMovement
{
    GameObject[] PlayerBullets; 
    IDictionary<GameObject, float> ClosestBullets = new Dictionary<GameObject, float>();
    
    static void MergeSort(IDictionary<GameObject, float> Dictionary)
    {
        if (Dictionary.Count < 2)
        {
            return;
        }
        else
        {
            int midpoint = Dictionary.Count / 2;
            IDictionary<GameObject, float> LeftHalf = new Dictionary<GameObject, float>();
            for(int i = 0; i < midpoint; i++)
            {
                
            }
        }
    }
    
    private void Update()
    {
        PlayerBullets = GameObject.FindGameObjectsWithTag("Bullet");
        if(PlayerBullets.Length == 0)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector2 Diff = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
                distance = Diff.magnitude;
            }
        }
        
        for(int i = 0; i < PlayerBullets.Length; i++)
        {
            GameObject NextBullet = PlayerBullets[i];
            Vector2 Difference = NextBullet.GetComponent<Transform>().position - GetComponent<Transform>().position;
            if (Difference.magnitude <= 8f)
            {
                ClosestBullets.Add(NextBullet, Difference.magnitude);
            }
        }
        if(ClosestBullets.Count > 1)
        {

        }
    }

    private void FixedUpdate()
    {
        if(PlayerBullets.Length == 0)
        {
            moveSpeed = 2.5f;
            if (player != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                if (player.GetComponent<CircleCollider2D>().radius + radius > distance)
                {
                    NeedToGetStats.PlayDeathSFX();
                    NeedToGetStats.Life--;
                    NeedToGetStats.PlayerSpawnState = RandomSpawner.PlayerJustSpawned.SpawnPlayerAgain;
                    Destroy(player);
                }
            }
        }
        else
        {
            moveSpeed = 10f;
        }
    }
}
