﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Line;

public class Connector : MonoBehaviour
{
    private Dictionary<int, string> rows = new Dictionary<int, string>
    {
        { 2, "A" },
        { 1, "B" },
        { 0, "C" },
        { -1, "D" },
        { -2, "E" },
    };

    private Dictionary<int, string> columns = new Dictionary<int, string>
    {
        { 2, "1" },
        { 1, "2" },
        { 0, "3" },
        { -1, "4" },
        { -2, "5" },
    };

    [SerializeField] Transform Source;
    [SerializeField] Line LineParent;
    [SerializeField] float LeftBorder;
    [SerializeField] float RightBorder;
    [SerializeField] float TopBorder;
    [SerializeField] float BottomBorder;

    public Caller CurrentlyConnectedTo
    {
        get
        {
            return currentlyConnectedTo;
        }
        set
        {
            if (value != null)
            {
                Debug.Log($"Line connected to caller: {currentlyConnectedTo}");
                value.ConnectedLine = LineParent;
            }
            else
            {
                currentlyConnectedTo.CallIncoming = false;
                currentlyConnectedTo.ConnectedLine = null;
                currentlyConnectedTo.RequestedReceiver = null;
            }
            

            currentlyConnectedTo = value;
        }
    }

    private Caller currentlyConnectedTo;

    Vector3 screenPoint;
    Vector3 offset;
    Vector3 cursorScreenPoint;
    Vector3 cursorPosition;

    Vector3 goalRotation;

    private void OnMouseDown()
    {
        if (CurrentlyConnectedTo != null)
        {
            CurrentlyConnectedTo.ConnectedLine = null;
            CurrentlyConnectedTo = null;
        }

        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        if (transform.position == Source.position)
        {
            transform.Rotate(new Vector3(-90, 0, 0));
        }
    }
    
    private void OnMouseDrag()
    {
        cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;

        cursorPosition = new Vector3(
            Mathf.Max(Mathf.Min(cursorPosition.x, LeftBorder), RightBorder),
            Mathf.Max(Mathf.Min(cursorPosition.y, TopBorder), BottomBorder),
            cursorPosition.z);
        transform.position = cursorPosition;
    }

    private void OnMouseUp()
    {
        var roundedPosition = new Vector3(
            Mathf.RoundToInt(cursorPosition.x),
            Mathf.RoundToInt(cursorPosition.y),
            Mathf.RoundToInt(cursorPosition.z));

        if (roundedPosition.y == -3)
        {
            roundedPosition = Source.position;
            transform.Rotate(new Vector3(90, 0, 0));
        }
        else
        {
            roundedPosition = new Vector3(roundedPosition.x, roundedPosition.y, 0);
        }

        transform.position = roundedPosition;

        if (roundedPosition != Source.position)
        {
            var positionString = rows[(int)transform.position.y] + columns[(int)transform.position.x];
            CurrentlyConnectedTo = Manager.callersDictionary[positionString];
        }
    }
}
