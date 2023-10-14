using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameRoot.InGame.Units.Movement;

namespace GameRoot.InGame.Navigation.SelectionSystem.UnitManager
{
    public class UnitSelectionManager : MonoBehaviour
    {
        public List<GameObject> unitList = new List<GameObject>();
        public List<GameObject> selectedUnits = new List<GameObject>();

        private static UnitSelectionManager _instance;
        public static UnitSelectionManager Instance {  get { return _instance; } }

        public void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void AddSelection(GameObject unit)
        {
            selectedUnits.Add(unit);
            unit.GetComponent<UnitMovement>().enabled = true;
        }

        public void RemoveSelection(GameObject unit)
        {
            selectedUnits.Remove(unit);
            unit.GetComponent<UnitMovement>().enabled = false;
        }

        public void ClickSelect(GameObject unit)
        {
            DeselectAll();
            AddSelection(unit);
            unit.transform.GetChild(0).gameObject.SetActive(true);
        }

        public void ShiftSelect(GameObject unit)
        {
            if (selectedUnits.Contains(unit))
            {
                unit.transform.GetChild(0).gameObject.SetActive(false);
                RemoveSelection(unit);
            }
            else
            {
                AddSelection(unit);
                unit.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        public void DragSelect(GameObject unit) 
        { 
            if (!selectedUnits.Contains(unit))
            {
                unit.transform.GetChild(0).gameObject.SetActive(true);
                AddSelection(unit);
            }
        }

        public void DeselectAll()
        {
            foreach (var unit in selectedUnits)
            {
                if (unit != null)
                {
                    unit.transform.GetChild(0).gameObject.SetActive(false);
                    unit.GetComponent<UnitMovement>().enabled = false;
                }
            }
            selectedUnits.Clear();
        }
    }
}