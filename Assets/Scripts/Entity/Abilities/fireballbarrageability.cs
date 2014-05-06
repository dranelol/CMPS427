using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireballBarrageAbility : Ability
{
    public FireballBarrageAbility(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }

    public override void AttackHandler(GameObject source, Entity attacker, bool isPlayer)
    {
        DoBuff(source, attacker);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.5f, isPlayer));
    }


    /// <summary>
    /// Adds the buff to the enemy. Pretty straightforward
    /// 
    /// </summary>
    /// <param name="target">the gameobject of the target</param>
    /// <param name="source">the entity that is applying the buff/debuff</param>
    public void DoBuff(GameObject target, Entity source)
    {
        target.GetComponent<EntityAuraManager>().Add("fireballbarrage", source);

    }

    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {
        
    }

    public override void DoPhysics(GameObject source, GameObject target)
    {

    }

    public override IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target = null)
    {
        /*GameObject particles;



        particles = (GameObject)GameObject.Instantiate(particlePrefab, source.transform.position, Quaternion.Euler(90, 90, 0));


        //particles.transform.parent = attacker.transform;

        yield return new WaitForSeconds(time);

        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in particleSystems)
        {
            Debug.Log("asd");
            item.transform.parent = null;
            item.emissionRate = 0;
            item.enableEmission = false;

        }

        GameObject.Destroy(particles);
         */
        yield return null;
        
    }
}