using UnityEngine;

public class Key : MonoBehaviour, ICollectible
{
    public bool IsCollected { get; private set; } = false;

    public void Collect()
    {
        if (!IsCollected)
        {
            IsCollected = true;
            Debug.Log("Key collected!");
            this.gameObject.SetActive(false);
        }
    }
}
