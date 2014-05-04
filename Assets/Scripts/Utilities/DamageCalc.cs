using UnityEngine;
using System.Collections;

public class DamageCalc
{
    public const float MagicNumber = 100.0f;

    static public float DamageCalculation(Entity attacker, Entity defender, float damagemod)
    {
        float damageAmt;

        float maincalc;

        Attributes attackatts = attacker.currentAtt;
        Attributes defendatts = defender.currentAtt;

        maincalc = (attackatts.MaxDamage - attackatts.MinDamage) * (attackatts.Power - defendatts.Defense);
        maincalc = maincalc / Mathf.Sqrt((attackatts.Power - defendatts.Defense) * (attackatts.Power - defendatts.Defense) + MagicNumber);

        damageAmt = Random.Range(Mathf.Clamp(maincalc + attackatts.MinDamage, attackatts.MinDamage, maincalc + attackatts.MinDamage),
                                Mathf.Clamp(maincalc + attackatts.MaxDamage, maincalc + attackatts.MaxDamage, attackatts.MaxDamage));

        

        damageAmt = damageAmt + damagemod;


        return damageAmt;
    }

    static public float HealCalculation(Entity healer, Entity recipient, float multscalingfactor, float addscalingfactor)
    {
        float healamt;

        healamt = (healer.currentAtt.Power * multscalingfactor) + addscalingfactor;

        return healamt;

    }
}
