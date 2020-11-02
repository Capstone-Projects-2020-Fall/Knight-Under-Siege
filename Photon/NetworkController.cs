using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    /*
     * 
     * Documentation: https://doc.photonengine.com/en-us/pun/current/getting-started/pun-intro
     * Scripting API: https://doc-api.photonengine.com/en/pun/v2/index.html
     * 
    */

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "TitleMenu")
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to " + PhotonNetwork.CloudRegion + " server");
        //base.OnConnectedToMaster();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (SceneManager.GetActiveScene().name != "TitleMenu")
        {
            Debug.Log("You disconnected.");
            SceneManager.LoadScene("TitleMenu");
        }
    }
    
    void Update()
    {
        
    }

}
