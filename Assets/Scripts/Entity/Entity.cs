using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    /* Buff List - list of timed attribute changes. Used to remove buffs after a given time (I dunno how to do dat)
     * Current Attribute object
     * Temp Attribute object - contains all affects entity is currently under (Current - Temp = Base Attributes) */

    ArrayList bufflist;
    Attributes currentAtt;
    Attributes tempAtt;
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
    /// <param name="att">An Attributes object containing the base stats of this entity.</param>
    public Entity(Attributes att)
    {
        currentAtt = att;
        equipAtt = new Attributes();
    }

    /// <summary>
    /// Add the attributes of an equipment item to the character.
    /// </summary>
    /// <param name="equ">The attributes object from an item being equipped.</param>
    public void addEquipmentAtt(Attributes equ)
    {
        currentAtt.Add(equ);
        tempAtt.Add(equ);
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
    /// Removing the attributes of an equipment item from the character.
    /// </summary>
    /// <param name="equ">The attribute object from the item being unequipped.</param>
    public void removeEquipmentAtt(Attributes equ)
    {
        currentAtt.Subtract(equ);
        tempAtt.Subtract(equ);
    }

    public void addBuff(Attributes equ, float duration)
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
}
