using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
public class SwitchScenes : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject playAgainButton;
    [SerializeField]
    private GameObject titleScreenButton;
    [SerializeField]
    private GameObject Map2Button;
    [SerializeField]
    private GameObject Map3Button;
    [SerializeField]
    private GameObject SettingsButton;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SingleplayerGame");
    }

    public void PlayAgainMultiplayer()
    {
        SceneManager.LoadScene("Room");
    }
    
    public void TitleScreen()
    {
        if (SceneManager.GetActiveScene().name == "Room Select")
        {
            PhotonNetwork.LeaveLobby();
        }
        else
        {
            SceneManager.LoadScene("TitleMenu");
        }
    }
    
    public override void OnLeftLobby()
    {
        SceneManager.LoadScene("TitleMenu");
    }

    public void Map2()
    {
        SceneManager.LoadScene("Map2");
        CreatePlayer();
    }

    public void Map3()
    {
        SceneManager.LoadScene("Map3");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
    
    public void RoomSelect()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        if (SceneManager.GetActiveScene().name == "Room" || SceneManager.GetActiveScene().name == "EndScreen")
        {
            PhotonNetwork.JoinLobby();
        }
    }
    
    public override void OnJoinedLobby()
    {
        if (SceneManager.GetActiveScene().name == "Room" || SceneManager.GetActiveScene().name == "EndScreen")
        {
            SceneManager.LoadScene("Room Select");
        }
    }

    private void CreatePlayer()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");

        Debug.Log(selectedCharacter);

        switch (selectedCharacter)
        {
            case 0:
                PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Archer Mini HP"), Vector3.zero, Quaternion.identity);
                break;
            case 1:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNecromancer"), Vector3.zero, Quaternion.identity);
                break;
            case 2:
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNecromancer"), Vector3.zero, Quaternion.identity);
                break;
        }
    }
}
