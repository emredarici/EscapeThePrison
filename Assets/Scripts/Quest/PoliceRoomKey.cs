using UnityEngine;
using Player;

public class PoliceRoomKey : MonoBehaviour, ICollectible
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
            Debug.Log("Police Room Key collected!");
            this.gameObject.SetActive(false);
        }
    }
    public void CollectAudio()
    {
        AudioManager.Instance.PlayAudio(audioSource, AudioManager.Instance.colletSouce, 0);
    }
}

