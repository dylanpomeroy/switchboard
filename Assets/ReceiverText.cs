using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ReceiverText : MonoBehaviour
{
    public Line LineParent;

    private TextMesh textComponent;

    private void Start()
    {
        textComponent = GetComponent<TextMesh>();
    }

    void Update()
    {
        if (LineParent.CurrentConnectionState == Line.ConnectionState.LineSwitchOff)
        {
            WriteDots();
            return;
        }
        else
        {
            writingDots = false;
        }

        if (LineParent.Connector0.CurrentlyConnectedTo?.RequestedReceiver != null
            && LineParent.LineSwitch.OnState)
        {
            textComponent.text = $"\"{LineParent.Connector0.CurrentlyConnectedTo.RequestedReceiver.name}\"";
        }
        else if (LineParent.Connector1.CurrentlyConnectedTo?.RequestedReceiver != null
            && LineParent.LineSwitch.OnState)
        {
            textComponent.text = $"\"{LineParent.Connector1.CurrentlyConnectedTo.RequestedReceiver.name}\"";
        }
        else
        {
            textComponent.text = string.Empty;
        }
    }

    private bool writingDots;
    private async void WriteDots()
    {
        if (writingDots) return;
        writingDots = true;

        while (true)
        {
            await Task.Delay(500);
            Debug.Log($"About to write one dot. writingDots is {writingDots}");
            if (!writingDots) return;
            textComponent.text = ".";

            await Task.Delay(500);
            Debug.Log($"About to write one dot. writingDots is {writingDots}");
            if (!writingDots) return;
            textComponent.text = "..";

            await Task.Delay(500);
            Debug.Log($"About to write one dot. writingDots is {writingDots}");
            if (!writingDots) return;
            textComponent.text = "...";
        }
    }
}
