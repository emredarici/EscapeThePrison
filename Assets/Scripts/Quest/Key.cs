using UnityEngine;
using Player;
using UnityEngine.Rendering;

public class Key : MonoBehaviour, ICollectible
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
            Debug.Log("Key collected!");
            this.gameObject.SetActive(false);
            IsCollected = true;
        }
    }
    public void CollectAudio()
    {
        AudioManager.Instance.PlayAudio(audioSource, AudioManager.Instance.colletSouce, 0);
    }
}
