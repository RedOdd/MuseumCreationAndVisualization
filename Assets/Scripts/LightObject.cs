using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObject : MonoBehaviour
{
    public int LightID;

    public string Name = "";

    public Color Color;

    public float Intensity;

    public float Range;

    public void SetInfo(int lightID, string name)
    {
        LightID = lightID;
        Name = name;
        Color = Color.white;
        Intensity = 1;
        Range = 10;
    }

    public void ChangeColor(Color color)
    {
        Color = color;
        gameObject.GetComponent<Light>().color = Color;
    }

    public void ChangeIntensity(float intensity)
    {
        Intensity = intensity;
        gameObject.GetComponent<Light>().intensity = Intensity;
    }

    public void ChangeRange(float range)
    {
        Range = range;
        gameObject.GetComponent<Light>().range = Range;
    }
}
