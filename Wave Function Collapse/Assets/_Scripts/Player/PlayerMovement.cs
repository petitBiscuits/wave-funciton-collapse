using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera Option")]
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Transform _lookAt;

    [SerializeField]
    private float _acceleration = .0f;
    [SerializeField]
    private float _maxSpeed = 10f;
    [SerializeField]
    private Rigidbody _rigidBody;
    [SerializeField]
    private Animator _animator;


    private Coroutine _coroutine;
    private Vector3 _lastRotation;

    private Keyboard _keyboard;
    private Mouse _mouse;

    void Start()
    {
        this._keyboard = Keyboard.current;
        this._mouse = Mouse.current;
    }

    private void Update()
    {
        Vector3 direction = Vector3.zero;
        if (_keyboard.wKey.isPressed)
        {
            direction += new Vector3(0, 0, 1);
        }
        if (_keyboard.sKey.isPressed)
        {
            direction += new Vector3(0, 0, -1);
        }
        if (_keyboard.aKey.isPressed)
        {
            direction += new Vector3(-1,0,0);
        }
        if (_keyboard.dKey.isPressed)
        {
            direction += new Vector3(1, 0, 0);
        }

        // movement with momentum
        if (_rigidBody.velocity.magnitude > 0.1)
        {
            _animator.SetBool("isWalking", true);
            // set the speed of the animation by the velocity of the rigidbody
            _animator.speed =  Mathf.Clamp(_rigidBody.velocity.magnitude,0,2.5f);
        }
        else
        {
            _animator.SetBool("isWalking", false);
            _animator.speed = 1;
        }
        _rigidBody.velocity += direction * _acceleration * Time.deltaTime;
        // rotation of the bory equals to the direction of the acceleration
        if (direction != Vector3.zero && _lastRotation != direction)
        {
            _lastRotation = direction;
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            _coroutine = StartCoroutine(RotateAnimation(direction));
        }
        _camera.transform.LookAt(_lookAt);
    }

    IEnumerator RotateAnimation(Vector3 direction)
    {
        float time = 0;
        float duration = 0.2f;
        Quaternion startRotation = _rigidBody.rotation;
        Quaternion endRotation = Quaternion.LookRotation(direction);
        while (time < duration)
        {
            _rigidBody.rotation = Quaternion.Lerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _rigidBody.rotation = endRotation;
        _coroutine = null;
    }

    void FixedUpdate()
    {
        // clamp velocity
        _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, _maxSpeed);
    }
}