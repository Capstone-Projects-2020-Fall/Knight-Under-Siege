using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{
    public float health;
    public float mana;
    private HeroHealth healthBar;
    private HeroHealth healthBarMini;
    private PhotonView pv;
    public bool FriendlyFire;


    private void Start()
    {
        pv = GetComponent<PhotonView>();
        healthBar = (HeroHealth)GameObject.Find("Health Bar").GetComponent("HeroHealth");
        healthBar.SetHealth(health);


    }
    [PunRPC]
    private void RPC_NecromancerWin()
    {
        SceneManager.LoadScene("Defeat");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (FriendlyFire)
        {
            Debug.Log("Friendly Fire on");
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Projectile"))
            {
                health--;
                healthBar.SetHealth(health);

                if (health <= 0)
                {
                    PhotonNetwork.Destroy(gameObject);
                    if (GameObject.FindGameObjectWithTag("Player") == null)
                    {
                        Debug.Log("necromancer wins");
                        //pv.RPC("RPC_NecromancerWin", RpcTarget.All);
                        if (SceneManager.GetActiveScene().name != "SingleplayerGame")
                        {
                            PlayerPrefs.SetInt("HeroesWin", 0);
                            PhotonNetwork.LoadLevel("EndScreen");
                        }
                    }
                    else
                    {
                        Debug.Log("why");
                    }
                
                }
            }
        }
        else
        {
            Debug.Log("Friendly Fire off");
            if (collision.gameObject.CompareTag("Enemy"))
            {
                health--;
                healthBar.SetHealth(health);

                if (health <= 0)
                {
                    PhotonNetwork.Destroy(gameObject);
                    if (GameObject.FindGameObjectWithTag("Player") == null)
                    {
                        Debug.Log("necromancer wins");
                        //pv.RPC("RPC_NecromancerWin", RpcTarget.All);
                        if (SceneManager.GetActiveScene().name != "SingleplayerGame")
                        {
                            PlayerPrefs.SetInt("HeroesWin", 0);
                            PhotonNetwork.LoadLevel("EndScreen");
                        }
                    }
                    else
                    {
                        Debug.Log(GameObject.FindGameObjectWithTag("Player").ToString());
                    }
                
                }
            }
        }
    }
}
