using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject DarkPanel;
    public GameObject Help0Panel;
    public GameObject Help1Panel;
    public GameObject Help2Panel;
    public GameObject Help3Panel;
    public GameObject Help4Panel;
    public GameObject Help5Panel;

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
        Help0Panel.SetActive(true);
    }

    public async void Help0Closed()
    {
        Help0Panel.SetActive(false);
        await Task.Delay(1000);
        Help1Panel.SetActive(true);
    }

    public void Help1Closed()
    {
        Help1Panel.SetActive(false);
        Help2Panel.SetActive(true);
    }

    public void Help2Closed()
    {
        Help2Panel.SetActive(false);
        Help3Panel.SetActive(true);
    }

    public void Help3Closed()
    {
        Help3Panel.SetActive(false);
        Help4Panel.SetActive(true);
    }

    public void Help4Closed()
    {
        Help4Panel.SetActive(false);
        Help5Panel.SetActive(true);
    }

    public void Help5Closed()
    {
        Help5Panel.SetActive(false);
    }
}
