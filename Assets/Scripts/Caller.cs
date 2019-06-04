using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Caller : MonoBehaviour
{
    public AudioClip ErrorClip;
    private AudioSource audioSourceComponent;

    public bool CallIncoming
    {
        get
        {
            return callIncoming;
        }
        set
        {
            callIncoming = value;

            if (callIncoming)
                CountdownCallRequest();
            else
                IndicatorLight.lightState = 2;
        }
    }

    private bool callIncoming;

    public bool OverrideCallRequestedTimeout { get; set; }

    public Caller RequestedReceiver;

    public IndicatorLight IndicatorLight;

    public bool canHaveCall;

    public Line ConnectedLine {
        get
        {
            return connectedLine;
        }
        set
        {
            connectedLine = value;

            if (callIncoming && connectedLine != null)
                IndicatorLight.lightState = 0;
            else if (callIncoming && connectedLine == null)
                IndicatorLight.lightState = 1;
            else
                IndicatorLight.lightState = 2;
        }
    }

    private Line connectedLine;

    private void Start()
    {
        audioSourceComponent = GetComponent<AudioSource>();
    }

    private async void CountdownCallRequest()
    {
        timeBetweenBlinks = 1000;
        BlinkLight();

        if (OverrideCallRequestedTimeout)
            return;

        await Task.Delay(10000);
        if (ConnectedLine != null) return;
        timeBetweenBlinks = 500;

        await Task.Delay(10000);
        if (ConnectedLine != null) return;
        timeBetweenBlinks = 200;

        await Task.Delay(10000);
        if (ConnectedLine != null) return;
        timeBetweenBlinks = -1;

        if (Manager.LoseLifeOnTimeout) Manager.Lives--;
        audioSourceComponent.PlayOneShot(ErrorClip);
        CallIncoming = false;
    }

    private int timeBetweenBlinks;
    private async void BlinkLight()
    {
        while (timeBetweenBlinks != -1)
        {
            IndicatorLight.lightState = 1;

            await Task.Delay(timeBetweenBlinks);
            if (ConnectedLine != null)
            {
                timeBetweenBlinks = -1;
                return;
            }
            IndicatorLight.lightState = 2;

            await Task.Delay(timeBetweenBlinks);
            if (ConnectedLine != null)
            {
                timeBetweenBlinks = -1;
                return;
            }
        }
    }
}
