using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class NecromancerController : MonoBehaviour
{
    public float speed;
    private PhotonView pv;
    private bool manaIncreasing;
    public float mana =100;
    private float skeletonCost;
    private float startTime;
    private bool AtSpawnPoint = false;
    private string SpawnName;
    private int SpawnPoint; 
    private Rigidbody2D rb;
    private NecromancerMana manaMiniBar;
    private Camera camera;
    private Vector2 velocity;
    private Vector2 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
        skeletonCost = 5;
        mana = 100;
        startTime = Time.time;
        manaIncreasing = true;
        //camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        manaMiniBar = (NecromancerMana)GameObject.Find("Mana Bar Mini").GetComponent("NecromancerMana");
        manaMiniBar.SetMana(mana);
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
            /*
            //movement.x = Input.GetAxisRaw("Horizontal");
            //movement.y = Input.GetAxisRaw("Vertical");

            movement.x = 0;
            movement.y = 0;

            //check for horizontal movement
            if(KeyBindingManager.GetKey(KeyAction.right) && !KeyBindingManager.GetKey(KeyAction.left))
            {
                movement.x = 1;
            }

            if(KeyBindingManager.GetKey(KeyAction.left) && !KeyBindingManager.GetKey(KeyAction.right))
            {
                movement.x = -1;
            }

            //check for vertical movement    
            if(KeyBindingManager.GetKey(KeyAction.up) && !KeyBindingManager.GetKey(KeyAction.down))
            {
                movement.y = 1;
            }

            if(KeyBindingManager.GetKey(KeyAction.down) && !KeyBindingManager.GetKey(KeyAction.up))
            {
                movement.y = -1;
            }*/

            //TODO: Replace this with UI button after spawn positions have been implemented
            if (KeyBindingManager.GetKeyDown(KeyAction.fire1))
            {
                //TODO: Update this so it can be preset instead of just reading the mouse position
                spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.zero);
                
                if(hit.collider == null){
                    AtSpawnPoint = false;
                }
                else if(hit.collider.tag == "Spawn Point"){
                    Debug.Log("Clicking spawn point");
                    SpawnSkeleton(spawnPosition);
                    AtSpawnPoint = true;
                }
            }

            if (manaIncreasing)
            {
                if (mana < 100)
                {
                    mana = Mathf.Min(mana + 3*(Time.time - startTime), 100);
                    startTime = Time.time;
                    manaMiniBar.SetMana(mana);
                }
                else
                {
                    manaIncreasing = false;
                }
            }
        }

    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        //camera.transform.Translate(movement * speed * Time.fixedDeltaTime);
    }

    //TODO: Add additional methods for all minions

    /// <summary>
    /// Spawns a new Skeleton minion at the given position.
    /// </summary>
    /// <param name="spawnPosition">A Vector2 position for the minion to be spawned.</param>
    private void SpawnSkeleton(Vector2 spawnPosition)
    {
        if (mana >= skeletonCost)
        {
            if(AtSpawnPoint){
                mana -= skeletonCost;
                //manaBar.SetMana(mana);
                startTime = Time.time;
                manaIncreasing = true;
                GameObject skeleton = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonSkeleton"), spawnPosition, Quaternion.identity);
                AtSpawnPoint = false;
            }
        }
    }
    /*private int GetSpawnLocation(){
        if(AtSpawnPoint){
            if(SpawnName == "Vent 1"){
                Debug.Log("In " + SpawnName);
                SpawnPoint = 0;
            }
            if(SpawnName == "Vent 2"){
                Debug.Log("In " + SpawnName);
                SpawnPoint = 1;
            }
            if(SpawnName == "Vent 3"){
                Debug.Log("In " + SpawnName);
                SpawnPoint = 2;
            }
            if(SpawnName == "Vent 4"){
                Debug.Log("In " + SpawnName);
                SpawnPoint = 3;
            }
            if(SpawnName == "Vent 5"){
                Debug.Log("In " + SpawnName);
                SpawnPoint = 4;
            }
            if(SpawnName == "Vent 6"){
                Debug.Log("In " + SpawnName);
                SpawnPoint = 5;
            }
            if(SpawnName == "Vent 7"){
                Debug.Log("In " + SpawnName);
                SpawnPoint = 6;
            }
            if(SpawnName == "Vent 8"){
                Debug.Log("In " + SpawnName);
                SpawnPoint = 7;
            }
        }
        return SpawnPoint;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Spawn Point"))
        {
            Debug.Log("Necromancer entered" + collider.name);
            SpawnName = collider.name;
            //AtSpawnPoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Spawn Point"))
        {
            Debug.Log("Necromancer exited " + collider.name);
            //AtSpawnPoint = false;
        }
    }*/
}
