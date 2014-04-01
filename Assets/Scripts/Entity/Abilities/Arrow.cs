using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arrow : Ability
{
    public Arrow(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, id, readable, particles)
    {
       
    }

    public override void AttackHandler(GameObject source, GameObject target, bool isPlayer)
    {
        if (isPlayer == true)
        {
            if (target.GetComponent<AIController>().IsResetting() == false
                    && target.GetComponent<AIController>().IsDead() == false)
            {
                DoDamage(source, target, isPlayer);
            }
        }

        else
        {
            DoDamage(source, target, isPlayer);
        }
    }

    public override void DoDamage(GameObject source, GameObject target, bool isPlayer)
    {
        //Debug.Log(defender.ToString());
        Entity attacker = source.GetComponent<Entity>();
        Entity defender = target.GetComponent<Entity>();

        float damageAmt = DamageCalc.DamageCalculation(attacker, defender, damageMod);

        if (isPlayer == true)
        {
            Debug.Log("damage: " + damageAmt);
        }

        defender.currentHP -= damageAmt;

        float ratio = (defender.currentHP / defender.maxHP);

        if (isPlayer == true)
        {
            target.renderer.material.color = new Color(1.0f, ratio, ratio);
        }
    }
}
