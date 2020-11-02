using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ControlsMenu : MonoBehaviour
{
    public GameObject Panel;

    bool active;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Room")
        {
            Panel.gameObject.SetActive(false);
        }
        active = false;
    }

    public void showHidePanel()
    {
        if (SceneManager.GetActiveScene().name != "Room" || PhotonNetwork.IsMasterClient)
        {
            if(active == false)
            {
                Panel.gameObject.SetActive(true);
                active = true;
            }
            else
            {
                Panel.gameObject.SetActive(false);
                active = false;
            }
        }
    }

    
}
