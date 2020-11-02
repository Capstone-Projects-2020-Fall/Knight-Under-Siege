using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject victory;
    [SerializeField]
    private GameObject defeat;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("HeroesWin") == 1)
        {
            if (PlayerPrefs.GetInt("selectedCharacter") != 3)
            {
                victory.SetActive(true);
            }
            else
            {
                defeat.SetActive(true);
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("selectedCharacter") != 3)
            {
                defeat.SetActive(true);
            }
            else
            {
                victory.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
