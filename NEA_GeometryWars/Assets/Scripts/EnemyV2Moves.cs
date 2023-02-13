using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyV2Moves : EnemyMovement
{
    private float RotationSpeed = 300f;
    Vector2 GoToHere;
    bool ChooseNewPosition = true;
    float GoToX;
    float GoToY;
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 PosAsVector2 = new Vector2(transform.position.x, transform.position.y);
        
        if(PosAsVector2 == GoToHere)
        {
            ChooseNewPosition = true;
        }

        if (ChooseNewPosition)
        {
            do
            {
                GoToX = Random.Range(-BoundsOfPosition.x + radius, BoundsOfPosition.x - radius);
                GoToY = Random.Range(-BoundsOfPosition.y + radius, BoundsOfPosition.y - radius);
                GoToHere = new Vector2(GoToX, GoToY);
            } while (GoToHere == PosAsVector2); 
            ChooseNewPosition = false;
        }
        

        Vector2 Diff = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
        distance = Diff.magnitude;
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, GoToHere, moveSpeed * Time.deltaTime);
        if (player != null)
        {
            if (player.GetComponent<CircleCollider2D>().radius + radius > distance)
            {
                NeedToGetStats.PlayDeathSFX();
                NeedToGetStats.Life--;
                NeedToGetStats.PlayerSpawnState = RandomSpawner.PlayerJustSpawned.SpawnPlayerAgain;
                Destroy(player);
            }
        }
    }
}
