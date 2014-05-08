using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnHitNormal : Ability
{
    public OnHitNormal(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }

    public float low = 10;
    public float high = 20;


    public override void AttackHandler(GameObject attacker, GameObject defender, bool isPlayer)
    {
        Debug.Log("sfjdkls;ajfkld;saj");
        if (isPlayer == true)
        {
            if (defender.GetComponent<AIController>().IsResetting() == false
                    && defender.GetComponent<AIController>().IsDead() == false)
            {
                DoDamage(attacker, defender, attacker.GetComponent<Entity>(), defender.GetComponent<Entity>(), isPlayer);
                //DoPhysics(attacker, defender);
                if (defender.GetComponent<AIController>().IsInCombat() == false)
                {
                    defender.GetComponent<AIController>().BeenAttacked(attacker);
                }
            }
        }
        else
        {
            
            DoDamage(attacker, defender, attacker.GetComponent<Entity>(), defender.GetComponent<Entity>(), isPlayer);
            //DoPhysics(attacker, defender);

            
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
        yield return null;
    }
}
