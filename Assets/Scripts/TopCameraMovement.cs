using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCameraMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float scrollSpeed = 1000f;
    private float rotateSpeed = 500f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                transform.position += moveSpeed * transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0)) * Time.deltaTime;
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
            GetComponent<Camera>().orthographicSize += scrollSpeed* Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
            }
    }
}
