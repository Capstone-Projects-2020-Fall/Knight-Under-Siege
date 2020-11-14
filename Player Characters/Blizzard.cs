using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Blizzard : Projectile
{
    public float freezeDuration;
    public float radius;


    public void Start()
    {
        /*Default to these values if none have been set or given values don't make sense*/

        if (TimeToLive <= 0) TimeToLive = 5.0f;
        if (radius <= 0) radius = 0.5f;

        GetComponent<CircleCollider2D>().radius = radius;
        
        Destroy(gameObject, TimeToLive);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        /*Intentionally left blank*/
    }
}
