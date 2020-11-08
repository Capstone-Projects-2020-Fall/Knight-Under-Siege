using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float TimeToLive;
    public float damage;

    private void Start()
    {
        /*Default to these values if none have been set or given values don't make sense*/
        if (damage <= 0) damage = 1.0f;
        if (TimeToLive <= 0) TimeToLive = 5.0f;
        /*Cleanup for better memory management*/
        Destroy(gameObject, TimeToLive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: Include check for friendly fire
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
