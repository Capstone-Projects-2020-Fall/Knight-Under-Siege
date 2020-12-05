using UnityEngine;
using Pathfinding;
using Photon.Pun;
using System;
using System.Collections;

public class AISkeleton : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public Transform firePoint;
    public GameObject slashPrefab;
    [HideInInspector]public PhotonView pv;
    [HideInInspector]public Animator animator;
    private bool facingRight = true;
    [HideInInspector] public float prevX, prevY;
    private GameObject[] targets;
    [HideInInspector] public bool frozen = false, dead = false;
    public float health;
    private KillCounter killCounter;
    private GameObject currentTarget;

    //The distance we want to stop moving toward the target
    public float stoppingDistance;

    //The distance we want to run away from the target
    public float retreatDistance;

    //The rate of fire
    private float timeBetweenShots;

    //The time it takes to reload
    public float startTimeBetweenShots;

    //Target
    private Transform target;

    //How close the A.I. needs to be before it moves to the next way point
    public float nextWaypointDistance = 3f;

    //Current path that we are following
    Path path;

    // Current waypoint along the path it's on
    int currentWaypoint = 0;

    //if we reached the end of the path
    bool reachedEndPath = false;
    Seeker seeker;


    // Start is called before the first frame update
    void Start()
    {
        if (frozen || dead) return;

       

        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        prevX = transform.position.x;
        prevY = transform.position.y;
        FindAllTargets();
        killCounter = GameObject.FindWithTag("Kill Counter").GetComponent<KillCounter>();


        timeBetweenShots = startTimeBetweenShots;

        //Updates the path to follow and target different enemies, functions called, time to wait to call, then the repeat rate
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

        if (frozen || dead) return;
        
        try
        {
            currentTarget = PickTarget();
            target = currentTarget.transform;
            //target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (NullReferenceException)
        {
            reachedEndPath = true;
            return;
        }
        //Checks to see if there's a path
        if (path == null)
            return;

        //If we reached the goal then stop moving. 
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndPath = true;
            return;
        }
        else
        {
            reachedEndPath = false;
        }

        //Direction to next waypoint along the path, position of current waypoint - current position, normalized makes the length 1
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        //Movement force
        Vector2 force = direction * speed * Time.deltaTime;

        // Flippy
        animator.SetFloat("Speed", Mathf.Abs(prevX - transform.position.x) + Mathf.Abs(prevY - transform.position.y));
        if ((facingRight && prevX > transform.position.x) || (!facingRight && prevX < transform.position.x)) flip();

        // If the A.I. detects an enemy further than the stoppingDistance they move toward the enemny
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            //Actually movement
            rb.AddForce(force);
        }
        // If the A.I. detects an enemy within the stopping distance, and outside the retreat the A.I. will stop moving
        else if (Vector2.Distance(transform.position, target.position) < stoppingDistance && (Vector2.Distance(transform.position, target.position) > retreatDistance))
        {
            transform.position = this.transform.position;
        }

        //Same as the stop moving, but adds in an increment for shooting, so it's not too fast and not across the map
        if ((timeBetweenShots <= 0) && Vector2.Distance(transform.position, target.position) < stoppingDistance)
        {
            Attack();

            // Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }


        //Distance to next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        //If it's within range of the end of the current waypoint then move onto the next waypoint
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    //Path generation
    void UpdatePath()
    {
        //Prevents it from updating all the time
        if (seeker.IsDone())
            // Starts a new path, using current position, targets position, and the function called
            try
            {
                if (target.position == null)
                {
                    return;
                }
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
            catch (NullReferenceException)
            {
                return;
            }
    }

    //Path generation
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            //Current path equals P
            path = p;
            //Resets progress on path back to zero for the next path
            currentWaypoint = 0;
        }
    }

    //Attack stuff

    public void Attack()
    {
        pv.RPC("RPC_Attack", RpcTarget.All, firePoint.position);
    }

    /// <summary>
    /// Performs a melee attack at the given position.
    /// </summary>
    /// <param name="attackPosition">A Vector3 of the position the attack should occur.</param>
    [PunRPC]
    private void RPC_Attack(Vector3 attackPosition)
    {
        animator.SetTrigger("Attack");
        Instantiate(slashPrefab, attackPosition, Quaternion.identity);
    }

    // Flip stuff
    private void flip()
    {
        facingRight = !facingRight;
        Vector3 flippedScale = transform.localScale;
        flippedScale.x *= -1;
        transform.localScale = flippedScale;
    }

    public void FindAllTargets()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
    }

    
    public GameObject PickTarget()
    {
        //TODO: Introduce weighted priority system
        GameObject priorityTarget = null;
        float bestPriority = float.MaxValue;

        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            //attack priority = distance to target adjusted by target's personal priority
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
    /// Reduces the minion's health by the specified amount of damage then kills them if their health is at or below 0.
    /// </summary>
    /// <param name="damage">The amount of damage done.</param>
    [PunRPC]
    public void RPC_takeDamage(float damage)
    {
        float h = health;
        health -= damage;
        if (pv.IsMine)
        {
            animator.SetTrigger("Hurt");
            if (health <= 0)
            {
                Die();
            }
        }
        if (health <= 0 && h > 0)
        {
            killCounter.IncreaseKillCount();
        }
    }

    /// <summary>
    /// Set the minion as dead then start the coroutine to handle death animation and destroy.
    /// </summary>
    public void Die()
    {
        dead = true;
        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine("_Die");
    }

    /// <summary>
    /// Play the minion's death animation then destroy it after the animation is complete.
    /// </summary>
    private IEnumerator _Die()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        PhotonNetwork.Destroy(animator.gameObject);
    }

    /// <summary>
    /// A listener for gameobjects colliding with this minion.
    /// </summary>
    /// <param name="collision">A system-provided Collider2D object.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dead) return;
        GameObject collidingObject = collision.gameObject;

        //Debug.Log("minion got hit");

        if (collidingObject.CompareTag("Projectile"))
        {
            pv.RPC("RPC_takeDamage", RpcTarget.All, collidingObject.GetComponent<Projectile>().damage);
            //takeDamage(collidingObject.GetComponent<Projectile>().damage);
        }
        else if (collidingObject.CompareTag("Blizzard"))
        {
            pv.RPC("RPC_takeDamage", RpcTarget.All, collidingObject.GetComponent<Blizzard>().damage);
            if (!frozen) StartCoroutine("Freeze", collidingObject.GetComponent<Blizzard>().freezeDuration);
        }
    }

    /// <summary>
    /// Adds the frozen condition to this minion and turns them blue until the duration has ended.
    /// </summary>
    /// <param name="freezeDuration">The time in seconds for the frozen effect to last.</param>
    //[PunRPC]
    private IEnumerator Freeze(float freezeDuration)
    {
        frozen = true; //you have to let it go
        GetComponent<SpriteRenderer>().color = Color.cyan;
        animator.speed = 0;

        yield return new WaitForSeconds(freezeDuration);

        frozen = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        animator.speed = 1;
    }

}
