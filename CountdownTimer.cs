using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CountdownTimer : MonoBehaviour
{  
    private bool timerIsRunning = false;
    private float startTime = 600;
    private float timeRemaining;
    [SerializeField]
    private Text countdownText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            Debug.Log("Timer Length = <" + PhotonNetwork.CurrentRoom.CustomProperties["Timer Length"].ToString() + ">");
            startTime = Convert.ToSingle(PhotonNetwork.CurrentRoom.CustomProperties["Timer Length"]);
        }
        timerIsRunning =  true;
        timeRemaining = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerIsRunning)
        {
            if(timeRemaining > 0)
            {
                DisplayTime(timeRemaining);
                timeRemaining -= Time.deltaTime; 
            }
            else{
                timeRemaining = 0;
                timerIsRunning = false;
                if (SceneManager.GetActiveScene().name != "SingleplayerGame")
                {
                    GameObject necromancer = GameObject.FindGameObjectWithTag("Necromancer");
                    NecromancerController nc = necromancer.GetComponent(typeof(NecromancerController)) as NecromancerController;
                    PhotonView nv = nc.pv;
                    Debug.Log("heroes win");
                    nv.RPC("RPC_HeroWin", RpcTarget.All);
                }
                else
                {
                    SceneManager.LoadScene("Victory");
                }
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
