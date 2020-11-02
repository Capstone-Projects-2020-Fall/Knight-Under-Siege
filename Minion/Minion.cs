using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Minion : MonoBehaviour
{

    GameObject[] targets;
    public float speed;
    public float health;
    public float attackRange;
    // Start is called before the first frame update
    void Start()
    {
        FindAllTargets();
    }

    // Update is called once per frame
    void Update()
    {

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
    /// Checks through the list of targets and picks one based off of priority.
    /// </summary>
    /// <returns>A reference to the priority target.</returns>
    public GameObject PickTarget()
    {
        //TODO: Introduce weighted priority system
        GameObject nearestTarget = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject target in targets)
        {
            if (target == null) continue;
            float currentDistance = Vector2.Distance(this.transform.position, target.transform.position);
            if (currentDistance < nearestDistance)
            {
                nearestTarget = target;
                nearestDistance = currentDistance;
            }
        }
        return nearestTarget;
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
    }

    /// <summary>
    /// Destroy this minion.
    /// </summary>
    public void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// A listener for gameobjects colliding with this minion.
    /// </summary>
    /// <param name="collision">A system-provided Collider2D object.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            health--;
            if (health <= 0)
            {
                Die();
            }
        }
    }
}
