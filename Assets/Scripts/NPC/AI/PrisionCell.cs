using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PrisonCell : MonoBehaviour
{
    public Transform bedPosition;
    public Transform barsPosition;
    public Transform toiletPosition;
    public Transform rectimePosition;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 1 = tamamen 3D, 0 = tamamen 2D
        audioSource.minDistance = 1f;  // Yakınlık ayarı
        audioSource.maxDistance = 15f; // Uzaklık ayarı
    }
}
