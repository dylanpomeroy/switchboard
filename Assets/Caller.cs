using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caller : MonoBehaviour
{
    public bool CallIncoming
    {
        get
        {
            return callIncoming;
        }
        set
        {
            callIncoming = value;
            IndicatorLight.lightState = callIncoming ? 1 : 2;
        }
    }

    private bool callIncoming;

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
}
