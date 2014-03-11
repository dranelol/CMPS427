using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public abstract class Aura
{
    #region Constants

    public const string DEFAULT_DESCRIPTION = "description...";

    public const string DEFAULT_FLAVOR_TEXT = "";

    public const int MINIMUM_NUMBER_OF_STACKS = 1;
    public const int MAXIMUM_NUMBER_OF_STACKS = 99;

    public const int MINIMUM_DURATION = 1;
    public const int MAXIMUM_DURATION = 3600;

    #endregion

    #region Enums

    public enum AuraType
    {
        Buff,
        Debuff
    }

    #endregion

    #region Properties
    
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

    private bool _isPrototype; // Determines if this aura is a prototype or an active affect. 
    public bool IsPrototype
    {
        get { return _isPrototype; }
    }

    private int _timeRemaining; // The time remaining before the aura will fallout. Will be 0 if the aura is static.
    public int TimeRemaining
    {
        get { return _timeRemaining; }
    }

    private int _numberOfStacks; // The current number of stacks of this aura the entity possesses. 
    public int NumberOfStacks
    {
        get { return _numberOfStacks; } 
    }

    private bool _isStaticAura; // Determines if this aura is static or timed.
    public bool IsStaticAura
    {
        get { return _isStaticAura; }
    }

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

    private int _stackLimit; // The maximum number of stacks of this aura en entity may possess.
    public int StackLimit
    {
        get { return _stackLimit; }
    }

    private AuraType _type; // The type, debuff or buff, of this aura.
    public AuraType Type
    {
        get { return _type; }
    }

    private int _duration; // The maximum amount of time remaining this aura can have.
    public int Duration
    {
        get { return _duration; }
    }

    private HealthTick _healthOverTime; // An object used to describe if, and if then how, the affected entity's health will change over time.
    public HealthTick HealthOverTime
    {
        get { return _healthOverTime; }
    }

    private List<AttributeModification> _attributesAffected; // A list of AttributeMod objects describing how this aura affects the entity.
    public IList<AttributeModification> AttributesAffected
    {
        get { return _attributesAffected.AsReadOnly(); }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// This is the base constructor used by the programmer when designing a new status effect for prototyping. The programmer will provide all input 
    /// except the ID which will be passed by derived class's constructor so the ID which will be the integer key mapped to this effect in the 
    /// manager storing all status effect protoypes. The status effect will inherit the Aura class. Strict input validation is performed and 
    /// exceptions are thrown for invalid input. Since the instance of this object created by this constructor will only be used as a protoype, 
    /// not a functional aura, all input must be correct.</summary>
    /// <param name="id">The unique integer ID for this aura. Cannot be negative.</param>
    /// <param name="name">The name of this aura. Cannot be empty.</param>
    /// <param name="description">The description of the aura. Can be empty.</param>
    /// <param name="flavorText">The flavor text of the aura. Can be empty.</param>
    /// <param name="stackLimit">The maximum number of stacks for this aura. Clamped between 1 and 99.</param>
    /// <param name="type">The type of aura: buff or debuff.</param>
    /// <param name="duration">The default duration of the aura. If the given value is greater than 0, the aura will be
    /// treated as non-static and will falloff after the duration given. Otherwise, the aura will be treated as static and will
    /// remain on the entity until it is removed by a manager. When copying the prototype, an optional duration can be given that will be
    /// used as the duration for that instance of the aura if it should be different from the default duration.</param>
    /// <param name="healthOverTime">A HealthTick object that describes the way the entity's health changes over time. Enter ModType.None
    /// if no change in health should occurr.</param>
    /// <param name="attributesAffected">A list of AttributeMod objects which contains information about how the aura will change the
    /// entity's attributes.</param>
    protected Aura(int id, string name, string description, string flavorText, int stackLimit, int duration, AuraType type, HealthTick healthOverTime, params AttributeModification[] attributesAffected)
    {
        if (id < 0)
        {
            throw new FormatException("The aura ID cannot be a negative number");
        }

        _id = id;

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

        _stackLimit = Mathf.Clamp(stackLimit, 1, MAXIMUM_NUMBER_OF_STACKS);

        _type = type;

        if (_duration <= 0)
        {
            _isStaticAura = true;
            _duration = 0;
        }

        else
        {
            _isStaticAura = false;
            _duration = duration;
        }

        _healthOverTime = healthOverTime;

        _attributesAffected = attributesAffected.ToList();

        _isPrototype = true;
    }

    protected Aura(Entity target, Entity caster)
    {
        _isPrototype = false;

        if (target == null)
        {
            throw new NullReferenceException("The target entity cannot be null");
        }

        /*
        _target = target;
        _caster = caster;
        _numberOfStacks = Mathf.Clamp(initialStackCount, 1, StackLimit);

        if (!protoType._isStaticAura)
        {
            _duration = _timeRemaining = Mathf.Max(duration, 1);
        }

        else
        {
            _duration = _timeRemaining = protoType._duration;
        }

        _isStaticAura = protoType._isStaticAura;
        _healthOverTime = protoType._healthOverTime;
        _id = protoType._id;
        _name = protoType._name;
        _description = protoType._description;
        _flavorText = protoType._flavorText;
        _stackLimit = protoType._stackLimit;
        _type = protoType._type;
        _attributesAffected = protoType._attributesAffected;*/
    }

    #endregion

    #region Public Methods

    public void Apply(int count, int duration = 0)
    {

    }

    public void OnApplication()
    {
        if (_isPrototype)
        {
            throw new InvalidOperationException("OnApplication, OnTick, and OnFalloff cannot be called by a prototype aura.");
        }

        // Do stuff

        OnApplicationAbstract();
        OnTick();
    }

    public bool OnTick()
    {
        if (_isPrototype)
        {
            throw new InvalidOperationException("OnApplication, OnTick, and OnFalloff cannot be called by a prototype aura.");
        }

        if (_healthOverTime != null)
        {
            _target.ModifyHealth(_healthOverTime.GetModification(_target.currentAtt.Health) * _numberOfStacks);
        }

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

    public void OnFalloff()
    {
        if (_isPrototype)
        {
            throw new InvalidOperationException("OnApplication, OnTick, and OnFalloff cannot be called by a prototype aura.");
        }

        // Do stuff

        OnFalloffAbstract();
    }

   

    #endregion

    #region Virtual Methods

    protected virtual void OnApplicationAbstract() { }

    protected virtual void OnTickAbstract() { }

    protected virtual void OnFalloffAbstract() { }

    #endregion

    #region Sub Classes

    public class AttributeModification
    {
        #region Enums

        public enum ModType
        {
            Percentage,
            Value
        }

        #endregion

        #region Properties

        private Attributes.Stats _attribute; // The attribute to modify.
        public Attributes.Stats Attribute
        {
            get { return _attribute; }
        }

        private ModType _modificationType; // The type of modification.
        public ModType ModificationType
        {
            get { return _modificationType; }
        }

        private float _modification; // The amount of modification.
        public float Modification
        {
            get { return _modification; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new object that describes the way an attribute will be affected by this status effect.
        /// </summary>
        /// <param name="attribute">The attribute to change.</param>
        /// <param name="modificationType">A change based on a percentage or value.</param>
        /// <param name="modification">The percentage (0.01 is 1%, 1.0 is 100% etc.) or value used to modify this attribute. Cannot be 0.</param>
        public AttributeModification(Attributes.Stats attribute, ModType modificationType, float modification)
        {
            if (modification == 0)
            {
                throw new FormatException("An attribute modification cannot be modify something by 0.");
            }

            _attribute = attribute;
            _modificationType = modificationType;
            _modification = modification;
        }

        #endregion

        #region Methods

        internal float GetModification(float attribute)
        {
            if ((AttributeModification.ModType)_modificationType == AttributeModification.ModType.Percentage)
            {
                return _modification * attribute;
            }

            else
            {
                return _modification;
            }
        }

        #endregion
    }

    public class HealthTick
    {
        #region Enums

        public enum ModType
        {
            Percentage,
            Value,
            None,
        }

        #endregion

        #region Properties

        private ModType _modificationType; // The type of tick.
        public ModType ModificationType
        {
            get { return _modificationType; }
        }

        private float _modification; // The amount of health per tick.
        public float Modification
        {
            get { return _modification; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new object that describes the way health will change over the duration of the aura.
        /// </summary>
        /// <param name="modificationType">A change based on a percentage or value.</param>
        /// <param name="modification">The percentage (0.01 is 1%, 1.0 is 100% etc.) or value used to modify this attribute. Cannot be 0.</param>
        public HealthTick(ModType modificationType, float modification = 0)
        {
            if ((ModType)modificationType == ModType.None || modification == 0)
            {
                _modificationType = ModType.None;
                _modification = 0;
            }

            else
            {
                _modificationType = modificationType;
                _modification = modification;
            }
        }

        #endregion

        #region Methods

        internal float GetModification(float attribute)
        {
            if ((AttributeModification.ModType)_modificationType == AttributeModification.ModType.Percentage)
            {
                return _modification * attribute;
            }

            else
            {
                return _modification;
            }
        }

        #endregion
    }

    #endregion
}