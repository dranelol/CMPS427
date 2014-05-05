using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBaseAtts : MonoBehaviour {

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

    public List<string> possibleAbilities;
    public int maxAbilities = 3;

    public bool flees;
    public bool wanders;
    public float _aggroRadius;
    public float _swingSpeed;

    public float LootDropChance;
    public int MinLootDrops;
    public int MaxLootDrops;

    public void InitializeStats()
    {
        Attributes tempatts = new Attributes();
        int templvl = gameObject.GetComponent<Entity>().Level;

        tempatts.Health = baseHP + scalingHP * (templvl - 1);
        tempatts.Resource = baseResource + scalingResource * (templvl - 1);
        tempatts.Power = basePower + scalingPower * (templvl - 1);
        tempatts.Defense = baseDefense + scalingDefense * (templvl - 1);
        tempatts.MinDamage = baseMinDmg + scalingMinDmg * (templvl - 1);
        tempatts.MaxDamage = baseMaxDmg + scalingMaxDmg * (templvl - 1);
        tempatts.MovementSpeed = baseMoveSpd + scalingMoveSpd * (templvl - 1);
        tempatts.AttackSpeed = baseAttackSpd + scalingAttackSpd * (templvl - 1);

        gameObject.GetComponent<Entity>().baseAtt = tempatts;

        GetComponent<AIController>().doesWander = wanders;
        GetComponent<AIController>().aggroRadius = _aggroRadius;
        GetComponent<AIPursuit>().doesFlee= flees;
    }

    public void SetAbilities()
    {
        List<string> actualAblities = new List<string>();

        for(int i = 0; i < possibleAbilities.Count; i++)
        {
            string abilityname = possibleAbilities[i];

            if(actualAblities.Contains(abilityname) == false)
            {
                actualAblities.Add(abilityname);
                gameObject.GetComponent<Entity>().abilityManager.AddAbility(GameManager.Abilities[abilityname], i);
                gameObject.GetComponent<Entity>().abilityIndexDict[abilityname] = i;
            }
        }
    }


    // Use this for initialization
    void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
