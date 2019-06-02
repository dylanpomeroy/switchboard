using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSwitch : MonoBehaviour
{
    public bool OnState
    {
        get
        {
            return onState;
        }
        set
        {
            onState = value;
        }
    }

    private bool onState;

    private void OnMouseDown()
    {
        OnState = !OnState;
    }

    private void Update()
    {
        if (OnState)
        {
            transform.localRotation = Quaternion.Euler(-20, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(20, 0, 0);
        }
    }
}
