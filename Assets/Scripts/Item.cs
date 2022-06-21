using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID { get; private set; }
    public GameObject Prefab { get; private set; }
    public string Name { get; private set; }
    public Item(int id, GameObject prefab, string name)
    {
        ID = id;
        Prefab = prefab;
        Name = name;
    }

    public void SetPrefab (GameObject prefab)
    {
        Prefab = prefab;
    }
}
