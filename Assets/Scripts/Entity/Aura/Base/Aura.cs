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

    private AuraType _type; // The type, debuff or buff, of this aura.
    public AuraType Type
    {
        get { return _type; }
    }

    #endregion

    #region Aura Properies

    private bool _isStaticAura; // Determines if this aura is static or timed.
    public bool IsStaticAura
    {
        get { return _isStaticAura; }
    }

    private int _duration; // The maximum amount of time remaining this aura can have.
    public int Duration
    {
        get { return _duration; }
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

    private int _timeRemaining; // The time remaining before the aura will fallout. Will be 0 if the aura is static.
    public int TimeRemaining
    {
        get { return _timeRemaining; }
    }

    private int _stackCount; // The current number of stacks of this aura the entity possesses. 
    public int StackCount
    {
        get { return _stackCount; }
    }

    private List<Module> _allModules;
    private List<TickAttribute> _tickAttributes;

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
    /// <param name="duration">The duration of the aura. If the given value is 0 or Mathf.Infinity, the aura will be treated as static and will 
    /// remain on the entity until it is removed by a manager. Otherwise, the given value will be used as the duration for the effect and will 
    /// be treated as a non-static aura. The maxium duration for non-static auras is 1 hour. Static auras will have their stack limit clamped to
    /// one and will restrict an entity to only having one instance of this aura for any caster. Non-static auras may be applied multiple times 
    /// to the same entity given that they are from differing sources (external stacking). Non-static auras applied by the same caster will 
    /// stack up to the stack limit defined below (internal stacking).</param>
    /// <param name="stackLimit">The maximum number of stacks for this aura. Clamped between 1 and 99. This will only affect non-static
    /// auras. Static auras always have a stack limit of 1.</param>
    protected Aura(int id, string name, string description, string flavorText, string textureFileName, AuraType type, int duration, int stackLimit, params Module[] auraMods)
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
            _stackLimit = MINIMUM_NUMBER_OF_STACKS;
        }

        else
        {
            _isStaticAura = false;
            _duration = Mathf.Clamp(duration, MINIMUM_DURATION, MAXIMUM_DURATION);
            _stackLimit = Mathf.Clamp(stackLimit, MINIMUM_NUMBER_OF_STACKS, MAXIMUM_NUMBER_OF_STACKS);
        }

        _allModules = new List<Module>();
        _tickAttributes = new List<TickAttribute>();

        
        foreach (Aura.Module module in auraMods)
        {
            _allModules.Add(module);

            if (module.GetType().BaseType == typeof(TickAttribute))
            {
                _tickAttributes.Add((TickAttribute)module);
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

        _timeRemaining = 0;
        _stackCount = 0;

        _isPrototype = false;
    }

    #endregion

    #region Methods

    #region Event Methods

    /// <summary>
    /// Attempts to add the number of stacks given by count and refreshes the duration. If the stack count increases as a result of calling 
    /// this function, all modules will be updated to perform the correct actions. Count must be greater than 0. When overriding this function,
    /// you MUST call the base method first.
    /// </summary>
    /// <param name="count">The number of applications to apply. If count is not given, count is 0, or count is greater than the stack limit,
    /// this will add the maximum number of stacks allowed by the stack limit.</param>
    public virtual void Apply(int count = 1)
    {
        if (_isPrototype)
        {
            throw new InvalidOperationException("Cannot call aura methods for prototypes.");
        }

        if (count > 0) // Check input
        {
            if (_stackCount == 0) // If this is the first time this aura is being applied,
            {
                OnStart(); // Call OnStart event
            }

            if (_stackCount < _stackLimit) // If the stack count is not at the limit,
            {
                _stackCount = Mathf.Clamp(_stackCount + count, MINIMUM_NUMBER_OF_STACKS, _stackLimit); // Update the stack count
                OnUpdate(); // Call OnUpdate event
            }

            if (!_isStaticAura) // If the aura is non-static
            {
                _timeRemaining = _duration; // Refresh the duration
            }
        }

        else
        {
            throw new ArgumentOutOfRangeException("The number applications applied must be greater than 0.");
        }
    }

    /// <summary>
    /// Called every second by the buff manager to handle Ticks. This returns true if the aura has expired. Calls the OnFalloff 
    /// event in the derived class, then calls the OnRemoval event before returning true.
    /// </summary>
    /// <returns>Returns true if the aura has expired, false otherwise.</returns>
    public virtual bool Tick()
    {
        if (_isPrototype)
        {
            throw new InvalidOperationException("Cannot call aura methods for prototypes.");
        }

        OnTick(); // Call OnTick event

        if (!_isStaticAura) // If the aura is non-static
        {
            _timeRemaining--; // Decrement the time counter

            if (_timeRemaining <= 0) // If the aura has expired,
            {
                OnEnd(); // Call OnEnd event
                return true; // return true
            }
        }

        return false;
    }


    /// <summary>
    /// Removes the given number of stacks from the aura up to the current number of stacks. All modules will be updated to perform correct 
    /// actions. If the number of stacks is now 0, returns true.
    /// </summary>
    /// <returns>Returns true if there are no more applications left, false otherwise.</returns>
    public virtual bool Remove(int count = 1)
    {
        if (_isPrototype)
        {
            throw new InvalidOperationException("Cannot call aura methods for prototypes.");
        }

        if (count > 0) // Check input
        {
            _stackCount = Mathf.Max(_stackCount - count, 0); // Update the stack count

            if (_stackCount > 0) // If there are still stacks remaining
            {
                OnUpdate(); // Call OnUpdate event
                return false; // return false
            }

            else
            {
                OnEnd(); // Call OnEnd event
                return true; // return true
            }
        }

        else
        {
            throw new ArgumentOutOfRangeException("The number applications removed must be greater than 0.");
        }
    }

    #endregion

    #region Private Methods

    private void OnStart()
    {
        foreach (Module module in _allModules)
        {
            module.OnStart(Target);
        }
    }

    private void OnUpdate()
    {
        foreach (Module module in _allModules)
        {
            module.OnUpdate(_stackCount);
        }
    }

    private void OnTick()
    {
        foreach (TickAttribute tickModule in _tickAttributes)
        {
            tickModule.OnTick();
        }
    }

    private void OnEnd()
    {
        foreach (Module module in _allModules)
        {
            module.OnEnd();
        }
    }

    #endregion

    #endregion

    #region Modules

    #region Functional Modules (Top Tier)

    sealed protected class HoT : TickAttribute
    {
        #region Constructors

        /// <summary>
        /// Define a Tick module that heals the target for an amount every second. If ModType is Percentage, the target
        /// will be healed for the given percentage of its health every second for the duration of the effect. If ModType is Value,
        /// the target will be healed by some function of the given magnitude and the caster's ability to heal.
        /// </summary>
        /// <param name="modType">The type of healing to be done.</param>
        /// <param name="magnitude">The percentage of health to heal by if ModType == Percentage  where 0.03 = 3%, or the amount of raw 
        /// healing to be done if ModType == Value, each second. Must be positive. </param>
        public HoT(ModType modType, float magnitude) : base(Attributes.Stats.HEALTH, modType, magnitude, 1) { }

        #endregion

        #region Methods

        public override void OnTick()
        {
            if ((ModType)ModificationType == ModType.Percentage)
            {
                EntityAffected.ModifyHealth(EntityAffected.currentHP * Magnitude * Sign * Count);
            }

            else
            {
                Debug.Log("Static health change for now. Do we need to create a healing formula for this to use?");
                EntityAffected.ModifyHealth(Magnitude * Sign * Count); // THIS NEEDS TO BE UPDATED
            }
        }

        #endregion
    }

    sealed protected class DoT : TickAttribute
    {
        #region Properties

        private DamageType _damageType;
        public DamageType TypeOfDamage
        {
            get { return _damageType; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Define a Tick module that deals the given magnitude of the given damage type to a target 
        /// every second for the duration of the effect.
        /// </summary>
        /// <param name="damageType">The type of damage to deal.</param>
        /// <param name="magnitude">The amount of damage to deal using the given damage type. Must be positive.</param>
        public DoT(DamageType damageType, float magnitude) : base(Attributes.Stats.HEALTH, ModType.Value, magnitude, -1)
        {
            _damageType = damageType;
        }

        /// <summary>
        /// Define a Tick module that deals a percentage of the target's health every second
        /// for the duration of the effect. There is no damage type.
        /// </summary>
        /// <param name="magnitude">The percentage of health to remove each second where 0.03 = 3%. Must be positive.</param>
        public DoT(float magnitude) : base(Attributes.Stats.HEALTH, ModType.Percentage, magnitude, -1)
        {
            _damageType = DamageType.PHYSICAL;
        }

        #endregion

        #region Methods

        public override void OnTick()
        {
            if ((ModType)ModificationType == ModType.Percentage)
            {
                EntityAffected.ModifyHealth(EntityAffected.currentHP * Magnitude * Sign * Count);
            }

            else
            {
                Debug.Log("Static health change for now, use damage formula in the future.");
                EntityAffected.ModifyHealth(Magnitude * Sign * Count); // THIS NEEDS TO GO THROUGH THE DAMAGE FORMULA
            }
        }

        #endregion
    }

    sealed protected class Invigorate : TickAttribute
    {
        #region Constructors

        /// <summary>
        /// Define a Tick module restores a percentage of an entity's resource each second for the duration of the aura.
        /// </summary>
        /// <param name="magnitude">The percentage of resource to restore each second. Must be positive.</param>
        public Invigorate(float magnitude) : base(Attributes.Stats.RESOURCE, ModType.Percentage, magnitude, 1) { }

        #endregion

        #region Methods

        public override void OnTick()
        {
            Debug.Log("Entity class needs implementation for adding/substracting resources.");
        }

        #endregion
    }

    sealed protected class Exhaust : TickAttribute
    {
        #region Constructors

        /// <summary>
        /// Define a Tick module removes a percentage of an entity's resource each second for the duration of the aura.
        /// </summary>
        /// <param name="magnitude">The percentage of resource to remove each second. Must be positive.</param>
        public Exhaust(float magnitude) : base(Attributes.Stats.RESOURCE, ModType.Percentage, magnitude, -1) { }

        #endregion

        #region Methods

        public override void OnTick()
        {
            Debug.Log("Entity class needs implementation for adding/substracting resources.");
        }

        #endregion
    }

    sealed protected class FortifyAttribute : StaticAttribute
    {
        #region Constructors

        /// <summary>
        /// Define an AttributeModification module that increases an attribute once while the aura is in effect and returns the affected 
        /// attributes to normal once the aura expires.
        /// </summary>
        /// <param name="attribute">The attribute to fortify.</param>
        /// <param name="magnitude">The percentage of the attribute to alter where 0.03 = 3%. Must be greater than 0. </param>
        public FortifyAttribute(Attributes.Stats attribute, float magnitude) : base(attribute, ModType.Percentage, magnitude, 1) { }

        #endregion
    }

    sealed protected class DamageAttribute : StaticAttribute
    {
        #region Constructors

        /// <summary>
        /// Define an AttributeModification module that decreases an attribute once while the aura is in effect and returns the affected 
        /// attributes to normal once the aura expires.
        /// </summary>
        /// <param name="attribute">The attribute to damage.</param>
        /// <param name="magnitude">The percentage of the attribute to alter where 0.03 = 3%. Must be greater than 0. </param>
        public DamageAttribute(Attributes.Stats attribute, float magnitude) : base(attribute, ModType.Percentage, magnitude, -1) { }

        #endregion
    }

    #endregion

    #region Abstract Modules (Lower Tier)

    #region Tier 0 (Base Module)

    protected abstract class Module
    {
        #region Properties

        private Entity _entityAffected;
        public Entity EntityAffected
        {
            get { return _entityAffected; }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
        }

        #endregion

        #region Constructor

        protected Module() 
        { 
            _entityAffected = null; 
        }

        #endregion

        #region Virtual Methods

        public virtual void OnStart(Entity entity) 
        { 
            _entityAffected = entity; 
        }

        public virtual void OnUpdate(int count) 
        {
            _count = Mathf.Max(count, 0);
        }

        public virtual void OnEnd() 
        {
            _count = 0;
        }

        #endregion
    }

    #endregion 

    #region Tier 1 Modules

    protected abstract class AttributeModification : Aura.Module
    {
        #region Properties

        private Attributes.Stats _attribute;
        public Attributes.Stats Attribute
        {
            get { return _attribute; }
        }

        private ModType _modType;
        public ModType ModificationType
        {
            get { return _modType; }
        }

        private float _magnitude;
        public float Magnitude
        {
            get { return _magnitude; }
        }

        private short _sign;
        public short Sign
        {
            get { return _sign; }
        }

        #endregion

        #region Constructors

        protected AttributeModification(Attributes.Stats attribute, ModType modType, float magnitude, short sign) : base()
        {
            _attribute = attribute;
            _modType = modType;

            if (magnitude > 0)
            {
                _magnitude = magnitude;
            }

            else
            {
                _magnitude = 0;
                throw new ArgumentOutOfRangeException("You may only create an AttributeModification obect with a positive magnitude.");
            }

            _sign = sign;
        }

        #endregion
    }

    #endregion

    #region Tier 2 Modules

    protected abstract class TickAttribute : AttributeModification
    {
        #region Constructors

        protected TickAttribute(Attributes.Stats attribute, ModType modType, float magnitude, short sign) : base(attribute, modType, magnitude, sign) { }

        #endregion

        #region Virtual Methods

        public abstract void OnTick();

        #endregion
    }

    protected abstract class StaticAttribute : AttributeModification
    {
        #region Properties

        private float _attributeSnapshot;
        private float _attributeChange;

        #endregion

        #region Constructors

        protected StaticAttribute(Attributes.Stats attribute, ModType modType, float magnitude, short sign) : base(attribute, modType, magnitude, sign) 
        {
            _attributeSnapshot = 0;
            _attributeChange = 0;
        }

        #endregion

        #region Abstract Methods

        public override void OnStart(Entity entity)
        {
            base.OnStart(entity);
            _attributeSnapshot = EntityAffected.currentAtt.GetValue(Attribute);
        }

        public override void OnUpdate(int count)
        {
            base.OnUpdate(count);

            if (Count > 0)
            {
                float newAttributeChange = _attributeSnapshot * Magnitude * Count * Sign; // Calculate new change in attribute
                float appliedAttribute = newAttributeChange - _attributeChange; // Get the difference from the old change

                Attributes newAttribute = new Attributes();
                // do something to newAttribute
                //EntityAffected.currentAtt.Add(newAttribute); // Apply it

                _attributeChange = newAttributeChange; // Set the old change to the new change
            }
        }

        public override void OnEnd()
        {
            base.OnEnd();

            Attributes newAttribute = new Attributes();
            // do something to newAttribute (_attributeChange *= -1)
            //EntityAffected.currentAtt.Add(newAttribute); // Apply it
            _attributeChange = 0;
        }

        #endregion
    }

    #endregion

    #endregion

    #endregion
}

#region Enums

public enum AuraType
{
    Buff,
    Debuff
}

public enum ModType
{
    Percentage,
    Value,
}

#endregion