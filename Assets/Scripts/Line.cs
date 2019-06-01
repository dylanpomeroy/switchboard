using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineSwitch LineSwitch;
    public IndicatorLight IndicatorLight;
    public Connector Connector0;
    public Connector Connector1;

    public string Connector0Goal;
    public string Connector1Goal;

    private void Start()
    {
        Connector0Goal = "D3";
        Connector1Goal = "A2";
    }

    private void Update()
    {
        if (Connector0.CurrentlyConnectedTo == Connector0Goal
            && Connector1.CurrentlyConnectedTo == Connector1Goal)
        {
            IndicatorLight.lightState = 0;
        }
        else
        {
            IndicatorLight.lightState = 2;
        }
    }
}
