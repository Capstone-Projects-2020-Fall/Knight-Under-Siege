using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;

    public void NextCharacters()
    //Takes an array filled with all our character so we can do more than just the planned four
    //Then uses mod so while the player is cycling through it doesn't create an oout of bounds issue
    {
        //Makes the character selected to not being visable in the scence
        characters[selectedCharacter].SetActive(false);

        //Moves to the next charcter in the array
        selectedCharacter = (selectedCharacter + 1) % characters.Length;

        //Makes the charcter selected visable in the scene
        characters[selectedCharacter].SetActive(true);
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
    }
}
