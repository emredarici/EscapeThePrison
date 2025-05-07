using System.Collections;
using UnityEngine;
using Player;

public class BrakeDoorMG : MonoBehaviour, IMinigame
{
    public bool IsGameRunning { get; private set; } = false;
    private PlayerControls playerControls;
    private GameObject player;
    public GameObject door;
    public Crowbar crowbar;
    public GameObject CrowbarObject; // Crowbar nesnesi
    private int hitCount = 0;
    private const int maxHits = 5;

    [Header("Camera Settings")]
    public Camera minigameCamera; // Mini oyun sırasında kullanılacak normal kamera
    public Camera cinemachineCamera; // Cinemachine kamerayı temsil eden standart kamera

    public void StartMinigame()
    {
        playerControls = FindObjectOfType<PlayerControls>();
        player = playerControls.gameObject;
        Debug.Log("Brake Door Minigame Started");

        // Karakter kontrolünü devre dışı bırak
        player.GetComponent<CharacterController>().enabled = false;
        playerControls.movementControl.action.Disable();

        // Karakteri kapının yanına taşı
        playerControls.gameObject.transform.position = new Vector3(door.transform.position.x + 1.20f, playerControls.gameObject.transform.position.y, playerControls.gameObject.transform.position.z);

        // Crowbar nesnesini etkinleştir
        CrowbarObject.SetActive(true);

        // Mini oyun kamerasını etkinleştir
        SwitchToMinigameCamera();

        IsGameRunning = true;
        hitCount = 0; // Sayaç sıfırlanır
    }

    public void EndMinigame()
    {
        Debug.Log("Brake Door Minigame Ended");

        // Hareket kontrolünü etkinleştir
        playerControls.movementControl.action.Enable();
        player.GetComponent<CharacterController>().enabled = true;

        // Crowbar nesnesini gizle
        CrowbarObject.SetActive(false);

        // Cinemachine kamerayı geri yükle
        SwitchToCinemachineCamera();

        IsGameRunning = false;
        MinigameManager.Instance.minigameActive = false;
    }

    private void Update()
    {
        if (IsGameRunning && Input.GetKeyDown(KeyCode.Space) && crowbar.IsCollected)
        {
            HitDoor();
        }
    }

    private void HitDoor()
    {
        PlayerAnimationHandler.Instance.PlayHittingAnimation(() =>
        {
            hitCount++;
            Debug.Log($"Door hit count: {hitCount}");


            if (hitCount >= maxHits)
            {
                Destroy(door);
                EndMinigame();
            }
        });
    }

    private void SwitchToMinigameCamera()
    {
        if (minigameCamera != null && cinemachineCamera != null)
        {
            // Cinemachine kamerayı devre dışı bırak
            cinemachineCamera.enabled = false;

            // Mini oyun kamerasını etkinleştir
            minigameCamera.enabled = true;
        }
    }

    private void SwitchToCinemachineCamera()
    {
        if (minigameCamera != null && cinemachineCamera != null)
        {
            // Mini oyun kamerasını devre dışı bırak
            minigameCamera.enabled = false;

            // Cinemachine kamerayı etkinleştir
            cinemachineCamera.enabled = true;
        }
    }

    public void CameraShake()
    {
        StartCoroutine(CameraShakeCoroutine(.2f, .1f));
    }

    private IEnumerator CameraShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPosition = minigameCamera.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            minigameCamera.transform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Kamera pozisyonunu sıfırla
        minigameCamera.transform.localPosition = originalPosition;
    }
}
