using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    public GameObject FusRoDahParticles;

    public GameObject CleaveParticles;
    public GameObject WhirlwindParticles;

    public GameObject DeathgripParticles;
    public GameObject DeathgripTrailParticles;
    public GameObject HadoukenParticles;

    public GameObject ArrowParticles;
    public GameObject FireballProjectile;
    public GameObject InfernalFireballProjectile;
    public GameObject FireballTurretFireballProjectile;
    public GameObject ShadowboltProjectile;
    public GameObject PoisonboltProjectile;
    public GameObject BloodboltProjectile;
    public GameObject ChaosboltProjectile;
    public GameObject FiremineParticles;
    public GameObject GETOVERHEREParticles;

    public GameObject MineParticles;
    public GameObject ShockMineProjectile;

    public GameObject FireballExplosion;
    public GameObject InfernalFireballExplosion;
    public GameObject ChaosboltExplosion;

    public GameObject FlamestrikeParticles;

    public GameObject BlinkStrikeProjectile;
    public GameObject BlinkParticles;
    public GameObject BladeWaltzParticles;

    public GameObject OnHitNormalParticles;

    public GameObject AOEFreezeParticles;

    public GameObject ChaosBarrageParticles;
    public GameObject FireballBarrageParticles;

    public GameObject FireballTurretParticles;
    public GameObject FrozenOrbParticles;
    public GameObject IceBoltParticles;
    public GameObject IceBoltProjectile;

    public GameObject BoomerangBladeProjectile;
    public GameObject BoomerangBladeExplosion;
    public GameObject AxeThrowProjectile;
    public GameObject AxeThrowExplosion;
    public GameObject RotationEffect;

    public GameObject EnvironmentHealOrbProjectile;
    public GameObject EnvironmentHealOrbParticles;
    public GameObject EnvironmentHealOrbExplosion;

    public GameObject SpawnInParticles;

    public GameObject InfernalSpawn;

    public equipmentFactory EquipmentFactory;

    public static Dictionary<string, Ability> Abilities;
    public static Dictionary<int, Aura> Auras;
    public static Dictionary<string, int> AuraStringToIntMap;

    public static float GLOBAL_COOLDOWN = 0.5f;

    public AudioClip YEAAAAA;

    public bool loadSaveTest = false;

    public GameObject thing;
	// Use this for initialization
    public void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        if (Application.loadedLevel == 0)
        {
            Application.LoadLevel(1);
        }

        InfernalSpawn = (GameObject)Resources.Load("Enemy Prefabs/InfernalEnemy", typeof(GameObject));
        
        EquipmentFactory = new equipmentFactory();
        
        #region ability initialization
        Abilities = new Dictionary<string, Ability>();

        // Attack type, damage type, range, angle, cooldown, damagemod, resource cost

        #region player abilities

        #region spammed abilities

        Abilities["fireball"] = new Fireball(AttackType.PROJECTILE, DamageType.FIRE, 10.0f, 0.0f, 0.0f, 10.0f, 0f, "fireball", "Fireball", FireballExplosion);
        Abilities["shadowbolt"] = new Shadowbolt(AttackType.HONINGPROJECTILE, DamageType.SHADOW, 10.0f, 0.0f, 0.0f, 10.0f, 0f, "shadowbolt", "shadowbolt", FireballExplosion);
        Abilities["poisonbolt"] = new Poisonbolt(AttackType.HONINGPROJECTILE, DamageType.POISON, 10.0f, 0.0f, 0.0f, 10.0f, 0f, "poisonbolt", "poisonbolt", FireballExplosion);
        Abilities["bloodbolt"] = new Bloodbolt(AttackType.HONINGPROJECTILE, DamageType.PHYSICAL, 10.0f, 0.0f, 0.0f, 10.0f, 0f, "bloodbolt", "bloodbolt", FireballExplosion);
        Abilities["chaosbolt"] = new Chaosbolt(AttackType.HONINGPROJECTILE, DamageType.FIRE, 10.0f, 0.0f, 0.0f, 10.0f, 0f, "chaosbolt", "chaosbolt", ChaosboltExplosion);
        Abilities["icebolt"] = new IceBolt(AttackType.PROJECTILE, DamageType.WATER, 8f, 0f, 0.0f, 10.0f, 0f, "icebolt", "Ice Bolt", IceBoltParticles);
        Abilities["cleave"] = new Cleave(AttackType.MELEE, DamageType.PHYSICAL, 3.0f, 45.0f, 0.0f, 5.0f, 0f, "cleave", "Cleave", CleaveParticles);
        Abilities["arrow"] = new Arrow(AttackType.PROJECTILE, DamageType.PHYSICAL, 8.0f, 0.0f, 0.0f, 5.0f, 0f, "arrow", "Arrow", ArrowParticles);

        #endregion

        #region buff abilities

        Abilities["chaosbarrage"] = new ChaosBarrageAbility(AttackType.STATUS, DamageType.NONE, 10.0f, 0.0f, 30.0f, 0.0f, 100f, "chaosbarrage", "Chaos Barrage", ChaosBarrageParticles);
        Abilities["fireballbarrage"] = new FireballBarrageAbility(AttackType.STATUS, DamageType.NONE, 10.0f, 0.0f, 30.0f, 0.0f, 100f, "fireballbarrage", "Fireball Barrage", FireballBarrageParticles);
        Abilities["rootability"] = new RootAbility(AttackType.STATUS, DamageType.NONE, 10.0f, 360.0f, 15.0f, 0.0f, 0f, "rootability", "Root Ability", null);

        #endregion

        Abilities["hadouken"] = new Hadouken(AttackType.PBAOE, DamageType.AIR, 5.0f, 360.0f, 10.0f, 10.0f, 25.0f, "hadouken", "Hadouken", HadoukenParticles);
        Abilities["deathgrip"] = new Deathgrip(AttackType.PBAOE, DamageType.SHADOW, 5.0f, 360.0f, 10.0f, 15.0f, 25.0f, "deathgrip", "AoE Deathgrip", DeathgripParticles);
        Abilities["fusrodah"] = new Fusrodah(AttackType.PBAOE, DamageType.AIR, 5.0f, 45.0f, 5.0f, 10.0f, 20.0f, "fusrodah", "Fus Roh Dah", FusRoDahParticles);
		Abilities["flamestrike"] = new Flamestrike(AttackType.PBAOE, DamageType.FIRE, 5.0f, 360.0f, 5.0f, 1000.0f, 25.0f, "flamestrike", "Flamestrike", FlamestrikeParticles);
        Abilities["bladewaltz"] = new BladeWaltz(AttackType.PBAOE, DamageType.PHYSICAL, 5.0f, 360.0f, 30.0f, 0f, 50.0f, "bladewaltz", "Blade Waltz", BladeWaltzParticles);
        Abilities["erenwaltz"] = new ErenWaltz(AttackType.PBAOE, DamageType.PHYSICAL, 5.0f, 360.0f, 0.0f, 5.0f, 0f, "erenwaltz", "Eren Waltz", BladeWaltzParticles);Abilities["firemine"] = new FireMine(AttackType.PROJECTILE, DamageType.FIRE, 5.0f, 360.0f, 4.0f, 200.0f, 10f, "firemine", "Fire Mine", FiremineParticles);
        Abilities["GETOVERHERE"] = new GETOVERHERE(AttackType.PROJECTILE, DamageType.SHADOW, 4.0f, 0.0f, 5.0f, 0.1f, 10f, "GETOVERHERE", "Shadow Pull", GETOVERHEREParticles);
        Abilities["normalmine"] = new NormalMine(AttackType.PROJECTILE, DamageType.PHYSICAL, 5.0f, 360.0f, 4.0f, 1.0f, 10f, "normalmine", "Mine", MineParticles);
        Abilities["blinkstrike"] = new BlinkStrike(AttackType.PROJECTILE, DamageType.SHADOW, 4.0f, 1.0f, 7.0f, 5.0f, 10f, "blinkstrike", "Blink Strike", BlinkStrikeProjectile);
        Abilities["blink"] = new Blink(AttackType.GROUNDTARGET, DamageType.NONE, 5.0f, 0.0f, 7.0f, 0.0f, 25f, "blink", "Blink", BlinkParticles);Abilities["shockmine"] = new ShockMine(AttackType.PROJECTILE, DamageType.PHYSICAL, 7.0f, 360.0f, 3.0f, 30.0f, 5f, "shockmine", "Shock Mine", ShockMineProjectile);
        Abilities["aoefreeze"] = new AOEfreeze(AttackType.PBAOE, DamageType.WATER, 5.0f, 360f, 15f, 1f, 30f, "aoefreeze", "Flashfreeze", AOEFreezeParticles);
        Abilities["onhitnormal"] = new OnHitNormal(AttackType.MELEE, DamageType.PHYSICAL, 0.0f, 0.0f, 0.0f, 0.0f, 0f, "onhitnormal", "On Hit Normal", OnHitNormalParticles);
        Abilities["fireballturret"] = new FireballTurret(AttackType.PROJECTILE, DamageType.NONE, 10.0f, 360.0f, 2.0f, 0.0f, 40f, "fireballturret", "Fireball Turret", FireballTurretParticles);
        Abilities["fireballturretfireball"] = new FireballTurretFireball(AttackType.PROJECTILE, DamageType.FIRE, 10.0f, 0.0f, 0.0f, 5.0f, 0f, "fireballturretfireball", "Fireball Turret Fireball", FireballExplosion);
        Abilities["frozenorb"] = new FrozenOrb(AttackType.PROJECTILE, DamageType.NONE, 5.0f, 360.0f, 8.0f, 0.0f, 30f, "frozenorb", "Frozen Orb", FrozenOrbParticles);
        Abilities["frozenorbicebolt"] = new IceBolt(AttackType.PROJECTILE, DamageType.WATER, 8f, 0f, 0.0f, 0f, 0f, "frozenorbicebolt", "Frozen Orb Ice Bolt", IceBoltParticles);
        Abilities["boomerangblade"] = new BoomerangBlade(AttackType.PROJECTILE, DamageType.PHYSICAL, 5f, 0f, 4.0f, 0f, 20f, "boomerangblade", "Boomerang Blade", BoomerangBladeExplosion);
        Abilities["boomerangbladereturn"] = new BoomerangBladeReturn(AttackType.HONINGPROJECTILE, DamageType.PHYSICAL, 0.0f, 0.0f, 0.0f, 0.0f, 0f, "boomerangbladereturn", "Boomerang Blade(returning)", BoomerangBladeExplosion);
        Abilities["axethrow"] = new AxeThrow(AttackType.PROJECTILE, DamageType.PHYSICAL, 5.0f, 0.0f, 2.0f, 0.0f, 3f, "axethrow", "Axe Throw", AxeThrowExplosion);

        Abilities["infernalfireball"] = new InfernalFireball(AttackType.HONINGPROJECTILE, DamageType.FIRE, 5.0f, 360.0f, 0.0f, 30.0f, 0f, "infernalfireball", "Infernal Fireball", InfernalFireballExplosion);

        //Abilities["healorb"] = new HealOrb(AttackType.PROJECTILE, DamageType.NONE, 5.0f, 360.0f, 0.0f, 0.0f, "healorb", "Heal Orb", HealOrbExplosion);


        #endregion

        #region enemy abilities
        Abilities["enemyfireball"] = new Fireball(AttackType.PROJECTILE, DamageType.FIRE, 10.0f, 0.0f, 2.5f, 10.0f, 0f, "enemyfireball", "Enemy Fireball", FireballExplosion);
        Abilities["enemycleaveslow"] = new Cleave(AttackType.MELEE, DamageType.PHYSICAL, 3.0f, 45.0f, 5.0f, 5.0f, 0f, "cleave", "Cleave", CleaveParticles);
        Abilities["enemycleavenormal"] = new Cleave(AttackType.MELEE, DamageType.PHYSICAL, 3.0f, 45.0f, 2.5f, 2.5f, 0f, "cleave", "Cleave", CleaveParticles);
        Abilities["enemycleavefast"] = new Cleave(AttackType.MELEE, DamageType.PHYSICAL, 3.0f, 45.0f, 1.0f, 1.0f, 0f, "cleave", "Cleave", CleaveParticles);
        Abilities["enemydeathgrip"] = new Deathgrip(AttackType.PBAOE, DamageType.SHADOW, 10.0f, 360.0f, 2.0f, 0.0f, 0.0f, "deathgrip", "Enemy Deathgrip", DeathgripParticles);
        #endregion
        #endregion

    }

    public void Update()
    {
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

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
