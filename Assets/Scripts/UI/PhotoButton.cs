using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoButton : MonoBehaviour
{
    public int photoID { get; private set; }
    public int oisID { get; private set; }

    public void SetInfo(int pID,int oisid)
    {
        photoID = pID;
        oisID = oisid;
    }
}
