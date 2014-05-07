using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnHitSwordDrop : Ability
{
    public OnHitSwordDrop(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }

    public float low = 10;
    public float high = 20;


    public override void AttackHandler(GameObject attacker, GameObject defender, bool isPlayer)
    {
       // Debug.Log("sfjdkls;ajfkld;saj");
        if (isPlayer == true)
        {
            if (defender.GetComponent<AIController>().IsResetting() == false
                    && defender.GetComponent<AIController>().IsDead() == false)
            {
                if (attacker.GetComponent<Entity>().CurrentResource >= 5)
                {
                    DoDamage(attacker, defender, attacker.GetComponent<Entity>(), defender.GetComponent<Entity>(), isPlayer);

                    //DoPhysics(attacker, defender);
                    if (defender.GetComponent<AIController>().IsInCombat() == false)
                    {
                        defender.GetComponent<AIController>().BeenAttacked(attacker);
                    }

                    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(attacker, particleSystem, 0.4f, isPlayer, defender));
                    attacker.GetComponent<Entity>().ModifyResource(-5f);
                }
            }
        }
        else
        {

            DoDamage(attacker, defender, attacker.GetComponent<Entity>(), defender.GetComponent<Entity>(), isPlayer);
            //DoPhysics(attacker, defender);


            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(attacker, particleSystem, 5.4f, isPlayer, defender));
        }


    }

    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {
        float damageAmt = Random.Range(low, high);
        Debug.Log("damage: " + damageAmt);

        defender.ModifyHealth(-damageAmt);
    }

    public override void DoPhysics(GameObject source, GameObject target)
    {

    }

    public override IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target = null)
    {

        GameObject particles;
        Debug.Log("Making a sword... AN DROPPIN IT");
        particles = (GameObject)GameObject.Instantiate(particlePrefab, target.transform.position, source.transform.rotation);

        yield return new WaitForSeconds(time);

       // ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();

       // foreach (ParticleSystem item in particleSystems)
       // {
        //    item.transform.parent = null;
        //    item.emissionRate = 0;
        //    item.enableEmission = false;

        //}

        GameObject.Destroy(particles);

        yield return null;
    }
}
