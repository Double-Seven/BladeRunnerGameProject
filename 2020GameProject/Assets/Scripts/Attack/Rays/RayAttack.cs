using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RayAttack : Attack
{
    public LineRenderer lineRenderer;

    protected void Shoot(string targetName)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction);
        lineRenderer.SetPosition(0, transform.position);
        if (hitInfo)
        {
            Character character = hitInfo.transform.GetComponent<Character>();

            if (character != null && character.gameObject.name.Contains(targetName))
            {
                // hit target
                lineRenderer.SetPosition(1, hitInfo.point);
            } else
            {
                lineRenderer.SetPosition(1, transform.position + direction * 1000);
            }
            Debug.Log("ray hit");
            
            
        } else
        {
            lineRenderer.SetPosition(1, transform.position + direction * 1000);
        }

    }
}