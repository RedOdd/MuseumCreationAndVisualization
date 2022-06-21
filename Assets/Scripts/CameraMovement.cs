using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    enum CameraType
    {
        Creator,
        User
    }

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
        if (true)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                //transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")))
                transform.position += moveSpeed * transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0)) * Time.deltaTime;
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                transform.position += scrollSpeed * transform.TransformDirection(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel"))) * Time.deltaTime;
            }

            if (Input.GetMouseButton(1))
            {
                transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);
                float z = transform.eulerAngles.z;
                transform.Rotate(0, 0, -z);

            }

        }
    }
}
