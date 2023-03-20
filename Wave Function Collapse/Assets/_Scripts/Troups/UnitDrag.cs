using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitDrag : MonoBehaviour
{
    Camera myCam;

    [SerializeField]
    RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    // Start is called before the first frame update
    void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;

        DrawVisual();
    }

    // Update is called once per frame
    void Update()
    {
        // when clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPosition = Mouse.current.position.value;
            selectionBox = new Rect();
        }

        // when dragging
        if (Mouse.current.leftButton.isPressed)
        {
            endPosition = Mouse.current.position.value;
            DrawVisual();
            DrawSelection();
        }

        // when releasing
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            SelectUnits();
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection() 
    {
        // do X calculation

        if (Mouse.current.position.value.x < startPosition.x)
        {
            // dragging left
            selectionBox.xMin = Mouse.current.position.value.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            // dragging right
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Mouse.current.position.value.x;

        }

        // do Y calcultion

        if (Mouse.current.position.value.y < startPosition.y)
        {
            // dragging down
            selectionBox.yMin = Mouse.current.position.value.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            // draggin up
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Mouse.current.position.value.y;
        }
    }

    void SelectUnits()
    {
        foreach ( var unit in UnitSelections.Instance.unitList)
        {
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelections.Instance.DragSelect(unit);
            }
        }
    }
}
