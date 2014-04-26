using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlinkStrike : Ability
{
    public BlinkStrike(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }


    public override void SpawnProjectile(GameObject source, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
    {

        GameObject projectile = (GameObject)GameObject.Instantiate(particleSystem, source.transform.position, Quaternion.Euler(forward));

        projectile.GetComponent<ProjectileBehaviour>().owner = owner;
        projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 0.25f;
        projectile.GetComponent<ProjectileBehaviour>().abilityID = abilityID;

        projectile.rigidbody.velocity = forward.normalized * 20.0f;
       


    }

    public override void AttackHandler(GameObject source, GameObject target, Entity attacker, bool isPlayer)
    {

        /*
        Vector3 forward = Vector3.zero;

        // if its a player, attack based on mouse
        if (isPlayer == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayCastTarget;
            Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
            Vector3 vectorToMouse = rayCastTarget.point - source.transform.position;
            forward = new Vector3(vectorToMouse.x, source.transform.forward.y, vectorToMouse.z).normalized;
        }

        // if its an enemy, attack based on forward vector
        else
        {
            forward = source.transform.forward;
        }
         */


        if (isPlayer == true)
        {
            if (target.GetComponent<AIController>().IsResetting() == false
                    && target.GetComponent<AIController>().IsDead() == false)
            {
                Entity defender = target.GetComponent<Entity>();
                DoBlink(target, attacker.gameObject);
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
            DoBlink(target, attacker.gameObject);
            DoDamage(source, target, attacker, defender, isPlayer);
        }

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.2f, isPlayer, target));
    }

    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {
        float damageAmt;
        if (isPlayer == true)
        {
            damageAmt = DamageCalc.DamageCalculation(attacker, defender, damageMod);
        }
        else
        {
            damageAmt = DamageCalc.DamageCalculation(attacker, defender, 0);
        }
        if (isPlayer == true)
        {
            Debug.Log("damage: " + damageAmt);
        }

        defender.ModifyHealth(-damageAmt);

    }


    private void DoBlink(GameObject target, GameObject owner)
    {

        float portradius = 1.0f;
        Vector3 portpos = (target.transform.position - owner.transform.position);

        Vector3 offset = Vector3.Normalize(portpos) * portradius;

        portpos = portpos + offset + owner.transform.position;

        owner.GetComponent<NavMeshAgent>().Warp(portpos);
        Vector3 tempforward = target.transform.position - portpos;
        tempforward.y = 0;
        owner.transform.forward = Vector3.Normalize(tempforward);

    }


    public override IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target)
    {
       /* GameObject particles;
        Debug.Log("doing Animation");
        particles = (GameObject)GameObject.Instantiate(particlePrefab, target.transform.position, source.transform.rotation);

        yield return new WaitForSeconds(time);

        Debug.Log("after yield");
        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in particleSystems)
        {
            item.transform.parent = null;
            item.emissionRate = 0;
            item.enableEmission = false;

        }

        GameObject.Destroy(particles);
        */
        yield return null;
    }

}
