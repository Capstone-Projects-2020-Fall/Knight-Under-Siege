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
    private float cdTripleShot = 0f;
    public Transform firePointCenter, firePointTop, firePointBottom;
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
            cdTripleShot +=Time.deltaTime;

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
                if (KeyBindingManager.GetKeyDown(KeyAction.fire2) && Time.timeScale > 0 && cdTripleShot > 1f )
                {
                    pv.RPC("RPC_TripleShot", RpcTarget.All, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    cdTripleShot = 0;
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
        GameObject arrow = Instantiate(projectile, firePointCenter.position, Quaternion.Euler(0f, 0f, angle));
        arrow.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
    }

    /// <summary>
    /// Creates and fires three arrows towards the given position for all players on the network.
    /// </summary>
    /// <param name="clickPosition">A Vector3 corresponding with the mouse position.</param>
    [PunRPC]
    void RPC_TripleShot(Vector3 clickPosition)
    {
        Vector2 directionCenter = new Vector2(0,0);
        Vector2 directionTop = new Vector2(0,0);
        Vector2 directionBottom = new Vector2(0,0);

        directionCenter = clickPosition - firePointCenter.transform.position;
        directionTop = clickPosition - firePointCenter.transform.position;
        directionBottom = clickPosition - firePointCenter.transform.position;
    
        directionCenter.Normalize();
        directionTop.Normalize();
        directionBottom.Normalize();
 

        //Shooting up or down when x = 0
        if((directionCenter.y == -1f) && (directionCenter.x == 0f)){
            directionTop = new Vector2(directionCenter.x + 0.2f, (directionCenter.y));
            directionBottom = new Vector2(directionCenter.x - 0.2f, (directionCenter.y));
        }
        //Shooting left or right when y = 0
        else if((directionCenter.y == 1f) && (directionCenter.x == 0f)){
            directionTop = new Vector2(directionCenter.x + 0.2f, (directionCenter.y));
            directionBottom = new Vector2(directionCenter.x - 0.2f, (directionCenter.y));
        }
        //Shooting towards the right side
        else if(directionCenter.x > 0f){
            //Shooting upward
            if(directionCenter.y > 0f){
                if((directionCenter.y <= 0.8f) && (directionCenter.y >= 0.2f)){
                    directionTop = new Vector2(directionCenter.x - 0.1f, (directionCenter.y + 0.1f));
                    directionBottom = new Vector2(directionCenter.x + 0.1f, (directionCenter.y - 0.1f));
                }
                if((directionCenter.y < 0.2f) && directionCenter.y > 0f){
                    directionTop = new Vector2(directionCenter.x, (directionCenter.y + 0.2f));
                    directionBottom = new Vector2(directionCenter.x, (directionCenter.y - 0.2f));
                }
                if((directionCenter.y > 0.8f) && (directionCenter.y < 1f)){
                    directionTop = new Vector2(directionCenter.x - 0.2f, (directionCenter.y  + 0.1f));
                    directionBottom = new Vector2(directionCenter.x + 0.2f, (directionCenter.y - 0.1f));
                }
            }
            //Shooting downward
            if(directionCenter.y < 0f){
                if((directionCenter.y >= -0.8f) && (directionCenter.y <= -0.2f)){
                    directionTop = new Vector2(directionCenter.x + 0.1f, (directionCenter.y + 0.1f));
                    directionBottom = new Vector2(directionCenter.x - 0.1f, (directionCenter.y - 0.1f));
                }
                if((directionCenter.y > -0.2f) && (directionCenter.y < 0f)){
                    directionTop = new Vector2(directionCenter.x, (directionCenter.y + 0.2f));
                    directionBottom = new Vector2(directionCenter.x, (directionCenter.y - 0.2f));
                }
                if((directionCenter.y < -0.8f) && (directionCenter.y > -1f)){
                    directionTop = new Vector2(directionCenter.x + 0.2f, (directionCenter.y + 0.1f));
                    directionBottom = new Vector2(directionCenter.x - 0.2f, (directionCenter.y - 0.1f));
                }
            }
        }  
        //Shooting towards the left side
        else if(directionCenter.x < 0f){
            //Shooting upward
            if(directionCenter.y > 0f){
                if((directionCenter.y <= 0.8f) && (directionCenter.y >= 0.2f)){
                    directionBottom = new Vector2(directionCenter.x + 0.1f, (directionCenter.y + 0.1f));
                    directionTop = new Vector2(directionCenter.x - 0.1f, (directionCenter.y - 0.1f));
                }
                if((directionCenter.y < 0.2f) && directionCenter.y > 0f){
                    directionTop = new Vector2(directionCenter.x, (directionCenter.y - 0.2f));
                    directionBottom = new Vector2(directionCenter.x, (directionCenter.y + 0.2f));
                }
                if((directionCenter.y > 0.8f) && (directionCenter.y < 1f)){
                    directionTop = new Vector2(directionCenter.x - 0.2f, (directionCenter.y  + 0.1f));
                    directionBottom = new Vector2(directionCenter.x + 0.2f, (directionCenter.y - 0.1f));
                }
            }
            //Shooting downward
            if(directionCenter.y < 0f){
                if((directionCenter.y >= -0.8f) && (directionCenter.y <= -0.2f)){
                    directionBottom = new Vector2(directionCenter.x - 0.1f, (directionCenter.y + 0.1f));
                    directionTop = new Vector2(directionCenter.x + 0.1f, (directionCenter.y - 0.1f));
                }
                if((directionCenter.y > -0.2f) && (directionCenter.y < 0f)){
                    directionTop = new Vector2(directionCenter.x, (directionCenter.y - 0.2f));
                    directionBottom = new Vector2(directionCenter.x, (directionCenter.y + 0.2f));
                }
                if((directionCenter.y < -0.8f) && (directionCenter.y > -1f)){
                    directionTop = new Vector2(directionCenter.x + 0.2f, (directionCenter.y + 0.1f));
                    directionBottom = new Vector2(directionCenter.x - 0.2f, (directionCenter.y - 0.1f));
                }
            }
        }

        /*
        Debug.Log("Direction Center is: " + directionCenter);
        Debug.Log("Direction Top is: " + directionTop);
        Debug.Log("Direction Bottom is: " + directionBottom);
        */

        float angleCenter = Mathf.Atan2(directionCenter.y, directionCenter.x) * Mathf.Rad2Deg;
        float angleTop = Mathf.Atan2(directionTop.y, directionTop.x) * Mathf.Rad2Deg + 20;
        float angleBottom = Mathf.Atan2(directionBottom.y, directionBottom.x) * Mathf.Rad2Deg - 20;
        
        GameObject arrowCenter = Instantiate(projectile, firePointCenter.position, Quaternion.Euler(0f, 0f, angleCenter));
        GameObject arrowTop = Instantiate(projectile, firePointTop.position, Quaternion.Euler(0f, 0f, angleTop));
        GameObject arrowBottom = Instantiate(projectile, firePointBottom.position, Quaternion.Euler(0f, 0f, angleBottom));
        
        arrowCenter.GetComponent<Rigidbody2D>().velocity = directionCenter * projectileForce;
        arrowTop.GetComponent<Rigidbody2D>().velocity = directionTop * projectileForce;
        arrowBottom.GetComponent<Rigidbody2D>().velocity = directionBottom * projectileForce;
    }
    
}