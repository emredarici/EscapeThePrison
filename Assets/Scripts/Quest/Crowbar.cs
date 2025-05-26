using UnityEngine;

public class Crowbar : MonoBehaviour, ICollectible
{
    public bool IsCollected = false;

    public void Collect()
    {
        if (!IsCollected)
        {
            IsCollected = true;
            Debug.Log("Crowbar collected!");
            this.gameObject.SetActive(false); // Crowbar'ı gizle
            MinigameManager.Instance.RegisterMinigame("BrakeDoor", GetComponent<BrakeDoorMG>());
        }
    }
}
