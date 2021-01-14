using UnityEngine;
using System.Collections;

public class Monster_Bullet1 : Bullet
{
    public float maxDistance;  // max distance of this bullet can reach

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();  // call the start() in the base class

        Move(this.transform.position, direction, maxDistance);  // start moving this bullet (coroutine)
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    /// <summary>
    /// This function will be called automatically when the bullet is colliding with any collider
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            if (collision.gameObject.GetComponent<Character>().isInvincible) {
                Debug.Log("player dodged");
                return;
            }
        }
        if (collision.gameObject.tag != "Monster")
        {
            stop();  // stop the motion of bullet when hits something (monster, ground, force field, etc)
        }
    }

}
