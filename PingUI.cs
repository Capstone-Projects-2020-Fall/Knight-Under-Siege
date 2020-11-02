using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingUI : MonoBehaviour
{
    private Text pingText;

    private void Start()
    {
        pingText = this.GetComponent<Text>();
    }
    void FixedUpdate()
    {
        pingText.text = "" + PhotonNetwork.GetPing() + " ms";
    }
}
