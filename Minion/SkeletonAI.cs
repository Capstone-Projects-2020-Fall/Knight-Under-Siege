using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkeletonAI : MonoBehaviour
{
    public float speed;
    public float health;
    private Vector2 movement;
    private Rigidbody2D rb;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = new Vector2(-speed, 0);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Projectile"))
        {
            health--;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
