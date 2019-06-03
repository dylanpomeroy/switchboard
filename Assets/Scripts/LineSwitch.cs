using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSwitch : MonoBehaviour
{
    [SerializeField] AudioClip SwitchClip;
    private AudioSource audioSourceComponent;

    public bool OnState
    {
        get
        {
            return onState;
        }
        set
        {
            if (onState != value)
                audioSourceComponent.PlayOneShot(SwitchClip);
            onState = value;
        }
    }

    private bool onState;

    private void Start()
    {
        audioSourceComponent = GetComponent<AudioSource>();
    }

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
