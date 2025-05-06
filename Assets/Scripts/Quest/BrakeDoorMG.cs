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

    public void StartMinigame()
    {
        playerControls = FindObjectOfType<PlayerControls>();
        player = playerControls.gameObject;
        Debug.Log("Brake Door Minigame Started");
        player.GetComponent<CharacterController>().enabled = false; // Karakter kontrolcüsünü devre dışı bırak
        playerControls.gameObject.transform.position = new Vector3(door.transform.position.x + 1.20f, playerControls.gameObject.transform.position.y, playerControls.gameObject.transform.position.z);
        CrowbarObject.SetActive(true); // Crowbar nesnesini etkinleştir
        IsGameRunning = true;
        playerControls.movementControl.action.Disable(); // Hareket kontrolünü devre dışı bırak
        hitCount = 0; // Sayaç sıfırlanır
    }

    public void EndMinigame()
    {
        Debug.Log("Brake Door Minigame Ended");
        playerControls.movementControl.action.Enable(); // Hareket kontrolünü etkinleştir
        player.GetComponent<CharacterController>().enabled = true; // Karakter kontrolcüsünü etkinleştir
        IsGameRunning = false;
        CrowbarObject.SetActive(false); // Crowbar nesnesini gizle
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
}
