using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayAttack : RayAttack
{

    private void Start()
    {
       //  GameFlowManager.instance.getPlayer().CameraParent.ShakeCamera(0.5f, 0.005f);
        Shoot("Monster");
        Destroy(gameObject, 0.1f);
    }

    override protected void OnTriggerEnter2D(Collider2D collision)
    {

    }
}

