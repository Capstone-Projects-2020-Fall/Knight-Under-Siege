using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public GameObject Skelly;
    public float repeatTime = 3f;
    public GameObject[] SpawnLocations;
    private int rand;

    private void Start()
    {
        InvokeRepeating("Spawn", 2f, repeatTime);
    }
 
/// <summary>
/// Spawns a skeleton at a random location chosen from the SpawnLocations array as long as the game is unpaused.
/// </summary>
    void Spawn()
    {
        if(Time.timeScale > 0)
        {
            rand = Random.Range(0, SpawnLocations.Length);
            Instantiate(Skelly, SpawnLocations[rand].transform.position, Quaternion.identity);
        }
    }
}
