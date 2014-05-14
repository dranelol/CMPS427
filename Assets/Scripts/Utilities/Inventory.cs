using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// WE GOT DE BAG AND WE PUT DE STUF IN DE BAG
/// </summary>
public class Inventory
{
    private const int MAX = 64;
    public int Max
    {
        get { return MAX; }
    }

    private equipmentFactory factory;
    private List<equipment> items;
    private Dictionary<int, equipment> equippedItems;
    public List<equipment> Items { get { return items; } }
    

    public Inventory()
    {
        items = new List<equipment>();
        equippedItems = new Dictionary<int, equipment>();
        factory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory;

        //LoadItems();
    }

    public void AddItem(equipment item)
    {
        if (items.Count < MAX)
            items.Add(item);
    }

    public void RemoveItem(equipment item)
    {
        items.Remove(item);
    }

    public bool IsEmpty()
    {
        return (items.Count <= 0);
    }

    /// <summary>
    /// Saves current items.
    /// </summary>
    public void SaveItems()
    {
        int count = 0;
        foreach (equipment item in items)
        {
            factory.saveequipment(count + "inventory", item);
            count++;
        }

        foreach (equipment item in equippedItems.Values)
        {
            factory.saveequipment(count + "equipped", item);
            count++;
        }
    }

    public void UnSave()
    {
        for (int i = 0; i < MAX; i++)
        {
            factory.unsaveEquipment(i + "inventory");
        }
    }

    /// <summary>
    /// Loads items from PlayerPrefs, stopping when it encounters a null item.
    /// </summary>
    public void LoadItems()
    {
        for (int i = 0; i < MAX; i++)
        {
            /*
            equipment item = factory.loadequipment(i + "inventory");
            if (item == null)
            {
                break;
            }

            items.Add(item);
             * */
        }

        for (int i = 0; i < CharacterUI.EQUIPMENT_SLOTS; i++)
        {
            equipment item = factory.loadequipment(i + "equipped");
            if (item == null)
            {
                break;
            }

            equippedItems.Add(i, item);
        }
    }

    public string EquippedItem(int index) 
    {
        if (equippedItems.ContainsKey(index))
        {
            return equippedItems[index].equipmentName;
        }

        return "";
    }

    public void EquipItem(equipment item)
    {
        equippedItems[(int)item.validSlot] = item;
        items.Remove(item);
    }

    public void UnequipItem(int slot)
    {
        equipment item = equippedItems[slot];
        equippedItems.Remove(slot);
        items.Add(item);
    }
}
