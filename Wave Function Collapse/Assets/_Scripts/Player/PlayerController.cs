using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject _cam;
    private Camera _cameraView;

    public float distance = 10.0f; // the distance from the object
    public float xSpeed = 120f; // the speed of horizontal rotation
    public float ySpeed = 120f; // the speed of vertical rotation

    private float x = 0.0f;
    private float y = 40.0f;

    private Mouse _mouse;
    private Keyboard _keyboard;

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraView = _cam.GetComponent<Camera>();
        _mouse = Mouse.current;
        _keyboard = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceView();

        Movement();
        
        AngleView();
    }

    void DistanceView()
    {
        float scrollValue = _mouse.scroll.ReadValue().y;
        scrollValue = Mathf.Clamp(scrollValue, -1, 1);

        if (scrollValue != 0)
        {
            distance += scrollValue * 0.3f;
            distance = Mathf.Clamp(distance, 3, 13);
        }
    }

    void Movement()
    {
        Vector3 cameraForward = _cam.transform.forward;

        // Project the camera's forward vector onto the XZ plane
        Vector3 cameraForwardXZ = Vector3.ProjectOnPlane(cameraForward, Vector3.up).normalized;

        Vector3 cameraLeftXZ = Vector3.Cross(Vector3.up, cameraForwardXZ).normalized;

        if (_keyboard.wKey.isPressed)
        {
            transform.position += cameraForwardXZ * Time.deltaTime * 5;
        }
        if (_keyboard.sKey.isPressed)
        {
            transform.position -= cameraForwardXZ * Time.deltaTime * 5;
        }
        if (_keyboard.aKey.isPressed)
        {
            transform.position -= cameraLeftXZ * Time.deltaTime * 5;
        }
        if (_keyboard.dKey.isPressed)
        {
            transform.position += cameraLeftXZ * Time.deltaTime * 5;
        }
    }

    void AngleView()
    {
        if (_mouse.middleButton.isPressed)
        {
            var delta = _mouse.delta.ReadValue();
            x += delta.x * xSpeed * Time.deltaTime;
            y -= delta.y * ySpeed * Time.deltaTime;

            // limit the vertical rotation angle to avoid flipping
            y = ClampAngle(y, 10, 90);
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + transform.position;

        _cam.transform.rotation = rotation;
        _cam.transform.position = position;
    }

    private void FixedUpdate()
    {
        
    }

    // Helper function to clamp the rotation angle
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;

        return Mathf.Clamp(angle, min, max);
    }
}
