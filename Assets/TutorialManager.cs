using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

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
        await Task.Delay(1000);
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
        await Task.Delay(500);

        DarkPanel.SetActive(true);
        HelpPanels[7].SetActive(true);
    }

    public void Help7Closed()
    {
        HelpPanels[7].SetActive(false);
        DarkPanel.SetActive(false);
    }
}
