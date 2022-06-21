using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInSceneButton : MonoBehaviour
{
    public GameObject oisGO { get; private set; }
    Text Text;
    public int oisButtonID { get; private set; }
    public void SetInfo(GameObject oisgo, int id, string name)
    {
        oisGO = oisgo;
        Text = transform.Find("Text").gameObject.GetComponent<Text>();
        Text.text = name + " (" + id + ")";
        oisButtonID = id;
    }
}
