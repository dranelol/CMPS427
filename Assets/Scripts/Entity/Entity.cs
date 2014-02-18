using UnityEngine;
using System.Collections;

public interface Entity
{
    /* Buff List - list of timed attribute changes. Used to remove buffs after a given time (I dunno how to do dat)
     * Current Attribute object
     * Temp Attribute object - contains all affects entity is currently under (Current - Temp = Base Attributes) */

    ArrayList bufflist;
    Attributes currentAtt;
    Attributes tempAtt;

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
    /// Add the attributes of an equipment item to the character.
    /// </summary>
    /// <param name="equ">The attributes object from an item being equipped.</param>
    public void addEquipmentAtt(Attributes equ)
    {
        currentAtt.Add(equ);
        tempAtt.Add(equ);
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

    }
}
