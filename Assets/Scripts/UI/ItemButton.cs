using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    Text Text;
    public Item Item { get; private set; }

    public void SetItem(Item item)
    {
        Item = item;
        Text = transform.Find("Text").gameObject.GetComponent<Text>();
        Text.text = item.Name;
    }

    public Item GetItem()
    {
        return Item;
    }
    /*public ItemButton(Item item)
    {
        Item = item;
        Text = transform.Find("Text").gameObject.GetComponent<Text>();
        Text.text = item.Name;
    }

    private void AddListenet()
    {
        //GetComponent<Button>().onClick.AddListener();
    }
    public ItemButton(int itemID)
    {
        Item = Managers.Creation.ItemList.TakeItem(itemID);
        Text = transform.Find("Text").gameObject.GetComponent<Text>();
        Text.text = Item.Name;
    }*/


}
