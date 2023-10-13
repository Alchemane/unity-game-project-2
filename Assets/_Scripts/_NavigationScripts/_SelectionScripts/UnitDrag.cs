using GameRoot.InGame.Navigation.SelectionSystem.UnitManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRoot.InGame.Navigation.SelectionSystem.DragSelection
{
    public class UnitDrag : MonoBehaviour
    {
        private Camera mainCamera;
        public RectTransform selectionBoxGraphic;
        private Rect selectionBox;
        private Vector2 startPosition;
        private Vector2 endPosition;
        public LayerMask selectableUnits;


        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawBoxGraphic();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
                selectionBox = new Rect();
            }
            if (Input.GetMouseButton(0))
            {
                endPosition = Input.mousePosition;
                DrawBoxGraphic();
                DrawSelection();
            }
            if (Input.GetMouseButtonUp(0))
            {
                SelectUnits();
                startPosition = Vector2.zero;
                endPosition = Vector2.zero;
                DrawBoxGraphic();
            }
        }

        public void DrawBoxGraphic()
        {
            Vector2 boxStart = startPosition;
            Vector2 boxEnd = endPosition;
            Vector2 boxCenter = (boxStart - boxEnd) / 2;
            selectionBoxGraphic.position = boxCenter;
            Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
            selectionBoxGraphic.sizeDelta = boxSize;
        }

        public void DrawSelection()
        {
            if (Input.mousePosition.x < startPosition.x)
            {
                //dragging left
                selectionBox.xMin = Input.mousePosition.x;
                selectionBox.xMax = startPosition.x;
            }
            else
            {
                //dragging right
                selectionBox.xMin = startPosition.x;
                selectionBox.xMax = Input.mousePosition.x;
            }
            if (Input.mousePosition.y < startPosition.y)
            {
                //dragging down
                selectionBox.yMin = Input.mousePosition.y;
                selectionBox.yMax = startPosition.y;
            }
            else
            {
                //dragging up
                selectionBox.yMin = startPosition.y;
                selectionBox.yMax = Input.mousePosition.y;
            }
        }

        public void SelectUnits()
        {
            foreach (var unit in UnitSelectionManager.Instance.unitList)
            {
                if (selectionBox.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
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