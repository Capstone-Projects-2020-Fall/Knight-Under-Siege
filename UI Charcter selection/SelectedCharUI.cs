using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SelectedCharUI : MonoBehaviourPunCallbacks
{
    public GameObject[] characters;
    public int selectedCharacter = 0;
    public GameObject playerObject;
    private PlayerDetails playerDetails;
    private Room room;

    public void Start()
    {
        playerDetails = playerObject.GetComponent(typeof(PlayerDetails)) as PlayerDetails;
        characters[selectedCharacter].SetActive(true);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer.ActorNumber == playerDetails.id)
        {
            int c = Convert.ToInt32(changedProps["Character Number"]);
            if (c != selectedCharacter)
            {
                characters[selectedCharacter].SetActive(false);
                selectedCharacter = c;
                characters[selectedCharacter].SetActive(true);
            }
        }
    }

    public void NextCharacters()
        //Takes an array filled with all our character so we can do more than just the planned four
        //Then uses mod so while the player is cycling through it doesn't create an oout of bounds issue
    {
        //Makes the character selected to not being visable in the scene
        characters[selectedCharacter].SetActive(false);

        //Moves to the next charcter in the array
        selectedCharacter = (selectedCharacter + 1) % characters.Length;

        //Makes the charcter selected visable in the scene
        characters[selectedCharacter].SetActive(true);

        //Sets the int and saves it so it can be used in other scenes. 
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    }

    public void PreviousCharacter()
        //Same thing as the next char, but has the loop go the other way
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    }
}