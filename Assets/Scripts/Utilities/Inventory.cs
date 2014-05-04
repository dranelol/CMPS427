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
    public List<equipment> Items { get { return items; } }

    

    public Inventory()
    {
        items = new List<equipment>();
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
    }
}
