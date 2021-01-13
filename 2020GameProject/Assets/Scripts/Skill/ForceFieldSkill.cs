using System.Collections;
using UnityEngine;

public class ForceFieldSkill : Skill
{
    float cooldownTimer = 0f;
    MotionController movementcontroller;
    Character target;

    // Use this for initialization
    void Start()
    {
        cooldownTimer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    public ForceFieldSkill SetForceField(Attack attack, float cooldown, Character target)
    {
        base.SetSkill(attack, cooldown);
        
        base.targets.Add(target);
        this.target = target;
        movementcontroller = target.GetComponent<MotionController>();
        cooldownTimer = cooldown;
        this.cooldown = cooldown;
        return this;
    }

    public override void createSkill(Transform transform)
    {
        if (cooldownTimer < cooldown) return;
        if (((Player)target).isJumping) return;
        float deltaAngle = 90f;
        float moveForce = 100f;
        movementcontroller.animator.Play("Base Layer.BackJump", 0, -0.1f);
        Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * deltaAngle) * moveForce, Mathf.Sin(Mathf.Deg2Rad * deltaAngle) * moveForce);
        if (!target.isFacingRight)
            direction.x = direction.x * -1;
        target.thisRB.AddForce(direction);

        Instantiate(attack, target.transform.position, target.transform.rotation);

        cooldownTimer = 0f;
    }
}
