using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KnightController))]
public class Knight : Hero
{
    [HideInInspector]
    public bool tauntActive = false;
    public float priorityMin;

    public float tauntBoost;
    public float tauntDuration;
    public GameObject tauntParticles;

    /// <summary>
    /// Adjust priority calculations to take the taunt boost into account for the duration
    /// </summary>
    public void activateTaunt()
    {
        tauntActive = true;
        GetComponent<PhotonView>().RPC("RPC_PlayTauntParticles", RpcTarget.All);
        
        GetComponent<Knight>().updatePriority();
        StartCoroutine("endTauntBoost");
    }

    /// <summary>
    /// Update the knight's priority while taking the effects of taunt into consideration
    /// </summary>
    public override void updatePriority()
    {   
        priority = 1 - (priorityRange * health / maxHealth);
        if (tauntActive) priority -= tauntBoost;
        if (priority < priorityMin) priority = priorityMin;
    }

    /// <summary>
    /// Removes the boost to priority given by taunt
    /// </summary>
    IEnumerator endTauntBoost()
    {
        yield return new WaitForSeconds(tauntDuration);

        tauntActive = false;
        GetComponent<PhotonView>().RPC("RPC_StopTauntParticles", RpcTarget.All);
        GetComponent<Knight>().updatePriority();
    }

    /// <summary>
    /// Play taunt particle effect across the network
    /// </summary>
    [PunRPC]
    public void RPC_PlayTauntParticles()
    {
        tauntParticles.GetComponent<ParticleSystem>().Play();
    }

    /// <summary>
    /// Stop taunt particle effect across the network
    /// </summary>
    [PunRPC]
    public void RPC_StopTauntParticles()
    {
        tauntParticles.GetComponent<ParticleSystem>().Stop();
    }
}
