using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arrow : Ability
{
    public Arrow(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, id, readable, particles)
    {
       
    }

    /// <summary>
    /// Handler for this attack; figures out who will be attacked, and carries out everything needed for the attack to occur
    /// </summary>
    /// <param name="attacker">The gameobject carrying out the attack</param>
    /// <param name="defender">The gameobject defending against the attack</param>
    public override void AttackHandler(GameObject attacker, GameObject defender, bool isPlayer)
    {
        if (isPlayer == true)
        {
            if (defender.GetComponent<AIController>().IsResetting() == false
                    && defender.GetComponent<AIController>().IsDead() == false)
            {
                DoDamage(attacker, defender, isPlayer);
            }
        }

        else
        {
            DoDamage(attacker, defender, isPlayer);
        }
    }

    /// <summary>
    /// Figure out who will be affected by this attack
    /// </summary>
    /// <param name="attacker"></param>
    /// <returns>Returns a list of gameobjects this attack will affect</returns>
    public override List<GameObject> OnAttack(Transform attacker, bool isPlayer)
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        return enemiesToAttack;
    }

    /// <summary>
    /// Do damage with this attack
    /// </summary>
    /// <param name="attacker">The gameobject carrying out the attack</param>
    /// <param name="defender">The gameobject defending against the attack</param>
    public override void DoDamage(GameObject attacker, GameObject defender, bool isPlayer)
    {
        //Debug.Log(defender.ToString());
        Entity attackerEntity = attacker.GetComponent<Entity>();
        Entity defenderEntity = defender.GetComponent<Entity>();

        float damageAmt = DamageCalc.DamageCalculation(attacker, defender, damageMod);

        if (isPlayer == true)
        {
            Debug.Log("damage: " + damageAmt);
        }

        defenderEntity.currentHP -= damageAmt;

        float ratio = (defenderEntity.currentHP / defenderEntity.maxHP);

        if (isPlayer == true)
        {
            defender.renderer.material.color = new Color(1.0f, ratio, ratio);
        }
    }
}
