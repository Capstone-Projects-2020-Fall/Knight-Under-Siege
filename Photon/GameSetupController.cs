using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class GameSetupController : MonoBehaviour
{
    public bool FriendlyFire;
    public GameObject hero;
    
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        if (SceneManager.GetActiveScene().name != "SingleplayerGame")
        {
            FriendlyFire = (PhotonNetwork.CurrentRoom.CustomProperties["Friendly Fire"].ToString() == "True");
            Debug.Log("Friendly Fire = <" + FriendlyFire + ">");
        }
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating player");


        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");

        switch (selectedCharacter)
        {
            case 0:
                hero = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonArcher"), Vector3.zero, Quaternion.identity);
                break;
            case 1:
                hero = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonWizard"), Vector3.zero, Quaternion.identity);
                break;
            case 2:
                hero = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonKnight"), Vector3.zero, Quaternion.identity);
                break;
            case 3:
                hero = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNecromancer"), Vector3.zero, Quaternion.identity);
                break;
        }
        try
        {
            if (FriendlyFire)
            {
                Hero heroScript = hero.GetComponent(typeof(Hero)) as Hero;
                heroScript.FriendlyFire = true;
            }
            else
            {
                Hero heroScript = hero.GetComponent(typeof(Hero)) as Hero;
                heroScript.FriendlyFire = false;
            }
        }
        catch
        {
            Debug.Log("Unable to set FriendlyFire. Is this the necromancer?");
        }


        /*
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNecromancer"), Vector3.zero, Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine(""), Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonArcher"), Vector3.zero, Quaternion.identity);
        }
        */
        
    }
}
