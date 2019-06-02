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
        CallCleanup,
    }

    private void Update()
    {
        var connectedCaller0 = Connector0.CurrentlyConnectedTo;

        var connectedCaller1 = Connector1.CurrentlyConnectedTo;

        var connectedCaller0IsCaller = connectedCaller0?.RequestedReceiver != null;
        var connectedCaller0IsReceiver = connectedCaller1?.RequestedReceiver != null;

        var connectedCaller1IsCaller = connectedCaller1?.RequestedReceiver != null;
        var connectedCaller1IsReceiver = connectedCaller0?.RequestedReceiver != null;

        switch (CurrentConnectionState)
        {
            case ConnectionState.Disconnected:
                IndicatorLight.lightState = 2;

                if ((connectedCaller0 != null && connectedCaller0IsCaller)
                    || (connectedCaller1 != null && connectedCaller1IsCaller))
                {
                    Debug.Log($"Connected first caller. Moving to CallerConnected state.");
                    CurrentConnectionState = ConnectionState.CallerConnected;
                }
                break;

            case ConnectionState.CallerConnected:
                IndicatorLight.lightState = 1;
                if (connectedCaller0 == null && connectedCaller1 == null)
                {
                    Debug.Log($"Disconnected caller before completing call. Moving to CallCleanup state.");
                    Manager.TotalScore -= 5;
                    CurrentConnectionState = ConnectionState.CallCleanup;
                }
                else if (connectedCaller0IsCaller && connectedCaller1IsCaller)
                {
                    Debug.Log($"Connected two callers both requesting someone else. Moving to CallCleanup state.");
                    Manager.TotalScore -= 5;
                    CurrentConnectionState = ConnectionState.CallCleanup;
                }
                else if (LineSwitch.OnState)
                {
                    Debug.Log($"Line switch turned on. Moving to LineSwitchOn state.");
                    CurrentConnectionState = ConnectionState.LineSwitchOn;
                }
                break;

            case ConnectionState.LineSwitchOn:
                if (connectedCaller0 == null && connectedCaller1 == null)
                {
                    Debug.Log("Disconnected caller before completing call. Moving to CallCleanup state.");
                    Manager.TotalScore -= 5;
                    CurrentConnectionState = ConnectionState.CallCleanup;
                }
                else if (connectedCaller0IsCaller && connectedCaller1IsCaller)
                {
                    Debug.Log($"Connected two callers both requesting someone else. Moving to CallCleanup state.");
                    Manager.TotalScore -= 5;
                    CurrentConnectionState = ConnectionState.CallCleanup;
                }
                else
                {
                    if ((connectedCaller0 != null & connectedCaller1 != null)
                    && ((connectedCaller0IsCaller && connectedCaller0.RequestedReceiver == connectedCaller1)
                        || (connectedCaller1IsCaller && connectedCaller1.RequestedReceiver == connectedCaller0)))
                    {
                        Debug.Log($"Both callers connected. Moving to ReceiverConnected state.");
                        CurrentConnectionState = ConnectionState.ReceiverConnected;
                    }
                    else
                    {
                        var requestedCaller = connectedCaller0IsCaller ? connectedCaller0.RequestedReceiver.gameObject.name
                            : connectedCaller1.RequestedReceiver.gameObject.name;
                                Debug.Log($"Requested caller at {requestedCaller}");
                    }
                }

                break;

            case ConnectionState.ReceiverConnected:
                if (connectedCaller0 == null || connectedCaller1 == null)
                {
                    Debug.Log("Disconnected caller before completing call. Moving to CallCleanup state.");
                    Manager.TotalScore -= 5;
                    CurrentConnectionState = ConnectionState.CallCleanup;
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
                IndicatorLight.lightState = 0;

                if (connectedCaller0 == null || connectedCaller1 == null)
                {
                    Debug.Log("Disconnected caller before completing call. Moving to CallCleanup state.");
                    Manager.TotalScore -= 5;
                    CurrentConnectionState = ConnectionState.CallCleanup;
                    break;
                }

                RunCall();
                break;

            case ConnectionState.CallCompleted:
                Debug.Log($"Call completed. Adding to score and moving to CallCleanup state.");
                Manager.TotalScore += 10;

                CurrentConnectionState = ConnectionState.CallCleanup;
                break;

            case ConnectionState.CallCleanup:
                Debug.Log($"Call cleanup started. Restoring caller states and moving to Disconnected state.");

                if (connectedCaller0 != null)
                {
                    connectedCaller0.CallIncoming = false;
                    connectedCaller0.RequestedReceiver = null;
                    connectedCaller0.canHaveCall = true;
                }
                
                if (connectedCaller1 != null)
                {
                    connectedCaller1.CallIncoming = false;
                    connectedCaller1.RequestedReceiver = null;
                    connectedCaller1.canHaveCall = true;
                }

                IndicatorLight.lightState = 2;

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
        var randomMillisecondCallDuration = Random.Range(5000, 10000);
        Debug.Log($"Call will run for {randomMillisecondCallDuration / 1000} seconds");
        await Task.Delay(randomMillisecondCallDuration);

        callRunning = false;
        if (CurrentConnectionState != ConnectionState.LineSwitchOff)
        {
            // got disconnected while waiting, should not set call completed
            return;
        }

        Debug.Log($"Call completed. Moving to CallCompleted state.");
        Connector0.CurrentlyConnectedTo.IndicatorLight.lightState = 2;
        Connector1.CurrentlyConnectedTo.IndicatorLight.lightState = 2;
        CurrentConnectionState = ConnectionState.CallCompleted;
    }
}
