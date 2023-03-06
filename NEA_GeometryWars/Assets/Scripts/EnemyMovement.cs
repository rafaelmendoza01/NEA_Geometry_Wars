using UnityEngine;
public class EnemyMovement : MonoBehaviour
{

    //all these variables are to be shared with the child classes.
    protected RandomSpawner NeedToGetStats;
    protected float moveSpeed = 2f;
    protected float increaseSpeedBy = 0.04f;
    protected GameObject player;
    protected float radius;
    protected float distance;
    protected Vector2 BoundsOfPosition;

    protected void Start()
    { 
        BoundsOfPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        //this variable is only used by the EnemyV2Moves class as it needs to find a random position to go to but I included here to prevent writing a start method there and as well to use in other child classes if I ever need to/have time.
        radius = GetComponent<CircleCollider2D>().radius;
        NeedToGetStats = GameObject.FindObjectOfType<RandomSpawner>();

        //to ensure the speed of the enemy increases each time the player progresses in the normal game mode.
        if (!OptionsMenu.SpecialGameMode)
        {
            if (NeedToGetStats.level % 5 == 0 || NeedToGetStats.level > 5)
            {
                moveSpeed += (NeedToGetStats.level / 5 * increaseSpeedBy);
            }
        }
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 Diff = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
            distance = Diff.magnitude;
        }
    }

    void FixedUpdate()
    {
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
}