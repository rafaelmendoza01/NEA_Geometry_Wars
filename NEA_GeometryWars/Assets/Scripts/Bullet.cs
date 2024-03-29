using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject Enemy;
    private GameObject[] AllEnemies;
    protected float distance;
    protected float Speed = 20f;
    protected RandomSpawner ToGetStats;
    private GameObject[] AllEnemyBullets;
    private GameObject AnEnemyBullet;

    private Vector2 StartposAsVector;
    public Vector2 tempVector;

    [SerializeField]
    protected GameObject ExplodeEffect;

    protected Vector2 ScreenBounds;

    protected void CreateExplosionFX()
    {
        int SpawnHere = Random.Range(0, 360);
        for(int i = 0; i < 4; i++)
        {
            Instantiate(ExplodeEffect, transform.position, Random.rotation);
        }
    }

    private void Start()
    {
        StartposAsVector = new Vector2(transform.position.x, transform.position.y);
        ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        ToGetStats = GameObject.FindObjectOfType<RandomSpawner>();
    }
    private void Update()
    {
        AllEnemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        Vector2 NewPosAsVector = new Vector2(transform.position.x, transform.position.y);
        if(NewPosAsVector != StartposAsVector)
        {
            tempVector = NewPosAsVector - StartposAsVector;
            tempVector = tempVector.normalized;
        }
        

        if (OutOfScreen())
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < AllEnemies.Length; i++)
        {
            Enemy = AllEnemies[i];
            if (Enemy != null)
            {
                Vector2 Diff = Enemy.transform.position - transform.position;
                distance = Diff.magnitude;
                if (Enemy.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius > distance)
                {
                    CreateExplosionFX();

                    if (!OptionsMenu.SpecialGameMode)
                    {
                        PlayerMovement.KillsForLevel++;
                    }
                    ToGetStats.PlayExplodeSFX();
                    Destroy(Enemy);
                    ToGetStats.CurrentScore += 10;
                    
                    Destroy(gameObject);
                }
            }
           
        }

        for(int i = 0; i < AllEnemyBullets.Length; i++)
        {
            AnEnemyBullet = AllEnemyBullets[i];
            Vector2 Diff = AnEnemyBullet.transform.position - transform.position;
            distance = Diff.magnitude;
            if(AnEnemyBullet.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius > distance)
            {
                ToGetStats.CurrentScore += 5;
                ToGetStats.PlayExplodeSFX();
                CreateExplosionFX();
                Destroy(AnEnemyBullet);
                Destroy(gameObject);
            }
        } 
    }

    //this can be shared with the child class of an EnemyBullet as moving both bullets works exactly the same way
    protected void FixedUpdate()
    {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    //to destroy the gameobject once it is out of screen to
    //prevent too many objects existing and causing potential lag
    protected bool OutOfScreen()
    {
        if(Mathf.Abs(transform.position.x) > ScreenBounds.x)
        {
            return true;
        }
        
        if (Mathf.Abs(transform.position.y) > ScreenBounds.y)
        {
            return true;
        }
        return false;
    }
}