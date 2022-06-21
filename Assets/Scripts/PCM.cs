using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCM : MonoBehaviour
{
    public GameObject Controller;

    /*// horizontal rotation speed
    public float horizontalSpeed = 2f;
    // vertical rotation speed
    public float verticalSpeed = 2f;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    public float MovementSpeed = 25;
    public float Gravity = 9.8f;
    private float velocity = 0;

    private CharacterController _controller;*/

    public float walkingSpeed = 3.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float testSpeed = 0.5f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

    [HideInInspector]
    public bool canMove = true;

    float xDeg;
    float yDeg;
    private void Start()
    {
        //_controller = GetComponent<CharacterController>();
        Controller = GameObject.Find("Controller");

        characterController = GetComponent<CharacterController>();
        playerCamera = gameObject.GetComponent<Camera>();
        // Lock cursor
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        /*float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        gameObject.GetComponent<Camera>().transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);

        float horizontal = Input.GetAxis("Horizontal") * MovementSpeed;
        float vertical = Input.GetAxis("Vertical") * MovementSpeed;
        _controller.Move(transform.TransformDirection(Vector3.right * horizontal + Vector3.forward * vertical) / Time.fixedTime);

        // Gravity
        if (_controller.isGrounded)
        {
            velocity = 0;
        }
        else
        {
            velocity -= Gravity * Time.deltaTime;

            _controller.Move(new Vector3(0, velocity, 0));
        }*/
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX * testSpeed) + (right * curSpeedY* testSpeed);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);

        float speed = 5f;
        float friction = 0.5f;
        float lerpSpeed = 90f;
        xDeg -= Input.GetAxis("Mouse X") * speed * friction;
        yDeg += Input.GetAxis("Mouse Y") * speed * friction;
        Quaternion fromRotation;

        Quaternion toRotation;
        fromRotation = transform.rotation;
        toRotation = Quaternion.Euler(-yDeg, -xDeg, 0);
        transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);

        //RaycastHit hit;
        RaycastHit[] hits;
        if (Input.GetKey(KeyCode.E) && !Controller.GetComponent<ShowUI>().isActive)
        {
            GameObject oisGO;
            oisGO = null;
            hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), 5f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<ObjectInScene>().IsExhibit)
                {
                    oisGO = Instantiate(hit.collider.gameObject, new Vector3(0, 100, 0), Quaternion.identity);
                    oisGO.GetComponent<ObjectInScene>().ID =hit.collider.gameObject.GetComponent<ObjectInScene>().ID;
                    oisGO.GetComponent<Exhibit>().exhID = hit.collider.gameObject.GetComponent<Exhibit>().exhID;
                    oisGO.GetComponent<Exhibit>().photos = hit.collider.gameObject.GetComponent<Exhibit>().photos;
                    break;
                }
                if (oisGO != null) break;
            }

            if (oisGO != null)
            {
                Controller.GetComponent<ShowUI>().ActivateShowPanel();
                Controller.GetComponent<ShowUI>().SetGameObject(oisGO);
                GetComponent<Camera>().enabled = false;
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Controller.GetComponent<ShowUI>().DisactivateShowPanel();
            GetComponent<Camera>().enabled = true;
        }

    }

    public void OffUI()
    {
        Controller.GetComponent<ShowUI>().DisactivateShowPanel();
        GetComponent<Camera>().enabled = true;
    }
}
