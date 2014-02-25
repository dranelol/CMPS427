using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    public float currentHP = 50f; // Currently unused.
    public float maxHP = 50f; // Currently unused.
	public Attributes currentAtt; // The entity's current total attributes
	public Attributes equipAtt; // Attribute changes that are added on from equipment stat changes
	public Attributes buffAtt; // Attribute changes that are added on from buffs/debuffs
	
	private Dictionary<equipSlots.slots, Attributes> equipmentStats = new Dictionary<equipSlots.slots, Attributes>();
	
	/// <summary>
	/// Creates the entity with a given set of base attributes,
	/// </summary>
	/// <param name="att">An Attributes object containing the base stats of this entity.</param>
	public Entity(Attributes att)
	{
        //currentHP = 20f;
		currentAtt = att;
		equipAtt = new Attributes();
	}

    void Start()
    {
        currentHP = 50f;
        maxHP = 50f;
    }
	
	/// <summary>
	/// Add the attribute changes of an item to the entity. The item must correlate to one of the equipment slots,
	/// Head, Chest, Legs, Feet, Main, Off. Attribute changes are taken as an attributes object. Returns
	/// false if the slot is already filled.
	/// </summary>
	/// <param name="slot">The equipment slot being filled.</param>
	/// <param name="itemAtt">The attributes of the item being equipped.</param>
	/// <returns></returns>
	public bool addEquipment(equipSlots.slots slot , Attributes itemAtt)
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
	public bool removeEquipment(equipSlots.slots slot)
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

    /// <summary>
    /// Check if the entity is dead or not
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return currentHP <= 0;
    }

}
