using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public abstract class Aura
{
    #region Constants

    public const string DEFAULT_AURA_TEXTURE_FILE_PATH = "Assets/Resources/Aura Icons/";
    public const string DEFAULT_ICON_TEXTURE_FILE_NAME = "default_aura_texture.png";

    public const string DEFAULT_DESCRIPTION = "description...";
    public const string DEFAULT_FLAVOR_TEXT = "";

    public const int MINIMUM_NUMBER_OF_STACKS = 1;
    public const int MAXIMUM_NUMBER_OF_STACKS = 99;

    public const int MINIMUM_DURATION = 1;
    public const int MAXIMUM_DURATION = 3600;

    #endregion

    #region Properties

    #region Information

    private int _id; // A unique integer ID for this aura.
    public int ID
    {
        get { return _id; }
    }

    private string _name; // A unique name for this aura.
    public string Name
    {
        get { return _name; }
    }

    private string _description; // A description of the aura. Not required.
    public string Description
    {
        get { return _description; }
    }

    private string _flavorText; // A flavor text for the aura. Not required.
    public string FlavorText
    {
        get { return _flavorText; }
    }

    private Texture2D _icon; // The 2D texture for the aura.
    public Texture2D Icon
    {
        get { return _icon; }
    }

    #endregion

    #region Aura Properies

    private AuraType _type; // The type, debuff or buff, of this aura.
    public AuraType Type
    {
        get { return _type; }
    }

    private bool _isStaticAura; // Determines if this aura is static or timed.
    public bool IsStaticAura
    {
        get { return _isStaticAura; }
    }

    private int _timeRemaining; // The time remaining before the aura will fallout. Will be 0 if the aura is static.
    public int TimeRemaining
    {
        get { return _timeRemaining; }
    }

    private int _duration; // The maximum amount of time remaining this aura can have.
    public int Duration
    {
        get { return _duration; }
    }

    private int _stackCount; // The current number of stacks of this aura the entity possesses. 
    public int StackCount
    {
        get { return _stackCount; }
    }

    private int _stackLimit; // The maximum number of stacks of this aura en entity may possess per caster.
    public int StackLimit
    {
        get { return _stackLimit; }
    }

    #endregion

    #region Instance Properties

    private bool _isPrototype; // Determines if this aura is a prototype or an active affect. 
    public bool IsPrototype
    {
        get { return _isPrototype; }
    }

    private Entity _target; // The target whom this aura is affecting.
    public Entity Target
    {
        get { return _target; }
    }

    private Entity _caster; // The caster, if any, of this aura.
    public Entity Caster
    {
        get { return _caster; }
    }

    private bool _isLinked; // A boolean determining if this aura is linked to another one.
    public bool IsLinked
    {
        get { return _isLinked; }
    }

    private Aura _linkedAura; // The aura that is linked to this aura. When an aura is linked, they can must coexist. If one is removed, the other will be removed as well.
    public Aura LinkedAura
    {
        get { return _linkedAura; }
    }

    private HoT _hoTObject;
    public HoT HoTObject
    {
        get { return _hoTObject; }
    }

    private DoT _doTObject;
    public DoT DoTObject
    {
        get { return _doTObject; }
    }

    private List<AttributeModification> _attributeMods;
    public IList<AttributeModification> AttributeModifications
    {
        get { return _attributeMods.AsReadOnly(); }
    }

    #endregion

    #endregion

    #region Constructors

    /// <summary>
    /// This is the base constructor used by the programmer when designing a new status effect for prototyping. The programmer will provide all input 
    /// except the ID which will be passed by derived class's constructor so the ID which will be the integer key mapped to this effect in the 
    /// manager storing all status effect protoypes. The status effect will inherit the Aura class. Strict input validation is performed and 
    /// exceptions are thrown for invalid input. Since the instance of this object created by this constructor will only be used as a protoype, 
    /// not a functional aura, all input must be correct.</summary>
    /// <param name="id">The unique integer ID for this aura. Cannot be negative.</param>
    /// <param name="name">The name of this aura. This must be unique just like the integer ID.</param>
    /// <param name="description">The description of the aura. Can be empty.</param>
    /// <param name="flavorText">The flavor text of the aura. Can be empty.</param>
    /// <param name="textureFileName">The name of the texture for the GUI representation.</param>
    /// <param name="type">The type of aura: buff or debuff.</param>
    /// <param name="duration">The duration of the aura. If the given value is greater than 0 and not Mathf.Infinity, the aura will be
    /// treated as non-static and will falloff after the duration given. Otherwise, the aura will be treated as static and will remain on
    /// the entity until it is removed by a manager. Static auras will have their stack limit clamped to one and will restrict an entity 
    /// to only having one instance of this aura for any caster. Non-static auras may be applied multiple times to the same entity given 
    /// that they are from differing sources (external stacking). Non-static auras applied by the same caster will stack up to the stack
    /// limit defined (internal stacking).</param>
    /// <param name="stackLimit">The maximum number of stacks for this aura. Clamped between 1 and 99. This will only affect non-static
    /// auras. Static auras always have a stack limit of 1.</param>
    protected Aura(int id, string name, string description, string flavorText, string textureFileName, AuraType type, int duration, int stackLimit, params AuraModule[] auraMods)
    {
        if (id < 0)
        {
            throw new FormatException("The aura ID cannot be a negative number");
        }

        _id = id;

        _name = "";

        if (!String.IsNullOrEmpty(name))
        {
            throw new FormatException("The name cannot be null or empty");
        }

        _description = "";

        if (description != null)
        {
            _description = description;
        }

        _flavorText = "";

        if (flavorText != null)
        {
            _flavorText = flavorText;
        }

        if (textureFileName != null)
        {
            try
            {
                _icon = (Texture2D)Resources.Load(DEFAULT_AURA_TEXTURE_FILE_PATH + textureFileName, typeof(Texture2D));
            }

            catch
            {
                _icon = (Texture2D)Resources.Load(DEFAULT_AURA_TEXTURE_FILE_PATH + DEFAULT_ICON_TEXTURE_FILE_NAME, typeof(Texture2D));
                Debug.LogWarning("Could not load texture asset '" + textureFileName + "' for the " + _name + " aura. Using default texture.");
            }
        }

        _type = type;

        if (_duration <= 0 || _duration == Mathf.Infinity)
        {
            _isStaticAura = true;
            _duration = 0;
            _stackLimit = 1;
        }

        else
        {
            _isStaticAura = false;
            _duration = duration;
            _stackLimit = Mathf.Clamp(stackLimit, 1, MAXIMUM_NUMBER_OF_STACKS);
        }

        _hoTObject = null;
        _doTObject = null;
        _attributeMods = new List<AttributeModification>();

        foreach (AuraModule module in auraMods)
        {
            if (module.GetType() == typeof(HoT))
            {
                if (_hoTObject == null)
                {
                    _hoTObject = (HoT)module;
                }

                else
                {
                    Debug.LogWarning("A duplicate HoT object was defined for " + _name + ". It will be discarded.");
                }
            }

            else if (module.GetType() == typeof(DoT))
            {
                if (_doTObject == null)
                {
                    _doTObject = (DoT)module;
                }

                else
                {
                    Debug.LogWarning("A duplicate DoT object was defined for " + _name + ". It will be discarded.");
                }
            }

            else if (module.GetType() == typeof(AttributeModification))
            {
                _attributeMods.Add((AttributeModification)module);
            }

            else
            {
                Debug.LogWarning("Undefined AuraModule discarded.");
            }
        }

        _timeRemaining = 0;
        _stackCount = 0;

        _isPrototype = true;
        _caster = null;
        _target = null;
    }

    protected Aura(Entity target, Entity caster, Aura protoType)
    {
        if (target == null) 
        {
            throw new ArgumentNullException("The target entity is null.");
        }

        else if (caster == null)
        {
            throw new ArgumentNullException("The caster entity is null.");
        }

        _target = target;
        _caster = caster;
        _id = protoType.ID;
        _name = protoType.Name;
        _description = protoType.Description;
        _flavorText = protoType.FlavorText;
        _icon = protoType.Icon;
        _type = protoType.Type;
        _duration = protoType.Duration;

        _isStaticAura = protoType.IsStaticAura;
        _stackLimit = protoType.StackLimit;
        _hoTObject = protoType.HoTObject;
        _doTObject = protoType.DoTObject;

        _attributeMods = new List<AttributeModification>();

        foreach (AttributeModification attributeMod in protoType.AttributeModifications)
        {
            _attributeMods.Add(new AttributeModification(attributeMod, target));
        }

        _timeRemaining = 0;
        _stackCount = 0;

        _isPrototype = false;
    }

    private void ConstructorHelper() { }

    #endregion

    #region Control Methods

    /// <summary>
    /// Attempts to add the given number of stacks to the aura. Calling this method will add the given count to the number of applications
    /// up to the stack limit in addition to refreshing the duration. Count is an optional parameter which defaults to 1. If the number of 
    /// stacks increases, attributes will be updated and the OnApplication event will be called. If this is the first application of the aura, 
    /// the OnStart event will be called in addition to, and before, the OnApplication event.
    /// </summary>
    /// <param name="count">The number of applications to apply.</param>
    public void Apply(int count = 1)
    {
        if (_isPrototype)
        {
            throw new InvalidOperationException("Cannot call aura methods for prototypes.");
        }

        if (count > 0)
        {

        }
    }

    /// <summary>
    /// Removes the given number of stacks from the aura. Count is an optional parameter. If count is not passed or if count is 0, all 
    /// current stacks will be removed from the aura. If there are no more stacks on the aura, the OnRemoval event will be called but not
    /// the OnFalloff event.
    /// </summary>
    /// <returns>Returns true if there are no more applications left, false otherwise.</returns>
    public bool Remove(int count = 0)
    {
        if (_isPrototype)
        {
            throw new InvalidOperationException("Cannot call aura methods for prototypes.");
        }

        return false;
    }

    #endregion

    #region Event Methods

    private void OnStart()
    {

    }

    private void OnApplication()
    {


        // Do stuff

        OnApplicationAbstract();
    }

    /// <summary>
    /// Called every second by the buff manager to handle DoTs and HoTs. This returns true if the aura has expired. Calls the OnFalloff 
    /// event in the derived class, then calls the OnRemoval event before returning true.
    /// </summary>
    /// <returns>Returns true if the aura has expired, false otherwise.</returns>
    public bool OnTick()
    {
        OnTickAbstract();

        if (!_isStaticAura)
        {
            _timeRemaining--;

            if (_timeRemaining <= 0)
            {
                return true;
            }
        }

        return false;
    }

    private void OnFalloff()
    {

        // Do stuff

        OnFalloffAbstract();
    }

    private void OnRemoval()
    {

    }

    #endregion

    #region Virtual Event Methods

    protected virtual void OnStartAbstract() { }

    protected virtual void OnApplicationAbstract() { }

    protected virtual void OnTickAbstract() { }

    protected virtual void OnFalloffAbstract() { }

    protected virtual void OnRemovalAbstract() { }

    #endregion
}

#region Enums

public enum AuraType
{
    Buff,
    Debuff
}

#endregion