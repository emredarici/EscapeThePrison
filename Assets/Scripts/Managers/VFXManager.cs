using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    public GameObject locationMarkerPrefab;
    [HideInInspector]
    public GameObject marker;

    public void SpawnLocationMarker(Vector3 position)
    {
        marker = Instantiate(locationMarkerPrefab, position, Quaternion.identity);
        DebugToolKit.Log($"Character instantiated at position: {position}");
    }

    public void DestroyMarker()
    {
        Destroy(marker, 0f);
        DebugToolKit.Log(marker + " destroyed.");
    }

}
