using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject quickStartButton;
    [SerializeField]
    private GameObject quickCancelButton;
    [SerializeField]
    private int roomSize;
    [SerializeField]
    private Text playerName;


    void Start()
    {
        if (SceneManager.GetActiveScene().name == "TitleMenu")
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                quickStartButton.SetActive(true);
            }
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartButton.SetActive(true);
    }
    public void StartSingleplayer()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SingleplayerGame");
    }
    /*QuickStart button*/
    public void QuickStart()
    {
        SceneManager.LoadScene("Room Select");
    }

    public override void OnJoinedLobby()
    {
        //SceneManager.LoadScene("Room Select");
    }
    
    /*QuickStart button test*/
    /*public void QuickStartTest()
    {
        quickStartButton.SetActive(false);
        quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick start");
    }*/

    /*QuickCancel button*/
    public void QuickCancel()
    {
        quickCancelButton.SetActive(false);
        quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room");
        CreateRoom();
    }

    public void CreateRoom()
    {
        if (playerName.text != "")
        {
            Debug.Log("Creating room");
        
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
            ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
            if (!string.IsNullOrWhiteSpace(playerName.text))
            {
                ht.Add("Host", playerName.text);
            }
            else
            {
                ht.Add("Host", "Blank Name");
            }
            Debug.Log("Host: " + (ht["Host"] as string));
            
            ht.Add("Timer Length", 600);
            Debug.Log("Timer Length: " + (ht["Timer Length"].ToString()));
            
            ht.Add("Friendly Fire", false);
            Debug.Log("Friendly Fire: " + (ht["Friendly Fire"].ToString()));
            
            roomOps.CustomRoomProperties = ht;

            string[] hostName = new string[] {"Host"};
            roomOps.CustomRoomPropertiesForLobby = hostName;
        
            int randomRoomNumber = Random.Range(0, 10000);
            Debug.Log(randomRoomNumber);
            PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        }
        else
        {
            Debug.Log("Name needs to be entered.");
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("It's your room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room");
        CreateRoom();
    }

    public void JoinRoom(Text roomId)
    {
        InputField playerName = GameObject.FindGameObjectWithTag("Player Name").GetComponent(typeof(InputField)) as InputField;
        if (playerName.text != "")
        {
            Debug.Log("Trying to join " + roomId.text);
            PhotonNetwork.JoinRoom(roomId.text);
        }
        else
        {
            Debug.Log("Name needs to be entered.");
        }
    }

    public override void OnJoinedRoom()
    {
        if (SceneManager.GetActiveScene().name == "Room Select")
        {
            Debug.Log("Joined a real room");
            if (!string.IsNullOrWhiteSpace(playerName.text))
            {
                PhotonNetwork.NickName = playerName.text;
            }
            else
            {
                PhotonNetwork.NickName = "Blank Name";
            }
            SceneManager.LoadScene("Room");
        }
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room");
        SceneManager.LoadScene("Room Select");
    }
}
