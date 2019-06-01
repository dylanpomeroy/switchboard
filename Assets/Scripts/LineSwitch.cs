using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSwitch : MonoBehaviour
{
    public bool onState;

    private void OnMouseDown()
    {
        onState = !onState;
    }

    private void Update()
    {
        if (onState)
        {
            transform.localRotation = Quaternion.Euler(-20, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(20, 0, 0);
        }
    }
}
