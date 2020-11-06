using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class KillCounter : MonoBehaviour
{  
    private int totalKillCounter = 0;
    [SerializeField]
    private Text killCountText;
    
    // Start is called before the first frame update
    void Start()
    {
        totalKillCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayKillCount(totalKillCounter);
    }

    public void IncreaseKillCount(){
        totalKillCounter++;
    }

    void DisplayKillCount(float killCounter)
    {
        killCountText.text = string.Format("Kill Count: {0}", killCounter);
    }
}