using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameRoot.InGame.Navigation.CameraNavigation
{
    public class CameraController : MonoBehaviour
    {
        public Transform followTransform;
        public Transform cameraTransform;

        public float normalSpeed;
        public float fastSpeed;
        public float movementSpeed;
        public float movementTime;
        public float rotationAmount;
        public Vector3 zoomAmount;

        public Vector3 newPosition;
        public Quaternion newRotation;
        public Vector3 newZoom;

        public Vector3 dragStartPosition;
        public Vector3 dragCurrentPosition;
        public Vector3 rotateStartPosition;
        public Vector3 rotateCurrentPosition;

        // Start is called before the first frame update
        void Start()
        {
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            // wip follow selected game object
            if (followTransform != null)
            {
                transform.position = followTransform.position;
            }
            else
            {
                HandleKeyboardInput();
                HandleMouseInput();
            }
            // cancel follow game tracked object
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                followTransform = null;
            }
            // Boundary check
            newPosition = new Vector3(
                Mathf.Clamp(newPosition.x, minX, maxX),
                newPosition.y,
                Mathf.Clamp(newPosition.z, minZ, maxZ)
            );
        }

        void HandleMouseInput()
        {
            // is not over user interface elements
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.mouseScrollDelta.y != 0)
                {
                    newZoom += Input.mouseScrollDelta.y * zoomAmount;

                    // Zoom limit
                    newZoom = new Vector3(
                        Mathf.Clamp(newZoom.x, minZoomX, maxZoomX),
                        Mathf.Clamp(newZoom.y, minZoomY, maxZoomY),
                        Mathf.Clamp(newZoom.z, minZoomZ, maxZoomZ)
                    );
                }

                //navigate the world using mouse 1
                if (Input.GetMouseButtonDown(0))
                {
                    Plane plane = new Plane(Vector3.up, Vector3.zero);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    float entry;

                    if (plane.Raycast(ray, out entry))
                    {
                        dragStartPosition = ray.GetPoint(entry);
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    Plane plane = new Plane(Vector3.up, Vector3.zero);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    float entry;

                    if (plane.Raycast(ray, out entry))
                    {
                        dragCurrentPosition = ray.GetPoint(entry);

                        newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                    }
                }

                //rotate the world using mouse 3
                if (Input.GetMouseButtonDown(1))
                {
                    rotateStartPosition = Input.mousePosition;
                }
                if (Input.GetMouseButton(1))
                {
                    rotateCurrentPosition = Input.mousePosition;

                    Vector3 difference = rotateStartPosition - rotateCurrentPosition;

                    rotateStartPosition = rotateCurrentPosition;

                    newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
                }
            }
        }

        void HandleKeyboardInput()
        {
            // shift speed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }

            // movement keys
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * movementSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (transform.forward * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (transform.right * -movementSpeed);
            }

            // rotation keys
            if (Input.GetKey(KeyCode.E))
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            // zoom keys
            if (Input.GetKey(KeyCode.R))
            {
                newZoom += zoomAmount;
            }
            if (Input.GetKey(KeyCode.F))
            {
                newZoom -= zoomAmount;
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }
    }
}