using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoList : MonoBehaviour, IEnumerable<Photo>
{
    public List<Photo> Photos;

    public int PhotoListID { get; private set; }
    public int LastID;

    public void Start()
    {
        
    }
    public void SetInfo(int oisID)
    {
        Photos = new List<Photo>();
        PhotoListID = oisID;
        LastID = 0;
    }

    public Photo GetPhoto(int id)
    {
        return Photos.Find(i => i.ID == id);
    }

    public void AddPhoto(Sprite photo)
    {
        Photo ph = new Photo();
        ph.SetInfo(LastID, photo);
        Photos.Add(ph);
        LastID++;
    }

    public void LoadPhoto(Sprite photo, int photoID)
    {
        Photo ph = new Photo();
        ph.SetInfo(photoID, photo);
        Photos.Add(ph);
    }

    public void DeletePhoto(int pid)
    {
        Photos.Remove(GetPhoto(pid));
    }

    public IEnumerator<Photo> GetEnumerator()
    {
        foreach (Photo photo in Photos)
        {
            yield return photo;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
