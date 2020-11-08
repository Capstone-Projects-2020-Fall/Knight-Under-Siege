using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSkeleton : MultiplayerMinion
{
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
