using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

public abstract class Aura
{
    #region Constants

    public const string DEFAULT_AURA_TEXTURE_FILE_PATH = "Aura Icons/";
    public const string DEFAULT_AURA_PARTICLE_EFFECT_FILE_PATH = "Aura Particles/";
    public const string DEFAULT_ICON_TEXTURE_FILE_NAME = "default_aura_texture";
    public const string DeFAULT_AURA_PARTICLE_EFFECT_FILE_NAME = "default_aura_particle_effect";

    public const string DEFAULT_DESCRIPTION = "description...";
    public const string DEFAULT_FLAVOR_TEXT = "";

    public const int MINIMUM_NUMBER_OF_STACKS = 1;
    public const int MAXIMUM_NUMBER_OF_STACKS = 99;

    public const int MINIMUM_DURATION = 1;
    public const int MAXIMUM_DURATION = 3600;

    #endregion

    #region Properties

    #region Information

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

    private GameObject _particleEffect; // The particle effect displayed when the aura is applied
    public GameObject ParticleEffect
    {
        get { return _particleEffect; }
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

    private int _initialStackCount; // The number of stacks this aura has when it is first applied.
    public int InitialStackCount
    {
        get { return _initialStackCount; }
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
    private List<Tick> _tickModules;

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
    /// <param name="duration">The duration of the aura. If the given value is 0, the aura will be treated as static and will 
    /// remain on the entity until it is removed by a manager. Otherwise, the given value will be used as the duration for the effect and will 
    /// be treated as a non-static aura. The maxium duration for non-static auras is 1 hour. Static auras will have their stack limit clamped to
    /// one and will restrict an entity to only having one instance of this aura for any caster. Non-static auras may be applied multiple times 
    /// to the same entity given that they are from differing sources (external stacking). Non-static auras applied by the same caster will 
    /// stack up to the stack limit defined below (internal stacking).</param>
    /// <param name="stackLimit">The maximum number of stacks for this aura. Clamped between 1 and 99. This will only affect non-static
    /// auras. Static auras always have a stack limit of 1.</param>
    protected Aura(string name, string description, string flavorText, string textureFileName, string particleEffectFileName, AuraType type, int duration, int stackLimit, int initialStackCount, params Module[] auraMods)
    {
        if (String.IsNullOrEmpty(name))
        {
            throw new FormatException("The name cannot be null or empty");
        }

        _name = name;

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

        _icon = (Texture2D)Resources.Load(DEFAULT_AURA_TEXTURE_FILE_PATH + textureFileName, typeof(Texture2D));

        if (_icon == null)
        {
            _icon = (Texture2D)Resources.Load(DEFAULT_AURA_TEXTURE_FILE_PATH + DEFAULT_ICON_TEXTURE_FILE_NAME, typeof(Texture2D));
            Debug.LogWarning("Could not load texture asset '" + textureFileName + "' for the " + _name + " aura. Using default texture.");
        }

        _particleEffect = (GameObject)Resources.Load(DEFAULT_AURA_PARTICLE_EFFECT_FILE_PATH + particleEffectFileName, typeof(GameObject));

        if (_particleEffect == null)
        {
            _particleEffect = (GameObject)Resources.Load(DEFAULT_AURA_PARTICLE_EFFECT_FILE_PATH + DeFAULT_AURA_PARTICLE_EFFECT_FILE_NAME, typeof(GameObject));
            Debug.LogWarning("Could not load particle effect '" + particleEffectFileName + "' for the " + _name + " aura. Using default particle effect.");
        }

        _type = type;

        if (duration <= 0)
        {
            _isStaticAura = true;
            _duration = 0;
            _stackLimit = MINIMUM_NUMBER_OF_STACKS;
            _initialStackCount = 1;
        }

        else
        {
            _isStaticAura = false;
            _duration = Mathf.Clamp(duration, MINIMUM_DURATION, MAXIMUM_DURATION);
            _stackLimit = Mathf.Clamp(stackLimit, MINIMUM_NUMBER_OF_STACKS, MAXIMUM_NUMBER_OF_STACKS);
            _initialStackCount = Mathf.Clamp(initialStackCount, MINIMUM_NUMBER_OF_STACKS, _stackLimit);
        }

        _allModules = new List<Module>();
        _tickModules = new List<Tick>();

        
        foreach (Module module in auraMods)
        {
            _allModules.Add(module);

            if (Inherits(module, typeof(Tick)))
            {
                _tickModules.Add((Tick)module);
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
        _name = protoType.Name;
        _description = protoType.Description;
        _flavorText = protoType.FlavorText;
        _icon = protoType.Icon;
        _particleEffect = protoType.ParticleEffect;
        _type = protoType.Type;
        _duration = protoType.Duration;

        _isStaticAura = protoType.IsStaticAura;
        _stackLimit = protoType.StackLimit;

        _allModules = new List<Module>();
        _tickModules = new List<Tick>();

        foreach (Module mod in protoType._allModules)
        {
            Module modCopy = mod.Copy();

            if (Inherits(modCopy, typeof(Tick)))
            {
                _tickModules.Add((Tick)modCopy);
            }

            _allModules.Add(modCopy);
            
        }
        Debug.Log("length of tickmodules: " + _tickModules.Count);
        _timeRemaining = 0;
        _stackCount = 0;
        _initialStackCount = protoType.InitialStackCount;

        _isPrototype = false;
    }

    #endregion

    #region Methods

    #region Virtual Methods

    public virtual Aura Clone(Entity target, Entity caster, Aura prototpe)
    {
        return null;
    }
  
    #endregion

    #region Control Methods

    /// <summary>
    /// Called when the aura is first applied. 
    /// </summary>
    public IEnumerator Activate()
    {
        if (!_isPrototype)
        {
            _stackCount = _initialStackCount;
            _timeRemaining = _duration;

            OnStart();

            while (_stackCount > 0)
            {
                OnTick(); // Call OnTick event

                if (!_isStaticAura) // If the aura is non-static
                {
                    _timeRemaining--; // Decrement the time counter

                    if (_timeRemaining <= 0) // If the aura has expired,
                    {
                        _target.GetComponent<EntityAuraManager>().Remove(_name, _caster);
                        yield return null;
                    }
                }

                yield return new WaitForSeconds(1);
            }
        }

        else
        {
            throw new NullReferenceException("Cannot activate a prototype Aura");
        }
    }

    /// <summary>
    /// Attempts to add the number of stacks given by count and refreshes the duration. If the stack count increases as a result of calling 
    /// this function, all modules will be updated to perform the correct actions. Count must be greater than 0. When overriding this function,
    /// you MUST call the base method first.
    /// </summary>
    /// <param name="count">The number of applications to apply. If count is not given, count is 0, or count is greater than the stack limit,
    /// this will add the maximum number of stacks allowed by the stack limit.</param>
    public void Add(int count = 1)
    {
        if (_isPrototype)
        {
            throw new InvalidOperationException("Cannot call aura methods for prototypes.");
        }

        count = Mathf.Max(count, MINIMUM_NUMBER_OF_STACKS);
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

    /// <summary>
    /// Removes the given number of stacks from the aura up to the current number of stacks. All modules will be updated to perform correct 
    /// actions. If the number of stacks is now 0, returns true.
    /// </summary>
    /// <returns>Returns true if there are no more applications left, false otherwise.</returns>
    public void Remove(int count = 0)
    {
        if (_isPrototype)
        {
            throw new InvalidOperationException("Cannot call aura methods for prototypes.");
        }

        if (count <= 0)
        {
            count = _stackCount;
        }

        _stackCount = Mathf.Max(_stackCount - count, 0); // Update the stack count

        if (_stackCount > 0) // If there are still stacks remaining
        {
            OnUpdate(); // Call OnUpdate event
        }

        else
        {
            OnEnd(); // Call OnEnd event
        }
    }

    #endregion

    #region Module Events

    public void OnStart()
    {
        foreach (Module module in _allModules)
        {
            module.OnStart(Target, Caster, _stackCount);
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
        foreach (Tick tickModule in _tickModules)
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

    #region Private Methods

    private bool Inherits(Module mod, Type type)
    {
        Type _type = mod.GetType();

        while(_type != typeof(Module))
        {
            if (_type != type)
            {
                _type = _type.BaseType;
            }

            else
            {
                return true;
            }
        }

        return false;
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
                EntityAffected.ModifyHealth(EntityAffected.currentAtt.Health * Magnitude * Sign * Count);
            }

            else
            {
                Debug.Log("Static health change for now. Do we need to create a healing formula for this to use?");
                EntityAffected.ModifyHealth(EntityAffected.currentAtt.Health * Magnitude * Sign * Count); // THIS NEEDS TO BE UPDATED
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
                EntityAffected.ModifyHealth(EntityAffected.currentAtt.Health * Magnitude * Sign * Count);
            }

            else
            {
                Debug.Log("Static health change for now, use damage formula in the future.");
                EntityAffected.ModifyHealth(DamageCalc.DamageCalculation(SourceEntity, EntityAffected, Count * Magnitude) * Sign);
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
            EntityAffected.ModifyResource(EntityAffected.currentAtt.Power * Magnitude * Sign * Count);
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
            EntityAffected.ModifyResource(EntityAffected.currentAtt.Power * Magnitude * Sign * Count);
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

        private Entity _sourceEntity;
        public Entity SourceEntity
        {
            get { return _sourceEntity; }
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
        /*
        public Module Copy()
        {
            return (Module)this.MemberwiseClone();
        }
        */
        #endregion

        #region Methods

        public virtual void OnStart(Entity target, Entity source, int count) 
        {
            _count = Mathf.Max(count, 0);
            _entityAffected = target;
            _sourceEntity = source;
        }

        public virtual void OnUpdate(int count) 
        {
            _count = Mathf.Max(count, 0);
        }

        public virtual void OnEnd() 
        {
            _count = 0;
        }

        public Module Copy()
        {
            return (Module)this.MemberwiseClone();
        }

        #endregion
    }

    #endregion 

    #region Tier 1 Modules

    protected abstract class Tick : Module
    {
        #region Constructors

        protected Tick() : base() { }

        #endregion

        #region Methods

        public virtual void OnTick() { }

        #endregion
    }

    protected abstract class Static : Module
    {
        #region Constructors

        protected Static() : base() { }

        #endregion
    }

    #endregion

    #region Tier 2 Modules

    protected abstract class TickAttribute : Tick
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

        protected TickAttribute(Attributes.Stats attribute, ModType modType, float magnitude, short sign) : base()
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

    protected abstract class StaticAttribute : Static
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

        private float _attributeSnapshot;
        public float AttributeSnapshot
        {
            get { return _attributeSnapshot; }
        }

        private float _attributeChange;
        public float AttributeChange
        {
            get { return _attributeChange; }
        }

        #endregion

        #region Constructors

        protected StaticAttribute(Attributes.Stats attribute, ModType modType, float magnitude, short sign) : base() 
        {
            _attributeSnapshot = 0;
            _attributeChange = 0;

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

        #region Abstract Methods

        public override void OnStart(Entity target, Entity source, int count)
        {
            base.OnStart(target, source, count);
            _attributeSnapshot = EntityAffected.currentAtt.GetValue(Attribute);

            CalculateAttributeChange(EntityAffected, Count);
        }

        public override void OnUpdate(int count)
        {
            base.OnUpdate(count);

            CalculateAttributeChange(EntityAffected, Count);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            CalculateAttributeChange(EntityAffected, 0);
        }

        private void CalculateAttributeChange(Entity target, int count)
        {
            float newAttributeChange = _attributeSnapshot * Magnitude * Count * Sign; // Calculate new change in attribute
            float appliedAttribute = newAttributeChange - _attributeChange; // Get the difference from the old change
            Attributes newAttribute = new Attributes();

            newAttribute.ModifyValue(Attribute, appliedAttribute);

            target.ApplyBuff(newAttribute);
            _attributeChange += appliedAttribute;
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