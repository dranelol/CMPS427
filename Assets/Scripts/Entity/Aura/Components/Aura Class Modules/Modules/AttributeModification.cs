using UnityEngine;
using System.Collections;

public class AttributeModification : AuraModule
{
    #region Properties

    private float _attributeValueSnapshot;
    public float AttributeChange
    {
        get { return _attributeValueSnapshot; }
    }

    private Attributes.Stats _attribute;
    public Attributes.Stats Attribute
    {
        get { return _attribute; }
    }

    private float _percentage;
    public float Percentage
    {
        get { return _percentage; }
    }

    #endregion

    #region Constructors

    public AttributeModification(Attributes.Stats attribute, float percentage) : base()
    {
        _attributeValueSnapshot = 0;
        _attribute = attribute;

        if (percentage > 0)
        {
            _percentage = percentage;
        }

        else
        {
            Debug.LogError("You cannot create an AttributeModification obect that alters an attribute by 0%.");
            _percentage = 0;
        }
    }

    public AttributeModification(AttributeModification other, Entity target)
    {
        _attributeValueSnapshot = GetSnapshot(target);
        _attribute = other.Attribute;
        _percentage = other.Percentage;
    }

    private float GetSnapshot(Entity target)
    {
        return 0.2f;
    }

    #endregion
}
