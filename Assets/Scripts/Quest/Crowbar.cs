using UnityEngine;
using Player;

public class Crowbar : MonoBehaviour, ICollectible
{
    public bool IsCollected = false;
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }

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
    public void CollectAudio()
    {
        AudioManager.Instance.PlayAudio(audioSource, AudioManager.Instance.colletSouce, 1);
    }
}
