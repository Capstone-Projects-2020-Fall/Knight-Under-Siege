using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomSettingsDisplay : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject setting;
    [SerializeField]
    private GameObject content;
    
    private Room room;
    
    // Start is called before the first frame update
    void Start()
    {
        room = PhotonNetwork.CurrentRoom;
        MakeList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        MakeList();
        /*foreach (DictionaryEntry pair in propertiesThatChanged)
        {
            if (pair.Key != "Host")
            {
                int i = 0;
                foreach (Transform child in content.transform)
                {
                    if (i != 0)
                    {
                        GameObject g = child.gameObject;
                        Text settingsText = g.GetComponent(typeof Text) as Text;

                        if (settingsText.text.StartsWith(pair.Key))
                        {
                            settingsText.text = 
                        }
                    }

                    i++;
                }
            }
        }*/
    }
    
    public void MakeList()
    {
        int i = 0;
        foreach (Transform child in content.transform)
        {
            if (i != 0)
            {
                GameObject.Destroy(child.gameObject);
            }
            i++;
        }

        ExitGames.Client.Photon.Hashtable ht = room.CustomProperties;
        foreach (DictionaryEntry pair in ht)
        {
            if (!(pair.Key.ToString().Contains("Host") || pair.Value.ToString() == "Room"))
            {
                GameObject newSetting = (GameObject) Instantiate(setting);
                DisplayedSetting displayedSetting = newSetting.GetComponent(typeof(DisplayedSetting)) as DisplayedSetting;
                if (pair.Key.ToString() == "Timer Length")
                {
                    float minutes = Mathf.FloorToInt((int) pair.Value / 60);
                    float seconds = Mathf.FloorToInt((int) pair.Value % 60);
                    
                    displayedSetting.settingName.text = "Timer Length: " + string.Format("{0:0}:{1:00}", minutes, seconds);
                }
                else if(pair.Key.ToString() == "Friendly Fire")
                {
                    if ((bool) pair.Value)
                    {
                        displayedSetting.settingName.text = "Friendly Fire: On";
                    }
                    else
                    {
                        displayedSetting.settingName.text = "Friendly Fire: Off";
                    }
                }
                else if (pair.Key.ToString() == "Player Health")
                {
                    displayedSetting.settingName.text = "" + pair.Key + ": " + pair.Value.ToString();
                }
                else if (pair.Key.ToString() == "Minion Speed")
                {
                    displayedSetting.settingName.text = "" + pair.Key + ": " + pair.Value.ToString() + "x";
                }
                else
                {
                    Debug.Log("" + pair.Key + " is not accounted for.");
                    //displayedSetting.settingName.text = "" + pair.Key + ": " + pair.Value.ToString();
                }
                newSetting.transform.SetParent(content.transform, false);
            }
        }
    }
}
