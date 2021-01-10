using UnityEngine;
using System.Collections;

public class PlayerMonsterAttackController : AttackController
{
    [Header("Shooting values")]
    public Transform muzzlePoint;  // the muzzle point of weapon

    public Monster character;
    private Player player;
    private float spawnRange = 0.1f;  // the vertical spawan range for bullets (to add some randomness to the bullets spawning position)

    private IEnumerator coroutine; // the attack coroutine
    Vector3 direction;

    // Use this for initialization
    protected override void Start()
    {
        // get the player gameObject from the game flow manager
        player = GameFlowManager.instance.getPlayer();

        // get identical attacks from Player (need to refactor Attack target selection)
        // this.attacks = player.GetComponent<PlayerAttackController>().attacks;
        this.currentAttack = this.attacks[this.attackSelected];
    }

    // Update is called once per frame
    protected override void Update()
    {
        //fire(player);

    }

    /// <summary>
    /// Function to read input and parameters from the model to execute an attack from list of attacks
    /// </summary>
    protected override void spawnAttack()
    {

    }

    /*
    public void attack(GameObject target, float cooldown)
    {
        // start fire co-routine
        this.coroutine = Fire(target, cooldown);
        StartCoroutine(coroutine);
    }*/


    /// <summary>
    /// Function to handle the attack action of monster toward the target gameObject
    /// </summary>
    /// <param name="target"></param>
    /// <param name="cooldown"> The shooting cooldown between bullets</param>
    /// <param name="numBullets"> The num of Bullets to be shot</param>
    public void attack(Character target, float cooldown, int numBullets)
    {
        StartCoroutine(Fire(target, cooldown, numBullets));  // start the fire coroutine
    }

    //Co-routine for firing in the target direction
    protected IEnumerator Fire(Character target, float cooldown, int numBullets)
    {
        Vector3 spawnPos;
        Attack bullet;
       

        // While still have bullets to be shot
        while (numBullets > 0)
        {
            numBullets--;
            animator.SetBool("IsShooting", true);

            // add some randomness to the bullets spawning y-position
            spawnPos = new Vector3(this.muzzlePoint.position.x, Random.Range(this.muzzlePoint.position.y - spawnRange, this.muzzlePoint.position.y + spawnRange), this.muzzlePoint.transform.position.z);

            bullet = Instantiate(this.currentAttack, spawnPos, this.transform.rotation);  // generate a bullet
            // set the shooting direction of this bullet depending on the player position
            direction = (target.transform.position.x - this.transform.position.x) > 0 ? Vector3.right : Vector3.left;
            bullet.GetComponent<Attack>().setDirection(direction);

            // Yielding and wait for cooldown seconds before the shooting of the next bullet
            yield return new WaitForSeconds(cooldown);
        }
    }
}
