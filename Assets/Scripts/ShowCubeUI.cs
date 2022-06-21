using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCubeUI : MonoBehaviour
{
    public GameObject Controller;

    float rotSpeed = 20;
    float scrollSpeed = 2000f;

    float pervX;
    float pervY;

    float nextX;
    float nextY;

    bool dragging;

    Vector3 mOffset;
    void Start()
    {
        Controller = GameObject.Find("Controller");
    }

    public void ResetCube()
    {
        GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(500, 500, 500);
    }

    private void OnMouseDrag()
    {

        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        if (Controller.GetComponent<ShowUI>().isActive)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Controller.GetComponent<ShowUI>().RotateOISGO(rotX, rotY);
            }
        }
    }

    private void FixedUpdate()
    {

        if ((Input.GetAxis("Mouse ScrollWheel") != 0) && (Controller.GetComponent<ShowUI>().isActive))
        {
            //transform.localScale += scrollSpeed * new Vector3(Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Mouse ScrollWheel")) * Time.deltaTime;
            Controller.GetComponent<ShowUI>().ScaleOISGO(Input.GetAxis("Mouse ScrollWheel"));
        }


        float top = 0;
        float down = 0;
        float right = 0;
        float left = 0;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                top = 2;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                down = 2;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                right = 2;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                left = 2;
            }
            //transform.GetComponent<RectTransform>().localPosition += new Vector3(right - left, top - down, 0);
            Controller.GetComponent<ShowUI>().PositionOISGO(top, down, right, left);
        }
    }
}
