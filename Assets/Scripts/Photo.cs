using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photo : MonoBehaviour
{
    public int ID;

    public Sprite Sprite;

    public void SetInfo(int id, Sprite sprite)
    {
        ID = id;
        Sprite = sprite;
    }
}
