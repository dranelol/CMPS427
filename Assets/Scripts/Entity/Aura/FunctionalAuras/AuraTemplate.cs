using UnityEngine;
using System.Collections;

sealed public class AuraTemplate : Aura
{
    #region Template Constants

    /* ---------------------------------------------------AURA INFO----------------------------------------------------- *
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    /* Use these fields to enter information about the aura. These fields are for convenience only and should **NEVER**
     * be used for anything other than outlining status effect information in one place. The base class stores properties to use
     * when referencing status effect information. Use those instead. */

    internal const string TEMPLATE_AURA_NAME = "name"; // Name of the status effect (must be non-empty and unique)
    internal const string TEMPLATE_AURA_DESCRIPTION = "description"; // Description of the status effect (should be non-empty)
    internal const string TEMPLATE_AURA_FLAVOR_TEXT = "..."; // Flavor text for the status effect (optional)
    internal const string TEMPLATE_AURA_ICON_TEXTURE_NAME = "default_aura_texture.png"; // The name of the texture for this aura to be displayed on the GUI.
    internal const AuraType TEMPLATE_AURA_AURATYPE = AuraType.Buff; // The type of aura, buff or debuff.
    internal const int TEMPLATE_AURA_MAXIMUM_NUMBER_OF_STACKS = 1; // The number of times this effect can stack. Must be between 1 and 99 (inclusive)
    internal const int TEMPLATE_AURA_DURATION = 10; // The number of seconds this aura will remain on a target. The duration is an INTEGER because 
                                                   // status effects should have a finite number of seconds for the duration for simplicity.

    #endregion

    #region Constructors

    /// <summary>
    /// Use this constructor to create a prototype of this aura. The object returned by this constructor will strictly be used as 
    /// a prototype to copy from when applying this aura to an entity. This prototype's methods may not be called. The id parameter
    /// should be the integer key mapped to this prototype in its containing data structure and the programmer should ensure the 
    /// given name is unique.
    /// </summary>
    /// <param name="id">The unique integer ID.</param>
    public AuraTemplate(int id)
        : base(id, TEMPLATE_AURA_NAME, TEMPLATE_AURA_DESCRIPTION, TEMPLATE_AURA_FLAVOR_TEXT, TEMPLATE_AURA_ICON_TEXTURE_NAME,
        TEMPLATE_AURA_AURATYPE, TEMPLATE_AURA_MAXIMUM_NUMBER_OF_STACKS, TEMPLATE_AURA_DURATION)

        /* ----------------------------------------MODIFY THE REST HERE------------------------------------------------- *
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    { }

    #endregion

    #region Events

    #endregion

    #region Methods

    /// <summary>
    /// Call this from the aura manager to get a unique copy of this aura from the prototype. Target, caster, and prototype cannot be null."
    /// </summary>
    /// <param name="target">The target of the aura.</param>
    /// <param name="caster">The caster of the aura.</param>
    /// <param name="prototpe">The prototype from which to clone.</param>
    /// <returns></returns>
    public Aura Clone(Entity target, Entity caster, Aura prototpe)
    {
        return new AuraTemplate(target, caster, prototpe);
    }

    #endregion

    #region Private Constructor

    private AuraTemplate(Entity target, Entity caster, Aura prototype) : base(target, caster, prototype) { }

    #endregion
}