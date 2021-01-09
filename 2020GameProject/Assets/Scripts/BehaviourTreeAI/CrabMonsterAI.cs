using UnityEngine;
using System.Collections;
using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;


// Class for the monster AI using behaviour tree
public class CrabMonsterAI : MonoBehaviour
{

    public CrabMonsterMovementController movementController;
    public AttackController attackController;

    public float attackRange = 5f;  // the shooting range for this monster
    public float alertRange = 12f;  // the alerting range for this monster

    float jumpCooldown = 1f;
    float jumpCooldownTimer = 0f;

    float shootingCooldown = 1f;
    float shootingCooldownTimer = 0f;
    string incomingTag = "";

    private Player player;


    [SerializeField]
    private BehaviorTree _tree;

    private void Start()
    {
        // get the player gameObject from the game flow manager
        player = GameObject.Find("GameManager").GetComponent<GameFlowManager>().getPlayer();
        jumpCooldownTimer = jumpCooldown;
        // build the behaviour tree
        _tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("isIncoming", () => {
                        bool cooldownOK = jumpCooldownTimer > jumpCooldown;
                        // no need to raycast if cooldown
                        if (!cooldownOK) return false;
                        RaycastHit2D raycasthit = Physics2D.CircleCast(this.transform.position, 4f, Vector2.left);
                        // player attack or player itself enters circle cast
                        incomingTag = raycasthit.collider?.gameObject.tag;
                        return incomingTag.Contains("Player");
                    })
                    .Selector()
                        .Sequence()
                            .Condition("isBulletIncoming", () => {
                                bool isBullet = incomingTag.Contains("PlayerBullet");
                                return isBullet;
                            })
                            .Do("Jump", () => {
                                jumpCooldownTimer = 0f;
                                movementController.jump();
                                return TaskStatus.Success;
                            })
                        .End()
                        .Sequence()
                            .Condition("isPlayerIncoming", () => {
                                bool isPlayer = incomingTag.Equals(player.gameObject.tag);
                                return isPlayer;
                            })
                            .Do("Quickmove", () => {
                                jumpCooldownTimer = 0f;
                                movementController.quickMove();
                                return TaskStatus.Success;
                            })
                        .End()
                    .End()
                .End()
                .Sequence()
                    .Condition("isPlayerInAttackRange", () => {
                        bool cooldownOK = shootingCooldownTimer > shootingCooldown;
                        // check the distance between this monster and the player
                        return Vector3.Distance(this.transform.position, player.transform.position) <= attackRange && cooldownOK;
                    })
                    // .WaitTime(1f)  // wait 1 second before each attack action
                    .Do("Attack", () => {
                        shootingCooldownTimer = 0f;
                        movementController.pathFinding(player.transform.position - this.transform.position);
                        return TaskStatus.Success;
                    })
                .End()
                .Sequence()
                    .Condition("isPlayerInAlertRange", () => {
                        // check the distance between this monster and the player
                        return Vector3.Distance(this.transform.position, player.transform.position) <= alertRange;
                    })
                    .Do("PathFinding to Player", () => {
                        // perform path finding to the player
                        movementController.pathFinding(player.transform.position - this.transform.position);
                        return TaskStatus.Success;  // return failure to keep path finding if needed
                    })
                .End()
                .Do("Wandering", () => {
                    // movementController.wander();  // perform wander action
                    return TaskStatus.Success;
                })
            .End()
            .Build();
    }

    // Update is called once per frame
    void Update()
    {
        // do not tick if already dead
        if (!movementController.enabled || player == null) return;
        // Update our tree every frame
        _tree.Tick();
        //Debug.Log(Vector3.Distance(player.transform.position, this.transform.position));
        jumpCooldownTimer += Time.deltaTime;
        shootingCooldownTimer += Time.deltaTime;
    }
}
