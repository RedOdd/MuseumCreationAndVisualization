using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStatueRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.up, 1 * Time.deltaTime);
       // transform.Translate(Vector3.right * 5 * Time.deltaTime);
    }
}
