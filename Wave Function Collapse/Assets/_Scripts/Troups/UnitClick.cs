using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UnitClick : MonoBehaviour
{
    private Camera myCam;

    public LayerMask clickMask;
    public LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        myCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickMask))
            {
                if (Keyboard.current.shiftKey.isPressed)
                {
                    UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
                }
                else
                {
                    UnitSelections.Instance.ClickSelect(hit.collider.gameObject);
                }
            }
            else
            {
                if (!Keyboard.current.shiftKey.isPressed)
                {
                    UnitSelections.Instance.DeselectAll();
                }
            }
        }
    }
}
