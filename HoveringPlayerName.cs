using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class HoveringPlayerName : MonoBehaviour
{
    [SerializeField] 
    public Text name;

    void Start(){
        name.text = PhotonNetwork.NickName;
    }
}