using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Manager : MonoBehaviour
{
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

            callerTransform.GetChild(0).GetComponent<IndicatorLight>().lightState = 2;
        }

        callersList = callersDictionary.Select(kvp => kvp.Value).ToList();

        StartCallCycle();
    }

    private async void StartCallCycle()
    {
        InvokeIncomingCall();

        while (true)
        {
            var randomWaitTime = Random.Range(5000, 15000);
            await Task.Delay(randomWaitTime);

            InvokeIncomingCall();
        }
    }

    private void InvokeIncomingCall()
    {
        Debug.Log("Invoking call.");
        var randomIndex = Random.Range(0, callersList.Count);
        var randomCaller = callersList[randomIndex];

        var callerIndex = randomIndex;
        while (randomIndex == callerIndex)
        {
            randomIndex = Random.Range(0, callersList.Count);
        }

        var randomReceiver = callersList[randomIndex];

        randomCaller.CallIncoming = true;
        randomCaller.RequestedReceiver = randomReceiver;
    }
}
