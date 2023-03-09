using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private Transform Firepoint;
    private float AngleSpeed = 900f;
    public static int KillsForLevel = 0;
    private RandomSpawner GetStats;
    private GameObject AbombStillExist;
    private bool StartShooting = false;

    Vector2 moveDirection;
    Vector2 mousePosition;
    private void Start()
    {
        GetStats = GameObject.FindObjectOfType<RandomSpawner>();
    }

    // Since this is called once per frame, this function does everything not related to moving the players such as getting inputs 
    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        AbombStillExist = GameObject.FindGameObjectWithTag("Bomb");

        //for firing bullets when player decides to shoot with mouse
        if (Input.GetMouseButtonDown(0) && OptionsMenu.MouseToShoot == true && !PauseMenu.GameIsPaused)
        {
            if (!StartShooting)
            {
                StartShooting = true;
                StartCoroutine(FireBulletConstantly(StartShooting));
            }
            else
            {
                StopAllCoroutines();
                StartShooting = false;
            }
        }
        //to fire bullets if keyboard used to shoot
        if(Input.GetKeyDown(KeyCode.O) && OptionsMenu.KeyBoardToShoot == true && !PauseMenu.GameIsPaused)
        {
            if (!StartShooting)
            {
                StartShooting = true;
                StartCoroutine(FireBulletConstantly(StartShooting));
            }
            else
            {
                StopAllCoroutines();
                StartShooting = false;
            }
        }
        
        //for activating bombs
        if(Input.GetMouseButtonDown(1) && GetStats.BombsUsed > 0 && OptionsMenu.MouseToShoot == true && !PauseMenu.GameIsPaused)
        {
            if (AbombStillExist == null)
            {
                Instantiate(bombPrefab, Firepoint.position, Firepoint.rotation);
                GetStats.BombsUsed--;
            }
        }
        if (Input.GetKeyDown(KeyCode.P) && GetStats.BombsUsed > 0 && OptionsMenu.KeyBoardToShoot == true && !PauseMenu.GameIsPaused)
        {
            if (AbombStillExist == null)
            {
                Instantiate(bombPrefab, Firepoint.position, Firepoint.rotation);
                GetStats.BombsUsed--;
            }
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }

    //Anything related to moving the player is put here to ensure the player moves smoothly.
    private void FixedUpdate()
    {
        transform.Translate(moveDirection*moveSpeed*Time.deltaTime, Space.World);
        
        
        //To rotate the player with respect to the position of the mouse in the screen
        if (OptionsMenu.MouseToShoot == true)
        {
            Vector2 aimDirection;
            aimDirection.x = mousePosition.x - transform.position.x;
            aimDirection.y = mousePosition.y - transform.position.y;
            Quaternion toRotation = Quaternion.LookRotation(transform.forward, aimDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * AngleSpeed);
        }
        else
        {
            //to ensure the position the sprite is facing is the same as last frame.
            if (moveDirection != Vector2.zero)
            {
                Quaternion LookTowards = Quaternion.LookRotation(transform.forward, moveDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, LookTowards, Time.deltaTime * AngleSpeed);
            }
        }
    }

    IEnumerator FireBulletConstantly(bool ShootingNow)
    {
        const float ShootingInterval = 0.1f;
        GetStats.PlayShootingSound();
        while (ShootingNow)
        {
            Instantiate(bulletPrefab, Firepoint.position, Firepoint.rotation);
            yield return new WaitForSeconds(ShootingInterval);
        }
        yield break;
    }
}
