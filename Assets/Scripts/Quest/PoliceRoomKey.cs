using UnityEngine;

public class PoliceRoomKey : MonoBehaviour, ICollectible
{
    public bool IsCollected { get; private set; } = false;

    public void Collect()
    {
        if (!IsCollected)
        {
            IsCollected = true;
            Debug.Log("Police Room Key collected!");
            this.gameObject.SetActive(false);
        }
    }
}

