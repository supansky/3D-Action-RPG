using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }

    private Dictionary<string, int> items;

    public string equippedItem { get; private set; }

    private NetworkService network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Inventory manager starting up...");

        network = service;

        UpdateData(new Dictionary<string, int>());

        Status = ManagerStatus.Started;
    }
    public void AddItem(string itemName)
    {
        if (items.ContainsKey(itemName))
            items[itemName] += 1;
        else
            items[itemName] = 1;

        DisplayItems();
    }
    public void DisplayItems()
    {
        string display = "Items: ";

        foreach(KeyValuePair<string, int> item in items)
        {
            display += item.Key + "(" + item.Value + ") ";
        }
        Debug.Log(display);
    }
    public List<string> GetItemList()
    {
        List<string> list = new List<string>(items.Keys);
        return list;
    }
    public int GetItemCount(string name)
    {
        if (items.ContainsKey(name))
        {
            return items[name];
        }
        return 0;
    }
    public bool EquipItem(string name)
    {
        if(items.ContainsKey(name) && equippedItem != name)
        {
            equippedItem = name;
            Debug.Log($"Equipped {name}");
            return true;
        }
        equippedItem = null;
        Debug.Log("Unequipped");
        return false;
    }
    public bool ConsumeItem(string name)
    {
        if (items.ContainsKey(name))
        {
            items[name]--;
            if (items[name] == 0)
                items.Remove(name);
        }
        else
        {
            Debug.Log($"Cannot consume {name}");
            return false;
        }

        DisplayItems();
        return true;
    }

    public Dictionary<string, int> GetData()
    {
        return items;
    }
    public void UpdateData(Dictionary<string, int> items)
    {
        this.items = items;
    }
}
