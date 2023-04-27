using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] 
    private GameObject _indicator;
    
    [SerializeField]
    private GameObject _place;


    [SerializeField]
    private LayerMask _groundLayer;

    private GameObject _indicatorInstance;

    private List<GameObject> _movesPoints = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnitSelections.Instance.unitsSelected.Count == 0)
            return;
        
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
            {
                // create an indicator at the position of the hit
                _indicatorInstance = Instantiate(_indicator, hit.point + Vector3.up, Quaternion.identity);

                SpawnUnitPlacement(UnitFormation.GetMultipleCicle(_indicatorInstance));
            }
        }

        if (Mouse.current.rightButton.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
            {
                float angle = GetAngleBetweenTwoPoints(_indicatorInstance.transform.position, hit.point);

                _indicatorInstance.transform.rotation = Quaternion.Euler(0,angle,0);
                
            }
        }

        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            MoveAgents();
            Destroy(_indicatorInstance);
        }
    }

    private void SpawnUnitPlacement(List<Vector3> positions)
    {
        if (positions == null)
            return;
        foreach (var p in positions)
        {
            GameObject point = Instantiate(_place, _indicatorInstance.transform);
            point.transform.position = new Vector3(p.x, 0, p.z);
            _movesPoints.Add(point);
        }
    }
    
    private void MoveAgents()
    {
        int i = 0;

        foreach (GameObject o in UnitSelections.Instance.unitsSelected)
        {
            o.GetComponent<UnitMovement>().myAgent.SetDestination(_movesPoints[i].transform.position);
            o.GetComponent<UnitMovement>().endRotation = _movesPoints[i++].transform.rotation;
        }

        _movesPoints.Clear();
    }

    public static float GetAngleBetweenTwoPoints(Vector3 point1, Vector3 point2)
    {
        Vector3 direction = point2 - point1;
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }
}
