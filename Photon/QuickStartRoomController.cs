using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex;
    [SerializeField]
    private GameObject changeRoomSettingsButton;
    [SerializeField]
    private GameObject startButton;
    public LayerMask enemyLayers;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        if (SceneManager.GetActiveScene().name == "Room" && PhotonNetwork.IsMasterClient)
        {
            changeRoomSettingsButton.SetActive(true);
            startButton.SetActive(true);
        }
    }
    
    public override void OnMasterClientSwitched(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            changeRoomSettingsButton.SetActive(true);
            startButton.SetActive(true);
        }
        else
        {
            changeRoomSettingsButton.SetActive(false);
            startButton.SetActive(false);
        }
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        if (SceneManager.GetActiveScene().name == "TitleMenu")
        {
            Debug.Log("Joined room");
            StartGame();
        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting game");
            if (SceneManager.GetActiveScene().name == "Room")
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
            //since we set autosyncscene to true in QuickStartLobbyController other players should automatically join
        }
    }
}
