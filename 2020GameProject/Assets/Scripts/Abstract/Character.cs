using UnityEngine;
using System.Collections;


// An abstract class for characters
// will be inherited by the player and monsters
public abstract class Character : MonoBehaviour
{
    protected float healthPoint { get; set; }  // HP of this character

    public CapsuleCollider2D capsCollider;      //The collider component attached to this object(the bullet game object which has a script that derives this abstract class).
    public Rigidbody2D thisRB { get; set; }                //The Rigidbody2D component attached to this object(the bullet game object which has a script that derives this abstract class).
    public bool isFacingRight { get; set; } = true;  // For determining which way the player is currently facing.
    public bool isGrounded { get; set; } = true;          // Whether or not the player is grounded.
	public bool isDead { get; set; } = false;  // bool to store whether the player is dead (will be checked by GameFlowManager)
	public float fade = 1f; // death Dissolve effect
	Material material;
    public bool isInvincible = false;
    // Use this for initialization
    //Protected, virtual functions can be overridden by inheriting classes.
    protected virtual void Start()
    {
        //Get a component reference to this object's CapsuleCollider
        capsCollider = GetComponent<CapsuleCollider2D>();

        //Get a component reference to this object's Rigidbody2D
        thisRB = GetComponent<Rigidbody2D>();
        material = GetComponent<SpriteRenderer>().material;

    }

    /// <summary>
    /// Function to decrease the HP of character
    /// </summary>
    /// <param name="damage"></param>
    protected void getAttacked(int damage)
    {
        if (!isInvincible)
            this.healthPoint -= damage;
    }

    protected void checkDie() {
        // check for destroying condition
		if (checkHP() && !isDead)
		{
			isDead = true;
		}

		if (isDead && fade >= 0f) {
			fade -= Time.fixedDeltaTime;
		}

        if (isDead && fade < 0.1f) {
            // a hack to fix IsDying animation event bugs for PlayerMonster
            Destroy(gameObject);
        }

        if (material != null)
		    material.SetFloat("_Fade", fade);
    }


    /// <summary>
    /// Function to check the current HP of this character and perform dying action if necessary
    /// </summary>
    /// <returns>True if the HP <= 0</returns>
    public bool checkHP()
    {
        if (this.healthPoint <= 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Function of moving to be overriden by Player class
    /// </summary>
    /// <param name="move"></param>
    /// <param name="crouch"></param>
    /// <param name="jump"></param>
    public abstract void Move(float move, bool crouch, bool jump);

    /// <summary>
    /// Function of moving to be overriden by Monster class
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="speed"></param>
    public abstract void Move(Vector3 destination, float speed);

    /// <summary>
    /// The trigger function for collider of this game object
    /// </summary>
    /// <param name="collision"></param>
    protected abstract void OnTriggerEnter2D(Collider2D collision);



    protected void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
