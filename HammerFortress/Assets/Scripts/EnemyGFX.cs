using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Assets.Scripts;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aipath;
    bool isFacingRight = true;   

    // Update is called once per frame
    void Update()
    {
        bool wasFacingRight = isFacingRight;

        if (aipath.desiredVelocity.x > 0 && !isFacingRight)
        {
            Flip();            
        }
        else if (aipath.desiredVelocity.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        SpriteUtilities.Flip(transform);
    }
}
