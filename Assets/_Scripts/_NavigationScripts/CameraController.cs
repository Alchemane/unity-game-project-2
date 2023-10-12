using GameRoot.InGame.Environment.EnvironmentGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameRoot.InGame.Navigation.CameraNavigation
{
    public class CameraController : MonoBehaviour
    {
        public Transform followTransform; // wip
        public Transform cameraTransform;
        public GameObject gameMaster;

        // public inspector fields
        public float normalSpeed;
        public float fastSpeed;
        public float movementSpeed;
        public float movementTime;
        public float rotationAmount;
        public Vector3 zoomAmount;

        private Vector3 newPosition;
        private Quaternion newRotation;
        private Vector3 newZoom;
        private Vector3 dragStartPosition;
        private Vector3 dragCurrentPosition;
        private Vector3 rotateStartPosition;
        private Vector3 rotateCurrentPosition;

        private MapGeneration mapGeneration;
        private Vector3 minBounds;
        private Vector3 maxBounds;
        private float maxZoomOut;
        public float minZoomIn;

        // Start is called before the first frame update
        void Start()
        {
            mapGeneration = gameMaster.GetComponent<MapGeneration>();

            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;

            // get map data
            minBounds = mapGeneration.GetMinBounds();
            maxBounds = mapGeneration.GetMaxBounds();

            //zoom ceiling
            maxZoomOut = Mathf.Max(maxBounds.x - minBounds.x, maxBounds.z - minBounds.z);
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
                HandleMovementInput();
                HandleMouseInput();
            }
            // cancel follow game tracked object
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                followTransform = null;
            }

            // clamping the camera within the map boundaries
            float confinedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
            float confinedZ = Mathf.Clamp(transform.position.z, minBounds.z, maxBounds.z);

            // Update the camera position
            transform.position = new Vector3(confinedX, transform.position.y, confinedZ);
        }

        void HandleMouseInput()
        {
            // is not over user interface elements
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // scroll wheel zoom
                if (Input.mouseScrollDelta.y != 0)
                {
                    float zoomFactor = Mathf.Log(Mathf.Abs(newZoom.y) + 1) + 1; // Exponential factor
                    newZoom += Input.mouseScrollDelta.y * zoomAmount * zoomFactor;
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

        void HandleMovementInput()
        {
            float zoomFactor = Mathf.Log(Mathf.Abs(newZoom.y) + 1) + 1;
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
                newZoom.y += zoomAmount.y * zoomFactor;
            }
            if (Input.GetKey(KeyCode.F))
            {
                newZoom.y -= zoomAmount.y * zoomFactor;
            }

            // clamping the camera's zoom
            float confinedY = Mathf.Clamp(newZoom.y, minZoomIn, maxZoomOut);
            float confinedZoomZ = Mathf.Clamp(newZoom.z, -maxZoomOut, -minZoomIn); // clamp z axis
            newZoom = new Vector3(newZoom.x, confinedY, confinedZoomZ);

            // clamping the camera within the map boundaries
            float confinedX = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            float confinedZ = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);

            // clamping the newPosition with the confined values
            newPosition = new Vector3(confinedX, newPosition.y, confinedZ);

            // apply all transforms
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }
    }
}