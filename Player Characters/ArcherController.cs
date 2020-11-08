using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(BoxCollider2D))]
public class ArcherController : MonoBehaviour
{
    public float speed;
    public float projectileForce = 20f;
    public Transform firePoint;
    public GameObject projectile;
    public GameObject pauseMenu;
    private GameObject clonePause;

    private Rigidbody2D rb;
    private PhotonView pv;
    private Vector2 velocity;
    private bool paused;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            Vector2 input = new Vector2(0,0);
            input.x = 0;
            input.y = 0;

            if (!paused)
            {
                //check for horizontal movement
                if (KeyBindingManager.GetKey(KeyAction.right) && !KeyBindingManager.GetKey(KeyAction.left))
                {
                    input.x = 1;
                }

                if (KeyBindingManager.GetKey(KeyAction.left) && !KeyBindingManager.GetKey(KeyAction.right))
                {
                    input.x = -1;
                }

                //check for vertical movement    
                if (KeyBindingManager.GetKey(KeyAction.up) && !KeyBindingManager.GetKey(KeyAction.down))
                {
                    input.y = 1;
                }

                if (KeyBindingManager.GetKey(KeyAction.down) && !KeyBindingManager.GetKey(KeyAction.up))
                {
                    input.y = -1;
                }

                if (KeyBindingManager.GetKeyDown(KeyAction.fire1) && Time.timeScale > 0)
                {
                    pv.RPC("RPC_ShootProjectile", RpcTarget.All, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
            }

            velocity = input.normalized * speed;

            if (KeyBindingManager.GetKeyDown(KeyAction.pause))
            {
                if (paused)
                {
                    DestroyImmediate(clonePause, true);
                    paused = false;
                }
                else
                {
                    clonePause = Instantiate(pauseMenu);
                    paused = true;
                }
            }

            
        }

    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Creates and fires an arrow towards the given position for all players on the network.
    /// </summary>
    /// <param name="clickPosition">A Vector3 corresponding with the mouse position.</param>
    [PunRPC]
    void RPC_ShootProjectile(Vector3 clickPosition)
    {
        Vector2 direction = clickPosition - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject arrow = Instantiate(projectile, firePoint.position, Quaternion.Euler(0f, 0f, angle));
        arrow.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
    }
}