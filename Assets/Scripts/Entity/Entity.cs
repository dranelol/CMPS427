﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    protected float currentHP; 
    public float CurrentHP
    {
        get { return currentHP; }
    }

    protected float currentResource;
    public float CurrentResource
    {
        get { return currentResource; }
    }

    public Attributes currentAtt; // The entity's current total attributes
    public Attributes equipAtt; // Attribute changes that are added on from equipment stat changes
    public Attributes buffAtt; // Attribute changes that are added on from buffs/debuffs

    public AbilityManager abilityManager;
    private Dictionary<equipSlots.slots, equipment> equippedEquip = new Dictionary<equipSlots.slots, equipment>();

    public Dictionary<string, int> abilityIndexDict = new Dictionary<string, int>();

    public void Awake()
    {
        abilityManager = gameObject.GetComponent<AbilityManager>();

        currentAtt = new Attributes();
        //Debug.Log(currentAtt.ToString()); 
        equipAtt = new Attributes();
        buffAtt = new Attributes();

        currentAtt.Health = currentHP = 50;
        currentAtt.Resource = currentResource = 100;
        currentAtt.Power = 10;
        currentAtt.Defense = 10;
        currentAtt.AttackSpeed = 1.0f;
    }

    /// <summary>
    /// Creates the entity with a given set of base attributes,
    /// </summary>
    public void Start()
    {
        
    }

    /// <summary>
    /// Modifies the current health of the entity, clamped by the maximum health.
    /// Can be used for both taking damage and gaining health.
    /// </summary>
    /// <param name="value">Delta value to modify current health.</param>
    public void ModifyHealth(float delta) { currentHP = Mathf.Clamp(currentHP + delta, 0, currentAtt.Health); }

    public void ModifyResource(float delta)
    {
        currentResource = Mathf.Clamp(currentResource + delta, 0, currentAtt.Power);
    }

    /// <summary>
    /// Kind of obvious.
    /// </summary>
    /// <returns>True if the dude is dead.</returns>
    public bool IsDead() { return currentHP <= 0; }

    #region Equipment

    /// <summary>
    /// Add the attribute changes of an item to the entity. The item must correlate to one of the equipment slots,
    /// Head, Chest, Legs, Feet, Main, Off. Attribute changes are taken as an attributes object. Returns
    /// false if the slot is already filled.
    /// </summary>
    /// <param name="slot">The equipment slot being filled.</param>
    /// <param name="itemAtt">The attributes of the item being equipped.</param>
    /// <returns></returns>
    public bool addEquipment(equipSlots.slots slot, equipment item)
    {
        if (this.equippedEquip.ContainsKey(slot))
            return false;
        else if (item.twohand == true && this.equippedEquip.ContainsKey(equipSlots.slots.Off))
            return false;
        else if (slot == equipSlots.slots.Off && equippedEquip[equipSlots.slots.Main].twohand == true)
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
    /// Does the current entity have an item equipped in this slot
    /// </summary>
    /// <param name="slot">The slot to check</param>
    /// <returns>Returns false if entity does not have slot equipped; true if they do</returns>
    public bool HasEquipped(equipSlots.slots slot)
    {
        return equippedEquip.ContainsKey(slot);
    }

    /// <summary>
    /// Returns the equipped item of that slot
    /// </summary>
    /// <param name="slot">Slot to return item</param>
    /// <returns>Throws exception if the entity doesn't have anything equipped there, else returns the item equipped</returns>
    public equipment GetEquip(equipSlots.slots slot)
    {
        if (HasEquipped(slot) == false)
        {
            throw new KeyNotFoundException("Entity does not have this item equipped!");
        }

        else
        {
            return equippedEquip[slot];
        }
    }

    #endregion

    #region Buffs

    public void AddBuff(Attributes buffedAttributes)
    {

    }

    #endregion
}
