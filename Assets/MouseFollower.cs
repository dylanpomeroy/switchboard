using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 screenPoint;
    Vector3 offset;

    private void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    Vector3 cursorScreenPoint;
    Vector3 cursorPosition;
    private void OnMouseDrag()
    {
        cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        
        transform.position = cursorPosition;
    }

    private void OnMouseUp()
    {
        var roundedPosition = new Vector3(
            Mathf.RoundToInt(cursorPosition.x),
            Mathf.RoundToInt(cursorPosition.y),
            Mathf.RoundToInt(cursorPosition.z));

        transform.position = roundedPosition;
    }
}
