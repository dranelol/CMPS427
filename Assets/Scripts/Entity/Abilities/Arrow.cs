using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arrow : Ability
{
    public Arrow(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, id, readable, particles)
    {
       
    }


    public override void SpawnProjectile(GameObject source, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
    {

        GameObject projectile = (GameObject)GameObject.Instantiate(particleSystem, source.transform.position, Quaternion.Euler(forward));

        projectile.GetComponent<ProjectileBehaviour>().owner = owner;
        projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 5.0f;
        projectile.GetComponent<ProjectileBehaviour>().abilityID = abilityID;


        // apply velocity

        projectile.rigidbody.velocity = forward * 20.0f;
    }

    public override void AttackHandler(GameObject source, GameObject target, Entity attacker, bool isPlayer)
    {
        if (isPlayer == true)
        {
            if (target.GetComponent<AIController>().IsResetting() == false
                    && target.GetComponent<AIController>().IsDead() == false)
            {
                Entity defender = target.GetComponent<Entity>();
                DoDamage(source, target, attacker, defender, isPlayer);
                if (target.GetComponent<AIController>().IsInCombat() == false)
                {
                    target.GetComponent<AIController>().BeenAttacked(source);
                }
            }
        }

        else
        {
            Entity defender = target.GetComponent<Entity>();
            DoDamage(source, target, attacker, defender, isPlayer);
        }
    }

    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {

        float damageAmt = DamageCalc.DamageCalculation(attacker, defender, damageMod);

        if (isPlayer == true)
        {
            Debug.Log("damage: " + damageAmt);
        }

        defender.ModifyHealth(-damageAmt);

        float ratio = (defender.CurrentHP / defender.currentAtt.Health);

        if (isPlayer == true)
        {
            target.renderer.material.color = new Color(1.0f, ratio, ratio);
        }
    }
}
