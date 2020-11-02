using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class SinglePlayerArcherController : MonoBehaviour
{
    public float speed = 5f;
    //public float health = 5f;
    public float projectileForce = 20f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public GameObject firePoint;
    public GameObject projectilePrefab;
    public Transform shootDirection1, shootDirection2, shootDirection3;
    
    private bool facingRight = true;
    private int shootingDirection = 1;

    private float cdVolley = 0f;
    private float cdShoot = 0f;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        cdShoot +=Time.deltaTime;
        cdVolley +=Time.deltaTime;

        movement.x = 0;
        movement.y = 0;

        //check for horizontal movement
        if(KeyBindingManager.GetKey(KeyAction.right) && !KeyBindingManager.GetKey(KeyAction.left))
        {
            movement.x = 1;
        }

        if(KeyBindingManager.GetKey(KeyAction.left) && !KeyBindingManager.GetKey(KeyAction.right))
        {
            movement.x = -1;
        }

        //check for vertical movement    
        if(KeyBindingManager.GetKey(KeyAction.up) && !KeyBindingManager.GetKey(KeyAction.down))
        {
            movement.y = 1;
        }
        if(KeyBindingManager.GetKey(KeyAction.down) && !KeyBindingManager.GetKey(KeyAction.up))
        {
            movement.y = -1;
        }


        if ((KeyBindingManager.GetKeyDown(KeyAction.fire1) && Time.timeScale > 0) && cdShoot > 0.25f)
        { 
            ShootProjectile();
            cdShoot = 0f;
        }

        if (KeyBindingManager.GetKeyDown(KeyAction.fire2) && Time.timeScale > 0 && cdVolley > 1f )
        { 
            ArrowVolley();
            cdVolley = 0f;
        }

        if (KeyBindingManager.GetKeyDown(KeyAction.pause))
        {
            if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
            } else {
                Time.timeScale = 0;
            }
        }
    }

    void FixedUpdate()
    {
       rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
       

        //If's to flip the archer facing left and right
        if (KeyBindingManager.GetKey(KeyAction.right) && facingRight == false)
        {
            FlipLeftRight();
        }
        if (KeyBindingManager.GetKey(KeyAction.left) && facingRight == true){
            FlipLeftRight();
        }


        //If's to control the direction the firePoint is facing
        if (KeyBindingManager.GetKey(KeyAction.right) && shootingDirection != 1)
        {
            FlipTopBottom(1);
        }
        if (KeyBindingManager.GetKey(KeyAction.left) && shootingDirection != 2){
            FlipTopBottom(2);
        }
        if (KeyBindingManager.GetKey(KeyAction.up) && shootingDirection != 3)
        {
            FlipTopBottom(3);
        }
        if (KeyBindingManager.GetKey(KeyAction.down) && shootingDirection != 4)
        {
            FlipTopBottom(4);
        }
    }

    void FlipLeftRight()
    {
        facingRight = !facingRight;
        transform.Rotate(0f,180f,0f);
    }

    void FlipTopBottom(int directionfacing)
    {
        switch (directionfacing)
        {
            case 1:
                firePoint.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                shootingDirection = 1;
                break;
            case 2:
                firePoint.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                shootingDirection = 2;
                break;
            case 3:
                firePoint.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                shootingDirection = 3;
                break;
            case 4:
                firePoint.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                shootingDirection = 4;
                break;
        }
    }

    
    void ShootProjectile()
    {
        GameObject arrow = Instantiate(projectilePrefab, firePoint.transform.position, firePoint.transform.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.transform.right * projectileForce, ForceMode2D.Impulse);
    }

    void ArrowVolley()
    {

        Instantiate (projectilePrefab, shootDirection1.position, shootDirection1.rotation);
        Instantiate (projectilePrefab, shootDirection2.position, shootDirection2.rotation);
        Instantiate (projectilePrefab, shootDirection3.position, shootDirection3.rotation);
        //GameObject arrow = Instantiate(projectilePrefab, firePoint.transform.position, firePoint.transform.rotation);
        //Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        //rb.AddForce(firePoint.transform.right * projectileForce, ForceMode2D.Impulse);
    }
}
