using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface Entity
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
    public Attributes tempAtt; // Attributes that are added on top of the entity's base attributes

    private Dictionary<EquipmentSlots, Attributes> equipmentStats = new Dictionary<EquipmentSlots, Attributes>();
    private ArrayList bufflist; // To be implemented.

    /// <summary>
    /// Creates the entity with a given set of base attributes,
    /// </summary>
    /// <param name="att">An Attributes object containing the base stats of this entity.</param>
    public Entity(Attributes att)
    {
        currentAtt = att;
        tempAtt = new Attributes();
        bufflist = new ArrayList();
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
            tempAtt.Add(itemAtt);
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
            tempAtt.Subtract(removed);
            return true;
        }
        else
            return false;
    }
}
