using UnityEngine;

//enemey bullet inherits variables and a function from the Bullet class
public class EnemyBullet : Bullet
{
    private GameObject player;
    private void Start()
    {
        ToGetStats = GameObject.FindObjectOfType<RandomSpawner>();
        Speed = 3f;
        ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
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
            ToGetStats.Life--;
            ToGetStats.PlayerSpawnState = RandomSpawner.PlayerJustSpawned.SpawnPlayerAgain;
            ToGetStats.PlayDeathSFX();
            CreateExplosionFX();
            Destroy(player);
            Destroy(gameObject);         
        }
    }
}