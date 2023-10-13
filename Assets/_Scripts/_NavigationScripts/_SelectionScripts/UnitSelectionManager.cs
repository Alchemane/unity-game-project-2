using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRoot.InGame.Navigation.SelectionSystem.UnitManager
{
    public class UnitSelectionManager : MonoBehaviour
    {
        public List<GameObject> unitList = new List<GameObject>();
        private List<GameObject> selectedUnits = new List<GameObject>();

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
            // movement enabled here
        }

        public void RemoveSelection(GameObject unit)
        {
            selectedUnits.Remove(unit);
            // movement disabled here
        }

        public void ClickSelect(GameObject unitToAdd)
        {
            DeselectAll();
            AddSelection(unitToAdd);
            // selected indicator enabled here
        }

        public void ShiftSelect(GameObject unit)
        {
            if(selectedUnits.Contains(unit))
            {
                RemoveSelection(unit);
                // selected indicator disabled here
            }
            else
            {
                AddSelection(unit);
                // selected indicator enabled here
            }
        }

        public void DragSelect(GameObject unit) 
        { 
            if(!selectedUnits.Contains(unit))
            {
                AddSelection(unit);
            }
        }

        public void DeselectAll()
        {
            foreach(var unit in selectedUnits)
            {
                if (unit != null)
                {
                    // selected indicator disabled here
                    // unit movement disabled here
                }
            }
            selectedUnits.Clear();
        }







        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}