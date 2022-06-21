using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour, IEnumerable<Item>
{
    List<Item> items = new List<Item>();
    int lastID;
    public ItemList()
    {
        lastID = 0;
    }

    public void AddItem(GameObject prefab, string name)
    {
        if (items.FindIndex(0,items.Count,i=>i.Name ==name) != -1 )
        {
            items[items.FindIndex(0, items.Count, i => i.Name == name)].SetPrefab(prefab);
        }
        else 
        {
            items.Add(new Item(lastID, prefab, name));
            lastID++;
        }
    }

    public IEnumerator<Item> GetEnumerator()
    {
        foreach (Item item in items)
        {
            yield return item;
        }
    }

    public Item TakeItem(int id)
    {
        if (id >= lastID)
        {
            return null;
        }
        return items[id];
    }

    public Item TakeItem(string name)
    {
        return items.Find(i => i.Name == name);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
