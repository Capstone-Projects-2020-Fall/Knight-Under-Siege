using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Blizzard : Projectile
{
    public float freezeDuration;
    public float radius;


    private void Start()
    {
        /*Default to these values if none have been set or given values don't make sense*/
        if (damage < 0) damage = 0.0f;
        if (TimeToLive <= 0) TimeToLive = 5.0f;
        if (radius <= 0) radius = 1.0f;

        GetComponent<CircleCollider2D>().radius = radius;
        
        Destroy(gameObject, TimeToLive);
    }
}
