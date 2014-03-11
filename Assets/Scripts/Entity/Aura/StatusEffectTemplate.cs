using UnityEngine;
using System.Collections;

public class StatusEffectTemplate : Aura

{
    #region Template Constants

    /* -------------------------------------------SPELL EFFECT INFO----------------------------------------------------- *
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    /* Use these fields to enter information about the status effect. These fields are for convenience only and should **NEVER**
     * be used for anything other than outlining status effect information in one place. The base class stores properties to use
     * when referencing status effect information. Use those instead. */

    private const string TEMPLATE_AURA_NAME = "name"; // Name of the status effect (must be non-empty and unique)
    private const string TEMPLATE_AURA_DESCRIPTION = "description"; // Description of the status effect (should be non-empty)
    private const string TEMPLATE_AURA_FLAVOR_TEXT = "..."; // Flavor text for the status effect (optional)
    private const int TEMPLATE_AURA_MAXIMUM_NUMBER_OF_STACKS = 1; // The number of times this effect can stack. Must be between 1 and 99 (inclusive)
    private const int TEMPLATE_AURA_DURATION = 10; // The number of seconds this aura will remain on a target. The duration is an INTEGER because 
                                                   // status effects should have a finite number of seconds for the duration for simplicity.

    #endregion

    #region Constructors

    /// <summary>
    /// Use this constructor to create a prototype of this aura. The object returned by this constructor will strictly be used as 
    /// a prototype to copy from when applying this aura to an entity. This prototype's methods may not be called. The id parameter
    /// should be the integer key mapped to this prototype in its containing data structure.
    /// </summary>
    /// <param name="id">The unique integer ID.</param>
    public StatusEffectTemplate(int id)
        : base(id, TEMPLATE_AURA_NAME, TEMPLATE_AURA_DESCRIPTION, TEMPLATE_AURA_FLAVOR_TEXT, TEMPLATE_AURA_MAXIMUM_NUMBER_OF_STACKS, 
        TEMPLATE_AURA_DURATION,

        /* ----------------------------------------MODIFY THE REST HERE------------------------------------------------- *
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        /* * * * * * * This this a buff or debuff? * * * * * * * */
        Aura.AuraType.Buff, 

        /* * * * * * * Should health be incremented/decremented over time? * * * * * * * */
        new HealthTick(HealthTick.ModType.None), 
        
        /* * * * * * * What attributes does this status effect modify? * * * * * * * */
        new AttributeModification(Attributes.Stats.HEALTH, Aura.AttributeModification.ModType.Percentage, 0.5f))
    { }
    

    
    #endregion
}
