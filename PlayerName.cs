using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerName : MonoBehaviour
{

    [SerializeField] 
    private InputField playerName;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Room Select")
        {
            playerName.text = PlayerPrefs.GetString("Name");
            PhotonNetwork.NickName = PlayerPrefs.GetString("Name");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleanInput()
    {
        Debug.Log("Name changed.");
        playerName.text = Regex.Replace(playerName.text, @"[^a-zA-Z0-9 ]", "");
        PhotonNetwork.NickName = playerName.text;
        PlayerPrefs.SetString("Name", playerName.text);
    }
}
