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
    private string[] playerHealthOptions;
    private string[] minionSpeedOptions;

    private int timerIndex;
    private int friendlyFireIndex;
    private int playerHealthIndex;
    private int minionSpeedIndex;
    
    private Room room;

    // Start is called before the first frame update
    void Start()
    {
        room = PhotonNetwork.CurrentRoom;
        
        valueText = value.GetComponent(typeof(Text)) as Text;
        
        timerOptions = new string[] {"10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "1:00", "2:00", "3:00", "4:00", "5:00", "6:00", "7:00", "8:00", "9:00"};
        friendlyFireOptions = new string[] {"Off", "On"};
        playerHealthOptions = new string[] {"5", "6", "7", "8", "9", "10", "1", "2", "3", "4"};
        minionSpeedOptions = new string[] {"1x", "1.25x", "1.5x", "1.75x", "2x", "0.25x", "0.5x", "0.75x"};

        if (arrayNumber == 0)
        {
            timerIndex = Math.Max(Array.IndexOf(timerOptions, valueText.text), 0);
        }
        else if(arrayNumber == 1)
        {
            friendlyFireIndex = Math.Max(Array.IndexOf(friendlyFireOptions, valueText.text), 0);
        }
        else if (arrayNumber == 2)
        {
            timerIndex = Math.Max(Array.IndexOf(playerHealthOptions, valueText.text), 0);
        }
        else
        {
            minionSpeedIndex = Math.Max(Array.IndexOf(minionSpeedOptions, valueText.text), 0);
        }
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
    
    private void changePlayerHealthSettings()
    {
        float health = Convert.ToSingle(valueText.text);
        
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Player Health", health);

        room.SetCustomProperties(ht);
    }
    
    private void changeMinionSpeedSettings()
    {
        float speed = Convert.ToSingle(valueText.text.Substring(0, valueText.text.Length - 1));
        
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Minion Speed", speed);

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
        else if(arrayNumber == 1)
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
        else if (arrayNumber == 2)
        {
            if (playerHealthIndex == playerHealthOptions.Length - 1)
            {
                playerHealthIndex = 0;
            }
            else
            {
                playerHealthIndex++;
            }

            valueText.text = playerHealthOptions[playerHealthIndex];
            changePlayerHealthSettings();
        }
        else //if (arrayNumber == 3)
        {
            if (minionSpeedIndex == minionSpeedOptions.Length - 1)
            {
                minionSpeedIndex = 0;
            }
            else
            {
                minionSpeedIndex++;
            }

            valueText.text = minionSpeedOptions[minionSpeedIndex];
            changeMinionSpeedSettings();
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
        else if (arrayNumber == 1)
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
        else if (arrayNumber == 2)
        {
            if (playerHealthIndex == 0)
            {
                playerHealthIndex = playerHealthOptions.Length - 1;
            }
            else
            {
                playerHealthIndex--;
            }

            valueText.text = playerHealthOptions[playerHealthIndex];
            changePlayerHealthSettings();
        }
        else //if (arrayNumber == 3)
        {
            if (minionSpeedIndex == 0)
            {
                minionSpeedIndex = minionSpeedOptions.Length - 1;
            }
            else
            {
                minionSpeedIndex--;
            }

            valueText.text = minionSpeedOptions[minionSpeedIndex];
            changeMinionSpeedSettings();
        }
    }
}
