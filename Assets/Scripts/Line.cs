using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineSwitch LineSwitch;
    public IndicatorLight IndicatorLight;
    public Connector Connector0;
    public Connector Connector1;

    public ConnectionState CurrentConnectionState;
    public enum ConnectionState
    {
        Disconnected,
        CallerConnected,
        LineSwitchOn,
        ReceiverConnected,
        LineSwitchOff,
        CallCompleted,
    }

    private void Update()
    {
        var connectedCaller0 = Connector0.CurrentlyConnectedTo;
        var connectedCaller1 = Connector1.CurrentlyConnectedTo;

        switch (CurrentConnectionState)
        {
            case ConnectionState.Disconnected:
                if ((connectedCaller0 != null && connectedCaller0.CallIncoming)
                    || (connectedCaller1 != null && connectedCaller1.CallIncoming))
                {
                    Debug.Log($"Connected first caller. Moving to CallerConnected state.");
                    CurrentConnectionState = ConnectionState.CallerConnected;
                }
                break;

            case ConnectionState.CallerConnected:
                if (connectedCaller0 == null && connectedCaller1 == null)
                {
                    Debug.Log("Disconnected caller before completing call. Moving to Disconnected state.");
                    Manager.TotalScore -= 5;
                    CurrentConnectionState = ConnectionState.Disconnected;
                }
                else if (LineSwitch.OnState)
                {
                    Debug.Log($"Line switch turned on. Moving to LineSwitchOn state.");
                    CurrentConnectionState = ConnectionState.LineSwitchOn;
                }
                break;

            case ConnectionState.LineSwitchOn:
                Debug.Log($"Requested caller at {connectedCaller0?.RequestedReceiver?.gameObject.name ?? connectedCaller1?.RequestedReceiver?.gameObject.name ?? "error"}");
                if (connectedCaller0 == null && connectedCaller1 == null)
                {
                    Debug.Log("Disconnected caller before completing call. Moving to Disconnected state.");
                    Manager.TotalScore -= 5;
                    CurrentConnectionState = ConnectionState.Disconnected;
                }

                if ((connectedCaller0 != null && connectedCaller1 != null)
                    && ((connectedCaller0.RequestedReceiver != null && connectedCaller0.RequestedReceiver == connectedCaller1)
                        || (connectedCaller1.RequestedReceiver != null && connectedCaller1.RequestedReceiver == connectedCaller0)))
                {
                    Debug.Log($"Both callers connected. Moving to ReceiverConnected state.");
                    CurrentConnectionState = ConnectionState.ReceiverConnected;
                }
                break;

            case ConnectionState.ReceiverConnected:
                if (connectedCaller0 == null || connectedCaller1 == null)
                {
                    Debug.Log("Disconnected caller before completing call. Moving to Disconnected state.");
                    Manager.TotalScore -= 5;
                    CurrentConnectionState = ConnectionState.Disconnected;
                    break;
                }

                connectedCaller0.IndicatorLight.lightState = 0;
                connectedCaller1.IndicatorLight.lightState = 0;

                if (!LineSwitch.OnState)
                {
                    Debug.Log($"Line switch turned off. Moving to LineSwitchOff state.");
                    CurrentConnectionState = ConnectionState.LineSwitchOff;
                }
                break;

            case ConnectionState.LineSwitchOff:
                if (connectedCaller0 == null || connectedCaller1 == null)
                {
                    Debug.Log("Disconnected caller before completing call. Moving to Disconnected state.");
                    Manager.TotalScore -= 5;
                    CurrentConnectionState = ConnectionState.Disconnected;
                    break;
                }

                RunCall();
                break;

            case ConnectionState.CallCompleted:
                Debug.Log($"Call completed. Adding to score and setting line to disconnected.");
                Manager.TotalScore += 10;

                connectedCaller0.CallIncoming = false;
                connectedCaller0.RequestedReceiver = null;

                CurrentConnectionState = ConnectionState.Disconnected;
                break;
        }
    }

    private bool callRunning;
    private async void RunCall()
    {
        if (callRunning)
            return;

        callRunning = true;
        await Task.Delay(Random.Range(5000, 10000));

        if (CurrentConnectionState != ConnectionState.LineSwitchOff)
        {
            // got disconnected while waiting, should not set call completed
            callRunning = false;
            return;
        }

        Debug.Log($"Call completed. Moving to CallCompleted state.");
        Connector0.CurrentlyConnectedTo.IndicatorLight.lightState = 2;
        Connector1.CurrentlyConnectedTo.IndicatorLight.lightState = 2;
        CurrentConnectionState = ConnectionState.CallCompleted;
    }
}
