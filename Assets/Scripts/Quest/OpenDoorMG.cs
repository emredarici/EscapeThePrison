using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoorMG : MonoBehaviour, IMinigame
{
    public GameObject miniGameMainUI;

    public RectTransform miniGameRedAreaRect;
    public RectTransform miniGameGreenAreaRect;
    public RectTransform miniGameInputAreaRect;

    public float moveDuration = 1f;

    public int score = 0;
    public TextMeshProUGUI miniGameScoreText;

    public bool IsGameRunning { get; private set; }

    public void StartMinigame()
    {
        IsGameRunning = true;
        DebugToolKit.Log("Open Door Minigame Started");
        miniGameMainUI.SetActive(true);
        miniGameMainUI.transform.localScale = Vector3.zero;
        LeanTween.scale(miniGameMainUI, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack);

        MoveInputArea();
    }

    public void EndMinigame()
    {
        IsGameRunning = false;
        DebugToolKit.Log("Open Door Minigame Ended");
        LeanTween.cancel(miniGameInputAreaRect.gameObject);

        LeanTween.scale(miniGameMainUI, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            miniGameMainUI.SetActive(false);
        });


    }

    private void MoveInputArea()
    {
        float startX = miniGameRedAreaRect.offsetMin.x;
        float endX = miniGameRedAreaRect.offsetMax.x;
        float fixedY = miniGameInputAreaRect.anchoredPosition.y;

        void MoveToStart()
        {
            LeanTween.moveX(miniGameInputAreaRect, startX, moveDuration).setOnComplete(MoveToEnd);
        }

        void MoveToEnd()
        {
            LeanTween.moveX(miniGameInputAreaRect, endX, moveDuration).setOnComplete(MoveToStart);
        }

        // Sabit Y pozisyonunu koru
        miniGameInputAreaRect.anchoredPosition = new Vector2(miniGameInputAreaRect.anchoredPosition.x, fixedY);

        MoveToStart();
    }

    void Update()
    {
        if (IsGameRunning && Input.GetKeyDown(KeyCode.Space))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(miniGameGreenAreaRect, miniGameInputAreaRect.position))
            {
                score++;
                DebugToolKit.Log($"Score: {score}");
                miniGameScoreText.text = "Security Bypass: " + score + " / " + 5;
                if (score >= 5)
                {
                    MinigameManager.Instance.StopCurrentMinigame();
                    DebugToolKit.Log("Minigame Completed!");
                }
            }
            else
            {
                score = 0;
                miniGameScoreText.text = "Security Bypass: " + score + " / " + 5;

            }

            if (score < 5)
            {
                float randomX = Random.Range(miniGameRedAreaRect.offsetMin.x + 45, miniGameRedAreaRect.offsetMax.x - 45);
                Vector2 newPosition = new Vector2(randomX, miniGameGreenAreaRect.anchoredPosition.y);
                miniGameGreenAreaRect.anchoredPosition = newPosition;
            }
        }
    }
}
