using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{ 
    public float currentHP; // Currently unused.
    public float maxHP; // max HP
    public Attributes currentAtt; // The entity's current total attributes
    public Attributes equipAtt; // Attribute changes that are added on from equipment stat changes
    public Attributes buffAtt; // Attribute changes that are added on from buffs/debuffs

    public Dictionary<string, Ability> abilities; // set of abilities that this entity currently has access to

    private Dictionary<equipSlots.slots, equipment> equippedEquip = new Dictionary<equipSlots.slots, equipment>();

    /// <summary>
    /// Creates the entity with a given set of base attributes,
    /// </summary>
    public void Start()
    {
        currentAtt = new Attributes();
        equipAtt = new Attributes();
        buffAtt = new Attributes();
        abilities = new Dictionary<string, Ability>();

        currentAtt.Power = 100;

        maxHP = currentHP = 50f;
    }

    /// <summary>
    /// Add the attribute changes of an item to the entity. The item must correlate to one of the equipment slots,
    /// Head, Chest, Legs, Feet, Main, Off. Attribute changes are taken as an attributes object. Returns
    /// false if the slot is already filled.
    /// </summary>
    /// <param name="slot">The equipment slot being filled.</param>
    /// <param name="itemAtt">The attributes of the item being equipped.</param>
    /// <returns></returns>
    public bool addEquipment(equipSlots.slots slot , equipment item)
    {
        if (this.equippedEquip.ContainsKey(slot))
            return false;
        else
        {
            this.equippedEquip.Add(slot, item);
            currentAtt.Add(item.equipmentAttributes);
            this.equipAtt.Add(item.equipmentAttributes);
            return true;
        }
    }

    /// <summary>
    /// Remove an item from an equipment slot, clearing that slot. Returns false if that slot is already empty.
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool removeEquipment(equipSlots.slots slot)
    {
        if (equippedEquip.ContainsKey(slot))
        {
            equipment removed = equippedEquip[slot];
            equippedEquip.Remove(slot);
            currentAtt.Subtract(removed.equipmentAttributes);
            equipAtt.Subtract(removed.equipmentAttributes);
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

    /// <summary>
    /// Adds an ability to the entity
    /// </summary>
    /// <param name="toAdd">Name key of the ability to add</param>
    public void AddAbility(string toAdd)
    {
        abilities.Add(toAdd, GameManager.Abilities[toAdd]);
    }

    /// <summary>
    /// Removes an ability from the entity
    /// </summary>
    /// <param name="toRemove">Name key of the ability to remove</param>
    public void RemoveAbility(string toRemove)
    {
        abilities.Remove(toRemove);
    }
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
