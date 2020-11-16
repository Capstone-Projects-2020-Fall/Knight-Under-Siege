using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class MapSelectionUI : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;

    public void Start()
    {
        QuickStartRoomController.multiplayerSceneIndex = 1;
        characters[selectedCharacter].SetActive(true);
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

        if(selectedCharacter == 0)
        {
            QuickStartRoomController.multiplayerSceneIndex = 1;
        }
        if(selectedCharacter == 1)
        {
            QuickStartRoomController.multiplayerSceneIndex = 5;
        }
        if(selectedCharacter == 2)
        {
            QuickStartRoomController.multiplayerSceneIndex = 6;
        }
        if(selectedCharacter == 3)
        {
            QuickStartRoomController.multiplayerSceneIndex = 12;
        }
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

        if(selectedCharacter == 0)
        {
            QuickStartRoomController.multiplayerSceneIndex = 1;
        }
        if(selectedCharacter == 1)
        {
            QuickStartRoomController.multiplayerSceneIndex = 5;
        }
        if(selectedCharacter == 2)
        {
            QuickStartRoomController.multiplayerSceneIndex = 6;
        }
        if(selectedCharacter == 3)
        {
            QuickStartRoomController.multiplayerSceneIndex = 12;
        }
    }
}
