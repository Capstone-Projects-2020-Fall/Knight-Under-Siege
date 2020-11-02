using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class KnightController : MonoBehaviour
{
    public float speed;
    public float health;
    public float projectileForce = 20f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public Animator animator;

    private Rigidbody2D rb;
    private PhotonView pv;
    private Vector2 velocity;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            Vector2 input = new Vector2(0,0);
            input.x = 0;
            input.y = 0;

            //check for horizontal movement
            if(KeyBindingManager.GetKey(KeyAction.right) && !KeyBindingManager.GetKey(KeyAction.left))
            {
                input.x = 1;
            }

            if(KeyBindingManager.GetKey(KeyAction.left) && !KeyBindingManager.GetKey(KeyAction.right))
            {
                input.x = -1;
            }

            //check for vertical movement    
            if(KeyBindingManager.GetKey(KeyAction.up) && !KeyBindingManager.GetKey(KeyAction.down))
            {
                input.y = 1;
            }

            if(KeyBindingManager.GetKey(KeyAction.down) && !KeyBindingManager.GetKey(KeyAction.up))
            {
                input.y = -1;
            }

            velocity = input.normalized * speed;
            if (KeyBindingManager.GetKeyDown(KeyAction.fire1) && Time.timeScale > 0) SwingSword();
        }
    }
    private void FixedUpdate()
    {
        
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void SwingSword()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit" + enemy.name);
        }


        /*
        GameObject sword = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonSword"), firePoint.position, firePoint.rotation);
        Rigidbody2D rb = sword.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
        */
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<PhotonView>().IsMine) return;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health--;
            Debug.Log("ow");
            if (health <= 0)
            {
                PhotonNetwork.Destroy(collision.gameObject);
            }
        }
    }
}
