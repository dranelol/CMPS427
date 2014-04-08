using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// WE GOT DE BAG AND WE PUT DE STUF IN DE BAG
/// </summary>
public class Inventory {
    private const int MAX = 50;

    private equipmentFactory factory;
    private List<equipment> items;
    public List<equipment> Items { get { return items; } }
    
    public Inventory()
    {
        items = new List<equipment>();
        factory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory;
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

    private void SaveItem(equipment item)
    {
        
    }

    private void UnSaveItem(equipment item)
    {

    }
}
