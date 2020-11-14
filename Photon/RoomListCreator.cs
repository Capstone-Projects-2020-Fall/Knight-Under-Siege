using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class RoomListCreator : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject room;
    [SerializeField]
    private GameObject content;
    
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (SceneManager.GetActiveScene().name == "Room Select")
        {
            MakeList(roomList);
        }
    }
    
    public void MakeList(List<RoomInfo> roomList)
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach(RoomInfo roomInfo in roomList)
        {
            Debug.Log("room found");
            if (roomInfo.IsOpen && roomInfo.PlayerCount > 0)
            {
                GameObject newRoom = (GameObject) Instantiate(room);
                RoomDetails roomDetails = newRoom.GetComponent(typeof(RoomDetails)) as RoomDetails;
                roomDetails.hostName.text = roomInfo.CustomProperties["Host"] as string;
                Debug.Log("Text: <" + roomDetails.hostName.text + ">");
                Debug.Log("roomInfo.CustomProperties[Host]: <" + roomInfo.CustomProperties["Host"] + ">");
                roomDetails.playerCount.text =  "" + roomInfo.PlayerCount + "/" + (int) roomInfo.MaxPlayers;
                roomDetails.roomId.text = roomInfo.Name;
                QuickStartLobbyController qslc = GameObject.FindGameObjectWithTag("Quick Start Lobby Controller").GetComponent(typeof(QuickStartLobbyController)) as QuickStartLobbyController;
                roomDetails.join.onClick.AddListener(() => qslc.JoinRoom(roomDetails.roomId));
                newRoom.transform.parent = content.transform;
            }
        }
    }
}
