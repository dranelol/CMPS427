using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    public GameObject FusRoDahParticles;
    public GameObject CleaveParticles;
    public GameObject DeathgripParticles;
    public GameObject HadoukenParticles;

    public GameObject BladeWaltzParticles;

    public GameObject ArrowParticles;
    public GameObject FireballProjectile;
    public GameObject ShadowboltProjectile;
    public GameObject PoisonboltProjectile;
    public GameObject BloodboltProjectile;
    public GameObject ChaosboltProjectile;
    public GameObject FiremineParticles;
    public GameObject GETOVERHEREParticles;
    public GameObject MineParticles;
    public GameObject ShockMineProjectile;
    public GameObject FireballExplosion;
    public GameObject ChaosboltExplosion;
    public GameObject BlinkStrikeProjectile;

    public GameObject OnHitNormalParticles;
    public GameObject BlinkParticles;



    public equipmentFactory EquipmentFactory;

    public static Dictionary<string, Ability> Abilities;
    public static Dictionary<int, Aura> Auras;
    public static Dictionary<string, int> AuraStringToIntMap;

    public static float GLOBAL_COOLDOWN = 0.2f;


    public bool loadsavetest = false;

    public GameObject thing;
	// Use this for initialization
    public void Awake()
    {
        EquipmentFactory = new equipmentFactory();

        #region ability initialization
        Abilities = new Dictionary<string, Ability>();

        // Attack type, damage type, range, angle, cooldown, damagemod


        Abilities["hadouken"] = new Hadouken(AttackType.PBAOE, DamageType.AIR, 5.0f, 360.0f, 3.0f, 10.0f, "hadouken", "Hadouken", HadoukenParticles);
        Abilities["deathgrip"] = new Deathgrip(AttackType.PBAOE, DamageType.SHADOW, 5.0f, 360.0f, 3.0f, 15.0f, "deathgrip", "AoE Deathgrip", DeathgripParticles);
        Abilities["cleave"] = new Cleave(AttackType.MELEE, DamageType.PHYSICAL, 5.0f, 45.0f, 0.0f, 5.0f, "cleave", "Cleave", CleaveParticles);
        Abilities["fusrodah"] = new Fusrodah(AttackType.PBAOE, DamageType.AIR, 5.0f, 45.0f, 1.0f, 10.0f, "fusrodah", "Fus Roh Dah", FusRoDahParticles);

        Abilities["bladewaltz"] = new BladeWaltz(AttackType.PBAOE, DamageType.PHYSICAL, 5.0f, 360.0f, 0.0f, 5.0f, "bladewaltz", "Blade Waltz", BladeWaltzParticles);

        Abilities["arrow"] = new Arrow(AttackType.PROJECTILE, DamageType.PHYSICAL, 0.0f, 0.0f, 0.0f, 5.0f, "arrow", "Arrow", ArrowParticles);
        Abilities["fireball"] = new Fireball(AttackType.PROJECTILE, DamageType.FIRE, 10.0f, 0.0f, 0.1f, 10.0f, "fireball", "Fireball", FireballExplosion);
        Abilities["firemine"] = new firemine(AttackType.PROJECTILE, DamageType.FIRE, 5.0f, 360.0f, 4.0f, 1.0f, "firemine", "Fire Mine", FiremineParticles);
        Abilities["GETOVERHERE"] = new GETOVERHERE(AttackType.PROJECTILE, DamageType.SHADOW, 1.0f, 0.0f, 3.0f, 0.1f, "GETOVERHERE", "Shadow Pull", GETOVERHEREParticles);
        Abilities["normalmine"] = new normalmine(AttackType.PROJECTILE, DamageType.PHYSICAL, 5.0f, 360.0f, 4.0f, 1.0f, "normalmine", "Mine", MineParticles);
        Abilities["blinkstrike"] = new BlinkStrike(AttackType.PROJECTILE, DamageType.SHADOW, 1.0f, 1.0f, 2.0f, 5.0f, "blinkstrike", "Blink Strike", BlinkStrikeProjectile);

        Abilities["blink"] = new Blink(AttackType.GROUNDTARGET, DamageType.NONE, 5.0f, 0.0f, 2.0f, 0.0f, "blink", "Blink", BlinkParticles);

        Abilities["shadowbolt"] = new Shadowbolt(AttackType.HONINGPROJECTILE, DamageType.SHADOW, 10.0f, 0.0f, 0.1f, 10.0f, "shadowbolt", "shadowbolt", FireballExplosion);
        Abilities["poisonbolt"] = new Poisonbolt(AttackType.HONINGPROJECTILE, DamageType.POISON, 10.0f, 0.0f, 0.1f, 10.0f, "poisonbolt", "poisonbolt", FireballExplosion);
        Abilities["bloodbolt"] = new Bloodbolt(AttackType.HONINGPROJECTILE, DamageType.PHYSICAL, 10.0f, 0.0f, 0.1f, 10.0f, "bloodbolt", "bloodbolt", FireballExplosion);
        Abilities["chaosbolt"] = new Chaosbolt(AttackType.HONINGPROJECTILE, DamageType.FIRE, 10.0f, 0.0f, 0.1f, 10.0f, "chaosbolt", "chaosbolt", ChaosboltExplosion);
        
        Abilities["ShockMine"] = new ShockMine(AttackType.PROJECTILE, DamageType.PHYSICAL, 7.0f, 360.0f, 3.0f, 10.0f, "ShockMine", "Shock Mine", ShockMineProjectile);


        Abilities["onhitnormal"] = new OnHitNormal(AttackType.MELEE, DamageType.PHYSICAL, 0.0f, 0.0f, 0.0f, 0.0f, "onhitnormal", "On Hit Normal", OnHitNormalParticles);

        #endregion

        
    }

    public void RemovePhysics(Rigidbody toRemove, float time = 0.0f)
    {
        StartCoroutine(removePhysics(toRemove, time));
    }

    IEnumerator removePhysics(Rigidbody target, float time = 0.0f)
    {
        yield return new WaitForSeconds(time);

        if (target != null)
        {
            target.isKinematic = true;
        }

        yield break;
    }
    /*
    IEnumerator runParticleSystemForSeconds(Transform source, GameObject particleSystem, float time)
    {
        GameObject particles = (GameObject)GameObject.Instantiate(particleSystem, source.position, source.rotation);

        particles.transform.parent = source;

        yield return new WaitForSeconds(time);

        Destroy(particles);

        yield return null;
    }
    */

    public void RunParticleSystem(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
