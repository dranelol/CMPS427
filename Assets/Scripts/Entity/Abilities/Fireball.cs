using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fireball : Ability
{
    public Fireball(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {
        
    }

    public override void SpawnProjectile(GameObject source, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
    {
        int segments = 1;
        
        for(int i = 0; i < segments; i++)
        {
            
            GameObject projectile = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().FireballProjectile, CombatMath.GetCenter(source.transform) + Rotations.RotateAboutY(forward, (360 / segments) * i) * 2, Quaternion.LookRotation(Rotations.RotateAboutY(forward, (360 / segments) * i)));

            projectile.GetComponent<ProjectileBehaviour>().owner = owner;
            projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 3.0f;
            projectile.GetComponent<ProjectileBehaviour>().abilityID = abilityID;
            projectile.GetComponent<ProjectileBehaviour>().CollidesWithTerrain = false;

            projectile.rigidbody.velocity = Rotations.RotateAboutY(forward, (360 / segments) * i) * 20.0f;
        }

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

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.2f, isPlayer, target));
    }

    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {
        float damageAmt;
        if (isPlayer == true)
        {
            damageAmt = DamageCalc.DamageCalculation(attacker, defender, damageMod);

            if (attacker.gameObject.GetComponent<TalentManager>().Bonuses["fire"] == true)
            {
                damageAmt += damageAmt * 0.1f;
            }
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



    public override IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target)
    {
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, CombatMath.GetCenter(target.transform), source.transform.rotation);

        yield return new WaitForSeconds(time);

        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in particleSystems)
        {
            item.transform.parent = null;
            item.emissionRate = 0;
            item.enableEmission = false;

        }

        GameObject.Destroy(particles);

        yield return null;
    }
    
}
