using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class sword : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<PhotonView>().IsMine) return;
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            PhotonNetwork.Destroy(gameObject);

        }
    }
}
