using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GETOVERHERE : Ability
{
    public GETOVERHERE(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles, 2)
    {

    }


    public override void SpawnProjectile(GameObject source, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
    {
        //GameObject projectile = (GameObject)GameObject.Instantiate(particleSystem, source.transform.position, Quaternion.Euler(forward));
        GameObject projectile = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().BlinkStrikeProjectile, CombatMath.GetCenter(source.transform), Quaternion.Euler(forward));
        
        projectile.GetComponent<ProjectileBehaviour>().owner = owner;
        projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 0.25f;
        projectile.GetComponent<ProjectileBehaviour>().abilityID = abilityID;

        projectile.rigidbody.velocity = forward.normalized * 20.0f;
    }




    public override void AttackHandler(GameObject source, GameObject target, Entity attacker, bool isPlayer)
    {


        if (isPlayer == true)
        {
            if (target.GetComponent<AIController>().IsResetting() == false
                    && target.GetComponent<AIController>().IsDead() == false)
            {
                Entity defender = target.GetComponent<Entity>();
                DoPhysics(source, target);
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
            DoPhysics(source, target);
            DoDamage(source, target, attacker, defender, isPlayer);
        }
        // reaction particles
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.2f, isPlayer, target));
    }

    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {


    }
    public override void DoPhysics(GameObject source, GameObject target)
    {
        Vector3 relativeVector = (source.transform.position - target.transform.position).normalized;
        float normalizedMagnitude = Vector3.Distance(target.transform.position, source.transform.position);
        float force = (normalizedMagnitude / (Mathf.Pow(0.4f, 2)));
        target.GetComponent<MovementFSM>().AddForce(relativeVector * force * 7, 0.15f);
    }
}
