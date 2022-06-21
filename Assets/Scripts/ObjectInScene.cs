using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInScene : MonoBehaviour
{
    public int ID { get; set; } = 0;
    public string Name { get; set; } = "";

    public bool IsExhibit = false;
    public bool IsLight = false;

    public bool scaleWasChange;
    public Shader shader;

    public void SetInfo(int id, string name,bool isLight)
    {
        ID = id;
        Name = name;
        IsExhibit = false;
        IsLight = isLight;
        if (IsLight)
        {
            gameObject.AddComponent<LightObject>();
        }
        //shader = gameObject.GetComponent<Renderer>().sharedMaterial.shader;
    }
    public void EnableOutline()
    {
        gameObject.GetComponent<Outline>().enabled = true;
     }

    public void DisableOutline()
    {
        gameObject.GetComponent<Outline>().enabled = false;
    }

    public void ChangeShaderToWireframe()
    {
        foreach(Transform child in gameObject.transform)
        {
            child.GetComponent<Renderer>().sharedMaterial.shader = Managers.Creation.Wireframe;
        }
    }

    public void ChangeShaderToNormal()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<Renderer>().sharedMaterial.name != "TransparentMaterial")
            {
                child.GetComponent<Renderer>().sharedMaterial.shader = Managers.Creation.Opaque;
            }
            else
            {
                child.GetComponent<Renderer>().sharedMaterial.shader = Managers.Creation.Transparent;
            }
        }
    }
}

