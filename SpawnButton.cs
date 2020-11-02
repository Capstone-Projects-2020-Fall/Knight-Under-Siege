using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour {

    public float mana = 100f;
    private float skeletonCost;
    private bool manaIncreasing;
    private float startTime;
    private string SpawnName;
    private int SpawnPoint;
    private NecromancerMana manaBar;
    private NecromancerMana manaMiniBar;
    public GameObject Skelly;
    public GameObject[] SpawnLocations;
    public SinglePlayerNecromancer Necromancer;
    
    private void Start()
    {
        skeletonCost = 15f;  
        startTime = Time.time;
        manaIncreasing = true;

        //manaBar = (NecromancerMana)GameObject.Find("Mana Bar").GetComponent("NecromancerMana");
        //manaBar.SetMana(mana);

        manaMiniBar = (NecromancerMana)GameObject.Find("Mana Bar Mini").GetComponent("NecromancerMana");
        manaMiniBar.SetMana(mana);
    }

    void Update()
    {
        if (manaIncreasing)
        {
            if (mana < 100)
            {
                mana = Mathf.Min(mana + (Time.time - startTime), 100);
                startTime = Time.time;
//                manaBar.SetMana(mana);
                manaMiniBar.SetMana(mana);

            }
            else
            {
                manaIncreasing = false;
            }
        }

    }
    private int GetSpawnLocation(){
        if(Necromancer.GetSpawnPoint()){
            SpawnName = Necromancer.GetSpawnName();

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

    public void SpawnSkeleton()
    {
        if (mana >= skeletonCost)
        {
            if(Necromancer.GetSpawnPoint()){
                Debug.Log(Necromancer.GetSpawnPoint());
                mana -= skeletonCost;
                //manaBar.SetMana(mana);
                manaMiniBar.SetMana(mana);
                manaIncreasing=true;
                Debug.Log("Spawning skeleton");
                Instantiate(Skelly, SpawnLocations[GetSpawnLocation()].transform.position, Quaternion.identity);
            }
        }
    }
}