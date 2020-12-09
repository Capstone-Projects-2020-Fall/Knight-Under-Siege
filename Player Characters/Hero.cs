using Photon.Pun;
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
    private HeroHealth healthBar;
    private HeroHealth miniHealthBar;
    public PhotonView pv;



    [HideInInspector]
    public float priority = 1.0f;
    public float priorityRange;

    [HideInInspector]
    public bool FriendlyFire;


    public void Start()
    {
        pv = GetComponent<PhotonView>();
        healthBar = (HeroHealth)GameObject.Find("Health Bar").GetComponent("HeroHealth");
        miniHealthBar = (HeroHealth)GameObject.Find("Mini Health Bar").GetComponent("HeroHealth");

        health = Convert.ToSingle(PhotonNetwork.CurrentRoom.CustomProperties["Player Health"]);

        healthBar.SetStartingHealth(health);
        miniHealthBar.SetStartingHealth(health);
        maxHealth = health;

    }

    /// <summary>
    /// Adjust the characters health according to the amount of damage taken
    /// </summary>
    /// <param name="damage">The amount of damage being dealt</param>
    public void takeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        miniHealthBar.SetHealth(health);

        if (health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                GameObject necromancer = GameObject.FindGameObjectWithTag("Necromancer");
                NecromancerController nc = necromancer.GetComponent(typeof(NecromancerController)) as NecromancerController;
                PhotonView nv = nc.pv;
                Debug.Log("necromancer wins");
                nv.RPC("RPC_NecromancerWin", RpcTarget.All);
            }
            else
            {
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
        if (collision.gameObject.CompareTag("Enemy Attack") || (FriendlyFire && collision.gameObject.CompareTag("Projectile")))
        {
            if (collision.gameObject.CompareTag("Enemy Attack")) {
                takeDamage(collision.gameObject.GetComponent<EnemyDamage>().damage);
            }
            else
            {
                takeDamage(collision.gameObject.GetComponent<Projectile>().damage);
            }
        }
    }
}
