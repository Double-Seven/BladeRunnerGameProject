using System.Collections;
using UnityEngine;


public class ForceField : Attack
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            // blow monster away
            //Debug.Log("blow away by force field");
        }
    }

}
