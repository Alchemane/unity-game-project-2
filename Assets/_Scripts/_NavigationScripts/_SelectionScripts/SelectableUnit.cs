using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    public bool isSelected = false;
    public GameObject selectionCircle;

    void Start()
    {
        selectionCircle.SetActive(false);
    }

    public void Select()
    {
        // Add your code to highlight the unit or show a selection circle
        isSelected = true;
    }

    public void Deselect()
    {
        // Add your code to remove the highlight or hide the selection circle
        isSelected = false;
    }

    public void OnClick()
    {
        // Code for what happens when this unit is clicked
    }
}
