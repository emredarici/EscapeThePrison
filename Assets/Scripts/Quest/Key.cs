using UnityEngine;

public class Key : MonoBehaviour, ICollectible
{
    public bool IsCollected = false;

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
