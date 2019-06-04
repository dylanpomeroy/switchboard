using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentScoreText : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Text>().text = $"Current Score: {Manager.TotalScore}";        
    }
}
