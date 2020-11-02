using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SinglePlayerNecromancer : MonoBehaviour
{
    public float speed = 5f;
    public float mana = 100f;
    private bool AtSpawnPoint = false;
    private string SpawnName;
    private Rigidbody2D rb;
    private Vector2 movement;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if ((Input.GetButtonDown("Fire1") && Time.timeScale > 0))
        { 
            //Spawn minions
        }

        if (Input.GetButtonDown("Pause"))
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
       
    }
    public bool GetSpawnPoint(){
        return AtSpawnPoint;
    }

    public string GetSpawnName(){
        return SpawnName;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Necromancer collided with Player");
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Necromancer collided with Player");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Spawn Point"))
        {
            Debug.Log("Necromancer entered" + collider.name);
            SpawnName = collider.name;
            AtSpawnPoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Spawn Point"))
        {
            Debug.Log("Necromancer exited " + collider.name);
            AtSpawnPoint = false;
        }
    }
}