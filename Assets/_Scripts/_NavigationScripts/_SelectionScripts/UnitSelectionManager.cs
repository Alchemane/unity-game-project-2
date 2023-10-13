using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace GameRoot.InGame.Navigation.SelectionSystem.SelectionManager
{
    public class UnitSelectionManager : MonoBehaviour
    {
        private List<SelectableUnit> selectedUnits = new List<SelectableUnit>();
        private Vector3 startPos;
        private Vector3 endPos;
        private bool isSelecting = false;

        void Update()
        {
            // Handle mouse input for selection box
            HandleSelectionBox();

            // Handle unit selection logic
            HandleUnitSelection();
        }

        public void SelectUnit(SelectableUnit unit, bool multiSelect = false)
        {
            if (!multiSelect)
            {
                DeselectAllUnits();
            }

            selectedUnits.Add(unit);
            unit.Select();
        }

        public void DeselectUnit(SelectableUnit unit)
        {
            selectedUnits.Remove(unit);
            unit.Deselect();
        }

        public void DeselectAllUnits()
        {
            foreach (SelectableUnit unit in selectedUnits)
            {
                unit.Deselect();
            }
            selectedUnits.Clear();
        }

        void HandleSelectionBox()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSelecting = true;
                startPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isSelecting = false;
            }
            if (isSelecting)
            {
                endPos = Input.mousePosition;
                // TODO: Draw the selection box based on startPos and endPos
            }
        }

        void HandleUnitSelection()
        {
            if (isSelecting)
            {
                // Clear previously selected units
                selectedUnits.Clear();

                // TODO: Loop through all units and check if they are within the selection box
                // If they are, add them to selectedUnits
            }
        }


        private bool IsWithinSelectionBounds(GameObject gameObject)
        {
            var camera = Camera.main;
            var viewportBounds = Utils.GetViewportBounds(camera, startPos, endPos);

            return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
        }

        private void SelectUnitsInSelectionBox()
        {
            // Loop through all units to see if they are in the selection box
            foreach (var selectableObject in FindObjectsOfType<SelectableUnit>())
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    selectableObject.Select();
                }
                else
                {
                    selectableObject.Deselect();
                }
            }
        }

        void OnGUI()
        {
            if (isSelecting)
            {
                // Create a rect from both mouse positions
                var rect = Utils.GetScreenRect(startPos, endPos);
                Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
                Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
            }
        }
    }
}