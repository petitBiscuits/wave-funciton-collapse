using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    [SerializeField]
    private Animator mAnim;

    // Start is called before the first frame update
    void Start()
    {
        mAnim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mAnim != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                mAnim.SetTrigger("TrOpen");
            }
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                mAnim.SetTrigger("TrClose");
            }
        }
    }
}
