using UnityEngine;
using System.Collections;
using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;


// Class for the monster AI using behaviour tree
public class FlyingMonsterAI : MonoBehaviour
{
    
    public FlyingMonsterMovementController movementController;
    public FlyingMonsterAttackController attackController;

    public float attackRange = 20f;  // the shooting range for this monster
    public float alertRange = 12f;  // the alerting range for this monster

    private Player player;
    

    [SerializeField]
    private BehaviorTree _tree;

    private void Start()
    {
        // get the player gameObject from the game flow manager
        player = GameFlowManager.instance.getPlayer();

        // build the behaviour tree
        _tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("isPlayerInAttackRange", () => {
                        // check the distance between this monster and the player
                        return Vector3.Distance(this.transform.position, player.transform.position) <= attackRange;
                    })
                    .WaitTime(1f)  // wait 1 second before each attack action
                    .Do("Attack", () => {
                        attackController.attack(player, 0.25f, 3);
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
                        movementController.pathFinding(player.transform.position);
                        return TaskStatus.Success;  // return failure to keep path finding if needed
                    })
                .End()
                .Do("Wandering", () => {
                    movementController.wander();  // perform wander action
                    return TaskStatus.Success;
                })
            .End()
            .Build();
    }

    // Update is called once per frame
    void Update()
    {
        // Update our tree every frame
        if (player == null) return;
        _tree.Tick();
        //Debug.Log(Vector3.Distance(player.transform.position, this.transform.position));
    }
}
