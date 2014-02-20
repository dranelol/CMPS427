using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    public enum EquipmentSlots
    {
        HEAD,
        TORSO,
        LEFT_ARM,
        RIGHT_ARM,
        FEET
    };

    public float currentHP; // Currently unused.
    public Attributes currentAtt; // The entity's current total attributes
    public Attributes equipAtt; // Attribute changes that are added on from equipment stat changes
    public Attributes buffAtt; // Attribute changes that are added on from buffs/debuffs

    private Dictionary<EquipmentSlots, Attributes> equipmentStats = new Dictionary<EquipmentSlots, Attributes>();

    /// <summary>
    /// Creates the entity with a given set of base attributes,
    /// </summary>
    public void Start()
    {
        currentAtt = new Attributes();
        equipAtt = new Attributes();
        buffAtt = new Attributes();

        currentAtt.Power = 100;
    }

    /// <summary>
    /// Add the attribute changes of an item to the entity. The item must correlate to one of the equipment slots,
    /// HEAD, TORSO, LEFT_ARM, RIGHT_ARM, or FEET. Attribute changes are taken as an attributes object. Returns
    /// false if the slot is already filled.
    /// </summary>
    /// <param name="slot">The equipment slot being filled.</param>
    /// <param name="itemAtt">The attributes of the item being equipped.</param>
    /// <returns></returns>
    public bool addEquipment(EquipmentSlots slot, Attributes itemAtt)
    {
        if (equipmentStats.ContainsKey(slot))
            return false;
        else
        {
            equipmentStats.Add(slot, itemAtt);
            currentAtt.Add(itemAtt);
            equipAtt.Add(itemAtt);
            return true;
        }
    }

    /// <summary>
    /// Remove an item from an equipment slot, clearing that slot. Returns false if that slot is already empty.
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool removeEquipment(EquipmentSlots slot)
    {
        if (equipmentStats.ContainsKey(slot))
        {
            Attributes removed = equipmentStats[slot];
            equipmentStats.Remove(slot);
            currentAtt.Subtract(removed);
            equipAtt.Subtract(removed);
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Modifies the current health of the entity, clamped by the maximum health.
    /// Can be used for both taking damage and gaining health.
    /// </summary>
    /// <param name="value">Delta value to modify current health.</param>
    public void ModifyHealth(float delta) { currentHP = Mathf.Clamp(currentHP + delta, 0, currentAtt.Health); }

    /// <summary>
    /// Kind of obvious.
    /// </summary>
    /// <returns>True if the dude is dead.</returns>
    public bool IsDead() { return currentHP <= 0; }

    #region Buffs
    /// <summary>
    /// Adds a buff to the entity. The buff is given by an attributes object that is added to the entity's current
    /// stats. The duration of the buff is given in seconds.
    /// </summary>
    /// <param name="statChange">The attributes to be added by this buff</param>
    /// <param name="duration">The duration of this buff in seconds</param>
    public void addBuff(Attributes statChange, float duration)
    {
        StartCoroutine(newbuff(statChange, duration));
    }


    private IEnumerator newbuff(Attributes s, float d)
    {
        currentAtt.Add(s);
        buffAtt.Add(s);
        yield return new WaitForSeconds(d);
        currentAtt.Subtract(s);
        buffAtt.Subtract(s);
    }
    #endregion
}
