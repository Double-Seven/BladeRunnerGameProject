using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSkill : Skill {

    private int numBullets_skill1 = 12;  // number of bullets shooting by skill1

    float cooldownTimer;
    private void Start()
    {
        cooldownTimer = cooldown;
        GameFlowManager.instance.getPlayer().cooldownBarSkill2.SetMaxCooldown(cooldown);
    }

    private void Update()
    {
        GameFlowManager.instance.getPlayer().cooldownBarSkill2.SetCooldown(cooldownTimer);
        cooldownTimer += Time.deltaTime;
    }

    public void SetShootingSkill(Attack attack, float cooldown) {
        base.SetSkill(attack, cooldown);
    }

    public override void createSkill(Transform transform) {
        if (cooldownTimer > cooldown)
        {
            GameFlowManager.instance.getPlayer().GetComponent<MotionController>().animator.Play("Base Layer.Jump", 0, 0f);
            float deltaAngle = 180 / numBullets_skill1;
            // generate bullets flying from the player (one bullet for each deltaAngle degree around the player)
            for (int i = 0; i < numBullets_skill1; i++)
            {
                Attack bullet = MonoBehaviour.Instantiate(this.attack, transform.position, Quaternion.Euler(0, 0, deltaAngle * i));  // generate a bullet

                // set the shooting direction of this bullet
                bullet.GetComponent<Attack>().setDirection(new Vector2(Mathf.Cos(Mathf.Deg2Rad * deltaAngle * i), -Mathf.Sin(Mathf.Deg2Rad * deltaAngle * i)));
            }
            cooldownTimer = 0f;
        }
    }
}
