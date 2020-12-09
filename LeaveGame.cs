using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LeaveGame : MonoBehaviour
{
    
    public void Disconnect()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("TitleMenu");
    }
}
