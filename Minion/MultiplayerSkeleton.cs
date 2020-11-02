using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSkeleton : MultiplayerMinion
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Attack()
    {
        //TODO: Provide attack behavior.
    }

    public void Die()
    {
        base.Die();
        //TODO: Handle animations
    }
}
