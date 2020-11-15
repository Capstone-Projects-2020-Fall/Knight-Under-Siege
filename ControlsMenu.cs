using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

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
                setActiveTrue();
            }
            else
            {
                setActiveFalse();
                if (SceneManager.GetActiveScene().name == "Room")
                {
                    Room room = PhotonNetwork.CurrentRoom;
                    room.MaxPlayers = (byte) Convert.ToInt32(room.CustomProperties["Max Players"]);
                    int i = 0;
                    foreach (KeyValuePair<int, Player> playerInfo in room.Players)
                    {
                        if (i >= (int) room.MaxPlayers)
                        {
                            Debug.Log("Player should be kicked out.");
                            PhotonNetwork.CloseConnection(playerInfo.Value);
                        }

                        i++;
                    }
                }
            }
        }
    }

    public void setActiveFalse()
    {
        Panel.gameObject.SetActive(false);
        active = false;
    }

    public void setActiveTrue()
    {
        Panel.gameObject.SetActive(true);
        active = true;
    }

    
}
