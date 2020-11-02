using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class EditRoomSetting : MonoBehaviour
{
    [SerializeField]
    private int arrayNumber;
    [SerializeField]
    private GameObject value;

    private Text valueText;

    private string[] timerOptions;
    private string[] friendlyFireOptions;

    private int timerIndex;
    private int friendlyFireIndex;
    
    private Room room;

    // Start is called before the first frame update
    void Start()
    {
        room = PhotonNetwork.CurrentRoom;
        
        valueText = value.GetComponent(typeof(Text)) as Text;
        
        timerOptions = new string[] {"10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "1:00", "2:00", "3:00", "4:00", "5:00", "6:00", "7:00", "8:00", "9:00"};
        friendlyFireOptions = new string[] {"Off", "On"};

        if (arrayNumber == 0)
        {
            timerIndex = Math.Max(Array.IndexOf(timerOptions, valueText.text), 0);
        }
        else
        {
            friendlyFireIndex = Math.Max(Array.IndexOf(friendlyFireOptions, valueText.text), 0);
        }
        
        timerIndex = 0;
        friendlyFireIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void changeTimerSettings()
    {
        int colonIndex = valueText.text.IndexOf(":");
        int minutes = Convert.ToInt32(valueText.text.Substring(0, colonIndex));
        int seconds = Convert.ToInt32(valueText.text.Substring(colonIndex + 1));

        int time = minutes * 60 + seconds;
        
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Timer Length", time);

        room.SetCustomProperties(ht);
    }
    
    private void changeFriendlyFireSettings()
    {
        bool b = valueText.text == "On";
        
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Friendly Fire", b);

        room.SetCustomProperties(ht);
    }

    public void Plus()
    {
        if (arrayNumber == 0)
        {
            if (timerIndex == timerOptions.Length - 1)
            {
                timerIndex = 0;
            }
            else
            {
                timerIndex++;
            }

            valueText.text = timerOptions[timerIndex];
            changeTimerSettings();
        }
        else
        {
            if (friendlyFireIndex == friendlyFireOptions.Length - 1)
            {
                friendlyFireIndex = 0;
            }
            else
            {
                friendlyFireIndex++;
            }

            valueText.text = friendlyFireOptions[friendlyFireIndex];
            changeFriendlyFireSettings();
        }
    }
    
    public void Minus()
    {
        if (arrayNumber == 0)
        {
            if (timerIndex == 0)
            {
                timerIndex = timerOptions.Length - 1;
            }
            else
            {
                timerIndex--;
            }
            
            valueText.text = timerOptions[timerIndex];
            changeTimerSettings();
        }
        else
        {
            if (friendlyFireIndex == 0)
            {
                friendlyFireIndex = friendlyFireOptions.Length - 1;
            }
            else
            {
                friendlyFireIndex--;
            }

            valueText.text = friendlyFireOptions[friendlyFireIndex];
            changeFriendlyFireSettings();
        }
    }
}
