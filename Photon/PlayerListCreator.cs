using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListCreator : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject content;
    
    private Room room;
    
    // Start is called before the first frame update
    void Start()
    {
        room = PhotonNetwork.CurrentRoom;
        this.MakeList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        this.MakeList();
    }
    
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        this.MakeList();
        Debug.Log("If this message comes before OnMasterClientSwitched, new Master Client text might need to be blue.");
    }

    public override void OnMasterClientSwitched(Player newPlayer)
    {
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Host", newPlayer.NickName);
        room.SetCustomProperties(ht);
        
        Debug.Log("If this message comes first, we're probably fine.");
    }

    public void MakeList()
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Dictionary<int, Player> pi = room.Players;
        foreach (KeyValuePair<int, Player> playerInfo in room.Players)
        {
            GameObject newPlayer = (GameObject) Instantiate(player);
            PlayerDetails playerDetails = newPlayer.GetComponent(typeof(PlayerDetails)) as PlayerDetails;
            playerDetails.name.text = playerInfo.Value.NickName;
            playerDetails.id = playerInfo.Key;
            playerDetails.player = playerInfo.Value;
            if (playerInfo.Value.IsMasterClient)
            {
                playerDetails.name.color = Color.blue;
            }
            newPlayer.transform.parent = content.transform;
        }
    }
}
