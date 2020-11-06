using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


[RequireComponent(typeof(PhotonView))]
public class NecromancerController : MonoBehaviour
{
    public float speed;
    private PhotonView pv;
    private bool manaIncreasing;
    public float mana;
    public float skeletonCost;
    private float startTime;
    private bool AtSpawnPoint = false;
    private string SpawnName;
    private int SpawnPoint; 

    private NecromancerMana manaMiniBar;
    private Camera camera;
    private Vector2 velocity;
    private Vector2 spawnPosition;
    public GameObject spawnMarker;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        skeletonCost = 2;
        mana = 100;
        startTime = Time.time;
        manaIncreasing = true;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        manaMiniBar = (NecromancerMana)GameObject.Find("Mana Bar Mini").GetComponent("NecromancerMana");
        manaMiniBar.SetMana(mana);
        markSpawnLocations();
        
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
                    mana = Mathf.Min(mana + 4*(Time.time - startTime), 100);
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
        camera.transform.Translate(velocity * speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Adds a small particle effect for the necromancer to mark where spawn locations are
    /// </summary>
    private void markSpawnLocations()
    {
        GameObject[] spawnLocations = GameObject.FindGameObjectsWithTag("Spawn Point");
        foreach (GameObject spawn in spawnLocations)
        {
            Instantiate(spawnMarker, spawn.transform);
        }
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
}
