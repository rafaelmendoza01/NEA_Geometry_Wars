using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform Firepoint;
    public float radius;
    private float AngleSpeed = 900f;
    public int KillHistory = 0;
    private RandomSpawner ToPlaySFX;

    Vector2 moveDirection;
    Vector2 mousePosition;
    private void Start()
    {
        ToPlaySFX = GameObject.FindObjectOfType<RandomSpawner>();
    }

    // Since this is called once per frame, this function does everything not related to moving the players such as getting inputs 
    void Update()
    {

        radius = GetComponent<CircleCollider2D>().radius;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0) && OptionsMenu.MouseToShoot == true)
        {
            ToPlaySFX.PlayShootingSound();
            Instantiate(bulletPrefab, Firepoint.position, Firepoint.rotation);
        }

        if(Input.GetKeyDown(KeyCode.P) && OptionsMenu.KeyBoardToShoot == true)
        {
            ToPlaySFX.PlayShootingSound();
            Instantiate(bulletPrefab, Firepoint.position, Firepoint.rotation);
        }

       

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        
    }

    //Anything related to moving the player is put here to ensure the 
    private void FixedUpdate()
    {
        transform.Translate(moveDirection*moveSpeed*Time.deltaTime);
        Vector2 aimDirection;
        aimDirection.x = mousePosition.x - transform.position.x;
        aimDirection.y = mousePosition.y - transform.position.y;

        //To rotate the player with respect to the position of the mouse in the screen
        Quaternion toRotation = Quaternion.LookRotation(transform.forward, aimDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime*AngleSpeed);
    }
}
