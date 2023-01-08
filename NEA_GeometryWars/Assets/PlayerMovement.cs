using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform Firepoint;
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
    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0) && OptionsMenu.MouseToShoot == true)
        {
            ToPlaySFX.PlayShootingSound();
            Instantiate(bulletPrefab, Firepoint.position, Firepoint.rotation);
        }

        if(Input.GetKeyDown(KeyCode.O) && OptionsMenu.KeyBoardToShoot == true)
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
            Quaternion LookTowards = Quaternion.LookRotation(transform.forward, moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, LookTowards, Time.deltaTime * AngleSpeed);
        }
    }
}
