using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{
    public float health;
    [HideInInspector]
    public float maxHealth;
    public float mana;
    public GameObject mhb;
    private HeroHealth healthBar;
    private HeroHealth miniHealthBar;
    public PhotonView pv;
    public GameObject deadMenu;
    public GameObject hoveringPlayerName;
    public int playerNumber;


    [HideInInspector]
    public float priority = 1.0f;
    public float priorityRange;
    
    public bool FriendlyFire;


    public void Start()
    {
        pv = GetComponent<PhotonView>();
        //healthBar = (HeroHealth)GameObject.Find("Health Bar").GetComponent("HeroHealth");
        miniHealthBar = mhb.GetComponent<HeroHealth>();

        maxHealth = Convert.ToSingle(PhotonNetwork.CurrentRoom.CustomProperties["Player Health"]);
        health = maxHealth;

        if (pv.IsMine)
        {
            pv.RPC("RPC_SetHoveringName", RpcTarget.All, PhotonNetwork.NickName);
            pv.RPC("RPC_SetPlayerNumber", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        FriendlyFire = (PhotonNetwork.CurrentRoom.CustomProperties["Friendly Fire"].ToString() == "True");

        //healthBar.SetStartingHealth(maxHealth);
        miniHealthBar.SetStartingHealth(maxHealth);
        

    }

    /// <summary>
    /// Adjust the characters health according to the amount of damage taken
    /// </summary>
    /// <param name="damage">The amount of damage being dealt</param>
    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        health -= damage;
        //healthBar.SetHealth(health);
        miniHealthBar.SetHealth(health);

        if (health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                if (GameObject.FindGameObjectWithTag("Necromancer") != null){
                    GameObject necromancer = GameObject.FindGameObjectWithTag("Necromancer");
                    NecromancerController nc = necromancer.GetComponent(typeof(NecromancerController)) as NecromancerController;
                    PhotonView nv = nc.pv;
                    Debug.Log("necromancer wins");
                    nv.RPC("RPC_NecromancerWin", RpcTarget.All);
                }
                else if(GameObject.FindGameObjectWithTag("NecromancerAI") != null){
                    GameObject necromancerAI = GameObject.FindGameObjectWithTag("NecromancerAI");
                    SpawnPoint ncAI = necromancerAI.GetComponent(typeof(SpawnPoint)) as SpawnPoint;
                    PhotonView nvAI = ncAI.pv;
                    Debug.Log("necromancer wins");
                    nvAI.RPC("RPC_NecromancerWin", RpcTarget.All);
                }  
                
            }
            else
            {
                if(pv.IsMine){
                    Instantiate(deadMenu);
                }
                Debug.Log("Players still remaining");
            }

        }
    }

    /// <summary>
    /// Update the character's target priority
    /// </summary>
    public virtual void updatePriority()
    {
        priority = 1 - (priorityRange * health / maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pv.IsMine)
        {
            if (collision.gameObject.CompareTag("Enemy Attack") || (FriendlyFire && collision.gameObject.CompareTag("Projectile")) || (FriendlyFire && collision.gameObject.CompareTag("Blizzard")))
            {
                if (collision.gameObject.CompareTag("Enemy Attack")) {
                    pv.RPC("RPC_TakeDamage", RpcTarget.All, collision.gameObject.GetComponent<EnemyDamage>().damage);
                }
                else if(collision.gameObject.CompareTag("Projectile"))
                {
                    Debug.Log("Hit by projectile");
                    if (collision.gameObject.GetComponent<Projectile>().owner != playerNumber)
                    {
                        Debug.Log("Owner " + collision.gameObject.GetComponent<Projectile>().owner + " != Recepient " + playerNumber);
                        pv.RPC("RPC_TakeDamage", RpcTarget.All, collision.gameObject.GetComponent<Projectile>().damage);
                    }
                    else
                    {
                        Debug.Log("Owner " + collision.gameObject.GetComponent<Projectile>().owner + " = Recepient " + playerNumber);
                    }
                }
                else
                {
                    Debug.Log("Hit by Blizzard");
                    if (collision.gameObject.GetComponent<Blizzard>().owner != playerNumber)
                    {
                        pv.RPC("RPC_TakeDamage", RpcTarget.All, collision.gameObject.GetComponent<Blizzard>().damage);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void RPC_SetHoveringName(string name)
    {
        (hoveringPlayerName.GetComponent(typeof(HoveringPlayerName)) as HoveringPlayerName).name.text = name;
    }
    
    [PunRPC]
    private void RPC_SetPlayerNumber(int number)
    {
        playerNumber = number;
    }
}
