using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject DarkPanel;
    public GameObject HighlightParent;
    private List<GameObject> HelpPanels;
    private Dictionary<string, GameObject> Highlights;

    public Transform ConnectionObject;
    public static int TotalScore;
    public static Dictionary<string, Caller> callersDictionary;
    private static List<Caller> callersList;

    private void Start()
    {
        callersDictionary = new Dictionary<string, Caller>();

        for (var i = 0; i < ConnectionObject.childCount; i++)
        {
            var callerTransform = ConnectionObject.GetChild(i);
            callersDictionary[callerTransform.name] = callerTransform.GetComponent<Caller>();
            callersDictionary[callerTransform.name].canHaveCall = true;

            callerTransform.GetChild(0).GetComponent<IndicatorLight>().lightState = 2;
        }

        callersList = callersDictionary.Select(kvp => kvp.Value).ToList();

        DarkPanel.SetActive(true);
        HelpPanels = new List<GameObject>();
        for (var i = 0; i < DarkPanel.transform.childCount; i++)
        {
            HelpPanels.Add(DarkPanel.transform.GetChild(i).gameObject);
        }

        Highlights = new Dictionary<string, GameObject>();
        for (var i = 0; i < HighlightParent.transform.childCount; i++)
        {
            var highlightObject = HighlightParent.transform.GetChild(i).gameObject;
            Highlights.Add(highlightObject.name, highlightObject);
        }

        HelpPanels[0].SetActive(true);
    }

    public async void Help0Closed()
    {
        HelpPanels[0].SetActive(false);
        await Task.Delay(500);
        HelpPanels[1].SetActive(true);
        Highlights["CallerBoard"].SetActive(true);
    }

    public void Help1Closed()
    {
        Highlights["CallerBoard"].SetActive(false);
        HelpPanels[1].SetActive(false);
        HelpPanels[2].SetActive(true);
        Highlights["Caller49"].SetActive(true);
    }

    public void Help2Closed()
    {
        HelpPanels[2].SetActive(false);
        HelpPanels[3].SetActive(true);
        Highlights["Row4"].SetActive(true);
        Highlights["Column9"].SetActive(true);
    }

    public void Help3Closed()
    {
        Highlights["Caller49"].SetActive(false);
        Highlights["Row4"].SetActive(false);
        Highlights["Column9"].SetActive(false);
        HelpPanels[3].SetActive(false);
        HelpPanels[4].SetActive(true);
        Highlights["Caller26"].SetActive(true);
        Highlights["Row2"].SetActive(true);
        Highlights["Column6"].SetActive(true);
    }

    public void Help4Closed()
    {
        Highlights["Caller26"].SetActive(false);
        Highlights["Row2"].SetActive(false);
        Highlights["Column6"].SetActive(false);
        HelpPanels[4].SetActive(false);
        HelpPanels[5].SetActive(true);
        Highlights["LineBoard"].SetActive(true);
    }

    public void Help5Closed()
    {
        Highlights["LineBoard"].SetActive(false);
        HelpPanels[5].SetActive(false);
        HelpPanels[6].SetActive(true);
        Highlights["Line0"].SetActive(true);
    }

    public async void Help6Closed()
    {
        HelpPanels[6].SetActive(false);
        Highlights["Line0"].SetActive(false);
        DarkPanel.SetActive(false);

        await Task.Delay(500);
        var caller = callersDictionary["26"];
        caller.CallIncoming = true;
        caller.RequestedReceiver = callersDictionary["17"];
        caller.OverrideCallRequestedTimeout = true;
        await Task.Delay(1000);

        DarkPanel.SetActive(true);
        HelpPanels[7].SetActive(true);
    }

    public async void Help7Closed()
    {
        HelpPanels[7].SetActive(false);
        DarkPanel.SetActive(false);

        while (callersDictionary["26"].ConnectedLine == null)
        {
            await Task.Delay(100);
        }

        await Task.Delay(500);

        DarkPanel.SetActive(true);
        HelpPanels[8].SetActive(true);
        Highlights["Line0Switch"].SetActive(true);
    }

    public async void Help8Closed()
    {
        Highlights["Line0Switch"].SetActive(false);
        HelpPanels[8].SetActive(false);
        DarkPanel.SetActive(false);

        while (!callersDictionary["26"].ConnectedLine.LineSwitch.OnState)
        {
            await Task.Delay(100);
        }

        await Task.Delay(500);

        DarkPanel.SetActive(true);
        HelpPanels[9].SetActive(true);
        Highlights["Line0SecondConnector"].SetActive(true);
    }

    public async void Help9Closed()
    {
        Highlights["Line0SecondConnector"].SetActive(false);
        HelpPanels[9].SetActive(false);
        DarkPanel.SetActive(false);

        while (callersDictionary["26"].ConnectedLine.Connector0.CurrentlyConnectedTo == null
            || callersDictionary["26"].ConnectedLine.Connector1.CurrentlyConnectedTo == null)
        {
            await Task.Delay(100);
        }

        await Task.Delay(500);

        DarkPanel.SetActive(true);
        HelpPanels[10].SetActive(true);
    }

    public async void Help10Closed()
    {
        HelpPanels[10].SetActive(false);
        DarkPanel.SetActive(false);

        while (callersDictionary["26"].ConnectedLine.LineSwitch.OnState)
        {
            await Task.Delay(100);
        }

        await Task.Delay(5000);

        DarkPanel.SetActive(true);
        HelpPanels[11].SetActive(true); // "..." and call completed
    }

    public void Help11Closed()
    {
        HelpPanels[11].SetActive(false);
        HelpPanels[12].SetActive(true); // describe caller patience timeouts
    }

    public void Help12Closed()
    {
        HelpPanels[12].SetActive(false);
        HelpPanels[13].SetActive(true); // tell player to complete 3 more calls.
    }

    public async void Help13Closed()
    {
        HelpPanels[13].SetActive(false);
        DarkPanel.SetActive(false);

        var caller0 = callersDictionary["39"];
        caller0.CallIncoming = true;
        caller0.RequestedReceiver = callersDictionary["45"];
        caller0.OverrideCallRequestedTimeout = true;

        await Task.Delay(10000);

        var caller1 = callersDictionary["19"];
        caller1.CallIncoming = true;
        caller1.RequestedReceiver = callersDictionary["17"];
        caller1.OverrideCallRequestedTimeout = true;

        await Task.Delay(10000);

        var caller2 = callersDictionary["35"];
        caller2.CallIncoming = true;
        caller2.RequestedReceiver = callersDictionary["25"];
        caller2.OverrideCallRequestedTimeout = true;

        while (caller0.CallIncoming || caller1.CallIncoming || caller2.CallIncoming)
        {
            await Task.Delay(100);
        }

        await Task.Delay(1000);

        DarkPanel.SetActive(true);
        HelpPanels[14].SetActive(true); // congratz and mention call failures
    }

    public void Help14Closed()
    {
        HelpPanels[14].SetActive(false);
        HelpPanels[15].SetActive(true); // completed the tutorial!
    }

    public void Exit()
    {
        SceneManager.LoadScene("Title");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
