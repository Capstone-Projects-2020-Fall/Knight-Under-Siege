using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.UI;


[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
public class SpawnPoint : MonoBehaviour {

    public PhotonView pv;

    public float repeatTime;
    public float currentMana;
    public float maxMana;
    public float manaRecoveredPerSecond;

    public GameObject[] SpawnLocations;
    public string[] Minions;

    public float skeletonCost;
    public float goblinCost;
    public float flyingEyeCost;

    private float startTime;

    private int randSpawn;
    private int randMinionType;
    private float[] minionCosts;

    public bool on;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        minionCosts = new float[]{skeletonCost, goblinCost, flyingEyeCost};
        startTime = Time.time;
        InvokeRepeating("Spawn", 2f, repeatTime);
    }

    void Update()
    {
        if (currentMana < maxMana) RecoverMana();
        startTime = Time.time;
    }

    /// <summary>
    /// Increase mana based off of time passed.
    /// </summary>
    public void RecoverMana()
    {
            currentMana += manaRecoveredPerSecond * (Time.time - startTime);
            if (currentMana > maxMana) currentMana = maxMana;
    }
 
/// <summary>
/// Spawns a skeleton at a random location chosen from the SpawnLocations array as long as the game is unpaused.
/// </summary>
    void Spawn()
    {
        if (on)
        {
            randSpawn = Random.Range(0, 8);
            randMinionType = Random.Range(0, 3);

            if(Time.timeScale > 0 && currentMana >= minionCosts[randMinionType])
            {
                currentMana -= minionCosts[randMinionType];
                //Debug.Log("Current Mana is " + currentMana);
                startTime = Time.time;
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", Minions[randMinionType]), SpawnLocations[randSpawn].transform.position, Quaternion.identity);
            }
        }
    }

    [PunRPC]
    private void RPC_NecromancerWin()
    {
        Debug.Log("necromancer win method called");
        PlayerPrefs.SetInt("HeroesWin", 0);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("EndScreen");
        }
    }
    
    [PunRPC]
    private void RPC_HeroWin()
    {
        Debug.Log("hero win method called");
        PlayerPrefs.SetInt("HeroesWin", 1);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("EndScreen");
        }
    }
}
