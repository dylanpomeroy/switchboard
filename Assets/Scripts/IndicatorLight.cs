using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLight : MonoBehaviour
{
    public Material OnMaterial;
    public Material AlertMaterial;
    public Material OffMaterial;

    public int lightState;

    private void Update()
    {
        GetComponent<MeshRenderer>().material =
            lightState == 0 ? OnMaterial
            : lightState == 1 ? AlertMaterial
            : OffMaterial;
    }
}
