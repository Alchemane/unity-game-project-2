using GameRoot.InGame.Navigation.SelectionSystem.UnitManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRoot.InGame.Navigation.SelectionSystem.DragSelection
{
    public class UnitDrag : MonoBehaviour
    {
        private Camera mainCamera;
        private Rect selectionRect;
        private Vector2 startPosition;
        private Vector2 endPosition;
        public LayerMask selectableUnits;

        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                selectionRect = new Rect();
            }
            if (Input.GetMouseButton(0))
            {
                endPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                DrawSelection();
            }
            if (Input.GetMouseButtonUp(0))
            {
                SelectUnits();
                startPosition = Vector2.zero;
                endPosition = Vector2.zero;
                selectionRect = new Rect();  // Reset the rectangle
            }
        }

        void DrawSelection()
        {
            selectionRect.xMin = Mathf.Min(startPosition.x, endPosition.x);
            selectionRect.xMax = Mathf.Max(startPosition.x, endPosition.x);
            selectionRect.yMin = Screen.height - Mathf.Max(startPosition.y, endPosition.y);
            selectionRect.yMax = Screen.height - Mathf.Min(startPosition.y, endPosition.y);
        }

        void OnGUI()
        {
            if (selectionRect.width > 0 && selectionRect.height > 0)
            {
                GUI.Box(new Rect(selectionRect.xMin, Screen.height - selectionRect.yMax, selectionRect.width, selectionRect.height), "");
            }
        }

        public void SelectUnits()
        {
            foreach (var unit in UnitSelectionManager.Instance.unitList)
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);
                screenPos.y = Screen.height - screenPos.y;  // Flip the y-coordinate
                if (selectionRect.Contains(screenPos))
                {
                    if (((1 << unit.layer) & selectableUnits) != 0)
                    {
                        UnitSelectionManager.Instance.DragSelect(unit);
                    }
                }
            }
        }
    }
}