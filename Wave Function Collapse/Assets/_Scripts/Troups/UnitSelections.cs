using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//https://www.youtube.com/watch?v=vAVi04mzeKk
public class UnitSelections : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    private static UnitSelections _instance;

    public static UnitSelections Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ClickSelect(GameObject unitToAdd)
    {
        DeselectAll();
        unitsSelected.Add(unitToAdd);
        unitToAdd.GetComponent<Unit>().ShowHighlight();
        unitToAdd.GetComponent<UnitMovement>().enabled = true;
    }

    public void ShiftClickSelect(GameObject unitToAdd)
    {
        if (!unitsSelected.Contains(unitToAdd))
        {
            unitToAdd.GetComponent<Unit>().ShowHighlight();
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
            unitsSelected.Add(unitToAdd);
        }
        else
        {
            unitToAdd.GetComponent<Unit>().HideHighlight();
            unitToAdd.GetComponent<UnitMovement>().enabled = false;
            unitsSelected.Remove(unitToAdd);    
        }
    }

    public void DragSelect(GameObject unitToAdd)
    {
        if (!unitsSelected.Contains(unitToAdd))
        {
            unitsSelected.Add(unitToAdd);
            unitToAdd.GetComponent<Unit>().ShowHighlight();
            unitToAdd.GetComponent<UnitMovement>().enabled = true;

        }
    }

    public void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            unit.GetComponent<Unit>().HideHighlight();
            unit.GetComponent<UnitMovement>().enabled = false;
        }
        unitsSelected.Clear();
    }

    public void Deselect(GameObject unitToDeselect)
    {
        unitToDeselect.GetComponent<Unit>().HideHighlight();
        unitToDeselect.GetComponent<UnitMovement>().enabled = false;
        unitsSelected.Remove(unitToDeselect);
    }
}
