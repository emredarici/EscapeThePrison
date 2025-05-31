using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    public GameObject locationMarkerPrefab;
    public GameObject dialogueMarkerPrefab;
    [HideInInspector]
    public GameObject marker;
    [HideInInspector]
    public GameObject dialougeVFX;

    public void SpawnLocationMarker(Vector3 position)
    {
        marker = Instantiate(locationMarkerPrefab, position, Quaternion.identity);
        DebugToolKit.Log($"Character instantiated at position: {position}");
    }
    public void SpawnDialogueMarker(Vector3 position)
    {
        dialougeVFX = Instantiate(dialogueMarkerPrefab, position, Quaternion.identity);
        DebugToolKit.Log($"Dialogue marker instantiated at position: {position}");
    }

    public void DestroyMarker()
    {
        Destroy(marker, 0f);
        DebugToolKit.Log(marker + " destroyed.");
    }

    public void DestroyMarker(float delay)
    {
        Destroy(marker, delay);
        DebugToolKit.Log(marker + " destroyed after " + delay + " seconds.");
    }

    public void DestroyDialogueMarker()
    {
        Destroy(dialougeVFX, 0f);
        DebugToolKit.Log(dialougeVFX + " destroyed.");
    }
}
