using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    public NavMeshAgent myAgent;
    public LayerMask groundLayer;

    [SerializeField]
    private float _turnSpeed = .02f;
    public Quaternion endRotation;

    // Start is called before the first frame update
    void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                //myAgent.SetDestination(hit.point);
            }
        }
    }

    void FixedUpdate()
    {
        if (myAgent.remainingDistance <= myAgent.stoppingDistance + float.Epsilon)
        {
            myAgent.updateRotation = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, _turnSpeed);
        }
        else
        {
            myAgent.updateRotation = true;
        }
    }
}
