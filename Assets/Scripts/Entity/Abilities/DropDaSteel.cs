using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropDaSteel : Ability
{
    public DropDaSteel(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }


    public override void SpawnProjectile(GameObject source, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
    {
       
    }

    public override void AttackHandler(GameObject source, Entity attacker, bool isPlayer)
    {
        Debug.Log("in attackhandler");
       
        attacker.abilityManager.AddAbility(GameManager.Abilities["onhitsworddrop"], 6);
        attacker.abilityIndexDict["onhitsworddrop"] = 6;

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(takeaway(attacker, isPlayer));

    }

    public IEnumerator takeaway(Entity attacker, bool isplayer)
    {


        yield return new WaitForSeconds(30f);


        attacker.abilityManager.RemoveAbility(6);
        attacker.abilityIndexDict.Remove("onhitsworddrop");

        yield return null;
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



    public override IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target)
    {
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, target.transform.position, source.transform.rotation);

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
