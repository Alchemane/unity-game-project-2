using GameRoot.InGame.Navigation.SelectionSystem.UnitManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRoot.InGame.Navigation.SelectionSystem.DragSelection
{
    public class UnitDrag : MonoBehaviour
    {
        private Camera mainCamera;
        public RectTransform selectionBox; // canvas image reference
        private Rect selectionRect;
        private Vector2 startPosition;
        private Vector2 endPosition;
        private int selectableUnits = 6;


        // Start is called before the first frame update
        void Start()
        {
            selectionBox.transform.parent.gameObject.SetActive(true);
            mainCamera = Camera.main;
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
                selectionRect = new Rect();
            }
            if (Input.GetMouseButton(0))
            {
                endPosition = Input.mousePosition;
                DrawVisual();
                DrawSelection();
            }
            if (Input.GetMouseButtonUp(0))
            {
                SelectUnits();
                startPosition = Vector2.zero;
                endPosition = Vector2.zero;
                DrawVisual();
            }
        }

        public void DrawVisual()
        {
            Vector2 boxStart = startPosition;
            Vector2 boxEnd = endPosition;

            Vector2 boxCenter = (boxStart + boxEnd) / 2;
            selectionBox.position = boxCenter;

            Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

            selectionBox.sizeDelta = boxSize;
        }

        void DrawSelection()
        {
            // x calculations
            if (Input.mousePosition.x < startPosition.x)
            {
                //dragging left
                selectionRect.xMin = Input.mousePosition.x;
                selectionRect.xMax = startPosition.x;
            }
            else
            {
                //dragging right
                selectionRect.xMin = startPosition.x;
                selectionRect.xMax = Input.mousePosition.x;
            }

            // y calculations
            if (Input.mousePosition.y < startPosition.y)
            {
                //dragging down
                selectionRect.yMin = Input.mousePosition.y;
                selectionRect.yMax = startPosition.y;
            }
            else
            {
                //dragging up
                selectionRect.yMin = startPosition.y;
                selectionRect.yMax = Input.mousePosition.y;
            }
        }

        public void SelectUnits()
        {
            foreach (var unit in UnitSelectionManager.Instance.unitList)
            {
                if (selectionRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
                {
                    if (unit.layer == selectableUnits)
                    {
                        UnitSelectionManager.Instance.DragSelect(unit);
                    }
                }
            }
        }
    }
}