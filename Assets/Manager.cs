using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            callersDictionary[callerTransform.name].canHaveCall = true;

            callerTransform.GetChild(0).GetComponent<IndicatorLight>().lightState = 2;
        }

        callersList = callersDictionary.Select(kvp => kvp.Value).ToList();

        if (SceneManager.GetActiveScene().name == "Main")
            StartCallCycle();
        else
            Debug.Log("Manager script detected this is not the main scene." +
                " Will not start the call cycle but will instead let TutotorialManager lead.");
    }

    private bool stopCallCycle;
    private async void StartCallCycle()
    {
        InvokeIncomingCall();

        while (true)
        {
            var randomWaitTime = Random.Range(5000, 15000);
            await Task.Delay(randomWaitTime);

            if (stopCallCycle) break;
            InvokeIncomingCall();
        }
    }

    private void InvokeIncomingCall()
    {
        var randomAttempts = 0;

        //Debug.Log("Invoking call.");
        var randomIndex = Random.Range(0, callersList.Count);

        while (!callersList[randomIndex].canHaveCall && randomAttempts < 100)
        {
            randomIndex = Random.Range(0, callersList.Count);
            randomAttempts++;

            if (randomAttempts >= 100)
                return; // don't try forever
        }

        var randomCaller = callersList[randomIndex];
        randomCaller.canHaveCall = false;

        var callerIndex = randomIndex;
        while (!callersList[randomIndex].canHaveCall)
        {
            randomIndex = Random.Range(0, callersList.Count);
            randomAttempts++;

            if (randomAttempts >= 100)
            {
                randomCaller.canHaveCall = true;
                return; // don't try forever
            }
        }

        var randomReceiver = callersList[randomIndex];
        randomReceiver.canHaveCall = false;

        randomCaller.CallIncoming = true;
        randomCaller.RequestedReceiver = randomReceiver;
    }

    private void OnApplicationQuit()
    {
        stopCallCycle = true;
    }
}
