using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class MultiplayerMinion : MonoBehaviour
{
    GameObject[] targets;
    public float speed;
    public float health;
    public float attackRange;
    private KillCounter killCounter;

    private bool facingRight = true;
    private float prevX;

    /*Status Effects*/
    private bool frozen = false;
    

    public void Start()
    {
        prevX = transform.position.x;
        FindAllTargets();
        killCounter = GameObject.FindWithTag("Kill Counter").GetComponent<KillCounter>();
    }

    public void FixedUpdate()
    {
        if (frozen) return;

        GameObject currentTarget = PickTarget();
        if (currentTarget == null) return;
        float distanceToTarget = Vector2.Distance(this.transform.position, currentTarget.transform.position);

        if (distanceToTarget <= attackRange)
        {
            Attack();
        }
        else
        {
            MoveTo(currentTarget);
        }
    }


    /// <summary>
    /// The base attack for this minion. <b>You must provide an override in the child class.</b>
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// Finds all Player GameObjects within the scene then stores them in the targets array.
    /// </summary>
    public void FindAllTargets()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
    }


    /// <summary>
    /// Checks through the list of targets and picks one based off of priority with a lower priority being preferred.
    /// </summary>
    /// <returns>A reference to the priority target.</returns>
    public GameObject PickTarget()
    {
        //TODO: Introduce weighted priority system
        GameObject priorityTarget = null;
        float bestPriority = float.MaxValue;

        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            /*attack priority = distance to target adjusted by target's personal priority*/
            float currentPriority = target.GetComponent<Hero>().priority * Vector2.Distance(this.transform.position, target.transform.position);

            if (currentPriority < bestPriority)
            {
                priorityTarget = target;
                bestPriority = currentPriority;
            }
        }
        return priorityTarget;
    }

    /// <summary>
    /// Moves this minion towards the given target.
    /// </summary>
    /// <param name="target">A gameobject that this minion is targetting.</param>
    public void MoveTo(GameObject target)
    {
        //TODO: Introduce pathfinding
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);

        /*If we're facing right and going left OR facing left and going right then flip the character*/
        if ((facingRight && prevX > transform.position.x) || (!facingRight && prevX < transform.position.x)) flip();

        prevX = transform.position.x;
    }

    /// <summary>
    /// Reduces the minion's health by the specified amount of damage then kills them if their health is at or below 0.
    /// </summary>
    /// <param name="damage">The amount of damage done.</param>
    [PunRPC]
    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            killCounter.IncreaseKillCount();
            Die();
        }
    }

    /// <summary>
    /// Destroy this minion.
    /// </summary>
    public void Die()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    /// <summary>
    /// Adds the frozen condition to this minion and turns them blue until the duration has ended.
    /// </summary>
    /// <param name="freezeDuration">The time in seconds for the frozen effect to last.</param>
    IEnumerator Freeze(float freezeDuration)
    {
        frozen = true; //you have to let it go
        GetComponent<SpriteRenderer>().color = Color.blue; //da ba dee

        yield return new WaitForSeconds(freezeDuration);

        frozen = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Flip the character along the x-axis so they face the opposite direction.
    /// </summary>
    private void flip()
    {
        facingRight = !facingRight;
        Vector3 flippedScale = transform.localScale;
        flippedScale.x *= -1;
        transform.localScale = flippedScale;
    }

    /// <summary>
    /// A listener for gameobjects colliding with this minion.
    /// </summary>
    /// <param name="collision">A system-provided Collider2D object.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidingObject = collision.gameObject;
        
        if (collidingObject.CompareTag("Projectile")) takeDamage(collidingObject.GetComponent<Projectile>().damage);

        if (collidingObject.CompareTag("Blizzard"))
        {
            takeDamage(collidingObject.GetComponent<Blizzard>().damage);
            if (!frozen) Freeze(collidingObject.GetComponent<Blizzard>().freezeDuration);

        }
    }
}
