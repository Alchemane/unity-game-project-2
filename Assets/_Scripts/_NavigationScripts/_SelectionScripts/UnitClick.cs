using GameRoot.InGame.Navigation.SelectionSystem.UnitManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRoot.InGame.Navigation.SelectionSystem.ClickSelection
{
    public class UnitClick : MonoBehaviour
    {
        private Camera mainCamera;
        public GameObject groundMarker;
        private Vector3 movePoint;
        public LayerMask selectableUnits;
        public LayerMask ground;

        void Start()
        {
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (gameObject != null)
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectableUnits))
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            UnitSelectionManager.Instance.ShiftSelect(hit.collider.gameObject);
                        }
                        else
                        {
                            UnitSelectionManager.Instance.ClickSelect(hit.collider.gameObject);
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            UnitSelectionManager.Instance.DeselectAll();
                        }
                    }
                }
            }
            if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift))
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                    movePoint = hit.point;
                    groundMarker.transform.position = movePoint;
                    groundMarker.SetActive(true);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                groundMarker.SetActive(false);
            }
        }
    }
}