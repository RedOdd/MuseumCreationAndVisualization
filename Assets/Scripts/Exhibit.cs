using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exhibit : MonoBehaviour
{
    public int exhID = 0;

    public string Name = "";

    public PhotoList photos = new PhotoList();

    public string Discription = "";
    public void Start()
    {
        
    }

    public void SetInfo(int oisID)
    {
        //photos = new PhotoList();
        exhID = oisID;
        photos.SetInfo(exhID);
    }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void ChangeDiscription(string discription)
    {
        Discription = discription;
    }

    public void AddPhoto(Sprite photo)
    {
        photos.AddPhoto(photo);
    }

    public void LoadPhoto(Sprite photo, int photoID)
    {
        photos.LoadPhoto(photo, photoID);
    }

    public void DeletePhoto(int photoID)
    {
        photos.DeletePhoto(photoID);
    }
}
