using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public GameObject Statue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Statue.transform);
        //Statue.transform.position = new Vector3(Statue.transform.position.x, Statue.transform.position.y - 5f, Statue.transform.position.z);
    }
}
