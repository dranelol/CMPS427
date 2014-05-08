using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBaseAtts : MonoBehaviour {

    public const int maxAbilityCount = 4;
    public List<string> potentialAbilities;

    public float baseHP;
    public float baseResource;
    public float basePower;
    public float baseDefense;
    public float baseMinDmg;
    public float baseMaxDmg;
    public float baseAttackSpd;
    public float baseMoveSpd;

    public float scalingHP;
    public float scalingResource;
    public float scalingPower;
    public float scalingDefense;
    public float scalingMinDmg;
    public float scalingMaxDmg;
    public float scalingAttackSpd;
    public float scalingMoveSpd;

    public bool flees;
    public bool wanders;
    public float _aggroRadius;
    public float _swingSpeed;

    public float LootDropChance;
    public int MinLootDrops;
    public int MaxLootDrops;

    private Entity _entity;

    public void InitializeStats(int level)
    {
        Attributes tempatts = new Attributes();
        _entity = GetComponent<Entity>();

        tempatts.Health = baseHP + scalingHP * (level - 1);
        tempatts.Resource = baseResource + scalingResource * (level - 1);
        tempatts.Power = basePower + scalingPower * (level - 1);
        tempatts.Defense = baseDefense + scalingDefense * (level - 1);
        tempatts.MinDamage = baseMinDmg + scalingMinDmg * (level - 1);
        tempatts.MaxDamage = baseMaxDmg + scalingMaxDmg * (level - 1);
        tempatts.MovementSpeed = baseMoveSpd + scalingMoveSpd * (level - 1);
        tempatts.AttackSpeed = baseAttackSpd + scalingAttackSpd * (level - 1);

        _entity.baseAtt = tempatts;

        GetComponent<Entity>().UpdateCurrentAttributes();

        GetComponent<AIController>().doesWander = wanders;
        GetComponent<AIController>().aggroRadius = _aggroRadius;
        GetComponent<AIPursuit>().doesFlee= flees;
    }

    public void SetAbilities()
    {
        List<string> abilities = new List<string>();

        for (int i = 0; i < potentialAbilities.Count; i++)
        {
            string abilityName = potentialAbilities[i];

            if(abilities.Count <= maxAbilityCount && !abilities.Contains(abilityName))
            {
                abilities.Add(abilityName);
                _entity.abilityManager.AddAbility(GameManager.Abilities[abilityName], i);
                _entity.abilityIndexDict[abilityName] = i;
            }
        }

        GetComponent<AIPursuit>()._abilityCount = abilities.Count;
    }
}
