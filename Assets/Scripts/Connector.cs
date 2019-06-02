using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Line;

public class Connector : MonoBehaviour
{
    private Dictionary<int, string> rows = new Dictionary<int, string>
    {
        { 2, "4" },
        { 1, "3" },
        { 0, "2" },
        { -1, "1" },
        { -2, "0" },
    };

    private Dictionary<int, string> columns = new Dictionary<int, string>
    {
        { 2, "9" },
        { 1, "8" },
        { 0, "7" },
        { -1, "6" },
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
                Debug.Log($"Line connected to caller: {value.name}");
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

    private Rigidbody rigidBodyComponent;

    private void Start()
    {
        rigidBodyComponent = GetComponent<Rigidbody>();

        transform.position = Source.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnMouseDown()
    {
        rigidBodyComponent.isKinematic = true;

        if (CurrentlyConnectedTo != null)
        {
            CurrentlyConnectedTo.ConnectedLine = null;
            CurrentlyConnectedTo = null;
        }

        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }
    
    private void OnMouseDrag()
    {
        cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;

        cursorPosition = new Vector3(
            Mathf.Max(Mathf.Min(cursorPosition.x, LeftBorder), RightBorder),
            Mathf.Max(Mathf.Min(cursorPosition.y, TopBorder), BottomBorder),
            cursorPosition.z);
        transform.position = cursorPosition + new Vector3(0, 0, 1);

        if (transform.localPosition.y < 0.3f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
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
        }
        else
        {
            roundedPosition = new Vector3(roundedPosition.x, roundedPosition.y, 0.5f);
        }

        if (roundedPosition != Source.position)
        {
            var positionString = rows[(int)roundedPosition.y] + columns[(int)roundedPosition.x];

            if (Manager.callersDictionary[positionString].ConnectedLine != null)
            {
                rigidBodyComponent.isKinematic = false;
            }
            else
            {
                CurrentlyConnectedTo = Manager.callersDictionary[positionString];
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        transform.position = roundedPosition;
    }
}
