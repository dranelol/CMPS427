using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    protected float currentHP; 
    public float CurrentHP
    {
        get { return currentHP; }
    }

    protected float currentResource;
    public float CurrentResource
    {
        get { return currentResource; }
    }


    private int experience;
    public int Experience
    {
        get { return experience; }
        set { experience = value; }
    }

    private int level;
    public int Level
    {
        get { return level; }
        set { level = value; }
	}

    public Attributes currentAtt; // The entity's current total attributes
    public Attributes buffAtt; // Attribute changes that are added on from buffs/debuffs
    public Attributes equipAtt; // Attribute changes that are added on from equipment stat changes
    public Attributes baseAtt; // Base attributes without any status effects or gear
    private EntitySoundManager _soundManager;

    public float minMovementSpeed = 0;
    public float maxMovementSpeed = 3;
    public float minAttackSpeed = 0.5f;
    public float maxAttackSpeed = 2f;

    public AbilityManager abilityManager;

    public Dictionary<string, int> abilityIndexDict = new Dictionary<string, int>();
    private Dictionary<equipSlots.slots, equipment> equippedEquip;
    public Dictionary<equipSlots.slots, equipment> EquippedEquip
    {
        get { return equippedEquip; }
        set { equippedEquip = value; }
    }

    protected Inventory inventory;
    public Inventory Inventory { get { return inventory; } }

    private Mesh _weaponMesh;

    public void Awake()
    {
        inventory = new Inventory(); // LoadInventory();
        abilityManager = gameObject.GetComponent<AbilityManager>();
        equippedEquip = new Dictionary<equipSlots.slots, equipment>();
        _soundManager = GetComponent<EntitySoundManager>();

        equipAtt = new Attributes();
        buffAtt = new Attributes();
        baseAtt = new Attributes();

        baseAtt.Health = currentHP = 100;
        baseAtt.Resource = currentResource = 100;

        baseAtt.Power = 10;
        baseAtt.Defense = 10;

        baseAtt.AttackSpeed = 0;
        baseAtt.MovementSpeed = 1.0f;

        level = 1;
        experience = 0;
    }

    public void OnApplicationQuit() { }

    public void Start()
    {
        UpdateCurrentAttributes();
    }

    public void UpdateCurrentAttributes()
    {
        currentAtt = new Attributes();
        currentAtt.Add(baseAtt);
        currentAtt.Add(equipAtt);
        currentAtt.Add(buffAtt);

        currentAtt.MovementSpeed = Mathf.Clamp(currentAtt.MovementSpeed, minMovementSpeed, maxMovementSpeed);
        currentAtt.AttackSpeed = Mathf.Clamp(currentAtt.AttackSpeed, minAttackSpeed, maxAttackSpeed);

        GetComponent<MovementFSM>().UpdateMovementSpeed(currentAtt.MovementSpeed);

        currentHP = Mathf.Clamp(currentHP, 0, currentAtt.Health);
        currentResource = Mathf.Clamp(currentResource, 0, currentAtt.Resource);
    }
    /// <summary>
    /// Modifies the current health of the entity, clamped by the maximum health.
    /// Can be used for both taking damage and gaining health.
    /// </summary>
    /// <param name="value">Delta value to modify current health.</param>
    public void ModifyHealth(float delta) 
    {
        if (delta < 0 && delta < -0.2f * currentAtt.Health)
        {
            _soundManager.GetHit();
        }

        currentHP = Mathf.Clamp(currentHP + delta, 0, currentAtt.Health); 
    }

    public void ModifyHealthPercentage(float deltaPercent)
    {
        currentHP = Mathf.Clamp(currentHP + (currentAtt.Health / deltaPercent), 0, currentAtt.Health);
    }

    public void ModifyResource(float delta)
    {
        currentResource = Mathf.Clamp(currentResource + delta, 0, currentAtt.Resource);
    }

    public void SetLevel(int newlevel)
    {
        ModifyLevel(newlevel - level);
    }

    public void ModifyLevel(int delta)
    {
        level = Mathf.Clamp(level + delta, 1, 20);
    }

    public void ModifyXP(int delta)
    {
        experience = Mathf.Clamp(experience + delta, 0, 1000);
    }

    /// <summary>
    /// Kind of obvious.
    /// </summary>
    /// <returns>True if the dude is dead.</returns>
    public bool IsDead() { return currentHP <= 0; }

    #region Equipment

    /// <summary>
    /// Add the attribute changes of an item to the entity. The item must correlate to one of the equipment slots,
    /// Head, Chest, Legs, Feet, Main, Off. Attribute changes are taken as an attributes object. Returns
    /// false if the slot is already filled.
    /// 
    /// Removes the equipped item from the list of inventory items and adds it to the list of equipped items.
    /// </summary>
    /// <param name="slot">The equipment slot being filled.</param>
    /// <returns></returns>
    public bool addEquipment(equipment item)
    {

        if (this.equippedEquip.ContainsKey(item.validSlot))
        {
            return false;
        }

        else if (item.twohand == true && this.equippedEquip.ContainsKey(equipSlots.slots.Off))
        {
            return false;
        }

        else if (item.validSlot == equipSlots.slots.Off && equippedEquip.ContainsKey(equipSlots.slots.Main) && equippedEquip[equipSlots.slots.Main].twohand == true)
        {
            return false;
        }

        else
        {
            this.equippedEquip.Add(item.validSlot, item);
            this.equipAtt.Add(item.equipmentAttributes);
            UpdateCurrentAttributes();

            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.saveequipment(((int)item.validSlot).ToString(), item);

            if (item.validSlot == equipSlots.slots.Main && item.onhit != "")
            {
                abilityManager.RemoveAbility(6);
                abilityManager.AddAbility(GameManager.Abilities[item.onhit], 6);
                abilityIndexDict[item.onhit] = 6;
            }
            if (tag == "Player" && item.validSlot == equipSlots.slots.Main)
            {
                //GameObejct weaponModel = (Resources.Load(equipment.FILEPATH + item.modelname, typeof(GameObject)) as GameObject).GetComponent<MeshFilter>().mesh;          
            }

            inventory.RemoveItem(item);
            return true;
        }
    }

    /// <summary>
    /// Removing the attributes of an equipment item from the character.
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool removeEquipment(equipSlots.slots slot)
    {
        if (equippedEquip.ContainsKey(slot))
        {
            equipment removed = equippedEquip[slot];
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.unsaveEquipment(((int)slot).ToString());
           
            equippedEquip.Remove(slot);
            equipAtt.Subtract(removed.equipmentAttributes);
            UpdateCurrentAttributes();

            if (slot == equipSlots.slots.Main)
            {
                abilityManager.RemoveAbility(6);
         
            }
            inventory.AddItem(removed);
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Currently just initializes the inventory object.
    /// </summary>
    private void LoadInventory()
    {
        inventory = new Inventory();
        Inventory.LoadItems();
    }

    /// <summary>
    /// Does the current entity have an item equipped in this slot
    /// </summary>
    /// <param name="slot">The slot to check</param>
    /// <returns>Returns false if entity does not have slot equipped; true if they do</returns>
    public bool HasEquipped(equipSlots.slots slot)
    {
        return equippedEquip.ContainsKey(slot);
    }

    /// <summary>
    /// Returns the equipped item of that slot
    /// </summary>
    /// <param name="slot">Slot to return item</param>
    /// <returns>Throws exception if the entity doesn't have anything equipped there, else returns the item equipped</returns>
    public equipment GetEquip(equipSlots.slots slot)
    {
        if (HasEquipped(slot) == false)
        {
            throw new KeyNotFoundException("Entity does not have this item equipped!");
        }

        else
        {
            return equippedEquip[slot];
        }
    }

    #endregion

    #region Buffs

    public void ApplyBuff(Attributes buffedAttributes)
    {
        buffAtt.Add(buffedAttributes);
        UpdateCurrentAttributes();
    }

    #endregion
}
